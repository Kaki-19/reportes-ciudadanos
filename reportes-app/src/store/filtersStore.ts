import { create } from 'zustand';

// Filtros del listado de reportes (se usan a partir de la semana 3)
interface FiltersState {
  categoriaId: number | null;
  estadoId: number | null;
  setCategoria: (id: number | null) => void;
  setEstado: (id: number | null) => void;
  reset: () => void;
}

export const useFiltersStore = create<FiltersState>((set) => ({
  categoriaId: null,
  estadoId: null,
  setCategoria: (categoriaId) => set({ categoriaId }),
  setEstado: (estadoId) => set({ estadoId }),
  reset: () => set({ categoriaId: null, estadoId: null }),
}));