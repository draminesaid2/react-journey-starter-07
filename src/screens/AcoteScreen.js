import React, { useState, useEffect } from 'react';
import { 
  View, 
  StyleSheet, 
  SafeAreaView, 
  Text, 
  Platform, 
  StatusBar, 
  TouchableOpacity,
  FlatList 
} from 'react-native';
import { FooterNav } from '../components/FooterNav';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE, FONT_WEIGHT, getFontWeight } from '../theme/typography';
import { Calendar, MapPin, Navigation2, Info, Hotel, Utensils, Landmark, Coffee, AlertCircle } from 'lucide-react-native';
import * as Location from 'expo-location';
import * as Animatable from 'react-native-animatable';

const MOCK_NEARBY_PLACES = [
  {
    id: '1',
    name: 'Musée du Bardo',
    distance: 0.8,
    type: 'landmark',
    rating: 4.7,
    description: 'Musée national avec une collection exceptionnelle de mosaïques romaines',
    address: 'Rue Mongi Slim, Le Bardo, Tunis',
  },
  {
    id: '2',
    name: 'Restaurant Dar El Jeld',
    distance: 1.2,
    type: 'restaurant',
    rating: 4.5,
    description: 'Restaurant traditionnel tunisien dans une maison historique',
    address: 'Rue Dar El Jeld, Médina de Tunis',
  },
  {
    id: '3',
    name: 'Hôtel Dar El Medina',
    distance: 1.5,
    type: 'hotel',
    rating: 4.3,
    description: 'Hôtel de charme dans un ancien palais de la médina',
    address: 'Rue de la Médina, Tunis',
  },
  {
    id: '4',
    name: 'Café Malouf',
    distance: 0.5,
    type: 'cafe',
    rating: 4.1,
    description: 'Café traditionnel avec musique malouf en soirée',
    address: 'Avenue Habib Bourguiba, Tunis',
  },
  {
    id: '5',
    name: 'Cathédrale Saint-Vincent-de-Paul',
    distance: 1.0,
    type: 'landmark',
    rating: 4.4,
    description: 'Cathédrale catholique de style néo-roman construite au 19e siècle',
    address: 'Avenue Habib Bourguiba, Tunis',
  },
];

const AcoteScreen = ({ navigation }) => {
  const [nearbyPlaces, setNearbyPlaces] = useState(MOCK_NEARBY_PLACES);
  const [location, setLocation] = useState(null);
  const [errorMsg, setErrorMsg] = useState(null);
  const [loading, setLoading] = useState(true);
  const [selectedFilter, setSelectedFilter] = useState(null);

  useEffect(() => {
    (async () => {
      try {
        setLoading(true);
        let { status } = await Location.requestForegroundPermissionsAsync();
        if (status !== 'granted') {
          setErrorMsg('Permission to access location was denied');
          setLoading(false);
          return;
        }

        let location = await Location.getCurrentPositionAsync({});
        setLocation(location);
        
        // In a real app, we would fetch nearby places based on the location
        // For now, we'll just simulate a delay and use our mock data
        setTimeout(() => {
          setNearbyPlaces(MOCK_NEARBY_PLACES);
          setLoading(false);
        }, 1000);
      } catch (error) {
        console.error('Error getting location:', error);
        setErrorMsg('Could not determine your location');
        setLoading(false);
      }
    })();
  }, []);

  const getPlaceIcon = (type) => {
    switch (type) {
      case 'landmark':
        return <Landmark size={24} color={COLORS.primary} />;
      case 'hotel':
        return <Hotel size={24} color={COLORS.secondary} />;
      case 'restaurant':
        return <Utensils size={24} color={COLORS.error} />;
      case 'cafe':
        return <Coffee size={24} color={COLORS.warning} />;
      default:
        return <MapPin size={24} color={COLORS.primary} />;
    }
  };

  const getPlaceColor = (type) => {
    switch (type) {
      case 'landmark':
        return COLORS.primary;
      case 'hotel':
        return COLORS.secondary;
      case 'restaurant':
        return COLORS.error;
      case 'cafe':
        return COLORS.warning;
      default:
        return COLORS.primary;
    }
  };

  const filteredPlaces = selectedFilter 
    ? nearbyPlaces.filter(place => place.type === selectedFilter)
    : nearbyPlaces;

  const renderPlaceItem = ({ item, index }) => (
    <Animatable.View 
      animation="fadeInUp" 
      delay={index * 100}
      style={styles.placeCard}
    >
      <View style={styles.placeHeader}>
        <View style={[styles.placeIconContainer, { backgroundColor: getPlaceColor(item.type) + '20' }]}>
          {getPlaceIcon(item.type)}
        </View>
        <View style={styles.placeInfo}>
          <Text style={styles.placeName}>{item.name}</Text>
          <Text style={styles.placeAddress}>{item.address}</Text>
        </View>
        <View style={styles.placeDistance}>
          <Text style={styles.distanceText}>{item.distance} km</Text>
        </View>
      </View>
      
      <Text style={styles.placeDescription}>{item.description}</Text>
      
      <View style={styles.placeActions}>
        <TouchableOpacity 
          style={styles.actionButton}
          onPress={() => navigation.navigate('ProviderList')}
        >
          <Info size={16} color={COLORS.white} />
          <Text style={styles.actionButtonText}>Détails</Text>
        </TouchableOpacity>
        
        <TouchableOpacity style={[styles.actionButton, styles.actionButtonSecondary]}>
          <Navigation2 size={16} color={COLORS.primary} />
          <Text style={styles.actionButtonTextSecondary}>Itinéraire</Text>
        </TouchableOpacity>
      </View>
    </Animatable.View>
  );

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="light-content" backgroundColor={COLORS.primary_dark} />
      <View style={styles.header}>
        <View style={styles.headerContent}>
          <Text style={styles.title}>À côté</Text>
          <Text style={styles.subtitle}>Découvrez ce qui vous entoure</Text>
        </View>
        <TouchableOpacity 
          style={styles.reservationButton}
          onPress={() => navigation.navigate('Reservation')}
        >
          <Calendar size={20} color={COLORS.white} />
          <Text style={styles.reservationButtonText}>Réserver</Text>
        </TouchableOpacity>
      </View>

      <View style={styles.filtersContainer}>
        <TouchableOpacity 
          style={[
            styles.filterChip,
            selectedFilter === 'landmark' && { backgroundColor: COLORS.primary }
          ]}
          onPress={() => setSelectedFilter(selectedFilter === 'landmark' ? null : 'landmark')}
        >
          <Landmark size={18} color={selectedFilter === 'landmark' ? COLORS.white : COLORS.primary} />
          <Text 
            style={[
              styles.filterChipText,
              selectedFilter === 'landmark' && { color: COLORS.white }
            ]}
          >
            Sites
          </Text>
        </TouchableOpacity>
        
        <TouchableOpacity 
          style={[
            styles.filterChip,
            selectedFilter === 'hotel' && { backgroundColor: COLORS.secondary }
          ]}
          onPress={() => setSelectedFilter(selectedFilter === 'hotel' ? null : 'hotel')}
        >
          <Hotel size={18} color={selectedFilter === 'hotel' ? COLORS.white : COLORS.secondary} />
          <Text 
            style={[
              styles.filterChipText,
              selectedFilter === 'hotel' && { color: COLORS.white }
            ]}
          >
            Hôtels
          </Text>
        </TouchableOpacity>
        
        <TouchableOpacity 
          style={[
            styles.filterChip,
            selectedFilter === 'restaurant' && { backgroundColor: COLORS.error }
          ]}
          onPress={() => setSelectedFilter(selectedFilter === 'restaurant' ? null : 'restaurant')}
        >
          <Utensils size={18} color={selectedFilter === 'restaurant' ? COLORS.white : COLORS.error} />
          <Text 
            style={[
              styles.filterChipText,
              selectedFilter === 'restaurant' && { color: COLORS.white }
            ]}
          >
            Restaurants
          </Text>
        </TouchableOpacity>
        
        <TouchableOpacity 
          style={[
            styles.filterChip,
            selectedFilter === 'cafe' && { backgroundColor: COLORS.warning }
          ]}
          onPress={() => setSelectedFilter(selectedFilter === 'cafe' ? null : 'cafe')}
        >
          <Coffee size={18} color={selectedFilter === 'cafe' ? COLORS.white : COLORS.warning} />
          <Text 
            style={[
              styles.filterChipText,
              selectedFilter === 'cafe' && { color: COLORS.white }
            ]}
          >
            Cafés
          </Text>
        </TouchableOpacity>
      </View>

      {loading ? (
        <View style={styles.loadingContainer}>
          <Animatable.View animation="pulse" easing="ease-out" iterationCount="infinite">
            <MapPin size={48} color={COLORS.primary} />
          </Animatable.View>
          <Text style={styles.loadingText}>Recherche des lieux à proximité...</Text>
        </View>
      ) : errorMsg ? (
        <View style={styles.errorContainer}>
          <AlertCircle size={48} color={COLORS.error} />
          <Text style={styles.errorText}>{errorMsg}</Text>
          <TouchableOpacity 
            style={styles.retryButton}
            onPress={() => navigation.navigate('Acote')}
          >
            <Text style={styles.retryButtonText}>Réessayer</Text>
          </TouchableOpacity>
        </View>
      ) : (
        <FlatList
          data={filteredPlaces}
          renderItem={renderPlaceItem}
          keyExtractor={item => item.id}
          contentContainerStyle={styles.listContainer}
          showsVerticalScrollIndicator={false}
        />
      )}

      <FooterNav navigation={navigation} activeScreen="Acote" />
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
    padding: SPACING.lg,
    paddingTop: Platform.OS === 'android' ? StatusBar.currentHeight + SPACING.md : SPACING.lg,
    borderBottomLeftRadius: 20,
    borderBottomRightRadius: 20,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.2,
    shadowRadius: 4,
    elevation: 4,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  headerContent: {
    flex: 1,
  },
  title: {
    fontSize: FONT_SIZE.xl,
    fontWeight: getFontWeight('bold'),
    color: COLORS.white,
  },
  subtitle: {
    fontSize: FONT_SIZE.md,
    color: COLORS.white,
    opacity: 0.9,
    marginTop: SPACING.xs,
  },
  reservationButton: {
    backgroundColor: COLORS.secondary,
    borderRadius: 8,
    paddingVertical: SPACING.sm,
    paddingHorizontal: SPACING.md,
    flexDirection: 'row',
    alignItems: 'center',
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.2,
    shadowRadius: 2,
    elevation: 2,
  },
  reservationButtonText: {
    color: COLORS.white,
    fontWeight: getFontWeight('bold'),
    marginLeft: SPACING.xs,
    fontSize: FONT_SIZE.sm,
  },
  filtersContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    padding: SPACING.md,
    backgroundColor: COLORS.white,
    borderRadius: 12,
    margin: SPACING.md,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  filterChip: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: COLORS.light_gray,
    paddingVertical: SPACING.xs,
    paddingHorizontal: SPACING.sm,
    borderRadius: 20,
  },
  filterChipText: {
    fontSize: FONT_SIZE.xs,
    marginLeft: SPACING.xs,
    color: COLORS.black,
    fontWeight: getFontWeight('medium'),
  },
  listContainer: {
    padding: SPACING.md,
    paddingBottom: SPACING.xl * 2,
  },
  placeCard: {
    backgroundColor: COLORS.white,
    borderRadius: 12,
    padding: SPACING.md,
    marginBottom: SPACING.md,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  placeHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: SPACING.sm,
  },
  placeIconContainer: {
    width: 44,
    height: 44,
    borderRadius: 22,
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: SPACING.sm,
  },
  placeInfo: {
    flex: 1,
  },
  placeName: {
    fontSize: FONT_SIZE.md,
    fontWeight: getFontWeight('bold'),
    color: COLORS.black,
    marginBottom: SPACING.xs / 2,
  },
  placeAddress: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.gray,
  },
  placeDistance: {
    backgroundColor: COLORS.light_gray,
    paddingVertical: SPACING.xs / 2,
    paddingHorizontal: SPACING.sm,
    borderRadius: 12,
  },
  distanceText: {
    fontSize: FONT_SIZE.xs,
    fontWeight: getFontWeight('medium'),
    color: COLORS.gray,
  },
  placeDescription: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    marginBottom: SPACING.md,
    lineHeight: FONT_SIZE.md * 1.3,
  },
  placeActions: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  actionButton: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: COLORS.primary,
    paddingVertical: SPACING.xs,
    paddingHorizontal: SPACING.md,
    borderRadius: 8,
    flex: 1,
    marginRight: SPACING.xs,
    justifyContent: 'center',
  },
  actionButtonSecondary: {
    backgroundColor: COLORS.white,
    borderWidth: 1,
    borderColor: COLORS.primary,
    marginRight: 0,
    marginLeft: SPACING.xs,
  },
  actionButtonText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.sm,
    fontWeight: getFontWeight('medium'),
    marginLeft: SPACING.xs,
  },
  actionButtonTextSecondary: {
    color: COLORS.primary,
    fontSize: FONT_SIZE.sm,
    fontWeight: getFontWeight('medium'),
    marginLeft: SPACING.xs,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: SPACING.xl,
  },
  loadingText: {
    fontSize: FONT_SIZE.md,
    color: COLORS.primary,
    marginTop: SPACING.md,
    textAlign: 'center',
  },
  errorContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: SPACING.xl,
  },
  errorText: {
    fontSize: FONT_SIZE.md,
    color: COLORS.gray,
    marginTop: SPACING.md,
    marginBottom: SPACING.lg,
    textAlign: 'center',
  },
  retryButton: {
    backgroundColor: COLORS.primary,
    paddingVertical: SPACING.sm,
    paddingHorizontal: SPACING.lg,
    borderRadius: 8,
  },
  retryButtonText: {
    color: COLORS.white,
    fontWeight: getFontWeight('bold'),
  },
});

export default AcoteScreen;
