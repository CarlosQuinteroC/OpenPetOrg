import type { KeycloakTokenParsed } from 'keycloak-js'

export type AuthUser = {
  id: string;
  roles: string[];
};

export type AuthClient = {
  isEnabled: () => boolean;
  login: () => Promise<void>;
  logout: () => Promise<void>;
  getAccessToken: () => Promise<string | null>;
  getCurrentUser: () => Promise<AuthUser | null>;
};

type RolePayload = KeycloakTokenParsed & {
  preferred_username?: string;
  email?: string;
  sub?: string;
  realm_access?: {
    roles?: string[];
  };
  resource_access?: Record<string, { roles?: string[] }>;
};

type AuthEnv = {
  enabled: boolean;
  url: string;
  realm: string;
  clientId: string;
};

function readAuthEnv(): AuthEnv {
  const enabled = (import.meta.env.VITE_AUTH_ENABLED as string | undefined)?.toLowerCase() === 'true';

  return {
    enabled,
    url: (import.meta.env.VITE_KEYCLOAK_URL as string | undefined)?.trim() ?? '',
    realm: (import.meta.env.VITE_KEYCLOAK_REALM as string | undefined)?.trim() ?? '',
    clientId: (import.meta.env.VITE_KEYCLOAK_CLIENT_ID as string | undefined)?.trim() ?? '',
  };
}

class KeycloakAuthClient implements AuthClient {
  private readonly env = readAuthEnv();
  private keycloakInitPromise: Promise<import('keycloak-js').default | null> | null = null;

  isEnabled(): boolean {
    return this.env.enabled;
  }

  async login(): Promise<void> {
    const keycloak = await this.getKeycloak();
    if (!keycloak) {
      return;
    }

    await keycloak.login({ redirectUri: window.location.href });
  }

  async logout(): Promise<void> {
    const keycloak = await this.getKeycloak();
    if (!keycloak) {
      return;
    }

    await keycloak.logout({ redirectUri: window.location.origin });
  }

  async getAccessToken(): Promise<string | null> {
    const keycloak = await this.getKeycloak();
    if (!keycloak) {
      return null;
    }

    if (!keycloak.authenticated) {
      await this.login();
      return null;
    }

    await keycloak.updateToken(30);
    return keycloak.token ?? null;
  }

  async getCurrentUser(): Promise<AuthUser | null> {
    const keycloak = await this.getKeycloak();
    if (!keycloak?.authenticated) {
      return null;
    }

    const parsed = (keycloak.tokenParsed ?? {}) as RolePayload;
    const id = parsed.preferred_username ?? parsed.email ?? parsed.sub;

    if (!id) {
      return null;
    }

    return {
      id,
      roles: extractRoles(parsed),
    };
  }

  private async getKeycloak(): Promise<import('keycloak-js').default | null> {
    if (!this.env.enabled) {
      return null;
    }

    if (!this.keycloakInitPromise) {
      this.keycloakInitPromise = this.initializeKeycloak();
    }

    return this.keycloakInitPromise;
  }

  private async initializeKeycloak(): Promise<import('keycloak-js').default | null> {
    if (!this.env.url || !this.env.realm || !this.env.clientId) {
      throw new Error(
        'Keycloak auth is enabled but missing env vars. Set VITE_KEYCLOAK_URL, VITE_KEYCLOAK_REALM, and VITE_KEYCLOAK_CLIENT_ID.',
      );
    }

    const { default: Keycloak } = await import('keycloak-js');

    const keycloak = new Keycloak({
      url: this.env.url,
      realm: this.env.realm,
      clientId: this.env.clientId,
    });

    await keycloak.init({
      onLoad: 'check-sso',
      checkLoginIframe: false,
      pkceMethod: 'S256',
    });

    return keycloak;
  }
}

function extractRoles(parsed: RolePayload): string[] {
  const roles = new Set<string>();

  for (const role of parsed.realm_access?.roles ?? []) {
    roles.add(role);
  }

  for (const client of Object.values(parsed.resource_access ?? {})) {
    for (const role of client.roles ?? []) {
      roles.add(role);
    }
  }

  return Array.from(roles.values());
}

export function createAuthClient(): AuthClient {
  return new KeycloakAuthClient();
}
