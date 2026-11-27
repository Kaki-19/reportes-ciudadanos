import { z } from 'zod';

// Mismas reglas que las Data Annotations del backend (RNF-06: validar en cliente y servidor)

export const loginSchema = z.object({
  correo: z.string().trim().min(1, 'El correo es obligatorio').email('Correo no válido'),
  password: z.string().min(1, 'La contraseña es obligatoria'),
});

export const registerSchema = z
  .object({
    nombre: z.string().trim().min(2, 'Mínimo 2 caracteres').max(100, 'Máximo 100 caracteres'),
    correo: z.string().trim().min(1, 'El correo es obligatorio').email('Correo no válido').max(150),
    password: z.string().min(8, 'Mínimo 8 caracteres').max(64, 'Máximo 64 caracteres'),
    confirmPassword: z.string().min(1, 'Confirma tu contraseña'),
  })
  .refine((d) => d.password === d.confirmPassword, {
    message: 'Las contraseñas no coinciden',
    path: ['confirmPassword'],
  });

export type LoginForm = z.infer<typeof loginSchema>;
export type RegisterForm = z.infer<typeof registerSchema>;