import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Product } from '../types/product';
import { calculateFinalPrice, formatPrice } from '@/utils/priceCalculations';
import { PenLine } from 'lucide-react';
import { useInView } from 'react-intersection-observer';

interface ProductCardProps {
  product: Product;
}

const ProductCard = ({ product }: ProductCardProps) => {
  const navigate = useNavigate();
  const [imageLoaded, setImageLoaded] = useState(false);
  const [preloaded, setPreloaded] = useState(false); // To track if the full image is preloaded
  const { ref, inView } = useInView({
    triggerOnce: true,
    threshold: 0.1,
    rootMargin: '50px',
  });

  const hasDiscount =
    product.discount_product !== '' &&
    !isNaN(parseFloat(product.discount_product)) &&
    parseFloat(product.discount_product) > 0;

  const finalPrice = calculateFinalPrice(
    product.price,
    product.discount_product,
    product.itemgroup_product,
    product.personalization ? true : false
  );

  const thumbnailUrl = `${product.image}?w=50&q=10`;
  const fullImageUrl = `${product.image}?w=400&q=60`;

  // Preload the high-quality image when the component mounts
  useEffect(() => {
    const img = new Image();
    img.src = fullImageUrl;
    img.onload = () => setPreloaded(true);
  }, [fullImageUrl]);

  return (
    <div
      ref={ref}
      className="h-full hover:shadow-lg hover:transform hover:scale-[1.02] transition-all duration-300 cursor-pointer bg-white rounded-lg"
      onClick={() => navigate(`/product/${product.id}`)}
    >
      <div className="h-[300px] bg-transparent overflow-hidden mb-3 relative">
        {hasDiscount && parseFloat(product.discount_product) > 0 && (
          <div className="absolute top-2 right-2 bg-[#700100] text-white px-2 py-1 rounded-full text-sm font-medium z-10">
            -{product.discount_product}%
          </div>
        )}
        <div className={`w-full h-full transition-opacity duration-300 ${imageLoaded ? 'opacity-100' : 'opacity-0'}`}>
          {/* Always load the preloaded image if available */}
          {(preloaded || inView) && (
            <>
              {/* Extremely low-quality placeholder */}
              <img
                src={thumbnailUrl}
                alt=""
                className="absolute inset-0 w-full h-full object-contain blur-lg scale-110"
                style={{ opacity: imageLoaded ? 0 : 0.5 }}
              />
              {/* Main image */}
              <img
                src={fullImageUrl}
                alt={product.name}
                className="w-full h-full object-contain mix-blend-normal relative z-10"
                loading="lazy"
                decoding="async"
                onLoad={() => setImageLoaded(true)}
                onError={() => setImageLoaded(true)}
              />
            </>
          )}
          {!imageLoaded && (
            <div className="absolute inset-0 bg-gray-100 animate-pulse" />
          )}
        </div>
      </div>
      <div className="p-2 md:p-4">
        <div className="text-base font-['WomanFontRegular'] text-[#591C1C]">{product.name}</div>
        <div className="text-sm text-gray-600 uppercase">
          {product.material}
          <br />
          {product.color}
        </div>
        <div className="mt-2 font-['WomanFontRegular']">
          {hasDiscount && parseFloat(product.discount_product) > 0 ? (
            <div className="space-y-1">
              <span className="text-[#700100] font-bold">{formatPrice(finalPrice)} TND</span>
              <span className="text-gray-500 line-through block">{formatPrice(product.price)} TND</span>
            </div>
          ) : (
            <span className="text-black">{formatPrice(finalPrice)} TND</span>
          )}
          {product.personalization && product.itemgroup_product === 'chemises' && (
            <div className="flex items-center gap-1 text-xs text-[#700100] mt-1">
              <PenLine size={12} />
              Personnalisation: +30 TND
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProductCard;
