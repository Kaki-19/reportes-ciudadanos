import type { NavigatorScreenParams } from '@react-navigation/native';

export type AuthStackParamList = {
  Login: undefined;
  Register: undefined;
};

export type AppTabsParamList = {
  Mapa: undefined;
  MisReportes: undefined;
  NuevoReporte: undefined;
  Perfil: undefined;
};

export type AdminTabsParamList = {
  TodosReportes: undefined;
  Categorias: undefined;
  PerfilAdmin: undefined;
};

// En las semanas 3 y 4 se agregan aquí: DetalleReporte, EditarReporte, DetalleAdmin, FormCategoria
export type AdminStackParamList = {
  AdminTabs: NavigatorScreenParams<AdminTabsParamList>;
};