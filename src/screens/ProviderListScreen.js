import React, { useState } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  FlatList, 
  TouchableOpacity, 
  Image, 
  SafeAreaView,
  StatusBar,
  TextInput,
  Platform
} from 'react-native';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE } from '../theme/typography';
import * as Animatable from 'react-native-animatable';
import { useAuth } from '../auth/useAuth';
import { useTranslation } from 'react-i18next';

// Mock data for providers
const PROVIDERS = [
  {
    id: '1',
    name: 'Musée Carthage',
    type: 'Lieu historique',
    image: 'https://images.unsplash.com/photo-1560184611-ff3e53f00e8f',
    rating: 4.8,
    location: 'Carthage, Tunis'
  },
  {
    id: '2',
    name: 'Amphithéâtre El Jem',
    type: 'Monument',
    image: 'https://images.unsplash.com/photo-1604580864964-0462f5d5b1a8',
    rating: 4.9,
    location: 'El Jem, Mahdia'
  },
  {
    id: '3',
    name: 'Parc archéologique Dougga',
    type: 'Site archéologique',
    image: 'https://images.unsplash.com/photo-1549993900-7d3cd358d607',
    rating: 4.7,
    location: 'Dougga, Béja'
  },
  {
    id: '4',
    name: 'Musée du Bardo',
    type: 'Musée',
    image: 'https://images.unsplash.com/photo-1594736797933-d0501ba2fe65',
    rating: 4.8,
    location: 'Bardo, Tunis'
  },
  {
    id: '5',
    name: 'Site archéologique Bulla Regia',
    type: 'Site archéologique',
    image: 'https://images.unsplash.com/photo-1518998053901-5348d3961a04',
    rating: 4.6,
    location: 'Jendouba'
  }
];

export default function ProviderListScreen({ navigation }) {
  const [searchQuery, setSearchQuery] = useState('');
  const { isSignedIn, user } = useAuth();
  const { t } = useTranslation();

  const filteredProviders = PROVIDERS.filter(
    provider => provider.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    provider.type.toLowerCase().includes(searchQuery.toLowerCase()) ||
    provider.location.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const renderProviderItem = ({ item, index }) => (
    <Animatable.View
      animation="fadeInUp"
      delay={index * 100}
      duration={500}
    >
      <TouchableOpacity
        style={styles.providerCard}
        onPress={() => navigation.navigate('Message', { providerId: item.id, providerName: item.name })}
      >
        <Image
          source={{ uri: item.image }}
          style={styles.providerImage}
          resizeMode="cover"
        />
        <View style={styles.providerInfo}>
          <Text style={styles.providerName}>{item.name}</Text>
          <Text style={styles.providerType}>{item.type}</Text>
          <Text style={styles.providerLocation}>{item.location}</Text>
          <View style={styles.ratingContainer}>
            <Text style={styles.ratingText}>★ {item.rating}</Text>
          </View>
        </View>
        <View style={styles.messageIcon}>
          <Text style={styles.messageIconText}>💬</Text>
        </View>
      </TouchableOpacity>
    </Animatable.View>
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
        <Text style={styles.headerTitle}>Prestataires</Text>
      </View>

      <View style={styles.searchContainer}>
        <TextInput
          style={styles.searchInput}
          placeholder="Rechercher un prestataire..."
          placeholderTextColor={COLORS.gray}
          value={searchQuery}
          onChangeText={setSearchQuery}
        />
      </View>

      <FlatList
        data={filteredProviders}
        renderItem={renderProviderItem}
        keyExtractor={item => item.id}
        contentContainerStyle={styles.listContainer}
        showsVerticalScrollIndicator={false}
      />
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: COLORS.white,
  },
  header: {
    backgroundColor: COLORS.primary,
    paddingTop: Platform.OS === 'android' ? StatusBar.currentHeight : SPACING.md,
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
  searchContainer: {
    paddingHorizontal: SPACING.lg,
    paddingVertical: SPACING.md,
    backgroundColor: COLORS.light_gray,
  },
  searchInput: {
    backgroundColor: COLORS.white,
    borderRadius: 10,
    padding: SPACING.md,
    fontSize: FONT_SIZE.md,
    borderWidth: 1,
    borderColor: COLORS.light_gray,
  },
  listContainer: {
    padding: SPACING.lg,
  },
  providerCard: {
    backgroundColor: COLORS.white,
    borderRadius: 15,
    marginBottom: SPACING.lg,
    overflow: 'hidden',
    flexDirection: 'row',
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
    borderWidth: 1,
    borderColor: COLORS.light_gray,
  },
  providerImage: {
    width: 100,
    height: 100,
    borderRadius: 10,
    margin: SPACING.sm,
  },
  providerInfo: {
    flex: 1,
    padding: SPACING.md,
    justifyContent: 'center',
  },
  providerName: {
    fontSize: FONT_SIZE.md,
    fontWeight: 'bold',
    color: COLORS.primary,
    marginBottom: SPACING.xs,
  },
  providerType: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    marginBottom: SPACING.xs,
  },
  providerLocation: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    marginBottom: SPACING.xs,
  },
  ratingContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  ratingText: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.secondary,
    fontWeight: 'bold',
  },
  messageIcon: {
    padding: SPACING.md,
    justifyContent: 'center',
    alignItems: 'center',
  },
  messageIconText: {
    fontSize: FONT_SIZE.xl,
  },
});
