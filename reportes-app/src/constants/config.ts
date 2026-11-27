/**
 * URL base de la API.
 * En el celular (Expo Go) NO funciona "localhost": usa la IP de tu computadora en la red local.
 * Puedes cambiarla aquí o crear un archivo .env con:
 *   EXPO_PUBLIC_API_URL=http://192.168.1.50:5000/api
 */
export const API_URL = process.env.EXPO_PUBLIC_API_URL ?? 'http://192.168.1.50:5000/api';

export const TOKEN_KEY = 'reportes_token';
export const REQUEST_TIMEOUT_MS = 10000;