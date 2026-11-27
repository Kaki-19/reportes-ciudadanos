// Los tipos reflejan los DTOs de la API (ReportesApi/DTOs)

export type Rol = 'Ciudadano' | 'Administrador';

export interface User {
  id: number;
  nombre: string;
  correo: string;
  rol: Rol;
}

export interface AuthResponse {
  token: string;
  expiraEn: string;
  usuario: User;
}

export interface LoginRequest {
  correo: string;
  password: string;
}

export interface RegisterRequest {
  nombre: string;
  correo: string;
  password: string;
}

export interface Category {
  id: number;
  nombre: string;
  descripcion: string | null;
  activa: boolean;
}

export interface Status {
  id: number;
  nombre: string;
  color: string;
}

/** Formato estándar de error que devuelve la API */
export interface ApiError {
  success: false;
  message: string;
  errors: string[];
}
