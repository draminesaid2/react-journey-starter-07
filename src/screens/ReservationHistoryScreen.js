
import React, { useState, useEffect } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  FlatList, 
  TouchableOpacity, 
  SafeAreaView,
  StatusBar,
  Image,
  ActivityIndicator
} from 'react-native';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE } from '../theme/typography';
import * as Animatable from 'react-native-animatable';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../auth/useAuth';
import { FooterNav } from '../components/FooterNav';

const ReservationHistoryScreen = ({ navigation }) => {
  const { t } = useTranslation();
  const { isSignedIn, user } = useAuth();
  const [isLoading, setIsLoading] = useState(true);
  const [reservations, setReservations] = useState([]);
  const [activeTab, setActiveTab] = useState('past'); // past, upcoming, cancelled

  // Mock data
  const mockReservations = [
    {
      id: '1',
      placeId: '101',
      placeName: 'Colosseum of Rome',
      placeImage: 'https://images.unsplash.com/photo-1552832230-c0197dd311b5',
      date: new Date(Date.now() - 7 * 24 * 60 * 60 * 1000).toISOString(), // 7 days ago
      time: '10:00 AM',
      status: 'completed',
      price: '€15.00',
      providerId: 'p1',
      providerName: 'Rome Historical Tours'
    },
    {
      id: '2',
      placeId: '102',
      placeName: 'Bulla Regia',
      placeImage: 'https://images.unsplash.com/photo-1560769680-ba2f3767c785',
      date: new Date(Date.now() - 14 * 24 * 60 * 60 * 1000).toISOString(), // 14 days ago
      time: '09:30 AM',
      status: 'completed',
      price: '€10.00',
      providerId: 'p2',
      providerName: 'Tunisia Heritage'
    },
    {
      id: '3',
      placeId: '103',
      placeName: 'Carthage Ruins',
      placeImage: 'https://images.unsplash.com/photo-1557456170-0cf4f4d0d362',
      date: new Date(Date.now() + 3 * 24 * 60 * 60 * 1000).toISOString(), // 3 days from now
      time: '11:00 AM',
      status: 'upcoming',
      price: '€12.00',
      providerId: 'p3',
      providerName: 'Carthage Explorer'
    },
    {
      id: '4',
      placeId: '104',
      placeName: 'El Djem Amphitheatre',
      placeImage: 'https://images.unsplash.com/photo-1563296291-14f16db1f89a',
      date: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toISOString(), // 7 days from now
      time: '14:00 PM',
      status: 'upcoming',
      price: '€8.00',
      providerId: 'p4',
      providerName: 'Tunisia Tours'
    },
    {
      id: '5',
      placeId: '105',
      placeName: 'Dougga',
      placeImage: 'https://images.unsplash.com/photo-1528181304800-259b08848526',
      date: new Date(Date.now() - 5 * 24 * 60 * 60 * 1000).toISOString(), // 5 days ago
      time: '10:30 AM',
      status: 'cancelled',
      price: '€7.50',
      providerId: 'p5',
      providerName: 'Dougga Tours'
    },
  ];

  useEffect(() => {
    // Simulate API call
    setTimeout(() => {
      setReservations(mockReservations);
      setIsLoading(false);
    }, 1000);
  }, []);

  const formatDate = (dateString) => {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  const filteredReservations = () => {
    switch (activeTab) {
      case 'past':
        return reservations.filter(res => res.status === 'completed');
      case 'upcoming':
        return reservations.filter(res => res.status === 'upcoming');
      case 'cancelled':
        return reservations.filter(res => res.status === 'cancelled');
      default:
        return reservations;
    }
  };

  const contactProvider = (providerId, providerName) => {
    navigation.navigate('Message', { providerId, providerName });
  };

  const renderItem = ({ item }) => (
    <Animatable.View 
      animation="fadeIn" 
      duration={500} 
      style={styles.reservationCard}
    >
      <View style={styles.reservationHeader}>
        <Text style={styles.reservationStatus}>
          {item.status === 'completed' ? '✓ ' : item.status === 'upcoming' ? '⏱ ' : '✕ '}
          {item.status === 'completed' 
            ? t('reservationHistory.completed') 
            : item.status === 'upcoming' 
              ? t('reservationHistory.upcoming') 
              : t('reservationHistory.cancelled')}
        </Text>
        <Text style={styles.reservationPrice}>{item.price}</Text>
      </View>

      <View style={styles.reservationContent}>
        <Image 
          source={{ uri: item.placeImage }} 
          style={styles.placeImage}
          defaultSource={require('../../assets/favicon.png')}
        />
        <View style={styles.reservationDetails}>
          <Text style={styles.placeName}>{item.placeName}</Text>
          <Text style={styles.reservationDate}>
            {formatDate(item.date)} • {item.time}
          </Text>
          <Text style={styles.providerName}>{item.providerName}</Text>
        </View>
      </View>

      <View style={styles.reservationActions}>
        {item.status === 'upcoming' && (
          <TouchableOpacity 
            style={[styles.actionButton, styles.cancelButton]}
            onPress={() => {/* Handle cancel */}}
          >
            <Text style={styles.cancelButtonText}>
              {t('reservationHistory.cancel')}
            </Text>
          </TouchableOpacity>
        )}
        
        <TouchableOpacity 
          style={[styles.actionButton, styles.contactButton]}
          onPress={() => contactProvider(item.providerId, item.providerName)}
        >
          <Text style={styles.contactButtonText}>
            {t('reservationHistory.contact')}
          </Text>
        </TouchableOpacity>
      </View>
    </Animatable.View>
  );

  const TabButton = ({ title, tabName }) => (
    <TouchableOpacity
      style={[
        styles.tabButton,
        activeTab === tabName && styles.activeTabButton
      ]}
      onPress={() => setActiveTab(tabName)}
    >
      <Text style={[
        styles.tabButtonText,
        activeTab === tabName && styles.activeTabButtonText
      ]}>
        {title}
      </Text>
    </TouchableOpacity>
  );

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor={COLORS.primary} barStyle="light-content" />
      <View style={styles.header}>
        <TouchableOpacity 
          style={styles.backButton}
          onPress={() => navigation.goBack()}
        >
          <Text style={styles.backButtonText}>←</Text>
        </TouchableOpacity>
        <Text style={styles.headerTitle}>
          {t('reservationHistory.title')}
        </Text>
      </View>

      <View style={styles.tabContainer}>
        <TabButton 
          title={t('reservationHistory.past')} 
          tabName="past" 
        />
        <TabButton 
          title={t('reservationHistory.upcoming')} 
          tabName="upcoming" 
        />
        <TabButton 
          title={t('reservationHistory.cancelled')} 
          tabName="cancelled" 
        />
      </View>

      {isLoading ? (
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color={COLORS.primary} />
        </View>
      ) : filteredReservations().length > 0 ? (
        <FlatList
          data={filteredReservations()}
          renderItem={renderItem}
          keyExtractor={item => item.id}
          contentContainerStyle={styles.listContainer}
        />
      ) : (
        <View style={styles.emptyContainer}>
          <Text style={styles.emptyText}>
            {activeTab === 'past' 
              ? t('reservationHistory.noPastReservations')
              : activeTab === 'upcoming'
                ? t('reservationHistory.noUpcomingReservations')
                : t('reservationHistory.noCancelledReservations')
            }
          </Text>
        </View>
      )}

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
    flexDirection: 'row',
    alignItems: 'center',
  },
  backButton: {
    padding: SPACING.sm,
  },
  backButtonText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.xl,
  },
  headerTitle: {
    color: COLORS.white,
    fontSize: FONT_SIZE.lg,
    fontWeight: 'bold',
    marginLeft: SPACING.md,
  },
  tabContainer: {
    flexDirection: 'row',
    backgroundColor: COLORS.white,
    paddingVertical: SPACING.sm,
    paddingHorizontal: SPACING.md,
    borderBottomWidth: 1,
    borderBottomColor: COLORS.light_gray,
  },
  tabButton: {
    flex: 1,
    paddingVertical: SPACING.sm,
    alignItems: 'center',
    borderBottomWidth: 2,
    borderBottomColor: 'transparent',
  },
  activeTabButton: {
    borderBottomColor: COLORS.primary,
  },
  tabButtonText: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
  },
  activeTabButtonText: {
    fontWeight: 'bold',
    color: COLORS.primary,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  listContainer: {
    padding: SPACING.md,
    paddingBottom: 100, // Space for footer
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingHorizontal: SPACING.xl,
  },
  emptyText: {
    fontSize: FONT_SIZE.md,
    color: COLORS.gray,
    textAlign: 'center',
  },
  reservationCard: {
    backgroundColor: COLORS.white,
    borderRadius: 12,
    marginBottom: SPACING.md,
    overflow: 'hidden',
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
  },
  reservationHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingHorizontal: SPACING.md,
    paddingVertical: SPACING.sm,
    borderBottomWidth: 1,
    borderBottomColor: COLORS.light_gray,
  },
  reservationStatus: {
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
  },
  reservationPrice: {
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
    color: COLORS.primary,
  },
  reservationContent: {
    flexDirection: 'row',
    padding: SPACING.md,
  },
  placeImage: {
    width: 80,
    height: 80,
    borderRadius: 8,
    backgroundColor: COLORS.light_gray,
  },
  reservationDetails: {
    marginLeft: SPACING.md,
    flex: 1,
  },
  placeName: {
    fontSize: FONT_SIZE.md,
    fontWeight: 'bold',
    color: COLORS.black,
    marginBottom: SPACING.xs,
  },
  reservationDate: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    marginBottom: SPACING.xs,
  },
  providerName: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.primary,
  },
  reservationActions: {
    flexDirection: 'row',
    borderTopWidth: 1,
    borderTopColor: COLORS.light_gray,
    padding: SPACING.sm,
  },
  actionButton: {
    flex: 1,
    paddingVertical: SPACING.sm,
    borderRadius: 6,
    alignItems: 'center',
    marginHorizontal: SPACING.xs,
  },
  cancelButton: {
    backgroundColor: COLORS.light_gray,
  },
  cancelButtonText: {
    color: COLORS.gray_dark,
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
  },
  contactButton: {
    backgroundColor: COLORS.primary_light,
  },
  contactButtonText: {
    color: COLORS.primary_dark,
    fontSize: FONT_SIZE.sm,
    fontWeight: 'bold',
  },
});

export default ReservationHistoryScreen;
