import Ionicons from '@expo/vector-icons/Ionicons';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { colors } from '../constants/theme';
import { AdminReportsScreen } from '../screens/admin/AdminReportsScreen';
import { CategoriesScreen } from '../screens/admin/CategoriesScreen';
import { ProfileScreen } from '../screens/ProfileScreen';
import type { AdminStackParamList, AdminTabsParamList } from './types';

const Stack = createNativeStackNavigator<AdminStackParamList>();
const Tab = createBottomTabNavigator<AdminTabsParamList>();

const ICONOS: Record<keyof AdminTabsParamList, keyof typeof Ionicons.glyphMap> = {
  TodosReportes: 'document-text-outline',
  Categorias: 'pricetags-outline',
  PerfilAdmin: 'person-outline',
};

function AdminTabs() {
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarActiveTintColor: colors.primary,
        tabBarIcon: ({ color, size }) => <Ionicons name={ICONOS[route.name]} size={size} color={color} />,
      })}
    >
      <Tab.Screen name="TodosReportes" component={AdminReportsScreen} options={{ title: 'Reportes' }} />
      <Tab.Screen name="Categorias" component={CategoriesScreen} options={{ title: 'Categorías' }} />
      <Tab.Screen name="PerfilAdmin" component={ProfileScreen} options={{ title: 'Perfil' }} />
    </Tab.Navigator>
  );
}

/** Navegación del rol Administrador: stack cuya raíz son las pestañas de administración */
export function AdminStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="AdminTabs" component={AdminTabs} options={{ headerShown: false }} />
    </Stack.Navigator>
  );
}
