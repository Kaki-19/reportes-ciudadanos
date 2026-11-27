import { useEffect } from 'react';
import { ActivityIndicator, StyleSheet, View } from 'react-native';
import { colors } from '../constants/theme';
import { useAuthStore } from '../store/authStore';
import { AdminStack } from './AdminStack';
import { AppTabs } from './AppTabs';
import { AuthStack } from './AuthStack';

/**
 * Decide qué flujo mostrar según la sesión y el rol:
 *  - sin sesión  -> AuthStack (Login / Registro)
 *  - Ciudadano   -> AppTabs
 *  - Administrador -> AdminStack
 */
export function RootNavigator() {
  const status = useAuthStore((s) => s.status);
  const rol = useAuthStore((s) => s.user?.rol);
  const restoreSession = useAuthStore((s) => s.restoreSession);

  useEffect(() => {
    void restoreSession();
  }, [restoreSession]);

  if (status === 'loading') {
    return (
      <View style={styles.splash}>
        <ActivityIndicator size="large" color={colors.primary} />
      </View>
    );
  }

  if (status !== 'authenticated') return <AuthStack />;
  return rol === 'Administrador' ? <AdminStack /> : <AppTabs />;
}

const styles = StyleSheet.create({
  splash: { flex: 1, alignItems: 'center', justifyContent: 'center', backgroundColor: colors.background },
});