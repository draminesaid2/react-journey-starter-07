
import { useAuth as useClerkAuth, useUser } from '@clerk/clerk-expo';
import { useNavigation } from '@react-navigation/native';

export const useAuth = () => {
  const { isLoaded, isSignedIn, signOut } = useClerkAuth();
  const { user } = useUser();
  const navigation = useNavigation();

  const handleSignOut = async () => {
    try {
      await signOut();
      navigation.replace('Login');
    } catch (error) {
      console.error('Error signing out:', error);
    }
  };

  // Function to navigate back to login with signout
  const navigateToLogin = async () => {
    try {
      if (isSignedIn) {
        await signOut();
      }
      navigation.replace('Login');
    } catch (error) {
      console.error('Error during logout:', error);
      // Still navigate to login even if there's an error
      navigation.replace('Login');
    }
  };

  const isAdmin = () => {
    // For demo purposes - in production you'd check roles/claims
    return false;
  };

  const isProvider = () => {
    // For demo purposes - in production you'd check roles/claims
    return false;
  };

  return {
    isLoaded,
    isSignedIn,
    user,
    signOut: handleSignOut,
    navigateToLogin,
    isAdmin,
    isProvider,
  };
};
