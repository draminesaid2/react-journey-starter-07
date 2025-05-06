
import React, { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import { useTranslation } from 'react-i18next';
import { Helmet } from 'react-helmet';
import ProductsGrid from '../components/products/ProductsGrid';
import ProductsHero from '../components/products/ProductsHero';
import type { Product, ProductCategory } from '../types';

interface ProductsPageProps {
  selectedCategory: ProductCategory;
  selectedSubcategory?: string;
}

const Products: React.FC<ProductsPageProps> = ({ selectedCategory, selectedSubcategory }) => {
  const { t } = useTranslation();
  const [products, setProducts] = useState<Product[]>([]);
  const [selectedProduct, setSelectedProduct] = useState<string | null>(null);
  
  useEffect(() => {
    let filteredProducts: Product[] = [];
    
    // Import products dynamically to avoid the build error
    import('../config/products').then(({ default: productsConfig }) => {
      const allProducts = productsConfig.products || [];
      
      console.log('Products page filtering with:', { selectedCategory, selectedSubcategory });
      
      if (selectedCategory === 'tous') {
        // Show all products when 'tous' is selected
        filteredProducts = [...allProducts];
      } else {
        // Filter by categories - handle comma-separated categories
        const categories = selectedCategory.split(',');
        
        filteredProducts = allProducts.filter(product => {
          return categories.some(cat => product.category === cat);
        });
        
        // Further filter by subcategory if provided
        if (selectedSubcategory) {
          filteredProducts = filteredProducts.filter(product => 
            product.subcategory === selectedSubcategory
          );
        }
      }
      
      console.log(`Filtered to ${filteredProducts.length} products`);
      
      setProducts(filteredProducts);
    }).catch(error => {
      console.error("Error loading products:", error);
      setProducts([]);
    });
  }, [selectedCategory, selectedSubcategory]);
  
  const handleSelectProduct = (id: string) => {
    setSelectedProduct(id);
    
    // Dispatch event to navigate to the product detail page
    const navigateToProductEvent = new CustomEvent('navigateToProduct', {
      detail: { productId: id }
    });
    window.dispatchEvent(navigateToProductEvent);
  };
  
  // Get a descriptive category title based on selected category
  const getCategoryTitle = (): string => {
    // Handle multi-categories cases
    if (selectedCategory === 'tous') {
      return t('products.all_products');
    } else if (selectedCategory.includes('dattes-fraiches')) {
      if (selectedSubcategory === 'coffret-cadeaux') {
        return t('products.gift_boxes');
      } else if (selectedSubcategory === 'paquet') {
        return t('products.date_packages');
      } else {
        return t('products.fresh_dates');
      }
    } else if (selectedCategory.includes('dattes-transformees')) {
      return t('products.processed_dates');
    } else if (selectedCategory.includes('dattes-farcies')) {
      return t('products.stuffed_dates');
    } else if (selectedCategory.includes('figues-sechees')) {
      return t('products.dried_figs');
    } else if (selectedCategory.includes('cafe-dattes')) {
      return t('products.date_coffee');
    } else if (selectedCategory.includes('sucre-dattes')) {
      return t('products.date_sugar');
    } else if (selectedCategory.includes('sirop-dattes')) {
      return t('products.date_syrup');
    } else if (selectedCategory.includes('technical-products')) {
      return t('products.technical_products');
    }
    
    return selectedCategory;
  };
  
  return (
    <div className="min-h-screen bg-white">
      <Helmet>
        <title>{getCategoryTitle()} | Tazart</title>
        <meta name="description" content={`${getCategoryTitle()} - Tazart`} />
      </Helmet>
      
      <ProductsHero 
        title={getCategoryTitle()}
        description={t('products.browse_our_selection')}
        image="/hero-products.jpg"
      />
      
      <div className="container mx-auto px-4 py-8">
        <div className="mb-8">
          <h1 className="text-3xl md:text-4xl font-playfair text-[#700100] font-bold">
            {getCategoryTitle()}
          </h1>
          <p className="mt-2 text-gray-600 max-w-3xl">
            {t('products.quality_description')}
          </p>
        </div>
        
        <ProductsGrid 
          products={products} 
          onSelectProduct={handleSelectProduct}
          subcategory={selectedSubcategory}
        />
      </div>
    </div>
  );
};

export default Products;
