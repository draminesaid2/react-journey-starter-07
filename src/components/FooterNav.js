
import React from 'react';
import { View, TouchableOpacity, Platform, Text, StyleSheet } from 'react-native';
import { HomeIcon, UserIcon, Settings, Clock, MapPin, CalendarIcon, MessageCircle } from 'lucide-react-native';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE, FONT_WEIGHT } from '../theme/typography';
import * as Mixins from '../theme/mixins';

export const FooterNav = ({ navigation, activeScreen }) => {
  // Map is the main home screen
  const isHomeActive = activeScreen === 'Map';
  
  const handleHomePress = () => {
    navigation.navigate('Map');
  };
  
  return (
    <View style={styles.footer}>
      <TouchableOpacity 
        style={styles.navItem} 
        onPress={handleHomePress}
      >
        <View style={[
          styles.iconContainer, 
          isHomeActive && styles.activeIconContainer
        ]}>
          <HomeIcon 
            size={22} 
            color={isHomeActive ? COLORS.white : COLORS.gray} 
          />
        </View>
        <Text style={[
          styles.navText,
          isHomeActive && styles.activeText
        ]}>Accueil</Text>
        {isHomeActive && <View style={styles.activeIndicator} />}
      </TouchableOpacity>

      <TouchableOpacity 
        style={styles.navItem}
        onPress={() => navigation.navigate('Reservation')}
      >
        <View style={[
          styles.iconContainer, 
          activeScreen === 'Reservation' && styles.activeIconContainer
        ]}>
          <CalendarIcon 
            size={22} 
            color={activeScreen === 'Reservation' ? COLORS.white : COLORS.gray} 
          />
        </View>
        <Text style={[
          styles.navText,
          activeScreen === 'Reservation' && styles.activeText
        ]}>Réservation</Text>
        {activeScreen === 'Reservation' && <View style={styles.activeIndicator} />}
      </TouchableOpacity>

      <TouchableOpacity 
        style={styles.navItem}
        onPress={() => navigation.navigate('Acote')}
      >
        <View style={[
          styles.iconContainer, 
          activeScreen === 'Acote' && styles.activeIconContainer
        ]}>
          <MapPin 
            size={22} 
            color={activeScreen === 'Acote' ? COLORS.white : COLORS.gray} 
          />
        </View>
        <Text style={[
          styles.navText,
          activeScreen === 'Acote' && styles.activeText
        ]}>A côté</Text>
        {activeScreen === 'Acote' && <View style={styles.activeIndicator} />}
      </TouchableOpacity>

      <TouchableOpacity 
        style={styles.navItem}
        onPress={() => navigation.navigate('MessageList')}
      >
        <View style={[
          styles.iconContainer, 
          activeScreen === 'MessageList' && styles.activeIconContainer
        ]}>
          <MessageCircle 
            size={22} 
            color={activeScreen === 'MessageList' ? COLORS.white : COLORS.gray} 
          />
        </View>
        <Text style={[
          styles.navText,
          activeScreen === 'MessageList' && styles.activeText
        ]}>Messages</Text>
        {activeScreen === 'MessageList' && <View style={styles.activeIndicator} />}
      </TouchableOpacity>

      <TouchableOpacity 
        style={styles.navItem}
        onPress={() => navigation.navigate('Settings')}
      >
        <View style={[
          styles.iconContainer, 
          activeScreen === 'Settings' && styles.activeIconContainer
        ]}>
          <Settings 
            size={22} 
            color={activeScreen === 'Settings' ? COLORS.white : COLORS.gray} 
          />
        </View>
        <Text style={[
          styles.navText,
          activeScreen === 'Settings' && styles.activeText
        ]}>Paramètres</Text>
        {activeScreen === 'Settings' && <View style={styles.activeIndicator} />}
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    backgroundColor: COLORS.primary_dark,
    paddingVertical: SPACING.md,
    paddingBottom: Platform.OS === 'ios' ? SPACING.lg : SPACING.md,
    borderTopWidth: 1,
    borderTopColor: 'rgba(255, 255, 255, 0.1)',
    width: '100%',
    ...Platform.select({
      ios: {
        shadowColor: COLORS.black,
        shadowOffset: { width: 0, height: -2 },
        shadowOpacity: 0.2,
        shadowRadius: 4,
      },
      android: {
        elevation: 8,
      },
    }),
  },
  navItem: {
    alignItems: 'center',
    justifyContent: 'center',
    paddingHorizontal: SPACING.sm,
    position: 'relative',
    width: Mixins.screenWidth / 4 - SPACING.sm,
  },
  iconContainer: {
    width: 44,
    height: 44,
    borderRadius: 22,
    backgroundColor: 'rgba(255, 255, 255, 0.08)',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: SPACING.xs,
  },
  activeIconContainer: {
    backgroundColor: COLORS.primary,
    borderWidth: 2,
    borderColor: COLORS.secondary,
  },
  navText: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.gray,
    marginTop: SPACING.xs / 2,
    textAlign: 'center',
  },
  activeText: {
    color: COLORS.white,
    fontWeight: FONT_WEIGHT.bold,
  },
  activeIndicator: {
    position: 'absolute',
    bottom: -8,
    width: 24,
    height: 3,
    borderRadius: 1.5,
    backgroundColor: COLORS.secondary,
  },
});
