import { StyleSheet, Text, View } from 'react-native';
import { PrimaryButton } from '../components/PrimaryButton';
import { colors, radius, spacing } from '../constants/theme';
import { useAuthStore } from '../store/authStore';

/** Perfil compartido por ciudadano y administrador (RF-02: cerrar sesión) */
export function ProfileScreen() {
  const user = useAuthStore((s) => s.user);
  const logout = useAuthStore((s) => s.logout);

  return (
    <View style={styles.container}>
      <View style={styles.card}>
        <Text style={styles.name}>{user?.nombre}</Text>
        <Text style={styles.email}>{user?.correo}</Text>
        <View style={styles.badge}>
          <Text style={styles.badgeText}>{user?.rol}</Text>
        </View>
      </View>
      <PrimaryButton title="Cerrar sesión" variant="outline" onPress={() => void logout()} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: colors.background, padding: spacing.lg },
  card: { backgroundColor: colors.surface, borderRadius: radius.lg, padding: spacing.lg, marginBottom: spacing.lg, alignItems: 'center' },
  name: { fontSize: 22, fontWeight: '700', color: colors.text },
  email: { fontSize: 15, color: colors.textMuted, marginTop: spacing.xs },
  badge: { marginTop: spacing.md, backgroundColor: colors.primaryLight, paddingHorizontal: spacing.md, paddingVertical: spacing.xs, borderRadius: 999 },
  badgeText: { color: colors.primary, fontWeight: '700' },
});