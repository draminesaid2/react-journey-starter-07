import React from 'react';
import { ClerkProvider } from '@clerk/clerk-expo';
import * as SecureStore from 'expo-secure-store';
import { View, ActivityIndicator } from 'react-native';
import { COLORS } from '../theme/colors';

// SecureStore implementation for Clerk token storage
const tokenCache = {
  async getToken(key) {
    try {
      return SecureStore.getItemAsync(key);
    } catch (err) {
      console.error("Error getting token from secure store:", err);
      return null;
    }
  },
  async saveToken(key, value) {
    try {
      return SecureStore.setItemAsync(key, value);
    } catch (err) {
      console.error("Error saving token to secure store:", err);
    }
  }
};

const ClerkProviderWrapper = ({ children }) => {
  // Use the publishable key from environment or fallback
  const publishableKey = process.env.EXPO_PUBLIC_CLERK_PUBLISHABLE_KEY || 
    'pk_test_Y2FyaW5nLWJpc29uLTE5LmNsZXJrLmFjY291bnRzLmRldiQ';

  return (
    <ClerkProvider 
      publishableKey={publishableKey}
      tokenCache={tokenCache}
      signInUrl="/login"
      signUpUrl="/signup"
      afterSignInUrl="/map"
      afterSignUpUrl="/map"
    >
      {children}
    </ClerkProvider>
  );
};

export default ClerkProviderWrapper;
