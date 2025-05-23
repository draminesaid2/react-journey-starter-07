
import React, { useEffect, useState } from 'react';

const CustomMouse = () => {
  const [position, setPosition] = useState({ x: 0, y: 0 });
  const [isClicking, setIsClicking] = useState(false);
  const [isVisible, setIsVisible] = useState(false);
  
  useEffect(() => {
    // Track mouse movement
    const handleMouseMove = (e: MouseEvent) => {
      setPosition({ x: e.clientX, y: e.clientY });
      setIsVisible(true);
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
    <div className="pointer-events-none fixed z-50" style={{ 
      left: `${position.x}px`, 
      top: `${position.y}px`, 
      transform: 'translate(-50%, -50%)' 
    }}>
      {/* Main cursor effect */}
      <div className={`absolute rounded-full transition-all duration-300 ${
        isClicking ? 'h-4 w-4 bg-green-500/60' : 'h-6 w-6 bg-green-400/30'
      }`} />
      
      {/* Trailing effect */}
      <div className="absolute h-10 w-10 rounded-full bg-green-400/10 animate-pulse-slow" />
    </div>
  );
};

export default CustomMouse;
