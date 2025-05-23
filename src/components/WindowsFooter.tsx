
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Terminal, 
  Globe, 
  Home, 
  Settings, 
  Moon, 
  Sun, 
  Laptop,
  Search,
  BellRing 
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { useToast } from '@/hooks/use-toast';

interface WindowsFooterProps {
  theme: string | undefined;
  setTheme: (theme: string) => void;
}

const WindowsFooter: React.FC<WindowsFooterProps> = ({ theme, setTheme }) => {
  const navigate = useNavigate();
  const { toast } = useToast();
  const [showStartMenu, setShowStartMenu] = useState(false);
  
  const handleNavigation = (path: string) => {
    navigate(path);
  };
  
  const toggleTheme = () => {
    const newTheme = theme === 'dark' ? 'light' : 'dark';
    setTheme(newTheme);
    toast({
      title: "Theme Changed",
      description: `Switched to ${newTheme} mode`,
    });
  };
  
  const currentTime = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  
  return (
    <div className="absolute bottom-0 left-0 right-0 h-12 bg-windows-taskbar backdrop-blur-md border-t border-white/10 flex items-center justify-between px-2 z-50">
      {/* Start button and taskbar icons */}
      <div className="flex items-center space-x-2">
        <Popover open={showStartMenu} onOpenChange={setShowStartMenu}>
          <PopoverTrigger asChild>
            <Button 
              variant="ghost" 
              className="h-10 w-10 rounded-full bg-windows-blue text-white hover:bg-windows-blue/90"
              onClick={() => setShowStartMenu(!showStartMenu)}
            >
              <Home size={20} />
            </Button>
          </PopoverTrigger>
          <PopoverContent side="top" align="start" className="w-64 p-0 mb-2">
            <div className="bg-background/95 backdrop-blur-md border rounded-lg shadow-lg">
              <div className="p-3 text-sm font-medium border-b">
                Start Menu
              </div>
              <div className="p-2">
                <Button 
                  variant="ghost" 
                  className="w-full justify-start mb-1 text-sm"
                  onClick={() => {
                    handleNavigation('/');
                    setShowStartMenu(false);
                  }}
                >
                  <Globe className="mr-2 h-4 w-4" />
                  Browser
                </Button>
                <Button 
                  variant="ghost" 
                  className="w-full justify-start mb-1 text-sm"
                  onClick={() => {
                    const desktop = document.querySelector('.terminal-icon');
                    if (desktop) (desktop as HTMLElement).click();
                    setShowStartMenu(false);
                  }}
                >
                  <Terminal className="mr-2 h-4 w-4" />
                  Terminal
                </Button>
                <Button 
                  variant="ghost" 
                  className="w-full justify-start text-sm"
                  onClick={() => {
                    toggleTheme();
                    setShowStartMenu(false);
                  }}
                >
                  {theme === 'dark' ? (
                    <Sun className="mr-2 h-4 w-4" />
                  ) : (
                    <Moon className="mr-2 h-4 w-4" />
                  )}
                  {theme === 'dark' ? 'Light Mode' : 'Dark Mode'}
                </Button>
              </div>
            </div>
          </PopoverContent>
        </Popover>
        
        <Button 
          variant="ghost" 
          size="icon" 
          className="rounded-md"
          onClick={() => handleNavigation('/')}
        >
          <Globe size={18} />
        </Button>
        
        <Button 
          variant="ghost" 
          size="icon"
          className="terminal-icon rounded-md"
          onClick={() => {
            const desktop = document.querySelector('[data-desktop]');
            if (desktop) {
              const event = new CustomEvent('open-terminal');
              desktop.dispatchEvent(event);
            }
          }}
        >
          <Terminal size={18} />
        </Button>
      </div>
      
      {/* System tray */}
      <div className="flex items-center space-x-2">
        <Button 
          variant="ghost" 
          size="icon" 
          className="rounded-md"
          onClick={() => toast({ title: "No new notifications" })}
        >
          <BellRing size={16} />
        </Button>
        
        <Button 
          variant="ghost" 
          size="icon" 
          className="rounded-md"
          onClick={toggleTheme}
        >
          {theme === 'dark' ? <Moon size={16} /> : <Sun size={16} />}
        </Button>
        
        <div className="text-xs font-medium px-3">{currentTime}</div>
      </div>
    </div>
  );
};

export default WindowsFooter;
