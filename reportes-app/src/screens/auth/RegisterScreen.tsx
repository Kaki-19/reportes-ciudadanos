import { useState } from 'react';
import { KeyboardAvoidingView, Platform, ScrollView, StyleSheet, Text } from 'react-native';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import type { NativeStackScreenProps } from '@react-navigation/native-stack';
import { FormInput } from '../../components/FormInput';
import { PrimaryButton } from '../../components/PrimaryButton';
import { colors, spacing } from '../../constants/theme';
import type { AuthStackParamList } from '../../navigation/types';
import { getErrorMessage } from '../../services/api';
import { useAuthStore } from '../../store/authStore';
import { RegisterForm, registerSchema } from '../../utils/validators';

type Props = NativeStackScreenProps<AuthStackParamList, 'Register'>;

export function RegisterScreen({ navigation }: Props) {
  const register = useAuthStore((s) => s.register);
  const [serverError, setServerError] = useState<string | null>(null);

  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterForm>({
    resolver: zodResolver(registerSchema),
    defaultValues: { nombre: '', correo: '', password: '', confirmPassword: '' },
  });

  const onSubmit = async ({ nombre, correo, password }: RegisterForm) => {
    setServerError(null);
    try {
      await register({ nombre, correo, password }); // la API devuelve el token: queda con sesión iniciada
    } catch (e) {
      setServerError(getErrorMessage(e));
    }
  };

  return (
    <KeyboardAvoidingView style={styles.flex} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <ScrollView contentContainerStyle={styles.container} keyboardShouldPersistTaps="handled">
        <Text style={styles.title}>Crear cuenta</Text>

        <FormInput control={control} name="nombre" label="Nombre completo" placeholder="Tu nombre" error={errors.nombre?.message} />
        <FormInput
          control={control}
          name="correo"
          label="Correo electrónico"
          placeholder="tucorreo@ejemplo.com"
          keyboardType="email-address"
          autoCapitalize="none"
          error={errors.correo?.message}
        />
        <FormInput
          control={control}
          name="password"
          label="Contraseña"
          placeholder="Mínimo 8 caracteres"
          secureTextEntry
          error={errors.password?.message}
        />
        <FormInput
          control={control}
          name="confirmPassword"
          label="Confirmar contraseña"
          placeholder="Repite tu contraseña"
          secureTextEntry
          error={errors.confirmPassword?.message}
        />

        {serverError ? <Text style={styles.serverError}>{serverError}</Text> : null}

        <PrimaryButton title="Registrarme" onPress={handleSubmit(onSubmit)} loading={isSubmitting} />
        <PrimaryButton title="Ya tengo cuenta" variant="outline" onPress={() => navigation.goBack()} />
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  flex: { flex: 1, backgroundColor: colors.background },
  container: { flexGrow: 1, justifyContent: 'center', padding: spacing.lg },
  title: { fontSize: 26, fontWeight: '800', color: colors.primary, marginBottom: spacing.lg },
  serverError: { color: colors.error, textAlign: 'center', marginBottom: spacing.sm },
});