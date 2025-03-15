
import React, { useState, useRef, useEffect } from 'react';
import { 
  StyleSheet, 
  Text, 
  View, 
  TextInput, 
  TouchableOpacity, 
  FlatList,
  KeyboardAvoidingView,
  Platform,
  SafeAreaView,
  StatusBar,
  Image
} from 'react-native';
import { COLORS } from '../theme/colors';
import { SPACING } from '../theme/spacing';
import { FONT_SIZE } from '../theme/typography';
import * as Animatable from 'react-native-animatable';
import { useAuth } from '../auth/useAuth';
import { useTranslation } from 'react-i18next';
import { ArrowLeft, Send } from 'lucide-react-native';

export default function MessageScreen({ route, navigation }) {
  const { providerId, providerName, userId, userName, isProvider } = route.params;
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([
    {
      id: '1',
      text: isProvider 
        ? 'Comment puis-je vous aider aujourd\'hui ?' 
        : 'Bonjour, comment puis-je vous aider ?',
      sender: isProvider ? 'user' : 'provider',
      timestamp: new Date(Date.now() - 3600000).toISOString(),
    }
  ]);
  const { isSignedIn, user } = useAuth();
  const { t } = useTranslation();
  const flatListRef = useRef(null);

  const sendMessage = () => {
    if (message.trim() === '') return;
    
    const newMessage = {
      id: Date.now().toString(),
      text: message,
      sender: isProvider ? 'provider' : 'user',
      timestamp: new Date().toISOString(),
    };
    
    setMessages([...messages, newMessage]);
    setMessage('');
    
    // Simulate response after 1 second
    setTimeout(() => {
      const responseMessage = {
        id: (Date.now() + 1).toString(),
        text: isProvider
          ? `Merci pour votre message. Nous ferons de notre mieux pour répondre à votre demande.`
          : `Merci pour votre message. Notre équipe vous répondra bientôt.`,
        sender: isProvider ? 'user' : 'provider',
        timestamp: new Date().toISOString(),
      };
      setMessages(prevMessages => [...prevMessages, responseMessage]);
    }, 1000);
  };

  // Scroll to bottom when messages change
  useEffect(() => {
    if (messages.length > 0 && flatListRef.current) {
      setTimeout(() => {
        flatListRef.current.scrollToEnd({ animated: true });
      }, 100);
    }
  }, [messages]);

  const formatTime = (timestamp) => {
    const date = new Date(timestamp);
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  const renderMessageItem = ({ item }) => (
    <Animatable.View
      animation="fadeIn"
      duration={500}
      style={[
        styles.messageContainer,
        item.sender === (isProvider ? 'provider' : 'user') ? styles.userMessage : styles.providerMessage
      ]}
    >
      <Text style={[
        styles.messageText,
        { color: item.sender === (isProvider ? 'provider' : 'user') ? COLORS.white : COLORS.black }
      ]}>
        {item.text}
      </Text>
      <Text style={[
        styles.messageTime,
        { color: item.sender === (isProvider ? 'provider' : 'user') ? 'rgba(255, 255, 255, 0.7)' : COLORS.gray }
      ]}>
        {formatTime(item.timestamp)}
      </Text>
    </Animatable.View>
  );

  const navigateBack = () => {
    if (isProvider) {
      navigation.navigate('ProviderMessageList');
    } else {
      navigation.navigate('MessageListScreen');
    }
  };

  return (
    <SafeAreaView style={styles.container}>
      <StatusBar backgroundColor={COLORS.primary} barStyle="light-content" />
      <View style={styles.header}>
        <TouchableOpacity 
          style={styles.backButton}
          onPress={navigateBack}
        >
          <ArrowLeft color={COLORS.white} size={24} />
        </TouchableOpacity>
        <Text style={styles.headerTitle}>
          {isProvider ? userName : providerName}
        </Text>
      </View>

      <FlatList
        ref={flatListRef}
        data={messages}
        renderItem={renderMessageItem}
        keyExtractor={item => item.id}
        contentContainerStyle={styles.messagesList}
      />

      <KeyboardAvoidingView
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      >
        <View style={styles.inputContainer}>
          <TextInput
            style={styles.input}
            value={message}
            onChangeText={setMessage}
            placeholder={isProvider ? "Écrivez votre réponse ici..." : "Écrivez votre message ici..."}
            placeholderTextColor={COLORS.gray}
            multiline
          />
          <TouchableOpacity 
            style={[
              styles.sendButton,
              !message.trim() && styles.sendButtonDisabled
            ]}
            onPress={sendMessage}
            disabled={!message.trim()}
          >
            <Send size={20} color={COLORS.white} />
          </TouchableOpacity>
        </View>
      </KeyboardAvoidingView>
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
  messagesList: {
    padding: SPACING.md,
    paddingBottom: SPACING.xl,
  },
  messageContainer: {
    maxWidth: '80%',
    padding: SPACING.md,
    borderRadius: 15,
    marginVertical: SPACING.xs,
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 1,
    },
    shadowOpacity: 0.1,
    shadowRadius: 2,
    elevation: 2,
  },
  userMessage: {
    alignSelf: 'flex-end',
    backgroundColor: COLORS.primary_light,
    borderBottomRightRadius: 5,
  },
  providerMessage: {
    alignSelf: 'flex-start',
    backgroundColor: COLORS.white,
    borderBottomLeftRadius: 5,
  },
  messageText: {
    fontSize: FONT_SIZE.md,
    color: props => props.sender === 'user' ? COLORS.white : COLORS.black,
  },
  messageTime: {
    fontSize: FONT_SIZE.xs,
    color: props => props.sender === 'user' ? 'rgba(255, 255, 255, 0.7)' : COLORS.gray,
    alignSelf: 'flex-end',
    marginTop: SPACING.xs,
  },
  inputContainer: {
    flexDirection: 'row',
    padding: SPACING.md,
    backgroundColor: COLORS.white,
    borderTopWidth: 1,
    borderTopColor: COLORS.light_gray,
  },
  input: {
    flex: 1,
    backgroundColor: COLORS.light_gray,
    borderRadius: 20,
    paddingHorizontal: SPACING.md,
    paddingVertical: SPACING.sm,
    maxHeight: 100,
    fontSize: FONT_SIZE.md,
  },
  sendButton: {
    backgroundColor: COLORS.primary,
    borderRadius: 25,
    width: 50,
    height: 50,
    justifyContent: 'center',
    alignItems: 'center',
    marginLeft: SPACING.sm,
  },
  sendButtonDisabled: {
    backgroundColor: COLORS.gray_light,
  },
  sendButtonText: {
    color: COLORS.white,
    fontSize: FONT_SIZE.lg,
  },
});
