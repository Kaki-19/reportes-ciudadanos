import { useCallback, useState } from 'react';
import { ActivityIndicator, FlatList, RefreshControl, StyleSheet, Text, View } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { colors, radius, spacing } from '../../constants/theme';
import { getErrorMessage } from '../../services/api';
import { categoryService } from '../../services/categoryService';
import type { Category } from '../../types';

/**
 * Listado de categorías para el administrador (incluye inactivas).
 * Sirve para comprobar que el JWT y el rol Administrador funcionan contra la API.
 * El formulario de crear/editar se agrega en la semana 4.
 */
export function CategoriesScreen() {
  const [data, setData] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const load = useCallback(async () => {
    setError(null);
    try {
      setData(await categoryService.getAll(true));
    } catch (e) {
      setError(getErrorMessage(e));
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      void load();
    }, [load]),
  );

  if (loading) return <ActivityIndicator style={styles.center} size="large" color={colors.primary} />;

  return (
    <FlatList
      style={styles.list}
      contentContainerStyle={{ padding: spacing.md }}
      data={data}
      keyExtractor={(c) => String(c.id)}
      refreshControl={<RefreshControl refreshing={false} onRefresh={load} />}
      ListHeaderComponent={error ? <Text style={styles.error}>{error}</Text> : undefined}
      ListEmptyComponent={!error ? <Text style={styles.empty}>No hay categorías</Text> : undefined}
      renderItem={({ item }) => (
        <View style={styles.card}>
          <View style={{ flex: 1 }}>
            <Text style={styles.name}>{item.nombre}</Text>
            {item.descripcion ? <Text style={styles.desc}>{item.descripcion}</Text> : null}
          </View>
          <Text style={[styles.chip, item.activa ? styles.active : styles.inactive]}>
            {item.activa ? 'Activa' : 'Inactiva'}
          </Text>
        </View>
      )}
    />
  );
}

const styles = StyleSheet.create({
  center: { flex: 1 },
  list: { flex: 1, backgroundColor: colors.background },
  card: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: colors.surface,
    borderRadius: radius.md,
    padding: spacing.md,
    marginBottom: spacing.sm,
  },
  name: { fontSize: 16, fontWeight: '700', color: colors.text },
  desc: { fontSize: 13, color: colors.textMuted, marginTop: 2 },
  chip: { fontSize: 12, fontWeight: '700', paddingHorizontal: 10, paddingVertical: 4, borderRadius: 999, overflow: 'hidden' },
  active: { backgroundColor: '#DCFCE7', color: colors.success },
  inactive: { backgroundColor: '#FEE2E2', color: colors.error },
  error: { color: colors.error, marginBottom: spacing.md },
  empty: { textAlign: 'center', color: colors.textMuted, marginTop: spacing.xl },
});