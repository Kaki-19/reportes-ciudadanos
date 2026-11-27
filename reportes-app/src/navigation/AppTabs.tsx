import Ionicons from '@expo/vector-icons/Ionicons';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { colors } from '../constants/theme';
import { MapScreen } from '../screens/reports/MapScreen';
import { MyReportsScreen } from '../screens/reports/MyReportsScreen';
import { NewReportScreen } from '../screens/reports/NewReportScreen';
import { ProfileScreen } from '../screens/ProfileScreen';
import type { AppTabsParamList } from './types';

const Tab = createBottomTabNavigator<AppTabsParamList>();

const ICONOS: Record<keyof AppTabsParamList, keyof typeof Ionicons.glyphMap> = {
  Mapa: 'map-outline',
  MisReportes: 'list-outline',
  NuevoReporte: 'add-circle-outline',
  Perfil: 'person-outline',
};

/** Navegación del rol Ciudadano */
export function AppTabs() {
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarActiveTintColor: colors.primary,
        tabBarIcon: ({ color, size }) => <Ionicons name={ICONOS[route.name]} size={size} color={color} />,
      })}
    >
      <Tab.Screen name="Mapa" component={MapScreen} />
      <Tab.Screen name="MisReportes" component={MyReportsScreen} options={{ title: 'Mis reportes' }} />
      <Tab.Screen name="NuevoReporte" component={NewReportScreen} options={{ title: 'Nuevo reporte' }} />
      <Tab.Screen name="Perfil" component={ProfileScreen} />
    </Tab.Navigator>
  );
}
