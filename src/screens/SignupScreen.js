
import React, { useState } from 'react';
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
  ActivityIndicator,
  Pressable,
  Alert
} from 'react-native';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE } from '../theme/typography';
import { boxShadow } from '../theme/mixins';
import { useTranslation } from 'react-i18next';
import * as Animatable from 'react-native-animatable';
import { useSignUp, useOAuth } from '@clerk/clerk-expo';

export default function SignupScreen({ navigation }) {
  const [currentStep, setCurrentStep] = useState(0);
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [phone, setPhone] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const { t } = useTranslation();

  const { isLoaded, signUp, setActive } = useSignUp();
  const { startOAuthFlow } = useOAuth({ strategy: "oauth_google" });

  const steps = [
    {
      title: 'Informations personnelles',
      subtitle: 'Veuillez entrer vos informations personnelles.',
      fields: (
        <>
          <TextInput 
            style={styles.input}
            placeholder="Prénom"
            placeholderTextColor={COLORS.gray}
            value={firstName}
            onChangeText={setFirstName}
          />
          <TextInput 
            style={styles.input}
            placeholder="Nom"
            placeholderTextColor={COLORS.gray}
            value={lastName}
            onChangeText={setLastName}
          />
        </>
      )
    },
    {
      title: 'Coordonnées',
      subtitle: 'Veuillez entrer vos coordonnées.',
      fields: (
        <>
          <TextInput 
            style={styles.input}
            placeholder="E-mail"
            placeholderTextColor={COLORS.gray}
            keyboardType="email-address"
            autoCapitalize="none"
            value={email}
            onChangeText={setEmail}
          />
          <TextInput 
            style={styles.input}
            placeholder="Téléphone"
            placeholderTextColor={COLORS.gray}
            keyboardType="phone-pad"
            value={phone}
            onChangeText={setPhone}
          />
        </>
      )
    },
    {
      title: 'Sécurité',
      subtitle: 'Définissez un mot de passe sécurisé.',
      fields: (
        <>
          <View style={styles.passwordContainer}>
            <TextInput 
              style={styles.passwordInput}
              placeholder="Mot de passe"
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
          
          <View style={styles.passwordContainer}>
            <TextInput 
              style={styles.passwordInput}
              placeholder="Confirmer le mot de passe"
              placeholderTextColor={COLORS.gray}
              secureTextEntry={!showConfirmPassword}
              value={confirmPassword}
              onChangeText={setConfirmPassword}
            />
            <Pressable 
              style={styles.eyeIconContainer} 
              onPress={() => setShowConfirmPassword(!showConfirmPassword)}
            >
              <Text style={styles.eyeIcon}>{showConfirmPassword ? '👁️' : '👁️‍🗨️'}</Text>
            </Pressable>
          </View>
          
          <Text style={styles.passwordRequirements}>
            Le mot de passe doit contenir au moins 6 caractères, une majuscule, une minuscule et un chiffre.
          </Text>
        </>
      )
    }
  ];

  const validatePassword = (password) => {
    const hasMinLength = password.length >= 6;
    const hasUpperCase = /[A-Z]/.test(password);
    const hasLowerCase = /[a-z]/.test(password);
    const hasNumber = /[0-9]/.test(password);
    
    return hasMinLength && hasUpperCase && hasLowerCase && hasNumber;
  };

  const validateCurrentStep = () => {
    setErrorMessage('');
    switch(currentStep) {
      case 0:
        if (!firstName || !lastName) {
          setErrorMessage('Veuillez entrer votre prénom et nom');
          return false;
        }
        break;
      case 1:
        if (!email) {
          setErrorMessage('Veuillez entrer votre email');
          return false;
        }
        // Simple email validation
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
          setErrorMessage('Veuillez entrer un email valide');
          return false;
        }
        break;
      case 2:
        if (!password) {
          setErrorMessage('Veuillez entrer un mot de passe');
          return false;
        }
        if (!validatePassword(password)) {
          setErrorMessage('Le mot de passe doit contenir au moins 6 caractères, une majuscule, une minuscule et un chiffre');
          return false;
        }
        if (password !== confirmPassword) {
          setErrorMessage('Les mots de passe ne correspondent pas');
          return false;
        }
        break;
    }
    return true;
  };

  const handleNext = () => {
    if (!validateCurrentStep()) return;

    if (currentStep < steps.length - 1) {
      setCurrentStep(prev => prev + 1);
    } else {
      handleSignup();
    }
  };

  const handleBack = () => {
    setErrorMessage('');
    if (currentStep > 0) {
      setCurrentStep(prev => prev - 1);
    } else {
      navigation.goBack();
    }
  };

  const handleSignup = async () => {
    try {
      setIsLoading(true);
      
      if (!isLoaded) {
        throw new Error("Clerk not loaded");
      }

      await signUp.create({
        firstName,
        lastName,
        emailAddress: email,
        password,
      });

      await signUp.prepareEmailAddressVerification({ strategy: 'email_code' });

      // For this simple version, we'll skip email verification and go straight to the map
      await setActive({ session: signUp.createdSessionId });
      navigation.navigate('Map');
      
    } catch (err) {
      console.error('Error signing up:', err);
      setErrorMessage(err.errors?.[0]?.message || 'Une erreur est survenue lors de l\'inscription');
    } finally {
      setIsLoading(false);
    }
  };

  const handleGoogleSignup = async () => {
    try {
      setIsLoading(true);
      const { createdSessionId, setActive } = await startOAuthFlow();
      
      if (createdSessionId) {
        await setActive({ session: createdSessionId });
        navigation.navigate('Map');
      }
    } catch (err) {
      console.error('Error during Google OAuth:', err);
      setErrorMessage('Erreur lors de l\'inscription avec Google');
    } finally {
      setIsLoading(false);
    }
  };

  if (!isLoaded) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color={COLORS.primary} />
      </View>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <KeyboardAvoidingView 
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
        style={styles.container}
      >
        <ScrollView contentContainerStyle={styles.scrollContainer}>
          <Animatable.View 
            animation="fadeInDown" 
            duration={1000} 
            style={styles.header}
          >
            <TouchableOpacity 
              style={styles.backButton}
              onPress={handleBack}
            >
              <Text style={styles.backButtonText}>←</Text>
            </TouchableOpacity>
            
            <View style={styles.stepIndicator}>
              {steps.map((_, index) => (
                <View
                  key={index}
                  style={[
                    styles.stepDot,
                    currentStep === index && styles.activeStepDot,
                    currentStep > index && styles.completedStepDot
                  ]}
                />
              ))}
            </View>

            <Text style={styles.title}>{steps[currentStep].title}</Text>
            <Text style={styles.subtitle}>{steps[currentStep].subtitle}</Text>
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
            
            {steps[currentStep].fields}
            
            <TouchableOpacity 
              style={[styles.nextButton, isLoading && styles.nextButtonDisabled]}
              onPress={handleNext}
              disabled={isLoading}
            >
              {isLoading ? (
                <ActivityIndicator size="small" color={COLORS.white} />
              ) : (
                <Text style={styles.nextButtonText}>
                  {currentStep === steps.length - 1 ? 'S\'inscrire' : 'Suivant'}
                </Text>
              )}
            </TouchableOpacity>

            {currentStep === 0 && (
              <>
                <View style={styles.divider}>
                  <View style={styles.dividerLine} />
                  <Text style={styles.dividerText}>ou</Text>
                  <View style={styles.dividerLine} />
                </View>

                <TouchableOpacity 
                  style={[styles.googleButton, isLoading && styles.googleButtonDisabled]}
                  onPress={handleGoogleSignup}
                  disabled={isLoading}
                >
                  <Image 
                    source={{ uri: 'https://developers.google.com/identity/images/g-logo.png' }}
                    style={styles.googleIcon}
                  />
                  <Text style={styles.googleButtonText}>S'inscrire avec Google</Text>
                </TouchableOpacity>
              </>
            )}

            <View style={styles.loginContainer}>
              <Text style={styles.loginText}>
                Vous avez déjà un compte ?
              </Text>
              <TouchableOpacity onPress={() => navigation.navigate('Login')}>
                <Text style={styles.loginLink}>
                  Se connecter
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
    paddingTop: Platform.OS === 'ios' ? SPACING.xl : SPACING.xxl,
    paddingHorizontal: SPACING.lg,
    backgroundColor: COLORS.primary,
    paddingBottom: SPACING.xl,
    borderBottomLeftRadius: 30,
    borderBottomRightRadius: 30,
    minHeight: 200,
  },
  backButton: {
    marginBottom: SPACING.md,
  },
  backButtonText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.xl,
  },
  stepIndicator: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: SPACING.lg,
  },
  stepDot: {
    width: 10,
    height: 10,
    borderRadius: 5,
    backgroundColor: COLORS.light_gray,
    marginHorizontal: SPACING.xs,
  },
  activeStepDot: {
    backgroundColor: COLORS.white,
    width: 12,
    height: 12,
  },
  completedStepDot: {
    backgroundColor: COLORS.secondary,
  },
  title: {
    fontSize: FONT_SIZE.xxl,
    color: COLORS.white,
    fontWeight: 'bold',
    marginBottom: SPACING.sm,
  },
  subtitle: {
    fontSize: FONT_SIZE.md,
    color: COLORS.white,
    opacity: 0.8,
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
  passwordRequirements: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.gray,
    marginBottom: SPACING.md,
    fontStyle: 'italic',
  },
  nextButton: {
    backgroundColor: COLORS.primary,
    padding: SPACING.md,
    borderRadius: 10,
    alignItems: 'center',
    marginTop: SPACING.md,
    ...boxShadow,
  },
  nextButtonDisabled: {
    backgroundColor: COLORS.primary_light,
    opacity: 0.7,
  },
  nextButtonText: {
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
    ...boxShadow,
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
  loginContainer: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: SPACING.xl,
    paddingBottom: SPACING.xl,
  },
  loginText: {
    color: COLORS.gray,
    fontSize: FONT_SIZE.sm,
  },
  loginLink: {
    color: COLORS.primary,
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
    marginLeft: SPACING.xs,
  },
});
