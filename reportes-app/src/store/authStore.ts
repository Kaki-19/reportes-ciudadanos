import { create } from 'zustand';
import * as SecureStore from 'expo-secure-store';
import { TOKEN_KEY } from '../constants/config';
import { configureApi } from '../services/api';
import { authService } from '../services/authService';
import type { LoginRequest, RegisterRequest, User } from '../types';

type AuthStatus = 'loading' | 'authenticated' | 'unauthenticated';

interface AuthState {
  token: string | null;
  user: User | null;
  status: AuthStatus;
  login: (data: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => Promise<void>;
  restoreSession: () => Promise<void>;
}

export const useAuthStore = create<AuthState>((set, get) => ({
  token: null,
  user: null,
  status: 'loading',

  login: async (data) => {
    const res = await authService.login(data);
    await SecureStore.setItemAsync(TOKEN_KEY, res.token);
    set({ token: res.token, user: res.usuario, status: 'authenticated' });
  },

  register: async (data) => {
    const res = await authService.register(data);
    await SecureStore.setItemAsync(TOKEN_KEY, res.token);
    set({ token: res.token, user: res.usuario, status: 'authenticated' });
  },

  logout: async () => {
    await SecureStore.deleteItemAsync(TOKEN_KEY);
    set({ token: null, user: null, status: 'unauthenticated' });
  },

  // Al abrir la app: si hay token guardado, lo validamos con /auth/me
  restoreSession: async () => {
    try {
      const token = await SecureStore.getItemAsync(TOKEN_KEY);
      if (!token) {
        set({ status: 'unauthenticated' });
        return;
      }
      set({ token });
      const user = await authService.me();
      set({ user, status: 'authenticated' });
    } catch {
      await get().logout();
    }
  },
}));

// Conectamos el store con Axios
configureApi({
  getToken: () => useAuthStore.getState().token,
  onUnauthorized: () => {
    void useAuthStore.getState().logout();
  },
});