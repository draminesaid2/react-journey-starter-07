
import React, { useState, useEffect } from 'react';

const CustomMouse = () => {
  const [position, setPosition] = useState({ x: -100, y: -100 });
  const [isPointer, setIsPointer] = useState(false);
  const [isClicking, setIsClicking] = useState(false);
  const [trail, setTrail] = useState<{ x: number, y: number, opacity: number }[]>([]);
  
  useEffect(() => {
    const updatePosition = (e: MouseEvent) => {
      setPosition({ x: e.clientX, y: e.clientY });
      
      // Add a new point to the trail
      setTrail(prevTrail => {
        const newPoint = { x: e.clientX, y: e.clientY, opacity: 1 };
        const updatedTrail = [...prevTrail, newPoint].slice(-15); // Keep last 15 points
        return updatedTrail;
      });
      
      // Update the isPointer state based on the cursor style
      const target = e.target as HTMLElement;
      if (target) {
        const cursorStyle = window.getComputedStyle(target).cursor;
        setIsPointer(cursorStyle === 'pointer');
      }
    };
    
    const handleMouseDown = () => setIsClicking(true);
    const handleMouseUp = () => setIsClicking(false);
    
    // Fade trail points over time
    const fadeTrail = () => {
      setTrail(prevTrail => 
        prevTrail.map(point => ({
          ...point,
          opacity: point.opacity > 0 ? point.opacity - 0.05 : 0
        })).filter(point => point.opacity > 0)
      );
    };
    
    const fadeInterval = setInterval(fadeTrail, 20);
    
    window.addEventListener('mousemove', updatePosition);
    window.addEventListener('mousedown', handleMouseDown);
    window.addEventListener('mouseup', handleMouseUp);
    
    return () => {
      window.removeEventListener('mousemove', updatePosition);
      window.removeEventListener('mousedown', handleMouseDown);
      window.removeEventListener('mouseup', handleMouseUp);
      clearInterval(fadeInterval);
    };
  }, []);
  
  if (typeof window === 'undefined') return null;
  
  return (
    <>
      {/* Trail effect */}
      {trail.map((point, index) => (
        <div
          key={index}
          className="fixed pointer-events-none z-[9999]"
          style={{
            left: point.x,
            top: point.y,
            width: `${8 - index * 0.5}px`,
            height: `${8 - index * 0.5}px`,
            opacity: point.opacity,
            backgroundColor: isPointer ? 'rgba(156, 163, 255, 0.7)' : 'rgba(255, 255, 255, 0.7)',
            borderRadius: '50%',
            transform: 'translate(-50%, -50%)',
            transition: 'opacity 0.1s ease',
          }}
        />
      ))}
      
      {/* Main cursor */}
      <div
        className="fixed pointer-events-none z-[10000] mix-blend-difference"
        style={{
          left: position.x,
          top: position.y,
          transform: 'translate(-50%, -50%)',
          transition: 'width 0.2s, height 0.2s, background-color 0.2s'
        }}
      >
        {/* Outer ring */}
        <div
          className={`absolute rounded-full border-2 backdrop-blur-sm transition-all duration-200 ${
            isClicking ? 'scale-90' : 'scale-100'
          }`}
          style={{
            width: isPointer ? '30px' : '40px',
            height: isPointer ? '30px' : '40px',
            borderColor: isPointer ? 'rgba(99, 102, 241, 0.8)' : 'rgba(255, 255, 255, 0.8)',
            transform: 'translate(-50%, -50%)',
            top: '50%',
            left: '50%'
          }}
        />
        
        {/* Inner dot */}
        <div
          className={`absolute rounded-full transition-all duration-200 ${
            isClicking ? 'scale-75' : 'scale-100'
          }`}
          style={{
            width: isPointer ? '8px' : '5px',
            height: isPointer ? '8px' : '5px',
            backgroundColor: isPointer ? 'rgba(99, 102, 241, 1)' : 'rgba(255, 255, 255, 1)',
            transform: 'translate(-50%, -50%)',
            top: '50%',
            left: '50%'
          }}
        />
      </div>
    </>
  );
};

export default CustomMouse;
