
import React, { useEffect, useState } from 'react';
import FakeTerminal from './FakeTerminal';
import CustomMouse from './CustomMouse';
import WindowsFooter from './WindowsFooter';

interface DesktopProps {
  children: React.ReactNode;
}

const Desktop = ({ children }: DesktopProps) => {
  const [showTerminal, setShowTerminal] = useState<boolean>(true);

  // Auto-close the terminal after some time
  useEffect(() => {
    // Terminal will auto-close after 5 seconds
    const autoCloseTime = 5000;
    return () => {}; // Cleanup function for useEffect
  }, []);

  return (
    <div className="relative min-h-screen w-full overflow-hidden bg-gradient-to-br from-browser-DEFAULT via-browser-lighter to-browser-DEFAULT">
      {/* Desktop icons could be added here in the future */}
      
      {/* Main content */}
      <div className="relative z-10 h-full">
        {children}
      </div>
      
      {/* Initial terminal that shows on startup */}
      {showTerminal && (
        <FakeTerminal 
          onClose={() => setShowTerminal(false)}
          autoCloseAfter={3000} // Auto close after 3 seconds
        />
      )}

      {/* Custom mouse effect */}
      <CustomMouse />
      
      {/* Windows-like footer */}
      <WindowsFooter />
    </div>
  );
};

export default Desktop;
