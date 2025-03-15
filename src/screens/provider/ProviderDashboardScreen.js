
import React from 'react';
import { 
  StyleSheet, 
  View, 
  Text, 
  TouchableOpacity, 
  ScrollView, 
  SafeAreaView, 
  StatusBar 
} from 'react-native';
import { COLORS } from '../../theme/colors';
import { SPACING } from '../../theme/spacing';
import { FONT_SIZE } from '../../theme/typography';
import { 
  BarChart2, 
  Users, 
  Calendar, 
  Star, 
  Tag, 
  Settings,
  MessageCircle,
  Clipboard,
  TrendingUp,
  HelpCircle
} from 'lucide-react-native';

export default function ProviderDashboardScreen({ navigation }) {
  const menuItems = [
    {
      icon: <Users size={24} color={COLORS.primary} />,
      title: 'Gérer mon compte',
      screen: 'AccountManagement'
    },
    {
      icon: <Calendar size={24} color={COLORS.primary} />,
      title: 'Gérer les réservations',
      screen: 'ReservationManagement'
    },
    {
      icon: <Star size={24} color={COLORS.primary} />,
      title: 'Gérer les avis',
      screen: 'ReviewManagement'
    },
    {
      icon: <Tag size={24} color={COLORS.primary} />,
      title: 'Gérer les promotions',
      screen: 'PromotionManagement'
    },
    {
      icon: <MessageCircle size={24} color={COLORS.primary} />,
      title: 'Messages clients',
      screen: 'ProviderMessageList'
    },
    {
      icon: <Settings size={24} color={COLORS.primary} />,
      title: 'Paramètres',
      screen: 'Settings'
    }
  ];

  const statistics = [
    {
      icon: <Clipboard size={20} color={COLORS.primary} />,
      title: 'Réservations',
      value: '24',
      change: '+12%',
      positive: true
    },
    {
      icon: <MessageCircle size={20} color={COLORS.primary} />,
      title: 'Messages',
      value: '15',
      change: '+5',
      positive: true
    },
    {
      icon: <TrendingUp size={20} color={COLORS.primary} />,
      title: 'Visites',
      value: '142',
      change: '+22%',
      positive: true
    },
    {
      icon: <Star size={20} color={COLORS.primary} />,
      title: 'Avis',
      value: '4.7',
      change: '+0.2',
      positive: true
    }
  ];

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor={COLORS.primary} barStyle="light-content" />
      
      <View style={styles.header}>
        <View>
          <Text style={styles.welcomeText}>Bienvenue,</Text>
          <Text style={styles.businessName}>Carthage Découvertes</Text>
        </View>
        <TouchableOpacity style={styles.helpButton}>
          <HelpCircle size={24} color={COLORS.white} />
        </TouchableOpacity>
      </View>

      <ScrollView contentContainerStyle={styles.scrollContent}>
        <View style={styles.statsContainer}>
          <Text style={styles.sectionTitle}>Vue d'ensemble</Text>
          <View style={styles.statsGrid}>
            {statistics.map((stat, index) => (
              <View key={index} style={styles.statCard}>
                <View style={styles.statHeader}>
                  {stat.icon}
                  <Text style={styles.statTitle}>{stat.title}</Text>
                </View>
                <Text style={styles.statValue}>{stat.value}</Text>
                <Text style={[
                  styles.statChange,
                  stat.positive ? styles.positiveChange : styles.negativeChange
                ]}>
                  {stat.change}
                </Text>
              </View>
            ))}
          </View>
        </View>

        <View style={styles.menuContainer}>
          <Text style={styles.sectionTitle}>Gérer mon établissement</Text>
          <View style={styles.menuGrid}>
            {menuItems.map((item, index) => (
              <TouchableOpacity 
                key={index} 
                style={styles.menuItem}
                onPress={() => navigation.navigate(item.screen)}
              >
                <View style={styles.menuIconContainer}>
                  {item.icon}
                </View>
                <Text style={styles.menuText}>{item.title}</Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: COLORS.light_gray,
  },
  header: {
    backgroundColor: COLORS.primary,
    paddingTop: Platform.OS === 'android' ? StatusBar.currentHeight : SPACING.md,
    paddingBottom: SPACING.xl,
    paddingHorizontal: SPACING.lg,
    borderBottomLeftRadius: 20,
    borderBottomRightRadius: 20,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  welcomeText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.md,
    opacity: 0.9,
  },
  businessName: {
    color: COLORS.white,
    fontSize: FONT_SIZE.xl,
    fontWeight: 'bold',
    marginTop: SPACING.xs,
  },
  helpButton: {
    padding: SPACING.sm,
  },
  scrollContent: {
    padding: SPACING.lg,
  },
  sectionTitle: {
    fontSize: FONT_SIZE.lg,
    fontWeight: 'bold',
    marginBottom: SPACING.md,
    color: COLORS.black,
  },
  statsContainer: {
    marginBottom: SPACING.xl,
  },
  statsGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
  },
  statCard: {
    backgroundColor: COLORS.white,
    borderRadius: 12,
    padding: SPACING.md,
    marginBottom: SPACING.md,
    width: '48%',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  statHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: SPACING.xs,
  },
  statTitle: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    marginLeft: SPACING.sm,
  },
  statValue: {
    fontSize: FONT_SIZE.xl,
    fontWeight: 'bold',
    color: COLORS.black,
    marginVertical: SPACING.xs,
  },
  statChange: {
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
  },
  positiveChange: {
    color: COLORS.success,
  },
  negativeChange: {
    color: COLORS.error,
  },
  menuContainer: {
    marginBottom: SPACING.xl,
  },
  menuGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
  },
  menuItem: {
    backgroundColor: COLORS.white,
    borderRadius: 12,
    padding: SPACING.md,
    marginBottom: SPACING.lg,
    width: '48%',
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  menuIconContainer: {
    backgroundColor: COLORS.light_gray,
    width: 50,
    height: 50,
    borderRadius: 25,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: SPACING.sm,
  },
  menuText: {
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
    color: COLORS.black,
    textAlign: 'center',
  }
});
