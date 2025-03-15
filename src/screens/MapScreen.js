
import React, { useState, useRef, useEffect } from 'react';
import { View, StyleSheet, SafeAreaView, Text, Platform, TouchableOpacity, Animated, StatusBar } from 'react-native';
import MapView, { PROVIDER_GOOGLE, Marker, Callout, Circle } from 'react-native-maps';
import * as Location from 'expo-location';
import { FooterNav } from '../components/FooterNav';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE, FONT_WEIGHT, getFontWeight } from '../theme/typography';
import { useTranslation } from 'react-i18next';
import { MapPin, Navigation2, Info, Hotel, Utensils, Landmark, Crosshair, Coffee, Compass, AlertCircle } from 'lucide-react-native';

const places = [
  {
    id: 1,
    name: 'Bulla Regia',
    description: 'Site archéologique romain unique avec des villas souterraines',
    coordinate: {
      latitude: 36.5587,
      longitude: 8.7525,
    },
    type: 'landmark',
  },
  {
    id: 2,
    name: 'Chemtou',
    description: 'Ancient site de marbre avec musée archéologique',
    coordinate: {
      latitude: 36.4894,
      longitude: 8.5706,
    },
    type: 'landmark',
  },
  {
    id: 3,
    name: 'Hôtel Thugga',
    description: 'Hôtel 3 étoiles au centre-ville',
    coordinate: {
      latitude: 36.5012,
      longitude: 8.7845,
    },
    type: 'hotel',
  },
  {
    id: 4,
    name: 'Atlas Hotel',
    description: 'Hôtel moderne avec vue panoramique',
    coordinate: {
      latitude: 36.4982,
      longitude: 8.7801,
    },
    type: 'hotel',
  },
  {
    id: 5,
    name: 'Restaurant El Fell',
    description: 'Cuisine traditionnelle tunisienne',
    coordinate: {
      latitude: 36.5025,
      longitude: 8.7815,
    },
    type: 'restaurant',
  },
  {
    id: 6,
    name: 'Restaurant Le Kef',
    description: 'Spécialités locales et ambiance chaleureuse',
    coordinate: {
      latitude: 36.5001,
      longitude: 8.7790,
    },
    type: 'restaurant',
  },
  {
    id: 7,
    name: 'Café Andalous',
    description: 'Café traditionnel avec terrasse',
    coordinate: {
      latitude: 36.5040,
      longitude: 8.7830,
    },
    type: 'cafe',
  }
];

const mapStyle = [
  {
    featureType: 'water',
    elementType: 'geometry',
    stylers: [{ color: '#e9e9e9' }, { lightness: 17 }],
  },
  {
    featureType: 'landscape',
    elementType: 'geometry',
    stylers: [{ color: '#f5f5f5' }, { lightness: 20 }],
  },
  {
    featureType: 'road.highway',
    elementType: 'geometry.fill',
    stylers: [{ color: '#ffffff' }, { lightness: 17 }],
  },
  {
    featureType: 'poi.park',
    elementType: 'geometry',
    stylers: [{ color: '#e5e5e5' }, { lightness: 21 }],
  },
];

const MapScreen = ({ navigation }) => {
  const { t } = useTranslation();
  const [selectedMarker, setSelectedMarker] = useState(null);
  const [filterType, setFilterType] = useState(null);
  const [location, setLocation] = useState(null);
  const [errorMsg, setErrorMsg] = useState(null);
  const [loading, setLoading] = useState(true);
  const mapRef = useRef(null);
  const [fadeAnim] = useState(new Animated.Value(0));
  
  const initialRegion = {
    latitude: 36.5019,
    longitude: 8.7801,
    latitudeDelta: 0.0922,
    longitudeDelta: 0.0421,
  };

  useEffect(() => {
    let isMounted = true;
    
    const getLocation = async () => {
      try {
        setLoading(true);
        let { status } = await Location.requestForegroundPermissionsAsync();
        
        if (status !== 'granted') {
          if (isMounted) {
            setErrorMsg('Permission to access location was denied');
            setLoading(false);
          }
          return;
        }

        // Set a timeout to prevent infinite loading
        const locationTimeout = setTimeout(() => {
          if (isMounted && loading) {
            console.log('Location request timed out');
            setLoading(false);
            // Use default location if we timeout
            if (!location) {
              mapRef.current?.animateToRegion(initialRegion, 1000);
            }
          }
        }, 5000);

        try {
          let location = await Location.getCurrentPositionAsync({
            accuracy: Location.Accuracy.Low, // Lower accuracy for faster response
            timeout: 5000 // 5 second timeout
          });
          
          if (isMounted) {
            clearTimeout(locationTimeout);
            setLocation(location);
            setLoading(false);
            
            if (mapRef.current) {
              mapRef.current.animateToRegion({
                latitude: location.coords.latitude,
                longitude: location.coords.longitude,
                latitudeDelta: 0.0922,
                longitudeDelta: 0.0421,
              }, 1000);
            }
          }
        } catch (locationError) {
          console.error('Error getting precise location:', locationError);
          clearTimeout(locationTimeout);
          
          if (isMounted) {
            // Fall back to default region
            setLoading(false);
            mapRef.current?.animateToRegion(initialRegion, 1000);
          }
        }
      } catch (error) {
        console.error('Location error:', error);
        if (isMounted) {
          setErrorMsg('Error getting location');
          setLoading(false);
        }
      }
    };
    
    getLocation();
    
    Animated.timing(fadeAnim, {
      toValue: 1,
      duration: 500,
      useNativeDriver: true,
    }).start();
    
    return () => {
      isMounted = false;
    };
  }, []);

  const getMarkerIcon = (type) => {
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

  const getMarkerColor = (type) => {
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

  const goToCurrentLocation = async () => {
    try {
      if (location) {
        mapRef.current?.animateToRegion({
          latitude: location.coords.latitude,
          longitude: location.coords.longitude,
          latitudeDelta: 0.0922,
          longitudeDelta: 0.0421,
        }, 1000);
      } else {
        const { coords } = await Location.getCurrentPositionAsync({});
        setLocation({ coords });
        mapRef.current?.animateToRegion({
          latitude: coords.latitude,
          longitude: coords.longitude,
          latitudeDelta: 0.0922,
          longitudeDelta: 0.0421,
        }, 1000);
      }
    } catch (error) {
      console.error('Error getting location:', error);
    }
  };

  const filterPlaces = () => {
    if (!filterType) return places;
    return places.filter(place => place.type === filterType);
  };

  const toggleFilter = (type) => {
    if (filterType === type) {
      setFilterType(null);
    } else {
      setFilterType(type);
    }
  };

  if (errorMsg) {
    return (
      <SafeAreaView style={styles.container}>
        <StatusBar barStyle="light-content" backgroundColor={COLORS.primary_dark} />
        <View style={styles.header}>
          <Text style={styles.title}>{t('map.title') || 'Carte'}</Text>
          <Text style={styles.subtitle}>{t('map.subtitle') || 'Découvrez les sites et attractions'}</Text>
        </View>
        
        <View style={styles.errorContainer}>
          <AlertCircle size={64} color={COLORS.error} />
          <Text style={styles.errorText}>{errorMsg}</Text>
          <TouchableOpacity 
            style={styles.retryButton}
            onPress={() => navigation.navigate('Map')}
          >
            <Text style={styles.retryButtonText}>Réessayer</Text>
          </TouchableOpacity>
        </View>
        
        <FooterNav navigation={navigation} activeScreen="Map" />
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar barStyle="light-content" backgroundColor={COLORS.primary_dark} />
      <View style={styles.header}>
        <Text style={styles.title}>{t('map.title') || 'Carte'}</Text>
        <Text style={styles.subtitle}>{t('map.subtitle') || 'Découvrez les sites et attractions'}</Text>
      </View>

      <Animated.View style={[styles.mapContainerFullscreen, { opacity: fadeAnim }]}>
        {loading ? (
          <View style={styles.loadingContainer}>
            <Compass size={48} color={COLORS.primary} />
            <Text style={styles.loadingText}>Chargement de la carte...</Text>
          </View>
        ) : (
          <MapView
            ref={mapRef}
            provider={PROVIDER_GOOGLE}
            style={styles.mapFullscreen}
            initialRegion={initialRegion}
            customMapStyle={mapStyle}
            showsUserLocation={true}
            showsMyLocationButton={false}
            showsCompass={true}
            rotateEnabled={true}
            zoomEnabled={true}
            onMapReady={() => {
              console.log('Map is ready');
              setLoading(false);
            }}
            onError={(error) => {
              console.error('Map error:', error);
              setLoading(false);
              setErrorMsg('Error loading map');
            }}
          >
            {location && (
              <Circle
                center={{
                  latitude: location.coords.latitude,
                  longitude: location.coords.longitude,
                }}
                radius={300}
                fillColor={COLORS.info + '30'}
                strokeColor={COLORS.info}
                strokeWidth={1}
              />
            )}
            
            {filterPlaces().map((place) => (
              <React.Fragment key={place.id}>
                <Circle
                  center={place.coordinate}
                  radius={300}
                  fillColor={`${getMarkerColor(place.type)}20`}
                  strokeColor={getMarkerColor(place.type)}
                  strokeWidth={1}
                />
                <Marker
                  coordinate={place.coordinate}
                  onPress={() => setSelectedMarker(place.id)}
                >
                  <View style={[
                    styles.markerContainer,
                    selectedMarker === place.id && styles.selectedMarker
                  ]}>
                    {getMarkerIcon(place.type)}
                    <View style={[
                      styles.markerDot,
                      { backgroundColor: getMarkerColor(place.type) }
                    ]} />
                  </View>
                  <Callout 
                    tooltip
                    onPress={() => navigation.navigate('HistoricalPlaces')}
                  >
                    <View style={styles.calloutContainer}>
                      <View style={styles.callout}>
                        <Text style={styles.calloutTitle}>{place.name}</Text>
                        <Text style={styles.calloutDescription}>{place.description}</Text>
                        <View style={styles.calloutButtons}>
                          <TouchableOpacity 
                            style={styles.calloutButton}
                            onPress={() => navigation.navigate('HistoricalPlaces')}
                          >
                            <Info size={16} color={COLORS.white} />
                            <Text style={styles.calloutButtonText}>{t('map.details') || 'Détails'}</Text>
                          </TouchableOpacity>
                          <TouchableOpacity 
                            style={[styles.calloutButton, styles.calloutButtonSecondary]}
                          >
                            <Navigation2 size={16} color={COLORS.primary} />
                            <Text style={styles.calloutButtonTextSecondary}>{t('map.directions') || 'Itinéraire'}</Text>
                          </TouchableOpacity>
                        </View>
                      </View>
                      <View style={styles.calloutArrow} />
                    </View>
                  </Callout>
                </Marker>
              </React.Fragment>
            ))}
          </MapView>
        )}
        
        <TouchableOpacity 
          style={styles.locationButton}
          onPress={goToCurrentLocation}
        >
          <Crosshair size={24} color={COLORS.white} />
        </TouchableOpacity>

        <View style={styles.filtersContainer}>
          <TouchableOpacity 
            style={[
              styles.filterChip,
              filterType === 'landmark' && { backgroundColor: COLORS.primary }
            ]}
            onPress={() => toggleFilter('landmark')}
          >
            <Landmark size={18} color={filterType === 'landmark' ? COLORS.white : COLORS.primary} />
            <Text 
              style={[
                styles.filterChipText,
                filterType === 'landmark' && { color: COLORS.white }
              ]}
            >
              {t('map.landmarks') || 'Sites'}
            </Text>
          </TouchableOpacity>
          
          <TouchableOpacity 
            style={[
              styles.filterChip,
              filterType === 'hotel' && { backgroundColor: COLORS.secondary }
            ]}
            onPress={() => toggleFilter('hotel')}
          >
            <Hotel size={18} color={filterType === 'hotel' ? COLORS.white : COLORS.secondary} />
            <Text 
              style={[
                styles.filterChipText,
                filterType === 'hotel' && { color: COLORS.white }
              ]}
            >
              {t('map.hotels') || 'Hôtels'}
            </Text>
          </TouchableOpacity>
          
          <TouchableOpacity 
            style={[
              styles.filterChip,
              filterType === 'restaurant' && { backgroundColor: COLORS.error }
            ]}
            onPress={() => toggleFilter('restaurant')}
          >
            <Utensils size={18} color={filterType === 'restaurant' ? COLORS.white : COLORS.error} />
            <Text 
              style={[
                styles.filterChipText,
                filterType === 'restaurant' && { color: COLORS.white }
              ]}
            >
              {t('map.restaurants') || 'Restaurants'}
            </Text>
          </TouchableOpacity>
          
          <TouchableOpacity 
            style={[
              styles.filterChip,
              filterType === 'cafe' && { backgroundColor: COLORS.warning }
            ]}
            onPress={() => toggleFilter('cafe')}
          >
            <Coffee size={18} color={filterType === 'cafe' ? COLORS.white : COLORS.warning} />
            <Text 
              style={[
                styles.filterChipText,
                filterType === 'cafe' && { color: COLORS.white }
              ]}
            >
              Cafés
            </Text>
          </TouchableOpacity>
        </View>
      </Animated.View>

      <FooterNav navigation={navigation} activeScreen="Map" />
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
  mapContainerFullscreen: {
    flex: 1,
    position: 'relative',
  },
  mapFullscreen: {
    ...StyleSheet.absoluteFillObject,
  },
  markerContainer: {
    alignItems: 'center',
    justifyContent: 'center',
    padding: 8,
  },
  selectedMarker: {
    transform: [{ scale: 1.2 }],
  },
  markerDot: {
    width: 8,
    height: 8,
    borderRadius: 4,
    position: 'absolute',
    bottom: 4,
  },
  calloutContainer: {
    width: 250,
    backgroundColor: 'transparent',
  },
  callout: {
    backgroundColor: COLORS.white,
    borderRadius: 12,
    padding: SPACING.md,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  calloutTitle: {
    fontSize: FONT_SIZE.md,
    fontWeight: getFontWeight('bold'),
    color: COLORS.black,
    marginBottom: SPACING.xs,
  },
  calloutDescription: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    marginBottom: SPACING.md,
  },
  calloutButtons: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  calloutButton: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: COLORS.primary,
    paddingVertical: SPACING.xs,
    paddingHorizontal: SPACING.sm,
    borderRadius: 8,
    flex: 1,
    marginRight: SPACING.xs,
    justifyContent: 'center',
  },
  calloutButtonSecondary: {
    backgroundColor: COLORS.white,
    borderWidth: 1,
    borderColor: COLORS.primary,
    marginRight: 0,
    marginLeft: SPACING.xs,
  },
  calloutButtonText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.sm,
    fontWeight: getFontWeight('medium'),
    marginLeft: SPACING.xs,
  },
  calloutButtonTextSecondary: {
    color: COLORS.primary,
    fontSize: FONT_SIZE.sm,
    fontWeight: getFontWeight('medium'),
    marginLeft: SPACING.xs,
  },
  calloutArrow: {
    backgroundColor: 'transparent',
    borderWidth: 16,
    borderColor: 'transparent',
    borderTopColor: COLORS.white,
    alignSelf: 'center',
    marginTop: -0.5,
  },
  locationButton: {
    position: 'absolute',
    right: SPACING.lg,
    bottom: SPACING.xl,
    backgroundColor: COLORS.primary,
    padding: SPACING.md,
    borderRadius: 50,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
  filtersContainer: {
    position: 'absolute',
    top: SPACING.lg,
    left: SPACING.lg,
    right: SPACING.lg,
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
  },
  filterChip: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: COLORS.white,
    paddingVertical: SPACING.xs,
    paddingHorizontal: SPACING.sm,
    borderRadius: 20,
    marginBottom: SPACING.xs,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.2,
    shadowRadius: 2,
    elevation: 2,
  },
  filterChipText: {
    fontSize: FONT_SIZE.xs,
    marginLeft: SPACING.xs,
    color: COLORS.black,
    fontWeight: getFontWeight('medium'),
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  loadingText: {
    marginTop: SPACING.md,
    fontSize: FONT_SIZE.md,
    color: COLORS.primary,
  },
  errorContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: SPACING.xl,
    margin: SPACING.lg,
    backgroundColor: COLORS.white,
    borderRadius: 12,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  errorText: {
    fontSize: FONT_SIZE.md,
    color: COLORS.gray,
    textAlign: 'center',
    marginTop: SPACING.md,
    marginBottom: SPACING.lg,
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
    fontSize: FONT_SIZE.sm,
  }
});

export default MapScreen;
