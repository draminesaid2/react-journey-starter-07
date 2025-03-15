
import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import StartScreen from './src/screens/StartScreen';
import LoginScreen from './src/screens/LoginScreen';
import SignupScreen from './src/screens/SignupScreen';
import MapScreen from './src/screens/MapScreen';
import ReservationScreen from './src/screens/ReservationScreen';
import SettingsScreen from './src/screens/SettingsScreen';
import AcoteScreen from './src/screens/AcoteScreen';
import AdminDashboardScreen from './src/screens/admin/AdminDashboardScreen';
import UserManagementScreen from './src/screens/admin/UserManagementScreen';
import LocationManagementScreen from './src/screens/admin/LocationManagementScreen';
import ReviewModerationScreen from './src/screens/admin/ReviewModerationScreen';
import EventManagementScreen from './src/screens/admin/EventManagementScreen';
import GlobalSupervisionScreen from './src/screens/admin/GlobalSupervisionScreen';
import ProviderDashboardScreen from './src/screens/provider/ProviderDashboardScreen';
import AccountManagementScreen from './src/screens/provider/AccountManagementScreen';
import ReservationManagementScreen from './src/screens/provider/ReservationManagementScreen';
import ReviewManagementScreen from './src/screens/provider/ReviewManagementScreen';
import PromotionManagementScreen from './src/screens/provider/PromotionManagementScreen';
import ProviderListScreen from './src/screens/ProviderListScreen';
import MessageScreen from './src/screens/MessageScreen';
import MessageListScreen from './src/screens/MessageListScreen';
import ProviderMessageListScreen from './src/screens/provider/ProviderMessageListScreen';
import { I18nextProvider } from 'react-i18next';
import i18n from './src/i18n';
import ClerkProviderWrapper from './src/auth/ClerkProviderWrapper';

const Stack = createNativeStackNavigator();

export default function App() {
  return (
    <ClerkProviderWrapper>
      <I18nextProvider i18n={i18n}>
        <NavigationContainer>
          <Stack.Navigator 
            initialRouteName="Start"
            screenOptions={{ headerShown: false }}
          >
            <Stack.Screen name="Start" component={StartScreen} />
            <Stack.Screen name="Login" component={LoginScreen} />
            <Stack.Screen name="Signup" component={SignupScreen} />
            <Stack.Screen name="Map" component={MapScreen} />
            <Stack.Screen name="Reservation" component={ReservationScreen} />
            <Stack.Screen name="Settings" component={SettingsScreen} />
            <Stack.Screen name="Acote" component={AcoteScreen} />
            <Stack.Screen name="ProviderList" component={ProviderListScreen} />
            <Stack.Screen name="Message" component={MessageScreen} />
            <Stack.Screen name="MessageListScreen" component={MessageListScreen} />
            <Stack.Screen name="ProviderMessageList" component={ProviderMessageListScreen} />
            
            {/* Admin Screens */}
            <Stack.Screen name="AdminDashboard" component={AdminDashboardScreen} />
            <Stack.Screen name="UserManagement" component={UserManagementScreen} />
            <Stack.Screen name="LocationManagement" component={LocationManagementScreen} />
            <Stack.Screen name="ReviewModeration" component={ReviewModerationScreen} />
            <Stack.Screen name="EventManagement" component={EventManagementScreen} />
            <Stack.Screen name="GlobalSupervision" component={GlobalSupervisionScreen} />
            
            {/* Provider Screens */}
            <Stack.Screen name="ProviderDashboard" component={ProviderDashboardScreen} />
            <Stack.Screen name="AccountManagement" component={AccountManagementScreen} />
            <Stack.Screen name="ReservationManagement" component={ReservationManagementScreen} />
            <Stack.Screen name="ReviewManagement" component={ReviewManagementScreen} />
            <Stack.Screen name="PromotionManagement" component={PromotionManagementScreen} />
          </Stack.Navigator>
        </NavigationContainer>
      </I18nextProvider>
    </ClerkProviderWrapper>
  );
}
