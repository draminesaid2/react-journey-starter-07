import { toast } from "@/hooks/use-toast";
import { getCartItems, saveCartItems } from "./cartStorage";

const EXPIRATION_TIME = 30 * 60 * 1000; // 30 minutes in milliseconds
const WARNING_TIME = 15 * 60 * 1000; // 15 minutes in milliseconds

class CartExpirationManager {
  private expirationTimer: NodeJS.Timeout | null = null;
  private warningTimer: NodeJS.Timeout | null = null;

  startExpirationTimer() {
    // Clear any existing timers
    this.clearTimers();

    // Set expiration time
    const expirationTime = Date.now() + EXPIRATION_TIME;
    localStorage.setItem('cartExpirationTime', expirationTime.toString());

    // Set warning timer
    const timeUntilWarning = EXPIRATION_TIME - WARNING_TIME;
    this.warningTimer = setTimeout(() => {
      const minutesLeft = Math.ceil(WARNING_TIME / (60 * 1000));
      toast({
        title: "Attention !",
        description: `Votre panier expirera dans ${minutesLeft} minutes`,
        style: {
          backgroundColor: '#700100',
          color: 'white',
          border: '1px solid #590000',
        },
        duration: 10000, // Show for 10 seconds
      });
    }, timeUntilWarning);

    // Set expiration timer
    this.expirationTimer = setTimeout(() => {
      this.clearCart();
    }, EXPIRATION_TIME);
  }

  clearTimers() {
    if (this.expirationTimer) {
      clearTimeout(this.expirationTimer);
    }
    if (this.warningTimer) {
      clearTimeout(this.warningTimer);
    }
  }

  clearCart() {
    saveCartItems([]);
    localStorage.removeItem('cartExpirationTime');
    this.clearTimers();
    
    toast({
      title: "Panier expiré",
      description: "Votre panier a été vidé en raison de l'expiration du délai",
      style: {
        backgroundColor: '#700100',
        color: 'white',
        border: '1px solid #590000',
      },
    });
  }

  checkExpiration() {
    const expirationTime = localStorage.getItem('cartExpirationTime');
    if (expirationTime) {
      const timeLeft = parseInt(expirationTime) - Date.now();
      
      if (timeLeft <= 0) {
        this.clearCart();
      } else {
        // Restart timer with remaining time
        this.clearTimers();
        
        // Set warning if within warning period
        if (timeLeft <= WARNING_TIME) {
          const minutesLeft = Math.ceil(timeLeft / (60 * 1000));
          toast({
            title: "Attention !",
            description: `Votre panier expirera dans ${minutesLeft} minutes`,
            style: {
              backgroundColor: '#700100',
              color: 'white',
              border: '1px solid #590000',
            },
            duration: 10000,
          });
        }

        this.expirationTimer = setTimeout(() => {
          this.clearCart();
        }, timeLeft);

        if (timeLeft > WARNING_TIME) {
          this.warningTimer = setTimeout(() => {
            const minutesLeft = Math.ceil(WARNING_TIME / (60 * 1000));
            toast({
              title: "Attention !",
              description: `Votre panier expirera dans ${minutesLeft} minutes`,
              style: {
                backgroundColor: '#700100',
                color: 'white',
                border: '1px solid #590000',
              },
              duration: 10000,
            });
          }, timeLeft - WARNING_TIME);
        }
      }
    }
  }
}

export const cartExpirationManager = new CartExpirationManager();