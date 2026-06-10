import { createContext, useMemo, type PropsWithChildren } from 'react';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import { BrowserRouter } from 'react-router-dom';
import { createAuthClient, type AuthClient } from './auth/authClient';

const defaultTheme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1565c0',
    },
  },
});

export const AuthClientContext = createContext<AuthClient | null>(null);

function AuthProvider({ children }: PropsWithChildren) {
  const authClient = useMemo(() => createAuthClient(), []);

  return (
    <AuthClientContext.Provider value={authClient}>
      {children}
    </AuthClientContext.Provider>
  );
}

export function AppProviders({ children }: PropsWithChildren) {
  return (
    <ThemeProvider theme={defaultTheme}>
      <CssBaseline />
      <BrowserRouter>
        <AuthProvider>{children}</AuthProvider>
      </BrowserRouter>
    </ThemeProvider>
  );
}
