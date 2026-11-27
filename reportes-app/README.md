# Reporte Ciudadano – App móvil (React Native + Expo)

Expo SDK 58 · TypeScript · React Navigation 7 · Zustand · Axios · React Hook Form + Zod · expo-secure-store

## Correr en desarrollo

```bash
npm install
cp .env.example .env      # en Windows: copy .env.example .env
# edita .env y pon la IP de la computadora donde corre la API (ipconfig / ifconfig)
npx expo start
```

Abre **Expo Go** en el celular (misma red Wi-Fi que la computadora) y escanea el QR.

> No uses `localhost` en la URL: desde el celular, `localhost` es el propio celular.

## Avance semana 2
- Navegación completa: `RootNavigator` decide entre `AuthStack`, `AppTabs` (Ciudadano) y `AdminStack` (Administrador).
- Login y registro conectados a la API, con validación en cliente (Zod).
- Token guardado en SecureStore; la sesión se restaura al abrir la app (`/auth/me`).
- Interceptor de Axios que agrega `Authorization: Bearer <token>` y cierra sesión si la API responde 401.
- Perfil con cierre de sesión. Listado de categorías para el administrador.