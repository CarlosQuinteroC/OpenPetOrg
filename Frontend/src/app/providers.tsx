import { useMemo, type PropsWithChildren } from 'react';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import { BrowserRouter } from 'react-router-dom';
import { createAuthClient } from './auth/authClient';
import { ApiProvider } from '../services/api/context';
import { AuthClientContext } from './auth/authContext';

const defaultTheme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1565c0',
    },
  },
});

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
        <AuthProvider>
          <ApiProvider>{children}</ApiProvider>
        </AuthProvider>
      </BrowserRouter>
    </ThemeProvider>
  );
}
