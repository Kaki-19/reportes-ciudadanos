import axios, { AxiosError } from 'axios';
import { API_URL, REQUEST_TIMEOUT_MS } from '../constants/config';
import type { ApiError } from '../types';

export const api = axios.create({
  baseURL: API_URL,
  timeout: REQUEST_TIMEOUT_MS,
  headers: { 'Content-Type': 'application/json' },
});

// El store registra aquí cómo obtener el token y qué hacer al expirar la sesión.
// Así api.ts no importa el store (evita dependencias circulares).
let getToken: () => string | null = () => null;
let onUnauthorized: () => void = () => {};

export function configureApi(options: { getToken: () => string | null; onUnauthorized: () => void }) {
  getToken = options.getToken;
  onUnauthorized = options.onUnauthorized;
}

// Interceptor de petición: agrega el JWT
api.interceptors.request.use((config) => {
  const token = getToken();
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// Interceptor de respuesta: si el token expiró (401) cerramos la sesión
api.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiError>) => {
    const url = error.config?.url ?? '';
    const esLoginORegistro = url.includes('/auth/login') || url.includes('/auth/register');
    if (error.response?.status === 401 && !esLoginORegistro) onUnauthorized();
    return Promise.reject(error);
  },
);

/** Convierte cualquier error de Axios en un mensaje legible para el usuario */
export function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError<ApiError>(error)) {
    if (!error.response) {
      return 'No se pudo conectar con el servidor. Revisa tu conexión y la URL de la API.';
    }
    const data = error.response.data;
    if (data?.errors?.length) return `${data.message}: ${data.errors.join(', ')}`;
    if (data?.message) return data.message;
  }
  return 'Ocurrió un error inesperado';
}