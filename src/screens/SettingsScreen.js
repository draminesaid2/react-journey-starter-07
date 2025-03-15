import React, { useState } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  TouchableOpacity, 
  Switch, 
  ScrollView,
  SafeAreaView,
  StatusBar,
  Alert,
  Platform
} from 'react-native';
import { useTranslation } from 'react-i18next';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE } from '../theme/typography';
import * as Animatable from 'react-native-animatable';
import { FooterNav } from '../components/FooterNav';
import { useAuth } from '../auth/useAuth';

const SettingsScreen = ({ navigation }) => {
  const { t, i18n } = useTranslation();
  const [language, setLanguage] = useState(i18n.language);
  const [notifications, setNotifications] = useState(true);
  const [darkMode, setDarkMode] = useState(false);
  const [location, setLocation] = useState(true);
  const { navigateToLogin } = useAuth();

  const changeLanguage = (lng) => {
    i18n.changeLanguage(lng);
    setLanguage(lng);
  };

  const handleLogout = () => {
    Alert.alert(
      t('settings.logoutTitle'),
      t('settings.logoutMessage'),
      [
        {
          text: t('settings.cancel'),
          style: 'cancel'
        },
        {
          text: t('settings.confirm'),
          onPress: () => navigateToLogin()
        }
      ]
    );
  };

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor={COLORS.primary} barStyle="light-content" />
      <View style={styles.header}>
        <Text style={styles.headerTitle}>{t('settings.title')}</Text>
      </View>
      
      <ScrollView contentContainerStyle={styles.scrollContainer}>
        {/* Account Section */}
        <Animatable.View animation="fadeInUp" duration={300} style={styles.section}>
          <Text style={styles.sectionTitle}>{t('settings.account')}</Text>
          
          <TouchableOpacity style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.editProfile')}</Text>
              <Text style={styles.itemSubtext}>{t('settings.changePersonalInfo')}</Text>
            </View>
            <Text style={styles.chevron}>›</Text>
          </TouchableOpacity>
          
          <TouchableOpacity style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.security')}</Text>
              <Text style={styles.itemSubtext}>{t('settings.passwordAndAuthentication')}</Text>
            </View>
            <Text style={styles.chevron}>›</Text>
          </TouchableOpacity>
          
          <TouchableOpacity 
            style={styles.item}
            onPress={() => navigation.navigate('ReservationHistory')}
          >
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.reservationHistory')}</Text>
              <Text style={styles.itemSubtext}>{t('settings.viewPastReservations')}</Text>
            </View>
            <Text style={styles.chevron}>›</Text>
          </TouchableOpacity>
        </Animatable.View>
        
        {/* Preferences Section */}
        <Animatable.View animation="fadeInUp" duration={300} delay={100} style={styles.section}>
          <Text style={styles.sectionTitle}>{t('settings.preferences')}</Text>
          
          <View style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.language')}</Text>
            </View>
            <View style={styles.languageContainer}>
              <TouchableOpacity
                style={[styles.languageButton, language === 'en' ? styles.activeLanguage : null]}
                onPress={() => changeLanguage('en')}
              >
                <Text style={styles.languageText}>EN</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.languageButton, language === 'fr' ? styles.activeLanguage : null]}
                onPress={() => changeLanguage('fr')}
              >
                <Text style={styles.languageText}>FR</Text>
              </TouchableOpacity>
            </View>
          </View>
          
          <View style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.notifications')}</Text>
              <Text style={styles.itemSubtext}>{t('settings.enableDisableNotifications')}</Text>
            </View>
            <Switch
              value={notifications}
              onValueChange={setNotifications}
              trackColor={{ false: COLORS.gray_light, true: COLORS.primary }}
              thumbColor={notifications ? COLORS.secondary : COLORS.white}
            />
          </View>
          
          <View style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.darkMode')}</Text>
              <Text style={styles.itemSubtext}>{t('settings.enableDarkMode')}</Text>
            </View>
            <Switch
              value={darkMode}
              onValueChange={setDarkMode}
              trackColor={{ false: COLORS.gray_light, true: COLORS.primary }}
              thumbColor={darkMode ? COLORS.secondary : COLORS.white}
            />
          </View>
          
          <View style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.location')}</Text>
              <Text style={styles.itemSubtext}>{t('settings.allowLocationAccess')}</Text>
            </View>
            <Switch
              value={location}
              onValueChange={setLocation}
              trackColor={{ false: COLORS.gray_light, true: COLORS.primary }}
              thumbColor={location ? COLORS.secondary : COLORS.white}
            />
          </View>
        </Animatable.View>
        
        {/* About Section */}
        <Animatable.View animation="fadeInUp" duration={300} delay={200} style={styles.section}>
          <Text style={styles.sectionTitle}>{t('settings.about')}</Text>
          
          <TouchableOpacity style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.termsOfService')}</Text>
            </View>
            <Text style={styles.chevron}>›</Text>
          </TouchableOpacity>
          
          <TouchableOpacity style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.privacyPolicy')}</Text>
            </View>
            <Text style={styles.chevron}>›</Text>
          </TouchableOpacity>
          
          <TouchableOpacity style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.helpAndSupport')}</Text>
            </View>
            <Text style={styles.chevron}>›</Text>
          </TouchableOpacity>
          
          <TouchableOpacity style={styles.item}>
            <View style={styles.itemContent}>
              <Text style={styles.itemText}>{t('settings.appVersion')}</Text>
              <Text style={styles.itemSubtext}>1.0.0</Text>
            </View>
          </TouchableOpacity>
        </Animatable.View>
        
        {/* Logout Button */}
        <Animatable.View animation="fadeInUp" duration={300} delay={300}>
          <TouchableOpacity 
            style={styles.logoutButton}
            onPress={handleLogout}
          >
            <Text style={styles.logoutText}>{t('settings.logout')}</Text>
          </TouchableOpacity>
        </Animatable.View>
      </ScrollView>
      
      <FooterNav navigation={navigation} activeScreen="Settings" />
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: COLORS.light_gray,
  },
  header: {
    backgroundColor: COLORS.primary,
    paddingTop: Platform.OS === 'android' ? StatusBar.currentHeight : 0,
    paddingBottom: SPACING.md,
    paddingHorizontal: SPACING.lg,
    justifyContent: 'center',
    alignItems: 'center',
  },
  headerTitle: {
    color: COLORS.white,
    fontSize: FONT_SIZE.xl,
    fontWeight: 'bold',
  },
  scrollContainer: {
    padding: SPACING.lg,
    paddingBottom: 100,
  },
  section: {
    backgroundColor: COLORS.white,
    borderRadius: 15,
    padding: SPACING.md,
    marginBottom: SPACING.lg,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
  },
  sectionTitle: {
    fontSize: FONT_SIZE.lg,
    fontWeight: 'bold',
    color: COLORS.primary,
    marginBottom: SPACING.md,
    borderBottomWidth: 1,
    borderBottomColor: COLORS.light_gray,
    paddingBottom: SPACING.sm,
  },
  item: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: SPACING.md,
    borderBottomWidth: 1,
    borderBottomColor: COLORS.light_gray,
  },
  itemContent: {
    flex: 1,
  },
  itemText: {
    fontSize: FONT_SIZE.md,
    color: COLORS.black,
  },
  itemSubtext: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    marginTop: 4,
  },
  chevron: {
    fontSize: FONT_SIZE.lg,
    color: COLORS.gray,
  },
  languageContainer: {
    flexDirection: 'row',
  },
  languageButton: {
    paddingHorizontal: SPACING.md,
    paddingVertical: SPACING.xs,
    backgroundColor: COLORS.light_gray,
    borderRadius: 20,
    marginLeft: SPACING.xs,
  },
  activeLanguage: {
    backgroundColor: COLORS.primary,
  },
  languageText: {
    color: COLORS.black,
    fontWeight: 'bold',
  },
  logoutButton: {
    backgroundColor: COLORS.error,
    paddingVertical: SPACING.md,
    borderRadius: 15,
    alignItems: 'center',
    marginBottom: SPACING.xxl,
  },
  logoutText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.md,
    fontWeight: 'bold',
  },
});

export default SettingsScreen;
