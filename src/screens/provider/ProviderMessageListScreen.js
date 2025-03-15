import React, { useState, useEffect } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  FlatList, 
  TouchableOpacity, 
  SafeAreaView,
  StatusBar,
  Platform
} from 'react-native';
import { COLORS } from '../../theme/colors';
import { SPACING } from '../../theme/spacing';
import { FONT_SIZE, FONT_WEIGHT } from '../../theme/typography';
import { MessageCircle, ArrowLeft } from 'lucide-react-native';
import * as Animatable from 'react-native-animatable';

// Mock data for provider message threads
const MOCK_PROVIDER_MESSAGES = [
  {
    id: '1',
    userId: 'user1',
    userName: 'Sophie Martin',
    lastMessage: 'Est-ce que je peux réserver pour 4 personnes ce samedi ?',
    timestamp: new Date(Date.now() - 1000 * 60 * 30),
    unread: true,
    regarding: 'Réservation',
  },
  {
    id: '2',
    userId: 'user2',
    userName: 'Thomas Bernard',
    lastMessage: 'Quelles sont vos heures d\'ouverture le dimanche ?',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 5),
    unread: false,
    regarding: 'Information',
  },
  {
    id: '3',
    userId: 'user3',
    userName: 'Léa Dubois',
    lastMessage: 'Merci pour l\'excellent service hier soir !',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24 * 2),
    unread: false,
    regarding: 'Commentaire',
  },
];

export default function ProviderMessageListScreen({ navigation }) {
  const [messageThreads, setMessageThreads] = useState(MOCK_PROVIDER_MESSAGES);

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
    
    // Navigate to the message screen with the user's info
    navigation.navigate('Message', { 
      userId: thread.userId,
      userName: thread.userName,
      // For providers, we're reusing the same message screen but with customer info
      providerId: null, 
      providerName: null
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
          <View style={[styles.threadIcon, getRegardingStyle(item.regarding)]}>
            <MessageCircle size={22} color={COLORS.white} />
          </View>
          {item.unread && <View style={styles.unreadDot} />}
        </View>
        
        <View style={styles.threadContent}>
          <View style={styles.threadHeader}>
            <Text style={styles.userName}>{item.userName}</Text>
            <Text style={styles.timestamp}>{formatTime(item.timestamp)}</Text>
          </View>
          <View style={styles.regarding}>
            <Text style={styles.regardingText}>{item.regarding}</Text>
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

  // Helper to get style based on message type
  const getRegardingStyle = (regarding) => {
    switch (regarding) {
      case 'Réservation':
        return { backgroundColor: COLORS.primary };
      case 'Information':
        return { backgroundColor: COLORS.info };
      case 'Commentaire':
        return { backgroundColor: COLORS.tertiary };
      default:
        return { backgroundColor: COLORS.secondary };
    }
  };

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor={COLORS.primary} barStyle="light-content" />
      
      <View style={styles.header}>
        <TouchableOpacity 
          style={styles.backButton}
          onPress={() => navigation.goBack()}
        >
          <ArrowLeft color={COLORS.white} size={24} />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>Messages Clients</Text>
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
            Les messages de vos clients apparaîtront ici
          </Text>
        </View>
      )}
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
    flexDirection: 'row',
    alignItems: 'center',
  },
  backButton: {
    padding: SPACING.xs,
    marginRight: SPACING.sm,
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
  userName: {
    fontSize: FONT_SIZE.md,
    fontWeight: FONT_WEIGHT.bold,
    color: COLORS.black,
  },
  timestamp: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.gray,
  },
  regarding: {
    marginBottom: SPACING.xs,
  },
  regardingText: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.primary,
    fontWeight: FONT_WEIGHT.medium,
    backgroundColor: COLORS.badge,
    paddingHorizontal: SPACING.xs,
    paddingVertical: SPACING.xxs,
    borderRadius: 4,
    alignSelf: 'flex-start',
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
});
