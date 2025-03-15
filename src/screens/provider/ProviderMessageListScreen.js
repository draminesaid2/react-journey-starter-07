
import React, { useState } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  FlatList, 
  TouchableOpacity, 
  SafeAreaView,
  StatusBar,
  Platform,
  Image
} from 'react-native';
import { COLORS } from '../../theme/colors';
import { SPACING } from '../../theme/spacing';
import { FONT_SIZE, FONT_WEIGHT } from '../../theme/typography';
import { MessageCircle, ArrowLeft, Clock } from 'lucide-react-native';
import * as Animatable from 'react-native-animatable';
import { cardStyle, lightShadow } from '../../theme/mixins';

// Mock data for provider message threads
const MOCK_PROVIDER_MESSAGES = [
  {
    id: '1',
    userId: 'user1',
    userName: 'Sophie Martin',
    userAvatar: 'https://randomuser.me/api/portraits/women/44.jpg',
    lastMessage: 'Est-ce que je peux réserver pour 4 personnes ce samedi ?',
    timestamp: new Date(Date.now() - 1000 * 60 * 30),
    unread: true,
    regarding: 'Réservation',
  },
  {
    id: '2',
    userId: 'user2',
    userName: 'Thomas Bernard',
    userAvatar: 'https://randomuser.me/api/portraits/men/32.jpg',
    lastMessage: 'Quelles sont vos heures d\'ouverture le dimanche ?',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 5),
    unread: false,
    regarding: 'Information',
  },
  {
    id: '3',
    userId: 'user3',
    userName: 'Léa Dubois',
    userAvatar: 'https://randomuser.me/api/portraits/women/63.jpg',
    lastMessage: 'Merci pour l\'excellent service hier soir !',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24 * 2),
    unread: false,
    regarding: 'Commentaire',
  },
  {
    id: '4',
    userId: 'user4',
    userName: 'Antoine Moreau',
    userAvatar: 'https://randomuser.me/api/portraits/men/22.jpg',
    lastMessage: 'Je voudrais savoir si vous avez des places disponibles pour demain soir ?',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 8),
    unread: true,
    regarding: 'Réservation',
  },
  {
    id: '5',
    userId: 'user5',
    userName: 'Marie Leroy',
    userAvatar: 'https://randomuser.me/api/portraits/women/18.jpg',
    lastMessage: 'Pouvez-vous m\'envoyer des informations sur vos événements à venir ?',
    timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24),
    unread: false,
    regarding: 'Information',
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
      isProvider: true
    });
  };

  const renderMessageCard = ({ item }) => (
    <Animatable.View 
      animation="fadeIn" 
      duration={500}
      style={styles.cardContainer}
    >
      <TouchableOpacity
        style={[styles.messageCard, item.unread && styles.unreadCard]}
        onPress={() => handleThreadPress(item)}
      >
        <View style={styles.cardHeader}>
          <View style={styles.userInfoContainer}>
            <Image 
              source={{ uri: item.userAvatar }} 
              style={styles.userAvatar} 
            />
            <View>
              <Text style={styles.userName}>{item.userName}</Text>
              <View style={styles.regardingBadge}>
                <Text style={styles.regardingText}>{item.regarding}</Text>
              </View>
            </View>
          </View>
          <View style={styles.timeContainer}>
            <Clock size={12} color={COLORS.gray} style={styles.clockIcon} />
            <Text style={styles.timestamp}>{formatTime(item.timestamp)}</Text>
            {item.unread && <View style={styles.unreadDot} />}
          </View>
        </View>
        
        <View style={styles.messageContent}>
          <Text 
            style={[styles.lastMessage, item.unread && styles.unreadText]}
            numberOfLines={2}
          >
            {item.lastMessage}
          </Text>
        </View>
        
        <View style={styles.cardFooter}>
          <TouchableOpacity style={styles.replyButton}>
            <Text style={styles.replyButtonText}>Répondre</Text>
          </TouchableOpacity>
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
          <ArrowLeft color={COLORS.white} size={24} />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>Messages Clients</Text>
      </View>

      <View style={styles.statsContainer}>
        <View style={styles.statItem}>
          <Text style={styles.statNumber}>{messageThreads.filter(m => m.unread).length}</Text>
          <Text style={styles.statLabel}>Non lus</Text>
        </View>
        <View style={styles.statItem}>
          <Text style={styles.statNumber}>{messageThreads.length}</Text>
          <Text style={styles.statLabel}>Total</Text>
        </View>
        <View style={styles.statItem}>
          <Text style={styles.statNumber}>{messageThreads.filter(m => m.regarding === 'Réservation').length}</Text>
          <Text style={styles.statLabel}>Réservations</Text>
        </View>
      </View>

      {messageThreads.length > 0 ? (
        <FlatList
          data={messageThreads}
          renderItem={renderMessageCard}
          keyExtractor={item => item.id}
          contentContainerStyle={styles.list}
        />
      ) : (
        <View style={styles.emptyContainer}>
          <MessageCircle size={60} color={COLORS.gray_light} />
          <Text style={styles.emptyText}>Aucun message</Text>
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
  statsContainer: {
    flexDirection: 'row',
    backgroundColor: COLORS.white,
    padding: SPACING.md,
    marginBottom: SPACING.sm,
    ...lightShadow,
  },
  statItem: {
    flex: 1,
    alignItems: 'center',
    borderRightWidth: 1,
    borderRightColor: COLORS.light_gray,
    paddingVertical: SPACING.xs,
  },
  statItem: {
    flex: 1,
    alignItems: 'center',
  },
  statNumber: {
    fontSize: FONT_SIZE.lg,
    fontWeight: FONT_WEIGHT.bold,
    color: COLORS.primary,
  },
  statLabel: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.gray,
    marginTop: SPACING.xxs,
  },
  list: {
    padding: SPACING.sm,
  },
  cardContainer: {
    marginBottom: SPACING.md,
    width: '100%',
  },
  messageCard: {
    backgroundColor: COLORS.white,
    borderRadius: 12,
    padding: SPACING.md,
    ...cardStyle,
  },
  unreadCard: {
    borderLeftWidth: 4,
    borderLeftColor: COLORS.primary,
  },
  cardHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: SPACING.sm,
  },
  userInfoContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  userAvatar: {
    width: 40,
    height: 40,
    borderRadius: 20,
    marginRight: SPACING.sm,
  },
  userName: {
    fontSize: FONT_SIZE.md,
    fontWeight: FONT_WEIGHT.bold,
    color: COLORS.black,
  },
  regardingBadge: {
    backgroundColor: COLORS.badge,
    paddingHorizontal: SPACING.xs,
    paddingVertical: SPACING.xxs,
    borderRadius: 4,
    marginTop: SPACING.xxs,
    alignSelf: 'flex-start',
  },
  regardingText: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.primary,
    fontWeight: FONT_WEIGHT.medium,
  },
  timeContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  clockIcon: {
    marginRight: SPACING.xxs,
  },
  timestamp: {
    fontSize: FONT_SIZE.xs,
    color: COLORS.gray,
  },
  unreadDot: {
    width: 8,
    height: 8,
    borderRadius: 4,
    backgroundColor: COLORS.primary,
    marginLeft: SPACING.xs,
  },
  messageContent: {
    marginBottom: SPACING.md,
    paddingHorizontal: SPACING.xs,
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
  cardFooter: {
    borderTopWidth: 1,
    borderTopColor: COLORS.light_gray,
    paddingTop: SPACING.sm,
    alignItems: 'flex-end',
  },
  replyButton: {
    backgroundColor: COLORS.primary_light,
    paddingHorizontal: SPACING.md,
    paddingVertical: SPACING.xs,
    borderRadius: 16,
  },
  replyButtonText: {
    color: COLORS.primary,
    fontSize: FONT_SIZE.sm,
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
