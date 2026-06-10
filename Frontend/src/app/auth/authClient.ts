export type AuthUser = {
  id: string;
  roles: string[];
};

export type AuthClient = {
  getAccessToken: () => Promise<string | null>;
  getCurrentUser: () => Promise<AuthUser | null>;
};

export function createAuthClient(): AuthClient {
  return {
    async getAccessToken() {
      return null;
    },
    async getCurrentUser() {
      return null;
    },
  };
}
