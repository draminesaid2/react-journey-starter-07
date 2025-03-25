
import React, { useRef, useEffect } from 'react';
import { motion } from 'framer-motion';
import { Building2, ExternalLink } from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { Carousel, CarouselContent, CarouselItem, CarouselNext, CarouselPrevious } from "../../components/ui/carousel";
import { partners } from '../../config/partners';
import Image from '../ui/image';

interface PartnersSectionProps {
  onNavigateToRevendeurs: (e: React.MouseEvent) => void;
}

const PartnersSection = ({ onNavigateToRevendeurs }: PartnersSectionProps) => {
  const { t } = useTranslation();
  const carouselRef = useRef<HTMLDivElement>(null);

  // Auto-scroll effect with improved animation
  useEffect(() => {
    let interval: NodeJS.Timeout;
    let animationFrameId: number;
    let scrollAmount = 0;
    let scrollDirection = 1;
    const scrollSpeed = 0.7; // Slower, smoother scrolling
    
    // Start auto-scroll after a small delay
    const initTimeout = setTimeout(() => {
      if (carouselRef.current) {
        const carousel = carouselRef.current;
        
        const animate = () => {
          if (carousel) {
            scrollAmount += scrollSpeed * scrollDirection;
            carousel.scrollLeft = scrollAmount;
            
            // Check if we need to change direction
            if (scrollAmount >= (carousel.scrollWidth - carousel.clientWidth)) {
              scrollDirection = -1;
            } else if (scrollAmount <= 0) {
              scrollDirection = 1;
            }
            
            animationFrameId = requestAnimationFrame(animate);
          }
        };
        
        animate();
      }
    }, 1000);
    
    return () => {
      clearTimeout(initTimeout);
      clearInterval(interval);
      cancelAnimationFrame(animationFrameId);
    };
  }, []);
  
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      whileInView={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6 }}
      viewport={{ once: true }}
      className="my-16 p-8 bg-gray-50 rounded-xl"
    >
      <div className="flex items-center justify-center gap-2 text-[#700100] mb-3">
        <Building2 className="w-6 h-6" />
        <h2 className="text-2xl font-playfair text-center">{t('partners.our_partners')}</h2>
      </div>
      <p className="text-center text-gray-600 mb-8 max-w-2xl mx-auto">{t('partners.trust_description')}</p>
      
      <Carousel className="w-full mb-8" ref={carouselRef}>
        <CarouselContent>
          {partners.slice(0, 5).map((partner) => (
            <CarouselItem key={partner.id} className="md:basis-1/3 lg:basis-1/5">
              <div className="bg-white p-4 rounded-lg shadow-sm h-32 flex items-center justify-center">
                <Image
                  src={partner.logo}
                  alt={partner.name}
                  width={120}
                  height={80}
                  className="object-contain max-h-20"
                />
              </div>
            </CarouselItem>
          ))}
        </CarouselContent>
        <div className="flex justify-center mt-4">
          <CarouselPrevious className="relative static transform-none mr-2" />
          <CarouselNext className="relative static transform-none" />
        </div>
      </Carousel>
      
      <div className="text-center">
        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.98 }}
          onClick={onNavigateToRevendeurs}
          className="inline-flex items-center px-6 py-3 bg-[#96cc39] text-white rounded-full font-medium transition-colors hover:bg-[#89ba33] shadow-md hover:shadow-lg"
        >
          {t('partners.find_resellers')}
          <ExternalLink className="ml-2 h-4 w-4" />
        </motion.button>
      </div>
    </motion.div>
  );
};

export default PartnersSection;
