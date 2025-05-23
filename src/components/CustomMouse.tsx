import React, { useEffect, useState } from 'react';

const CustomMouse = () => {
  const [position, setPosition] = useState({ x: 0, y: 0 });
  const [isClicking, setIsClicking] = useState(false);
  const [isVisible, setIsVisible] = useState(false);
  const [trailPositions, setTrailPositions] = useState<{x: number, y: number}[]>([]);
  
  useEffect(() => {
    // Track mouse movement with smoother updates
    const handleMouseMove = (e: MouseEvent) => {
      setPosition({ x: e.clientX, y: e.clientY });
      setIsVisible(true);
      
      // Add position to trail with slight delay
      setTimeout(() => {
        setTrailPositions(prev => {
          // Keep only the most recent positions
          const newPositions = [...prev, { x: e.clientX, y: e.clientY }];
          if (newPositions.length > 5) {
            return newPositions.slice(newPositions.length - 5);
          }
          return newPositions;
        });
      }, 50);
    };
    
    // Track mouse clicks
    const handleMouseDown = () => setIsClicking(true);
    const handleMouseUp = () => setIsClicking(false);
    
    // Track mouse leaving window
    const handleMouseLeave = () => setIsVisible(false);
    const handleMouseEnter = () => setIsVisible(true);
    
    // Add event listeners
    document.addEventListener('mousemove', handleMouseMove);
    document.addEventListener('mousedown', handleMouseDown);
    document.addEventListener('mouseup', handleMouseUp);
    document.addEventListener('mouseleave', handleMouseLeave);
    document.addEventListener('mouseenter', handleMouseEnter);
    
    // Remove event listeners on cleanup
    return () => {
      document.removeEventListener('mousemove', handleMouseMove);
      document.removeEventListener('mousedown', handleMouseDown);
      document.removeEventListener('mouseup', handleMouseUp);
      document.removeEventListener('mouseleave', handleMouseLeave);
      document.removeEventListener('mouseenter', handleMouseLeave);
    };
  }, []);
  
  // Don't render if not visible
  if (!isVisible) return null;
  
  return (
    <>
      {/* Trail effect */}
      {trailPositions.map((pos, index) => (
        <div 
          key={index}
          className="pointer-events-none fixed z-50 rounded-full bg-green-400/10"
          style={{ 
            left: `${pos.x}px`, 
            top: `${pos.y}px`, 
            transform: 'translate(-50%, -50%)',
            width: `${8 + index * 3}px`,
            height: `${8 + index * 3}px`,
            opacity: 0.3 - index * 0.05
          }} 
        />
      ))}
    
      {/* Main cursor */}
      <div className="pointer-events-none fixed z-50" style={{ 
        left: `${position.x}px`, 
        top: `${position.y}px`, 
        transform: 'translate(-50%, -50%)' 
      }}>
        {/* Main cursor effect */}
        <div className={`absolute rounded-full transition-all duration-200 ${
          isClicking ? 'h-4 w-4 bg-green-500/80' : 'h-6 w-6 bg-green-400/50 backdrop-blur-sm'
        }`} />
        
        {/* Glow effect */}
        <div className="absolute h-10 w-10 rounded-full bg-green-400/20 animate-pulse-slow" />
        
        {/* Ring effect */}
        <div className="absolute h-16 w-16 rounded-full border border-green-400/10 animate-pulse" />
      </div>
    </>
  );
};

export default CustomMouse;
