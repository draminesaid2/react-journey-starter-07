
import React, { useState, useEffect } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  TextInput, 
  TouchableOpacity, 
  Image,
  SafeAreaView,
  KeyboardAvoidingView,
  Platform,
  ScrollView,
  StatusBar,
  useWindowDimensions,
  ActivityIndicator,
  Pressable
} from 'react-native';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE } from '../theme/typography';
import * as Mixins from '../theme/mixins';
import { useTranslation } from 'react-i18next';
import * as Animatable from 'react-native-animatable';
import { useSignIn, useSignUp, useOAuth } from '@clerk/clerk-expo';

export default function LoginScreen({ navigation }) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const { t } = useTranslation();
  const { width, height } = useWindowDimensions();
  const isSmallScreen = height < 700;

  const { signIn, setActive, isLoaded: isSignInLoaded } = useSignIn();
  const { signUp, isLoaded: isSignUpLoaded } = useSignUp();
  const { startOAuthFlow } = useOAuth({ strategy: "oauth_google" });

  const handleLogin = async () => {
    // Admin bypass - check first
    if (email.toLowerCase() === 'admin') {
      console.log('Admin login detected, navigating to AdminDashboard');
      navigation.navigate('AdminDashboard');
      return;
    }
    
    // Provider bypass - check second
    if (email.toLowerCase() === 'prestataire') {
      console.log('Provider login detected, navigating to ProviderDashboard');
      navigation.navigate('ProviderDashboard');
      return;
    }
    
    // Regular user Clerk login
    if (!email || !password) {
      setErrorMessage('Veuillez entrer votre email et mot de passe');
      return;
    }

    try {
      setIsLoading(true);
      setErrorMessage('');
      
      const { createdSessionId, status, firstFactorVerification } = await signIn.create({
        identifier: email,
        password,
      });

      if (firstFactorVerification.status === 'needs_second_factor') {
        // Handle 2FA if implemented
        console.log('2FA needed');
      } else {
        if (status === 'complete') {
          await setActive({ session: createdSessionId });
          navigation.navigate('Map');
        }
      }
    } catch (err) {
      console.error('Error during login:', err);
      setErrorMessage(err.errors?.[0]?.message || 'Une erreur est survenue lors de la connexion');
    } finally {
      setIsLoading(false);
    }
  };

  const handleGoogleLogin = async () => {
    try {
      setIsLoading(true);
      setErrorMessage('');
      
      const { createdSessionId, setActive } = await startOAuthFlow();
      
      if (createdSessionId) {
        await setActive({ session: createdSessionId });
        navigation.navigate('Map');
      }
    } catch (err) {
      console.error('Error during Google OAuth:', err);
      setErrorMessage('Erreur lors de la connexion avec Google');
    } finally {
      setIsLoading(false);
    }
  };

  if (!isSignInLoaded || !isSignUpLoaded) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color={COLORS.primary} />
      </View>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor={COLORS.primary} barStyle="light-content" />
      <KeyboardAvoidingView 
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
        style={styles.container}
      >
        <ScrollView 
          contentContainerStyle={styles.scrollContainer}
          bounces={false}
        >
          <Animatable.View 
            animation="fadeInDown" 
            duration={1000} 
            style={[
              styles.header,
              { paddingTop: Platform.OS === 'ios' ? (isSmallScreen ? SPACING.xl : SPACING.xxl) : SPACING.xxl * 1.5 }
            ]}
          >
            <Text style={styles.title}>{t('login.welcome')}</Text>
            <Text style={styles.subtitle}>{t('login.subtitle')}</Text>
          </Animatable.View>

          <Animatable.View 
            animation="fadeInUp" 
            duration={1000} 
            delay={300}
            style={styles.formContainer}
          >
            {errorMessage ? (
              <Animatable.View 
                animation="shake" 
                style={styles.errorContainer}
              >
                <Text style={styles.errorText}>{errorMessage}</Text>
              </Animatable.View>
            ) : null}
            
            <TextInput 
              style={styles.input}
              placeholder={t('login.email')}
              placeholderTextColor={COLORS.gray}
              keyboardType="email-address"
              autoCapitalize="none"
              value={email}
              onChangeText={setEmail}
            />
            
            <View style={styles.passwordContainer}>
              <TextInput 
                style={styles.passwordInput}
                placeholder={t('login.password')}
                placeholderTextColor={COLORS.gray}
                secureTextEntry={!showPassword}
                value={password}
                onChangeText={setPassword}
              />
              <Pressable 
                style={styles.eyeIconContainer} 
                onPress={() => setShowPassword(!showPassword)}
              >
                <Text style={styles.eyeIcon}>{showPassword ? '👁️' : '👁️‍🗨️'}</Text>
              </Pressable>
            </View>
            
            <TouchableOpacity 
              style={[styles.loginButton, isLoading && styles.loginButtonDisabled]}
              onPress={handleLogin}
              disabled={isLoading}
            >
              {isLoading ? (
                <ActivityIndicator size="small" color={COLORS.white} />
              ) : (
                <Text style={styles.loginButtonText}>{t('login.login')}</Text>
              )}
            </TouchableOpacity>

            <View style={styles.divider}>
              <View style={styles.dividerLine} />
              <Text style={styles.dividerText}>{t('login.or')}</Text>
              <View style={styles.dividerLine} />
            </View>

            <TouchableOpacity 
              style={[styles.googleButton, isLoading && styles.googleButtonDisabled]}
              onPress={handleGoogleLogin}
              disabled={isLoading}
            >
              <Image 
                source={{ uri: 'https://developers.google.com/identity/images/g-logo.png' }}
                style={styles.googleIcon}
              />
              <Text style={styles.googleButtonText}>{t('login.googleLogin')}</Text>
            </TouchableOpacity>

            <TouchableOpacity 
              style={styles.forgotPassword}
              onPress={() => {}}
            >
              <Text style={styles.forgotPasswordText}>
                {t('login.forgotPassword')}
              </Text>
            </TouchableOpacity>

            <View style={styles.signupContainer}>
              <Text style={styles.signupText}>
                {t('login.noAccount')}
              </Text>
              <TouchableOpacity onPress={() => navigation.navigate('Signup')}>
                <Text style={styles.signupLink}>
                  {t('login.signup')}
                </Text>
              </TouchableOpacity>
            </View>
          </Animatable.View>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: COLORS.white,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: COLORS.white,
  },
  scrollContainer: {
    flexGrow: 1,
  },
  header: {
    backgroundColor: COLORS.primary,
    paddingBottom: SPACING.xl,
    borderBottomLeftRadius: 30,
    borderBottomRightRadius: 30,
    justifyContent: 'center',
    paddingHorizontal: SPACING.lg,
    minHeight: 150,
  },
  title: {
    fontSize: FONT_SIZE.xxl,
    color: COLORS.white,
    fontWeight: 'bold',
    marginBottom: SPACING.sm,
    textShadowColor: 'rgba(0, 0, 0, 0.3)',
    textShadowOffset: { width: 1, height: 1 },
    textShadowRadius: 2,
  },
  subtitle: {
    fontSize: FONT_SIZE.md,
    color: COLORS.white,
    opacity: 0.9,
    textShadowColor: 'rgba(0, 0, 0, 0.2)',
    textShadowOffset: { width: 0.5, height: 0.5 },
    textShadowRadius: 1,
  },
  formContainer: {
    flex: 1,
    paddingHorizontal: SPACING.lg,
    paddingTop: SPACING.xl,
  },
  errorContainer: {
    backgroundColor: COLORS.error_light,
    borderRadius: 10,
    padding: SPACING.md,
    marginBottom: SPACING.md,
    borderLeftWidth: 4,
    borderLeftColor: COLORS.error,
  },
  errorText: {
    color: COLORS.error,
    fontSize: FONT_SIZE.sm,
  },
  input: {
    backgroundColor: COLORS.light_gray,
    borderRadius: 10,
    padding: SPACING.md,
    marginBottom: SPACING.md,
    fontSize: FONT_SIZE.md,
    borderWidth: 1,
    borderColor: COLORS.light_gray,
  },
  passwordContainer: {
    flexDirection: 'row',
    backgroundColor: COLORS.light_gray,
    borderRadius: 10,
    borderWidth: 1,
    borderColor: COLORS.light_gray,
    marginBottom: SPACING.md,
    alignItems: 'center',
  },
  passwordInput: {
    flex: 1,
    padding: SPACING.md,
    fontSize: FONT_SIZE.md,
    backgroundColor: 'transparent',
  },
  eyeIconContainer: {
    padding: SPACING.md,
  },
  eyeIcon: {
    fontSize: FONT_SIZE.lg,
  },
  loginButton: {
    backgroundColor: COLORS.primary,
    padding: SPACING.md,
    borderRadius: 10,
    alignItems: 'center',
    marginTop: SPACING.md,
    ...Mixins.boxShadow,
  },
  loginButtonDisabled: {
    backgroundColor: COLORS.primary_light,
    opacity: 0.7,
  },
  loginButtonText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.md,
    fontWeight: 'bold',
  },
  divider: {
    flexDirection: 'row',
    alignItems: 'center',
    marginVertical: SPACING.lg,
  },
  dividerLine: {
    flex: 1,
    height: 1,
    backgroundColor: COLORS.light_gray,
  },
  dividerText: {
    color: COLORS.gray,
    paddingHorizontal: SPACING.md,
    fontSize: FONT_SIZE.sm,
  },
  googleButton: {
    flexDirection: 'row',
    backgroundColor: COLORS.white,
    padding: SPACING.md,
    borderRadius: 10,
    alignItems: 'center',
    justifyContent: 'center',
    borderWidth: 1,
    borderColor: COLORS.light_gray,
    ...Mixins.boxShadow,
  },
  googleButtonDisabled: {
    opacity: 0.7,
  },
  googleIcon: {
    width: 24,
    height: 24,
    marginRight: SPACING.sm,
  },
  googleButtonText: {
    color: COLORS.black,
    fontSize: FONT_SIZE.md,
  },
  forgotPassword: {
    alignItems: 'center',
    marginTop: SPACING.lg,
  },
  forgotPasswordText: {
    color: COLORS.primary,
    fontSize: FONT_SIZE.sm,
  },
  signupContainer: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: SPACING.xl,
    paddingBottom: SPACING.xl,
  },
  signupText: {
    color: COLORS.gray,
    fontSize: FONT_SIZE.sm,
  },
  signupLink: {
    color: COLORS.primary,
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
    marginLeft: SPACING.xs,
  },
});
