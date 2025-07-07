
import { useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { CustomScrollbar } from '@/components/ui/custom-scrollbar';
import Header from '@/components/Header';
import Footer from '@/components/Footer';
import WhatsAppButton from '@/components/ui/WhatsAppButton';
import LoadingScreen from '@/components/LoadingScreen';
import { useVisitorTracking } from '@/hooks/useVisitorTracking';
import { useEnhancedScrollAnimation } from '@/hooks/useEnhancedScrollAnimation';
import { useAutoScroll } from '@/hooks/useAutoScroll';
import { Button } from '@/components/ui/button';

const Index = () => {
  const navigate = useNavigate();

  // Track visitor for homepage
  useVisitorTracking('Homepage');
  const [showLoading, setShowLoading] = useState(false);

  // Animation hooks for mobile books
  const book1Animation = useEnhancedScrollAnimation({
    delay: 100,
    staggerDelay: 0
  });
  const book2Animation = useEnhancedScrollAnimation({
    delay: 200,
    staggerDelay: 0
  });
  const book3Animation = useEnhancedScrollAnimation({
    delay: 300,
    staggerDelay: 0
  });
  const book4Animation = useEnhancedScrollAnimation({
    delay: 400,
    staggerDelay: 0
  });
  const book5Animation = useEnhancedScrollAnimation({
    delay: 500,
    staggerDelay: 0
  });
  const book6Animation = useEnhancedScrollAnimation({
    delay: 600,
    staggerDelay: 0
  });

  const handlePersonalizeClick = useCallback(() => {
    setShowLoading(true);
  }, []);

  const handleLoadingComplete = useCallback(() => {
    setShowLoading(false);
    window.scrollTo(0, 0);
    navigate('/child-count');
  }, [navigate]);

  // Auto-scroll for Notre Impact cards
  const desktopAutoScroll = useAutoScroll({
    interval: 4000,
    scrollAmount: 420
  });
  const mobileAutoScroll = useAutoScroll({
    interval: 3500,
    scrollAmount: 300
  });

  if (showLoading) {
    return <LoadingScreen onLoadingComplete={handleLoadingComplete} />;
  }

  return <>      
      <CustomScrollbar className="min-h-screen" style={{
      background: 'linear-gradient(135deg, #E8D5FF 0%, #F3E8FF 25%, #E0E7FF 50%, #F0F4FF 75%, #F8FAFF 100%)'
    }}>
        <div className="min-h-screen relative font-baloo">
          
          <Header />
          
          {/* Hero Section */}
          <div className="relative z-20 pt-28 md:pt-40">
            <div className="container mx-auto px-8 py-0 my-0">
              <div className="flex flex-col lg:flex-row items-center gap-12">
                {/* Left side - Text and Button */}
                <div className="flex-1 text-center lg:text-left">
                  <h1 className="text-4xl md:text-5xl lg:text-6xl font-bold text-gray-800 mb-6 leading-tight">
                    Créez votre livre personnalisé pour enfants
                  </h1>
                  <p className="text-lg md:text-xl text-gray-600 mb-8 max-w-2xl">
                    Concevez un livre unique pour enfants avec l'IA, adapté spécifiquement à chaque enfant. Donnez vie à une histoire personnalisée et à des illustrations époustouflantes.
                  </p>
                  <Button onClick={handlePersonalizeClick} className="w-full h-12 bg-gradient-to-r from-[#a6428d] to-purple-400 hover:from-[#924077] hover:to-purple-300 text-white font-medium text-base rounded-lg shadow-lg transform transition-all duration-200 hover:scale-105 sm:w-auto sm:px-8">
                    JE PERSONALISE MON HISTOIRE
                  </Button>
                </div>

                {/* Right side - Book Grid (Desktop) */}
                <div className="hidden lg:block flex-1 relative">
                  <div className="max-w-lg ml-auto relative z-10">
                    {/* Books arranged in structured grid with staggered heights */}
                    <div className="grid grid-cols-3 gap-4 h-96 relative my-0 py-0 pt-8">
                      {/* Left column - starts lower */}
                      <div className="flex flex-col gap-4 pt-12 py-0 my-[150px]">
                        {/* Book 1 - Top left (lower start) */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl animate-fade-in shadow-xl shadow-primary/20 hover:shadow-primary/30" style={{
                        animationDelay: '0.1s'
                      }}>
                          <img alt="Thomas and the Magical Judo Adventure" className="w-full h-36 object-cover rounded-lg shadow-2xl shadow-orange-500/25" src="/lovable-uploads/e4c91fce-bc18-4e42-a454-1f9b8fece80e.png" />
                        </div>
                        {/* Book 3 - Bottom left */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl animate-fade-in shadow-xl shadow-primary/20 hover:shadow-primary/30" style={{
                        animationDelay: '0.3s'
                      }}>
                          <img alt="The Magical Unicorn of Friendship" className="w-full h-36 object-cover rounded-lg shadow-2xl shadow-purple-500/25" src="/lovable-uploads/0fc670a6-d247-4ff6-a08f-892d9c684d76.png" />
                        </div>
                      </div>
                      
                      {/* Center column - starts highest */}
                      <div className="flex flex-col gap-4 pt-0 my-[115px]">
                        {/* Book 2 - Top center (highest start) */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl animate-fade-in shadow-xl shadow-primary/20 hover:shadow-primary/30" style={{
                        animationDelay: '0.2s'
                      }}>
                          <img alt="Alice Among the Enchanted Dreams" className="w-full h-36 object-cover rounded-lg shadow-2xl shadow-blue-500/25" src="/lovable-uploads/cc3ab2c5-c5af-410e-9519-7abddb26f463.png" />
                        </div>
                        {/* Book 4 - Bottom center */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl animate-fade-in shadow-xl shadow-primary/20 hover:shadow-primary/30" style={{
                        animationDelay: '0.4s'
                      }}>
                          <img alt="James Space Flight" className="w-full h-36 object-cover rounded-lg shadow-2xl shadow-indigo-500/25" src="/lovable-uploads/29c0557a-22be-4375-af08-8f745adf0e60.png" />
                        </div>
                      </div>
                      
                      {/* Right column - starts middle height */}
                      <div className="flex flex-col gap-4 pt-6 my-0">
                        {/* New Book - Top right (new addition) */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl animate-fade-in shadow-xl shadow-primary/20 hover:shadow-primary/30" style={{
                        animationDelay: '0.5s'
                      }}>
                          <img alt="The Magic Forest Adventure" className="w-full h-36 object-cover rounded-lg shadow-2xl shadow-green-500/25" src="/lovable-uploads/e3eb0ac3-a28d-47b8-807d-3963b2ce38df.png" />
                        </div>
                        {/* Book 5 - Top right (middle start) */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl animate-fade-in shadow-xl shadow-primary/20 hover:shadow-primary/30" style={{
                        animationDelay: '0.6s'
                      }}>
                          <img alt="Cardboard the Brave Dinos Adventure" className="w-full h-36 object-cover rounded-lg shadow-2xl shadow-yellow-500/25" src="/lovable-uploads/6bb1affc-16ed-4c1c-8385-7588a54d5023.png" />
                        </div>
                        {/* Book 6 - Bottom right */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl animate-fade-in shadow-xl shadow-primary/20 hover:shadow-primary/30" style={{
                        animationDelay: '0.7s'
                      }}>
                          <img alt="The Adventures of Lisa and the Unicorn" className="w-full h-36 object-cover rounded-lg shadow-2xl shadow-pink-500/25" src="/lovable-uploads/d5574267-4370-41e1-ae3d-52f3db0cac80.png" />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                {/* Mobile book grid - bigger books with staggered animations */}
                <div className="lg:hidden w-full relative">
                  <div className="max-w-md mx-auto relative z-10">
                    {/* Books arranged in structured grid with staggered heights - bigger and positioned lower */}
                    <div className="grid grid-cols-3 gap-3 h-64 relative my-0 py-0 pt-12 mt-[-22%]">
                      {/* Left column - starts lower */}
                      <div className="flex flex-col gap-3 pt-8 py-0 my-[60px]">
                        {/* Book 1 - Top left (lower start) */}
                        <div ref={book1Animation.ref} className={`transform hover:scale-105 transition-all duration-500 hover:shadow-2xl ${book1Animation.isVisible ? 'animate-fade-in' : 'opacity-0'} hover:shadow-primary/30`}>
                          <img alt="Thomas and the Magical Judo Adventure" className="w-full h-24 object-cover rounded-lg shadow-2xl shadow-orange-500/25" src="/lovable-uploads/e4c91fce-bc18-4e42-a454-1f9b8fece80e.png" />
                        </div>
                        {/* Book 3 - Bottom left */}
                        <div ref={book3Animation.ref} className={`transform hover:scale-105 transition-all duration-500 hover:shadow-2xl ${book3Animation.isVisible ? 'animate-fade-in' : 'opacity-0'} hover:shadow-primary/30`}>
                          <img alt="The Magical Unicorn of Friendship" className="w-full h-24 object-cover rounded-lg shadow-2xl shadow-purple-500/25" src="/lovable-uploads/0fc670a6-d247-4ff6-a08f-892d9c684d76.png" />
                        </div>
                      </div>
                      
                      {/* Center column - starts highest */}
                      <div className="flex flex-col gap-3 pt-0 my-[50px]">
                        {/* Book 2 - Top center (highest start) */}
                        <div ref={book2Animation.ref} className={`transform hover:scale-105 transition-all duration-500 hover:shadow-2xl ${book2Animation.isVisible ? 'animate-fade-in' : 'opacity-0'} hover:shadow-primary/30`}>
                          <img alt="Alice Among the Enchanted Dreams" className="w-full h-24 object-cover rounded-lg shadow-2xl shadow-blue-500/25" src="/lovable-uploads/cc3ab2c5-c5af-410e-9519-7abddb26f463.png" />
                        </div>
                        {/* Book 4 - Bottom center */}
                        <div ref={book4Animation.ref} className={`transform hover:scale-105 transition-all duration-500 hover:shadow-2xl ${book4Animation.isVisible ? 'animate-fade-in' : 'opacity-0'} hover:shadow-primary/30`}>
                          <img alt="James Space Flight" className="w-full h-24 object-cover rounded-lg shadow-2xl shadow-indigo-500/25" src="/lovable-uploads/29c0557a-22be-4375-af08-8f745adf0e60.png" />
                        </div>
                      </div>
                      
                      {/* Right column - starts middle height, adjusted positioning */}
                      <div className="flex flex-col gap-3 pt-2 my-0">
                        {/* New Book - Top right (adjusted positioning) */}
                        <div ref={book5Animation.ref} className={`transform hover:scale-105 transition-all duration-500 hover:shadow-2xl ${book5Animation.isVisible ? 'animate-fade-in' : 'opacity-0'} hover:shadow-primary/30`}>
                          <img alt="The Magic Forest Adventure" className="w-full h-24 object-cover rounded-lg shadow-2xl shadow-green-500/25" src="/lovable-uploads/e3eb0ac3-a28d-47b8-807d-3963b2ce38df.png" />
                        </div>
                        {/* Book 5 - Top right (middle start) */}
                        <div ref={book6Animation.ref} className={`transform hover:scale-105 transition-all duration-500 hover:shadow-2xl ${book6Animation.isVisible ? 'animate-fade-in' : 'opacity-0'} hover:shadow-primary/30`}>
                          <img alt="Cardboard the Brave Dinos Adventure" className="w-full h-24 object-cover rounded-lg shadow-2xl shadow-yellow-500/25" src="/lovable-uploads/6bb1affc-16ed-4c1c-8385-7588a54d5023.png" />
                        </div>
                        {/* Book 6 - Bottom right */}
                        <div className="transform hover:scale-105 transition-all duration-500 hover:shadow-2xl hover:shadow-primary/30">
                          <img alt="The Adventures of Lisa and the Unicorn" className="w-full h-24 object-cover rounded-lg shadow-2xl shadow-pink-500/25" src="/lovable-uploads/d5574267-4370-41e1-ae3d-52f3db0cac80.png" />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Steps Section */}
          <div className="bg-white py-12 relative z-[999] rounded-2xl shadow-lg mx-4 md:mx-8 -mt-[-6%]">
            <div className="container mx-auto px-6">
              <div className="max-w-5xl mx-auto">
                {/* Step 1 - Image first on mobile, text first on desktop */}
                <div className="flex flex-col lg:flex-row items-center gap-8 mb-12">
                  {/* Image - shows first on mobile, first on desktop */}
                  <div className="flex-1 flex justify-center order-1 lg:order-1">
                    <div className="relative">
                      <img alt="Personnage principal" src="/lovable-uploads/13d2bd7c-0fd7-48b9-89ec-5a1de54016a9.png" className="w-60 h-60 object-cover" />
                    </div>
                  </div>
                  {/* Text - shows second on mobile, second on desktop */}
                  <div className="flex-1 text-left order-2 lg:order-2">
                    <div className="flex items-center justify-start mb-3">
                      <div className="bg-orange-500 text-white rounded-full w-8 h-8 min-w-[32px] min-h-[32px] flex items-center justify-center font-bold text-base mr-3 flex-shrink-0">
                        1
                      </div>
                      <h3 className="text-xl font-bold text-gray-800 md:text-2xl">VOUS REMPLISSEZ UN PETIT FORMULAIRE</h3>
                    </div>
                    <p className="text-base text-gray-600 leading-relaxed">
                      Remplissez les noms et détails des personnages et téléchargez leurs photos 
                      afin que les <strong>illustrations basées sur vos photos</strong> soient générées.
                    </p>
                  </div>
                </div>

                {/* Step 2 - Image first on mobile, image on right on desktop */}
                <div className="flex flex-col lg:flex-row items-center gap-8 mb-12">
                  {/* Text - shows second on mobile, first on desktop */}
                  <div className="flex-1 text-left order-2 lg:order-1">
                    <div className="flex items-center justify-start mb-3">
                      <div className="bg-orange-500 text-white rounded-full w-8 h-8 min-w-[32px] min-h-[32px] flex items-center justify-center font-bold text-base mr-3 flex-shrink-0">
                        2
                      </div>
                      <h3 className="text-xl md:text-2xl font-bold text-gray-800">VOUS CHOISSEZ LE THEME DE HISTOIRE</h3>
                    </div>
                    <p className="text-base text-gray-600 leading-relaxed">
                      Avec notre <strong>IA générative</strong>, vous créez une <strong>histoire personnalisée</strong>. 
                      Choisissez la trame narrative et les options de style qui correspondent le mieux aux 
                     <strong>intérêts de l'enfant</strong>.
                    </p>
                  </div>
                  {/* Image - shows first on mobile, second on desktop */}
                  <div className="flex-1 flex justify-center order-1 lg:order-2">
                    <div className="relative">
                      <img alt="Histoire personnalisée" src="/lovable-uploads/3ec02707-40fc-4a2a-a282-cd23fd929c56.png" className="w-60 h-60 object-cover" />
                    </div>
                  </div>
                </div>

                {/* Step 3 - Image first on mobile, image on left on desktop */}
                <div className="flex flex-col lg:flex-row items-center gap-8 mb-12">
                  {/* Image - shows first on mobile, first on desktop */}
                  <div className="flex-1 flex justify-center order-1 lg:order-1">
                    <div className="relative">
                      <img alt="Livre en différents formats" src="/lovable-uploads/e0e26efc-2cdf-4f67-ac48-d968d3485b52.png" className="w-60 h-60 object-cover" />
                    </div>
                  </div>
                  {/* Text - shows second on mobile, second on desktop */}
                  <div className="flex-1 text-left order-2 lg:order-2">
                    <div className="flex items-center justify-start mb-3">
                      <div className="bg-orange-500 text-white rounded-full w-8 h-8 min-w-[32px] min-h-[32px] flex items-center justify-center font-bold text-base mr-3 flex-shrink-0">
                        3
                      </div>
                      <h3 className="text-xl md:text-2xl font-bold text-gray-800">ON VOUS CREE UNE HISTOIRE SUR MESURRE</h3>
                    </div>
                    <p className="text-base text-gray-600 leading-relaxed">
                      Recevez le <strong>livre électronique pour 7,99€</strong> immédiatement. 
                      Voulez-vous aussi une copie physique ? Alors choisissez un 
                     <strong>livre relié pour 34,99€</strong> par la suite.
                    </p>
                  </div>
                </div>

                {/* CTA Button */}
                <div className="text-center">
                  <Button onClick={handlePersonalizeClick} className="w-full h-12 bg-gradient-to-r from-[#a6428d] to-purple-400 hover:from-[#924077] hover:to-purple-300 text-white font-medium text-base rounded-lg shadow-lg transform transition-all duration-200 hover:scale-105 sm:w-auto sm:px-6 sm:py-3">
                    Je personnalise mon histoire
                  </Button>
                </div>
              </div>
            </div>
          </div>

          {/* Notre Impact Section */}
          <div className=" py-16 relative z-[998]">
            <div className="container px-0 mx-0">
              <h2 className="text-3xl md:text-4xl font-bold text-gray-800 text-center mb-12">
                Notre impact
              </h2>
              
              {/* Desktop Cards */}
              <div className="hidden md:block">
                <div ref={desktopAutoScroll.scrollRef} className="flex gap-8 overflow-x-auto scrollbar-hide" {...desktopAutoScroll.scrollProps}>
                  {/* Card 1 */}
                  <div className="min-w-[650px] bg-white rounded-2xl shadow-lg overflow-hidden flex h-80">
                    <div className="flex-1 p-8 flex flex-col justify-center">
                      <div className="flex items-start gap-3 mb-4">
                        <div className="w-6 h-6 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-1">
                          <span className="text-white text-sm">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-base leading-relaxed">
                          VALORISE L'ENFANT : IL VOIT SON PRÉNOM ET SON VISAGE DANS L'HISTOIRE, CE QUI LE REND UNIQUE, SPÉCIAL ET IMPORTANT
                        </p>
                      </div>
                      <div className="flex items-start gap-3">
                        <div className="w-6 h-6 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-1">
                          <span className="text-white text-sm">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-base leading-relaxed">
                          EN DEVENANT LE HÉROS, L'ENFANT SE SENT CAPABLE, COURAGEUX ET IMPORTANT
                        </p>
                      </div>
                    </div>
                    <div className="w-1/2 h-full">
                      <img alt="Enfant avec livre personnalisé" className="w-full h-full object-cover rounded-r-2xl" src="/lovable-uploads/32b8d1c7-0558-4f96-8b64-2be0e76f0092.jpg" />
                    </div>
                  </div>

                  {/* Card 2 */}
                  <div className="min-w-[650px] bg-white rounded-2xl shadow-lg overflow-hidden flex h-80">
                    <div className="flex-1 p-8 flex flex-col justify-center">
                      <div className="flex items-start gap-3 mb-4">
                        <div className="w-6 h-6 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-1">
                          <span className="text-white text-sm">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-base leading-relaxed">
                          NOS HISTOIRES SONT VALIDÉES PAR UN PÉDOPSYCHIATRE ET ADAPTÉES À SON ÂGE
                        </p>
                      </div>
                      <div className="flex items-start gap-3">
                        <div className="w-6 h-6 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-1">
                          <span className="text-white text-sm">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-base leading-relaxed">
                          CHAQUE LIVRE CONTIENT UNE MORALE CLAIRE POUR L'AIDER À GRANDIR AVEC CONFIANCE
                        </p>
                      </div>
                    </div>
                    <div className="w-1/2 h-full">
                      <img alt="Enfant avec livre personnalisé" className="w-full h-full object-cover rounded-r-2xl" src="/lovable-uploads/4ddb4322-1d5e-459a-baaf-b62ef46ed5c3.png" />
                    </div>
                  </div>

                  {/* Card 3 */}
                  <div className="min-w-[650px] bg-white rounded-2xl shadow-lg overflow-hidden flex h-80">
                    <div className="flex-1 p-8 flex flex-col justify-center">
                      <div className="flex items-start gap-3 mb-4">
                        <div className="w-6 h-6 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-1">
                          <span className="text-white text-sm">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-base leading-relaxed">
                          IL APPREND DES VALEURS ET DES LEÇONS FONDAMENTALES POUR SON DÉVELOPPEMENT PERSONNEL
                        </p>
                      </div>
                      <div className="flex items-start gap-3">
                        <div className="w-6 h-6 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-1">
                          <span className="text-white text-sm">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-base leading-relaxed">
                          CHAQUE HISTOIRE AIDE À DÉCOUVRIR SA VRAIE FORCE INTÉRIEURE ET À CROIRE EN LUI
                        </p>
                      </div>
                    </div>
                    <div className="w-1/2 h-full">
                      <img alt="Enfant avec livre personnalisé" className="w-full h-full object-cover rounded-r-2xl" src="/lovable-uploads/37edb384-8f65-47c6-a599-0845eac26805.png" />
                    </div>
                  </div>
                </div>
                
                {/* Desktop Carousel Dots */}
                <div className="flex justify-center mt-6 gap-2">
                  <div className="w-2 h-2 bg-orange-500 rounded-full"></div>
                  <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                  <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                </div>
              </div>

              {/* Mobile Cards */}
              <div className="md:hidden">
                <div ref={mobileAutoScroll.scrollRef} className="flex gap-4 overflow-x-auto scrollbar-hide px-4" {...mobileAutoScroll.scrollProps}>
                  {/* Card 1 Mobile */}
                  <div className="min-w-[75vw] max-w-[75vw] bg-white rounded-2xl shadow-lg overflow-hidden h-96">
                    <div className="w-full h-48">
                      <img src="/lovable-uploads/1bdc8626-3a20-4e5b-ba67-86a2e4265fc4.png" alt="Enfant avec livre personnalisé" className="w-full h-full object-cover rounded-t-2xl" />
                    </div>
                    <div className="h-48 p-6 flex flex-col justify-center space-y-3">
                      <div className="flex items-start gap-3">
                        <div className="w-5 h-5 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5">
                          <span className="text-white text-xs">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-sm leading-relaxed">
                          VALORISE L'ENFANT : IL VOIT SON PRÉNOM ET SON VISAGE DANS L'HISTOIRE
                        </p>
                      </div>
                      <div className="flex items-start gap-3">
                        <div className="w-5 h-5 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5">
                          <span className="text-white text-xs">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-sm leading-relaxed">
                          EN DEVENANT LE HÉROS, L'ENFANT SE SENT CAPABLE ET IMPORTANT
                        </p>
                      </div>
                    </div>
                  </div>

                  {/* Card 2 Mobile */}
                  <div className="min-w-[75vw] max-w-[75vw] bg-white rounded-2xl shadow-lg overflow-hidden h-96">
                    <div className="w-full h-48">
                      <img src="/lovable-uploads/1bdc8626-3a20-4e5b-ba67-86a2e4265fc4.png" alt="Enfant avec livre personnalisé" className="w-full h-full object-cover rounded-t-2xl" />
                    </div>
                    <div className="h-48 p-6 flex flex-col justify-center space-y-3">
                      <div className="flex items-start gap-3">
                        <div className="w-5 h-5 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5">
                          <span className="text-white text-xs">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-sm leading-relaxed">
                          NOS HISTOIRES SONT VALIDÉES PAR UN PÉDOPSYCHIATRE
                        </p>
                      </div>
                      <div className="flex items-start gap-3">
                        <div className="w-5 h-5 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5">
                          <span className="text-white text-xs">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-sm leading-relaxed">
                          CHAQUE LIVRE CONTIENT UNE MORALE CLAIRE POUR GRANDIR
                        </p>
                      </div>
                    </div>
                  </div>

                  {/* Card 3 Mobile */}
                  <div className="min-w-[75vw] max-w-[75vw] bg-white rounded-2xl shadow-lg overflow-hidden h-96">
                    <div className="w-full h-48">
                      <img src="/lovable-uploads/1bdc8626-3a20-4e5b-ba67-86a2e4265fc4.png" alt="Enfant avec livre personnalisé" className="w-full h-full object-cover rounded-t-2xl" />
                    </div>
                    <div className="h-48 p-6 flex flex-col justify-center space-y-3">
                      <div className="flex items-start gap-3">
                        <div className="w-5 h-5 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5">
                          <span className="text-white text-xs">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-sm leading-relaxed">
                          IL APPREND DES VALEURS FONDAMENTALES POUR SON DÉVELOPPEMENT
                        </p>
                      </div>
                      <div className="flex items-start gap-3">
                        <div className="w-5 h-5 bg-green-500 rounded-full flex items-center justify-center flex-shrink-0 mt-0.5">
                          <span className="text-white text-xs">✓</span>
                        </div>
                        <p className="text-gray-800 font-medium text-sm leading-relaxed">
                          AIDE À DÉCOUVRIR SA VRAIE FORCE INTÉRIEURE
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
                
                {/* Mobile Carousel Dots */}
                <div className="flex justify-center mt-6 gap-2">
                  <div className="w-2 h-2 bg-orange-500 rounded-full"></div>
                  <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                  <div className="w-2 h-2 bg-gray-300 rounded-full"></div>
                </div>
              </div>
            </div>
          </div>

          {/* Footer - outside the background container */}
          <Footer />

          {/* WhatsApp Button */}
          <WhatsAppButton />
        </div>
      </CustomScrollbar>
    </>;
};

export default Index;
