import { useState } from 'react';
import { KeyboardAvoidingView, Platform, ScrollView, StyleSheet, Text, View } from 'react-native';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import type { NativeStackScreenProps } from '@react-navigation/native-stack';
import { FormInput } from '../../components/FormInput';
import { PrimaryButton } from '../../components/PrimaryButton';
import { colors, spacing } from '../../constants/theme';
import type { AuthStackParamList } from '../../navigation/types';
import { getErrorMessage } from '../../services/api';
import { useAuthStore } from '../../store/authStore';
import { LoginForm, loginSchema } from '../../utils/validators';

type Props = NativeStackScreenProps<AuthStackParamList, 'Login'>;

export function LoginScreen({ navigation }: Props) {
  const login = useAuthStore((s) => s.login);
  const [serverError, setServerError] = useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
    defaultValues: { correo: '', password: '' },
  });

  const onSubmit = async (data: LoginForm) => {
    setServerError(null);
    try {
      await login(data);
      // No navegamos manualmente: el RootNavigator cambia de stack al detectar la sesión
    } catch (e) {
      setServerError(getErrorMessage(e));
    }
  };

  return (
    <KeyboardAvoidingView style={styles.flex} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <ScrollView contentContainerStyle={styles.container} keyboardShouldPersistTaps="handled">
        <View style={styles.header}>
          <Text style={styles.title}>Reporte Ciudadano</Text>
          <Text style={styles.subtitle}>Inicia sesión para reportar problemas de tu ciudad</Text>
        </View>

        <FormInput
          control={control}
          name="correo"
          label="Correo electrónico"
          placeholder="tucorreo@ejemplo.com"
          keyboardType="email-address"
          autoCapitalize="none"
          autoComplete="email"
          error={errors.correo?.message}
        />
        <FormInput
          control={control}
          name="password"
          label="Contraseña"
          placeholder="••••••••"
          secureTextEntry
          error={errors.password?.message}
        />

        {serverError ? <Text style={styles.serverError}>{serverError}</Text> : null}

        <PrimaryButton title="Iniciar sesión" onPress={handleSubmit(onSubmit)} loading={isSubmitting} />
        <PrimaryButton title="Crear una cuenta" variant="outline" onPress={() => navigation.navigate('Register')} />
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  flex: { flex: 1, backgroundColor: colors.background },
  container: { flexGrow: 1, justifyContent: 'center', padding: spacing.lg },
  header: { marginBottom: spacing.xl },
  title: { fontSize: 28, fontWeight: '800', color: colors.primary },
  subtitle: { fontSize: 15, color: colors.textMuted, marginTop: spacing.sm },
  serverError: { color: colors.error, textAlign: 'center', marginBottom: spacing.sm },
});