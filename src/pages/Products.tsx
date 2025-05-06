
import { useState, useEffect } from 'react';
import ProductDetail from './ProductDetail';
import {
  ProductsHero,
  ProductsGrid,
  MadeInTunisia,
  ProductTestimonials,
  PartnersSection
} from '../components/products';
import { PRODUCTS } from '../config/products';
import { useIsMobile } from '../hooks/use-mobile';
import { Product, ProductCategory } from '../types/product';

interface ProductsProps {
  selectedCategory?: string;
  selectedSubcategory?: string;
  selectedProductId?: string;
}

const Products = ({ selectedCategory, selectedSubcategory, selectedProductId }: ProductsProps) => {
  const [selectedProductIdState, setSelectedProductId] = useState<string | null>(selectedProductId || null);
  const isMobile = useIsMobile();

  // Use products from the config
  const products = PRODUCTS;

  useEffect(() => {
    // Update the selected product ID when props change
    if (selectedProductId) {
      setSelectedProductId(selectedProductId);
    }
  }, [selectedProductId]);

  // Hard-coded mappings for specific subcategories to product IDs
  const subcategoryToProductIds: Record<string, string[]> = {
    'coffret-cadeaux': ['3', '4'], // Coffret de Dattes products
    'paquet': ['1', '2'], // Paquet de Dattes products
    'barquette': ['5', '6', '7'] // Barquette de Dattes products
  };

  // Filter products based on multiple criteria
  let filteredProducts = products;
  
  // If a specific product ID is provided, filter to only that product
  if (selectedProductId) {
    filteredProducts = products.filter(product => product.id === selectedProductId);
    // If no product found with this ID, don't filter
    if (filteredProducts.length === 0) {
      console.warn(`No product found with ID: ${selectedProductId}`);
      filteredProducts = products;
    }
  }
  // Otherwise, filter by category/subcategory
  else if (selectedCategory && selectedCategory !== 'tous') {
    console.log('Filtering by category:', selectedCategory, 'subcategory:', selectedSubcategory);
    
    // Special case for paquetdattes-fraiches (direct to product IDs 1 and 2)
    if (selectedCategory === 'paquetdattes-fraiches') {
      filteredProducts = products.filter(product => ['1', '2'].includes(product.id));
      console.log('Filtered to Paquet de Dattes products:', filteredProducts.length);
    }
    // Special case for coffretdattes-fraiches (direct to product IDs 3 and 4)
    else if (selectedCategory === 'coffretdattes-fraiches') {
      filteredProducts = products.filter(product => ['3', '4'].includes(product.id));
      console.log('Filtered to Coffret de Dattes products:', filteredProducts.length);
    }
    // Handle comma-separated categories
    else if (selectedCategory.includes(',')) {
      const categoryArray = selectedCategory.split(',');
      
      // If subcategory is specified, filter products within the categories that also match the subcategory
      if (selectedSubcategory) {
        const subcategoryProductIds = subcategoryToProductIds[selectedSubcategory] || [];
        
        if (subcategoryProductIds.length > 0) {
          // Filter products that match the specific product IDs for this subcategory
          filteredProducts = products.filter(product => 
            subcategoryProductIds.includes(product.id)
          );
        } else {
          // Fall back to standard subcategory filtering
          filteredProducts = products.filter(product => 
            categoryArray.includes(product.category) && product.subcategory === selectedSubcategory
          );
        }
        console.log('Filtered products by subcategory:', filteredProducts.length);
      } else {
        // Use the order of categories in the array to sort the products
        const orderedProducts: Product[] = [];
        
        categoryArray.forEach(category => {
          const productsInCategory = products.filter(product => product.category === category);
          orderedProducts.push(...productsInCategory);
        });
        
        // Set the filtered products to our ordered list
        filteredProducts = orderedProducts;
      }
    } else {
      // Single category
      filteredProducts = products.filter(product => product.category === selectedCategory);

      // Further filter by subcategory if provided
      if (selectedSubcategory) {
        const subcategoryProductIds = subcategoryToProductIds[selectedSubcategory] || [];
        
        if (subcategoryProductIds.length > 0) {
          // Filter products that match the specific product IDs for this subcategory
          filteredProducts = products.filter(product => 
            subcategoryProductIds.includes(product.id) &&
            product.category === selectedCategory
          );
        } else {
          // Fall back to standard subcategory filtering
          const beforeFilter = filteredProducts.length;
          filteredProducts = filteredProducts.filter(product => 
            product.subcategory === selectedSubcategory
          );
          console.log(`Filtered from ${beforeFilter} to ${filteredProducts.length} products by subcategory: ${selectedSubcategory}`);
        }
      }
    }
  }

  // Output some debug information to help troubleshoot
  console.log('Products filtering info:', { 
    selectedCategory,
    selectedSubcategory,
    selectedProductId,
    subcategoryProductIds: selectedSubcategory ? subcategoryToProductIds[selectedSubcategory] || [] : [],
    filteredCount: filteredProducts.length,
    isMobile,
    allProducts: products.length,
    firstProduct: filteredProducts.length > 0 ? {
      id: filteredProducts[0].id,
      category: filteredProducts[0].category,
      subcategory: filteredProducts[0].subcategory
    } : 'none'
  });

  // Handle navigation to the Revendeurs page
  const handleNavigateToRevendeurs = (e: React.MouseEvent) => {
    e.preventDefault();
    if (window.dispatchEvent) {
      const customEvent = new CustomEvent('navigateTo', { detail: { page: 'resellers' } });
      window.dispatchEvent(customEvent);
    }
  };

  if (selectedProductIdState) {
    // Find the product by ID
    const selectedProduct = products.find(p => p.id === selectedProductIdState);
    
    // If product exists, show its details, otherwise show the product grid
    if (selectedProduct) {
      return (
        <ProductDetail 
          productId={selectedProductIdState}
          onBack={() => setSelectedProductId(null)}
        />
      );
    } else {
      console.error(`Product with ID ${selectedProductIdState} not found`);
      // Reset the selected product ID
      setSelectedProductId(null);
    }
  }

  return (
    <div className="min-h-screen">
      <ProductsHero />
      
      <div className="container mx-auto px-4">
        <ProductsGrid 
          products={filteredProducts as Product[]} 
          onSelectProduct={setSelectedProductId}
          subcategory={selectedSubcategory}
        />
        
        <MadeInTunisia />
        
        <ProductTestimonials />
        
        <PartnersSection onNavigateToRevendeurs={handleNavigateToRevendeurs} />
      </div>
    </div>
  );
};

export default Products;