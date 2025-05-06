
import { useState, useEffect } from 'react';
import { ClientTypeModal } from './components/ClientTypeModal';
import Navbar from './components/Navbar';
import { Footer } from './components/Footer';
import { Contact } from './pages/Contact';
import Home from './pages/Home';
import About from './pages/About';
import Revendeurs from './pages/Revendeurs';
import Partners from './pages/Partners';
import Certifications from './pages/Certifications';
import Products from './pages/Products';
import ProductsAllPage from './pages/ProductsAllPage';
import ScrollToTop from './components/ScrollToTop';
import CookieConsent from './components/CookieConsent';
import type { ClientType, ProductCategory } from './types';
import Cookies from 'js-cookie';
import { AppProvider } from './context/AppContext';
import ProductDetail from './pages/ProductDetail';
import Dashboard from './pages/Dashboard';
import { trackVisitor } from './utils/visitorTracking';
import DateFarcie from './pages/DateFarcie';
import { useLocation } from 'react-router-dom';

function App() {
  const [clientType, setClientType] = useState<ClientType>(() => {
    const savedType = Cookies.get('clientType') as ClientType;
    return savedType || null;
  });
  const [currentPage, setCurrentPage] = useState('home');
  const [selectedCategory, setSelectedCategory] = useState<ProductCategory>('tous');
  const [selectedSubcategory, setSelectedSubcategory] = useState<string | undefined>(undefined);
  const [selectedProductId, setSelectedProductId] = useState<string | null>(null);
  const location = useLocation();

  // Track page views when the current page changes
  useEffect(() => {
    if (currentPage) {
      // Using French page names for tracking
      const pageName = getFrenchPageName(currentPage);
      trackVisitor(pageName);
    }
  }, [currentPage]);

  // Check if we're on a route that's handled outside of the main App component
  useEffect(() => {
    const path = location.pathname;
    if (path === '/dattes-farcies') {
      setCurrentPage('dattes-farcies');
    } else if (path === '/statistique') {
      setCurrentPage('statistique');
    } else if (path === '/') {
      setCurrentPage('home');
    }
  }, [location.pathname]);

  // Map English page names to French
  const getFrenchPageName = (pageName: string): string => {
    const pageNameMap: Record<string, string> = {
      'home': 'accueil',
      'products': 'produits',
      'products-all': 'tous-produits',
      'product-detail': 'detail-produit',
      'about': 'a-propos',
      'contact': 'contact',
      'resellers': 'revendeurs',
      'partners': 'nos-partenaires',
      'certifications': 'certifications',
      'statistique': 'statistique',
      'technical-products': 'produits-techniques',
      'dattes-farcies': 'dattes-farcies'
    };
    
    return pageNameMap[pageName] || pageName;
  };

  useEffect(() => {
    const handleNavigateEvent = (event: Event) => {
      const customEvent = event as CustomEvent<{ 
        page: string; 
        category?: string; 
        subcategory?: string; 
        productId?: string 
      }>;
      
      if (customEvent.detail) {
        console.log('App received navigation event:', customEvent.detail);
        setCurrentPage(customEvent.detail.page);
        
        if (customEvent.detail.category) {
          setSelectedCategory(customEvent.detail.category as ProductCategory);
        }
        
        if (customEvent.detail.subcategory) {
          setSelectedSubcategory(customEvent.detail.subcategory);
        } else {
          setSelectedSubcategory(undefined); // Reset if not provided
        }
        
        if (customEvent.detail.productId) {
          setSelectedProductId(customEvent.detail.productId);
          setCurrentPage('product-detail');
        } else if (customEvent.detail.page !== 'product-detail') {
          setSelectedProductId(null); // Reset product ID when not going to product detail
        }
      }
    };

    const handleNavigateToProduct = (event: Event) => {
      const customEvent = event as CustomEvent<{ productId: string }>;
      if (customEvent.detail && customEvent.detail.productId) {
        setSelectedProductId(customEvent.detail.productId);
        setCurrentPage('product-detail');
      }
    };

    const handleClientTypeChange = (event: Event) => {
      const customEvent = event as CustomEvent<{ type: ClientType }>;
      if (customEvent.detail && customEvent.detail.type) {
        handleClientTypeChange(customEvent.detail.type);
      }
    };

    window.addEventListener('navigateTo', handleNavigateEvent);
    window.addEventListener('navigateToProduct', handleNavigateToProduct);
    window.addEventListener('changeClientType', handleClientTypeChange);
    
    return () => {
      window.removeEventListener('navigateTo', handleNavigateEvent);
      window.removeEventListener('navigateToProduct', handleNavigateToProduct);
      window.removeEventListener('changeClientType', handleClientTypeChange);
    };
  }, []);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [currentPage]);

  const handleClientTypeChange = (type: ClientType) => {
    setClientType(type);
    if (type) {
      const hasConsent = Cookies.get('cookieConsent');
      if (hasConsent === 'accepted') {
        Cookies.set('clientType', type);
      }
    }
  };

  const handleCookieAccept = () => {
    if (clientType) {
      Cookies.set('clientType', clientType);
    }
  };

  const handleCookieDecline = () => {
    Cookies.remove('clientType');
  };

  const handlePageChange = (page: string, category?: ProductCategory, subcategory?: string, productId?: string) => {
    console.log('App handlePageChange:', { page, category, subcategory, productId });
    setCurrentPage(page);
    
    if (category) {
      setSelectedCategory(category);
    }
    
    if (subcategory) {
      setSelectedSubcategory(subcategory);
    } else {
      setSelectedSubcategory(undefined); // Reset if not provided
    }
    
    // Set product ID for product detail page or reset it
    if (productId) {
      setSelectedProductId(productId);
      setCurrentPage('product-detail');
    } else if (page !== 'product-detail') {
      setSelectedProductId(null);
    }

    // No need to track page visit here as it's already handled in the useEffect
  };

  const renderPage = () => {
    console.log('Rendering page:', { 
      currentPage, 
      selectedCategory, 
      selectedSubcategory, 
      selectedProductId 
    });
    
    switch (currentPage) {
      case 'products':
        return <Products 
          selectedCategory={selectedCategory} 
          selectedSubcategory={selectedSubcategory}
        />;
      case 'products-all':
        return <ProductsAllPage />;
      case 'product-detail':
        return selectedProductId ? (
          <ProductDetail 
            productId={selectedProductId}
            onBack={() => {
              setSelectedProductId(null);
              setCurrentPage('products-all');
            }}
          />
        ) : (
          <ProductsAllPage />
        );
      case 'about':
        return <About />;
      case 'contact':
        return <Contact />;
      case 'resellers':
        return <Revendeurs />;
      case 'partners':
        return <Partners />;
      case 'certifications':
        return <Certifications />;
      case 'statistique':
        return <Dashboard />;
      case 'dattes-farcies':
        return <DateFarcie />;
      default:
        return <Home clientType={clientType} />;
    }
  };

  // Determine the effective client type (default to B2C if null)
  const effectiveClientType = clientType || 'B2C';

  return (
    <AppProvider clientType={effectiveClientType}>
      <div className="min-h-screen flex flex-col">
        {clientType === null ? (
          <>
            <Navbar 
              clientType="B2C" 
              onPageChange={handlePageChange}
              currentPage={currentPage}
              onClientTypeChange={handleClientTypeChange}
            />
            {renderPage()}
            <Footer />
            <ClientTypeModal onSelect={handleClientTypeChange} />
          </>
        ) : (
          <>
            {currentPage !== 'statistique' && (
              <Navbar 
                clientType={clientType} 
                onPageChange={handlePageChange}
                currentPage={currentPage}
                onClientTypeChange={handleClientTypeChange}
              />
            )}
            {renderPage()}
            {currentPage !== 'statistique' && <Footer />}
            <ScrollToTop />
            <CookieConsent 
              onAccept={handleCookieAccept}
              onDecline={handleCookieDecline}
            />
          </>
        )}
      </div>
    </AppProvider>
  );
}

export default App;
