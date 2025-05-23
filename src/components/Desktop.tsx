
import React, { useEffect, useState } from 'react';
import FakeTerminal from './FakeTerminal';
import CustomMouse from './CustomMouse';
import WindowsFooter from './WindowsFooter';
import { Folder, Terminal } from 'lucide-react';
import DesktopIcon from './DesktopIcon';

interface DesktopProps {
  children: React.ReactNode;
}

const Desktop = ({ children }: DesktopProps) => {
  const [showTerminal, setShowTerminal] = useState<boolean>(true);
  const [showHackingAnimation, setShowHackingAnimation] = useState<boolean>(false);
  const [activeTerminalIndex, setActiveTerminalIndex] = useState<number>(0);

  // Auto-close the initial terminal after some time
  useEffect(() => {
    const timer = setTimeout(() => {
      setShowTerminal(false);
    }, 5000);
    
    return () => clearTimeout(timer);
  }, []);

  // Handle the hacking terminal animation
  useEffect(() => {
    if (!showHackingAnimation) return;
    
    let currentIndex = 0;
    const terminalCount = 4;
    const intervalTime = 2000; // Time each terminal stays visible
    
    const interval = setInterval(() => {
      currentIndex += 1;
      if (currentIndex >= terminalCount) {
        // Keep the last terminal visible
        clearInterval(interval);
      }
      setActiveTerminalIndex(currentIndex);
    }, intervalTime);
    
    return () => clearInterval(interval);
  }, [showHackingAnimation]);

  const handleOpenTerminal = () => {
    setShowHackingAnimation(true);
    setActiveTerminalIndex(0);
  };

  return (
    <div className="relative min-h-screen w-full overflow-hidden bg-gradient-to-br from-browser-DEFAULT via-browser-lighter to-browser-DEFAULT">
      {/* Desktop icons */}
      <div className="absolute top-4 left-4 grid gap-6">
        <DesktopIcon 
          icon={<Terminal size={24} />} 
          label="Terminal"
          onClick={handleOpenTerminal}
          className="hover:bg-white/10"
        />
        <DesktopIcon 
          icon={<Folder size={24} />} 
          label="My Files"
          className="hover:bg-white/10"
        />
      </div>
      
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

      {/* Hacking terminals animation */}
      {showHackingAnimation && (
        <>
          {/* First three terminals are temporary */}
          {activeTerminalIndex < 3 && (
            <FakeTerminal
              onClose={() => setShowHackingAnimation(false)}
              isHacking={true}
              autoCloseAfter={null}
              terminalTitle={`Terminal ${activeTerminalIndex + 1}/4`}
            />
          )}
          
          {/* Final terminal with the message */}
          {activeTerminalIndex >= 3 && (
            <FakeTerminal
              onClose={() => setShowHackingAnimation(false)}
              isHacking={false}
              autoCloseAfter={null}
              terminalTitle="Terminal - Final Result"
              finalMessage="yes i am very good ad cool features and says that i am a good react js node js fullstack developer with experience with laravel, php, react native mangodb and sql"
            />
          )}
        </>
      )}

      {/* Custom mouse effect */}
      <CustomMouse />
      
      {/* Windows-like footer */}
      <WindowsFooter onTerminalClick={handleOpenTerminal} />
    </div>
  );
};

export default Desktop;
