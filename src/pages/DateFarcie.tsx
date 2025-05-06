
import { motion } from 'framer-motion';
import DateFarcieComposer from '../components/dates/DateFarcieComposer';
import { Helmet } from 'react-helmet';
import { useTranslation } from 'react-i18next';
import Navbar from '../components/Navbar';
import { Footer } from '../components/Footer';
import { useApp } from '../context/AppContext';
import ScrollToTop from '../components/ScrollToTop';
import { useNavigate } from 'react-router-dom';

const DateFarcie = () => {
  const { t } = useTranslation();
  const { clientType } = useApp();
  const navigate = useNavigate();
  
  // Function to handle page navigation
  const handlePageChange = (page: string, category?: string, subcategory?: string, productId?: string) => {
    console.log(`DateFarcie: Navigation triggered to ${page} with category ${category || 'none'}`);
    
    // Dispatch custom navigation event to be caught by App.tsx
    const navigateEvent = new CustomEvent('navigateTo', {
      detail: { page, category, subcategory, productId }
    });
    window.dispatchEvent(navigateEvent);
    
    // Navigate back to the root which will handle routing via the custom event
    navigate('/');
  };

  // Function to handle client type changes
  const handleClientTypeChange = (type: any) => {
    // Dispatch event to update client type in the main App
    const changeClientTypeEvent = new CustomEvent('changeClientType', {
      detail: { type }
    });
    window.dispatchEvent(changeClientTypeEvent);
  };
  
  return (
    <div className="flex flex-col min-h-screen">
      <Helmet>
        <title>{t('date_composer.page_title')} | Tazart</title>
      </Helmet>
      
      <Navbar 
        clientType={clientType || 'B2C'} 
        onPageChange={handlePageChange}
        currentPage="dattes-farcies"
        onClientTypeChange={handleClientTypeChange}
      />
      
      <motion.div
        className="flex-grow pt-32 pb-16"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        exit={{ opacity: 0, y: -20 }}
      >
        <DateFarcieComposer />
      </motion.div>
      
      <ScrollToTop />
      <Footer />
    </div>
  );
};

export default DateFarcie;
