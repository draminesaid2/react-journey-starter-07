
import React, { useState, useEffect } from 'react';
import { useTheme } from 'next-themes';
import FakeTerminal from '../components/FakeTerminal';
import DesktopIcon from '../components/DesktopIcon';
import WindowsFooter from '../components/WindowsFooter';
import CustomMouse from '../components/CustomMouse';
import { FileText, Settings, Terminal, Globe } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

const Desktop = () => {
  const [showTerminal, setShowTerminal] = useState(true);
  const [mounted, setMounted] = useState(false);
  const { theme, setTheme } = useTheme();
  const navigate = useNavigate();
  
  // Ensure component is mounted before accessing theme
  useEffect(() => {
    setMounted(true);
  }, []);

  // Auto-hide terminal after 6 seconds
  useEffect(() => {
    if (showTerminal) {
      const timer = setTimeout(() => {
        setShowTerminal(false);
      }, 6000);
      return () => clearTimeout(timer);
    }
  }, [showTerminal]);

  // Handle icon clicks
  const handleIconClick = (iconType: string) => {
    switch (iconType) {
      case 'terminal':
        setShowTerminal(true);
        break;
      case 'browser':
        navigate('/');
        break;
      case 'settings':
        // Toggle theme
        setTheme(theme === 'dark' ? 'light' : 'dark');
        break;
      default:
        break;
    }
  };

  if (!mounted) return null;

  return (
    <div className="relative h-screen w-full overflow-hidden bg-gradient-to-br from-blue-500/20 to-purple-600/20 dark:from-blue-900/40 dark:to-purple-900/40">
      {/* Background wallpaper */}
      <div className="absolute inset-0 bg-cover bg-center bg-no-repeat opacity-50 dark:opacity-30"
        style={{ backgroundImage: `url('https://images.unsplash.com/photo-1579546929518-9e396f3cc809?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1470&q=80')` }}>
      </div>
      
      {/* Desktop icons */}
      <div className="absolute top-6 left-6 grid grid-cols-1 gap-6">
        <DesktopIcon
          icon={<Terminal size={24} />}
          label="Terminal"
          onClick={() => handleIconClick('terminal')}
        />
        <DesktopIcon
          icon={<Globe size={24} />}
          label="Browser"
          onClick={() => handleIconClick('browser')}
        />
        <DesktopIcon
          icon={<FileText size={24} />}
          label="Documents"
        />
        <DesktopIcon
          icon={<Settings size={24} />}
          label="Settings"
          onClick={() => handleIconClick('settings')}
        />
      </div>
      
      {/* Terminal window */}
      {showTerminal && (
        <FakeTerminal onClose={() => setShowTerminal(false)} autoCloseAfter={6000} />
      )}
      
      {/* Windows-like footer */}
      <WindowsFooter theme={theme} setTheme={setTheme} />
      
      {/* Custom mouse effect */}
      <CustomMouse />
    </div>
  );
};

export default Desktop;
