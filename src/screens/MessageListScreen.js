import React, { useState, useEffect } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  FlatList, 
  TouchableOpacity, 
  SafeAreaView,
  StatusBar,
  Image
} from 'react-native';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE, FONT_WEIGHT } from '../theme/typography';
import { FooterNav } from '../components/FooterNav';
import { useAuth } from '../auth/useAuth';
import { MessageCircle } from 'lucide-react-native';
import * as Animatable from 'react-native-animatable';

// Mock data for message threads
const MOCK_MESSAGES = [
  {
    id: '1',
    providerId: 'provider1',
    providerName: 'Restaurant Dar El Jeld',
    lastMessage: 'Nous vous attendons pour votre réservation samedi.',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 2),
    unread: true,
    placeType: 'restaurant',
  },
  {
    id: '2',
    providerId: 'provider2',
    providerName: 'Musée du Bardo',
    lastMessage: 'Merci pour votre visite, n\'oubliez pas de laisser un avis!',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24),
    unread: false,
    placeType: 'museum',
  },
  {
    id: '3',
    providerId: 'provider3',
    providerName: 'Hôtel Dar El Medina',
    lastMessage: 'Votre demande de réservation a été confirmée.',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 48),
    unread: false,
    placeType: 'hotel',
  },
];

export default function MessageListScreen({ navigation }) {
  const [messageThreads, setMessageThreads] = useState(MOCK_MESSAGES);
  const { isSignedIn, user } = useAuth();

  const formatTime = (timestamp) => {
    const now = new Date();
    const messageDate = new Date(timestamp);
    
    // If same day, show time
    if (messageDate.toDateString() === now.toDateString()) {
      return messageDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    } 
    
    // If this year, show month and day
    if (messageDate.getFullYear() === now.getFullYear()) {
      return messageDate.toLocaleDateString([], { month: 'short', day: 'numeric' });
    }
    
    // Otherwise show date with year
    return messageDate.toLocaleDateString([], { year: 'numeric', month: 'short', day: 'numeric' });
  };

  const handleThreadPress = (thread) => {
    // Mark as read when opened
    setMessageThreads(prev => 
      prev.map(item => 
        item.id === thread.id ? { ...item, unread: false } : item
      )
    );
    
    // Navigate to the message screen with the provider's info
    navigation.navigate('Message', { 
      providerId: thread.providerId,
      providerName: thread.providerName
    });
  };

  const renderThreadItem = ({ item }) => (
    <Animatable.View 
      animation="fadeIn" 
      duration={500}
    >
      <TouchableOpacity
        style={[styles.threadItem, item.unread && styles.unreadThread]}
        onPress={() => handleThreadPress(item)}
      >
        <View style={styles.threadIconContainer}>
          <View style={[styles.threadIcon, getPlaceTypeStyle(item.placeType)]}>
            <MessageCircle size={22} color={COLORS.white} />
          </View>
          {item.unread && <View style={styles.unreadDot} />}
        </View>
        
        <View style={styles.threadContent}>
          <View style={styles.threadHeader}>
            <Text style={styles.providerName}>{item.providerName}</Text>
            <Text style={styles.timestamp}>{formatTime(item.timestamp)}</Text>
          </View>
          <Text 
            style={[styles.lastMessage, item.unread && styles.unreadText]}
            numberOfLines={2}
          >
            {item.lastMessage}
          </Text>
        </View>
      </TouchableOpacity>
    </Animatable.View>
  );

  // Helper to get style based on place type
  const getPlaceTypeStyle = (type) => {
    switch (type) {
      case 'restaurant':
        return { backgroundColor: COLORS.primary };
      case 'museum':
        return { backgroundColor: COLORS.secondary };
      case 'hotel':
        return { backgroundColor: COLORS.tertiary };
      default:
        return { backgroundColor: COLORS.primary_light };
    }
  };

  if (!isSignedIn) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.centered}>
          <Text style={styles.title}>Veuillez vous connecter</Text>
          <TouchableOpacity 
            style={styles.loginButton}
            onPress={() => navigation.navigate('Login')}
          >
            <Text style={styles.loginButtonText}>Se connecter</Text>
          </TouchableOpacity>
        </View>
        <FooterNav navigation={navigation} activeScreen="MessageList" />
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor={COLORS.primary} barStyle="light-content" />
      
      <View style={styles.header}>
        <Text style={styles.headerTitle}>Messages</Text>
      </View>

      {messageThreads.length > 0 ? (
        <FlatList
          data={messageThreads}
          renderItem={renderThreadItem}
          keyExtractor={item => item.id}
          contentContainerStyle={styles.list}
        />
      ) : (
        <View style={styles.emptyContainer}>
          <MessageCircle size={60} color={COLORS.gray_light} />
          <Text style={styles.emptyText}>Aucun message pour le moment</Text>
          <Text style={styles.emptySubText}>
            Consultez les profils des prestataires pour commencer une conversation
          </Text>
        </View>
      )}

      <FooterNav navigation={navigation} activeScreen="MessageList" />
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
    paddingBottom: SPACING.md,
    paddingHorizontal: SPACING.lg,
  },
  headerTitle: {
    color: COLORS.white,
    fontSize: FONT_SIZE.lg,
    fontWeight: FONT_WEIGHT.bold,
    paddingVertical: SPACING.sm,
  },
  list: {
    padding: SPACING.sm,
  },
  threadItem: {
    flexDirection: 'row',
    backgroundColor: COLORS.white,
    borderRadius: 8,
    marginBottom: SPACING.sm,
    padding: SPACING.md,
    shadowColor: COLORS.black,
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.1,
    shadowRadius: 2,
    elevation: 2,
  },
  unreadThread: {
    backgroundColor: COLORS.light_gray,
    borderLeftWidth: 3,
    borderLeftColor: COLORS.primary,
  },
  threadIconContainer: {
    position: 'relative',
    marginRight: SPACING.md,
  },
  threadIcon: {
    width: 44,
    height: 44,
    borderRadius: 22,
    justifyContent: 'center',
    alignItems: 'center',
  },
  unreadDot: {
    position: 'absolute',
    top: 0,
    right: 0,
    width: 12,
    height: 12,
    borderRadius: 6,
    backgroundColor: COLORS.error,
    borderWidth: 2,
    borderColor: COLORS.white,
  },
  threadContent: {
    flex: 1,
  },
  threadHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: SPACING.xs,
  },
  providerName: {
    fontSize: FONT_SIZE.md,
    fontWeight: FONT_WEIGHT.bold,
    color: COLORS.black,
  },
  timestamp: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.gray,
  },
  lastMessage: {
    fontSize: FONT_SIZE.sm,
    color: COLORS.gray,
    lineHeight: FONT_SIZE.md * 1.3,
  },
  unreadText: {
    color: COLORS.black,
    fontWeight: FONT_WEIGHT.medium,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: SPACING.xl,
  },
  emptyText: {
    fontSize: FONT_SIZE.lg,
    fontWeight: FONT_WEIGHT.medium,
    color: COLORS.gray,
    marginTop: SPACING.lg,
  },
  emptySubText: {
    fontSize: FONT_SIZE.md,
    color: COLORS.gray,
    textAlign: 'center',
    marginTop: SPACING.sm,
    paddingHorizontal: SPACING.lg,
  },
  centered: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: SPACING.xl,
  },
  title: {
    fontSize: FONT_SIZE.xl,
    fontWeight: FONT_WEIGHT.bold,
    color: COLORS.primary,
    marginBottom: SPACING.lg,
  },
  loginButton: {
    backgroundColor: COLORS.primary,
    paddingVertical: SPACING.md,
    paddingHorizontal: SPACING.lg,
    borderRadius: 8,
  },
  loginButtonText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.md,
    fontWeight: FONT_WEIGHT.bold,
  },
});
