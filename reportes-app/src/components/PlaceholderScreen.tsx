import { StyleSheet, Text, View } from 'react-native';
import { colors, spacing } from '../constants/theme';

/** Pantalla temporal para las secciones que se desarrollan en semanas posteriores */
export function PlaceholderScreen({ title, semana }: { title: string; semana: number }) {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>{title}</Text>
      <Text style={styles.subtitle}>Se desarrolla en la semana {semana}</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, alignItems: 'center', justifyContent: 'center', backgroundColor: colors.background, padding: spacing.lg },
  title: { fontSize: 20, fontWeight: '700', color: colors.text },
  subtitle: { fontSize: 14, color: colors.textMuted, marginTop: spacing.sm },
});