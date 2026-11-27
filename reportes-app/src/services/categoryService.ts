import { api } from './api';
import type { Category } from '../types';

export const categoryService = {
  async getAll(incluirInactivas = false): Promise<Category[]> {
    const res = await api.get<Category[]>('/categorias', { params: { incluirInactivas } });
    return res.data;
  },

  async create(data: { nombre: string; descripcion?: string }): Promise<Category> {
    const res = await api.post<Category>('/categorias', data);
    return res.data;
  },

  async update(id: number, data: { nombre: string; descripcion?: string; activa: boolean }): Promise<Category> {
    const res = await api.put<Category>(`/categorias/${id}`, data);
    return res.data;
  },
};