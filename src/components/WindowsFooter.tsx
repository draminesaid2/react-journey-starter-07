
import React, { useState, useEffect } from 'react';
import { Home, Sun, Moon, Settings, Terminal } from 'lucide-react';
import { useTheme } from 'next-themes';
import { useToast } from "@/hooks/use-toast";
import { useNavigate } from 'react-router-dom';

const WindowsFooter = () => {
  const { theme, setTheme } = useTheme();
  const { toast } = useToast();
  const navigate = useNavigate();
  const [mounted, setMounted] = useState(false);
  const [showSettings, setShowSettings] = useState(false);

  // Ensure theme manipulation only happens after component mount
  useEffect(() => {
    setMounted(true);
  }, []);

  if (!mounted) return null;

  const toggleTheme = () => {
    const newTheme = theme === 'dark' ? 'light' : 'dark';
    setTheme(newTheme);
    toast({
      title: "Theme Changed",
      description: `Switched to ${newTheme} mode`,
    });
  };

  const openTerminal = () => {
    toast({
      description: "Opening Terminal...",
    });
    // You could implement opening a new terminal instance here
  };

  return (
    <>
      {/* Windows-style taskbar */}
      <div className="fixed bottom-0 left-0 z-20 w-full bg-windows-taskbar backdrop-blur-md shadow-lg">
        <div className="flex items-center justify-between px-2 py-1 h-12">
          {/* Start menu and taskbar buttons */}
          <div className="flex items-center space-x-2">
            <button 
              onClick={() => navigate('/')}
              className="p-2 rounded-md hover:bg-white/10 transition-colors"
            >
              <Home size={20} className="text-white" />
            </button>
            
            <button 
              onClick={openTerminal}
              className="p-2 rounded-md hover:bg-white/10 transition-colors"
            >
              <Terminal size={20} className="text-white" />
            </button>
            
            {/* You can add more app buttons here */}
          </div>
          
          {/* Right side controls */}
          <div className="flex items-center space-x-2">
            <button 
              onClick={toggleTheme}
              className="p-2 rounded-md hover:bg-white/10 transition-colors"
            >
              {theme === 'dark' ? (
                <Sun size={20} className="text-white" />
              ) : (
                <Moon size={20} className="text-white" />
              )}
            </button>
            
            <button 
              onClick={() => setShowSettings(!showSettings)}
              className="p-2 rounded-md hover:bg-white/10 transition-colors"
            >
              <Settings size={20} className="text-white" />
            </button>
          </div>
        </div>
      </div>
      
      {/* Settings panel */}
      {showSettings && (
        <div className="fixed bottom-14 right-2 z-20 w-60 rounded-lg bg-popover/90 backdrop-blur-md p-4 shadow-lg border border-border">
          <h3 className="text-sm font-medium mb-3">Settings</h3>
          
          <div className="space-y-3">
            <div className="flex items-center justify-between">
              <span className="text-sm">Theme</span>
              <div className="flex space-x-2">
                <button
                  onClick={() => setTheme('light')}
                  className={`p-1.5 rounded-md ${theme === 'light' ? 'bg-primary text-primary-foreground' : 'hover:bg-muted'}`}
                >
                  <Sun size={16} />
                </button>
                <button
                  onClick={() => setTheme('dark')}
                  className={`p-1.5 rounded-md ${theme === 'dark' ? 'bg-primary text-primary-foreground' : 'hover:bg-muted'}`}
                >
                  <Moon size={16} />
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default WindowsFooter;
