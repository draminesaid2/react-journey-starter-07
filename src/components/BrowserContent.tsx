import React, { useEffect, useState, useRef } from 'react';
import { 
  Globe, 
  Folder, 
  Files, 
  Trash2, 
  Settings, 
  Computer, 
  Info, 
  File, 
  Search, 
  Linkedin, 
  Github,
  Award,
  Briefcase,
  School, 
  Languages,
  Music,
  Mail,
  Phone,
  MapPin,
  Terminal
} from 'lucide-react';
import DesktopIcon from './DesktopIcon';
import SkillsFolder from './windows/SkillsFolder';
import EducationFolder from './windows/EducationFolder';
import WorkFolder from './windows/WorkFolder';
import LanguagesFolder from './windows/LanguagesFolder';
import HobbiesFolder from './windows/HobbiesFolder';
import FakeTerminal from './FakeTerminal';

interface BrowserContentProps {
  url: string;
  onTitleChange: (title: string) => void;
  onError: (error: string) => void;
}

const BrowserContent = ({ url, onTitleChange, onError }: BrowserContentProps) => {
  const [iframeUrl, setIframeUrl] = useState<string>('');
  const [proxyUrl, setProxyUrl] = useState<string>('');
  const [showBrowser, setShowBrowser] = useState<boolean>(false);
  const [openFolder, setOpenFolder] = useState<string | null>(null);
  const [currentTime, setCurrentTime] = useState<string>(
    new Date().toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'})
  );
  const [showTerminal, setShowTerminal] = useState<boolean>(true);
  const [currentDate, setCurrentDate] = useState<string>(
    new Date().toLocaleDateString('en-US', {weekday: 'short', month: 'short', day: 'numeric'})
  );
  const iframeRef = useRef<HTMLIFrameElement>(null);
  
  useEffect(() => {
    try {
      // Check if it's a search query or a valid URL
      if (url.startsWith('Search of')) {
        onTitleChange(url);
        setIframeUrl('');
        setProxyUrl('');
      } else if (!url.includes('://')) {
        // If no protocol is specified, add https://
        const urlWithProtocol = `https://${url}`;
        
        try {
          // For actual URLs, try to create a URL object
          const urlObj = new URL(urlWithProtocol);
          
          // Use a CORS proxy to fetch content
          const corsProxyUrl = `https://corsproxy.io/?${encodeURIComponent(urlWithProtocol)}`;
          setProxyUrl(corsProxyUrl);
          
          // For direct iframe loading (only works for sites that allow iframe embedding)
          setIframeUrl(urlWithProtocol);
          
          // Set title to domain
          onTitleChange(urlObj.hostname);
        } catch (urlError) {
          console.error("URL Error:", urlError);
          onError("Invalid URL format. Please check your URL and try again.");
          setIframeUrl('');
          setProxyUrl('');
        }
      } else {
        // For already formatted URLs
        try {
          const urlObj = new URL(url);
          
          // Use a CORS proxy to fetch content
          const corsProxyUrl = `https://corsproxy.io/?${encodeURIComponent(url)}`;
          setProxyUrl(corsProxyUrl);
          
          // For direct iframe loading (only works for sites that allow iframe embedding)
          setIframeUrl(url);
          
          // Set title to domain
          onTitleChange(urlObj.hostname);
        } catch (urlError) {
          console.error("URL Error:", urlError);
          onError("Invalid URL format. Please check your URL and try again.");
          setIframeUrl('');
          setProxyUrl('');
        }
      }
    } catch (error) {
      console.error("URL Error:", error);
      onError("Invalid URL format. Please check your URL and try again.");
      setIframeUrl('');
      setProxyUrl('');
    }
  }, [url, onTitleChange, onError]);

  // Update clock every minute
  useEffect(() => {
    const timer = setInterval(() => {
      const now = new Date();
      setCurrentTime(now.toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}));
      setCurrentDate(now.toLocaleDateString('en-US', {weekday: 'short', month: 'short', day: 'numeric'}));
    }, 60000);
    return () => clearInterval(timer);
  }, []);

  const handleIframeLoad = () => {
    try {
      // Try to get page title from iframe (only works for same-origin content)
      if (iframeRef.current?.contentDocument?.title) {
        onTitleChange(iframeRef.current.contentDocument.title);
      }
    } catch (e) {
      // Cross-origin restrictions prevent access
      console.log("Cannot access iframe content due to cross-origin restrictions");
    }
  };

  const handleIframeError = () => {
    onError("Failed to load the requested page. It might not allow embedding.");
  };

  const handleOpenBrowser = () => {
    setShowBrowser(true);
  };

  const handleOpenFolder = (folderName: string) => {
    setOpenFolder(folderName);
  };

  const handleCloseFolder = () => {
    setOpenFolder(null);
  };

  const handleCloseTerminal = () => {
    setShowTerminal(false);
  };

  const openExternalLink = (url: string) => {
    window.open(url, "_blank", "noopener,noreferrer");
  };

  const handleOpenTerminal = () => {
    setShowTerminal(true);
  };

  return (
    <div className="w-full h-full flex flex-col bg-[#0078D7] relative">
      {/* Fake Terminal */}
      {showTerminal && (
        <FakeTerminal 
          onClose={handleCloseTerminal} 
          autoCloseAfter={4000}
        />
      )}
      
      {showBrowser ? (
        // ... keep existing code (Browser iframe and fallback message section)
        <>
          {/* Browser iframe (when browser icon is clicked) */}
          {iframeUrl && (
            <iframe
              ref={iframeRef}
              src={iframeUrl}
              className="w-full h-full border-0"
              sandbox="allow-same-origin allow-scripts allow-popups allow-forms"
              onLoad={handleIframeLoad}
              onError={handleIframeError}
              title="browser content"
            />
          )}
          
          {/* Fallback message for blocked content */}
          <div className="absolute inset-0 flex items-center justify-center bg-browser-DEFAULT bg-opacity-90 text-white p-4 md:p-8 text-center backdrop-blur-sm">
            <div className="max-w-2xl bg-gray-900/80 p-4 md:p-8 rounded-xl border border-white/10 shadow-2xl">
              <h2 className="text-xl md:text-2xl font-bold mb-2 md:mb-4">Internet Browser</h2>
              <p className="mb-3 md:mb-6 text-sm md:text-base">
                Due to browser security restrictions, many websites block direct embedding in iframes. 
                For a full browser experience, we would need to set up a server-side proxy.
              </p>
              <div className="bg-black/30 p-2 md:p-4 rounded-md text-left mb-3 md:mb-6 font-mono text-xs md:text-sm">
                <p className="mb-1 md:mb-2">Attempting to load: {url}</p>
                <p>Via proxy: {proxyUrl}</p>
              </div>
              <div className="flex justify-center mt-2 md:mt-4">
                <button 
                  onClick={() => setShowBrowser(false)}
                  className="px-3 md:px-5 py-1.5 md:py-2.5 bg-blue-600 rounded-md hover:bg-blue-700 transition-colors font-medium text-sm md:text-base"
                >
                  Return to Desktop
                </button>
              </div>
            </div>
          </div>
        </>
      ) : (
        /* Windows Desktop View */
        <div 
          className="w-full h-full p-2 md:p-4 overflow-auto relative" 
          style={{ 
            backgroundImage: 'url("https://motionbgs.com/media/715/windows-11.jpg")', 
            backgroundSize: 'cover', 
            backgroundPosition: 'center',
            backgroundRepeat: 'no-repeat'
          }}
        >
          {/* Desktop Icons - modern grid layout with responsive adjustments */}
          <div className="grid grid-cols-3 sm:grid-cols-4 md:grid-cols-auto-fill gap-1 sm:gap-2 md:gap-3 pt-1 md:pt-2">
            <DesktopIcon
              icon={<Computer size={24} className="text-white" />}
              label="This PC"
              onClick={() => alert("Opening This PC")}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Trash2 size={24} className="text-white" />}
              label="Recycle Bin"
              onClick={() => alert("Opening Recycle Bin")}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Github size={24} className="text-white" />}
              label="GitHub"
              onClick={() => openExternalLink("https://github.com")}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Linkedin size={24} className="text-blue-400" />}
              label="LinkedIn"
              onClick={() => openExternalLink("https://linkedin.com")}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<School size={24} className="text-yellow-400" />}
              label="Education"
              onClick={() => handleOpenFolder('education')}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Briefcase size={24} className="text-gray-200" />}
              label="Work"
              onClick={() => handleOpenFolder('work')}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Award size={24} className="text-amber-400" />}
              label="Skills"
              onClick={() => handleOpenFolder('skills')}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Languages size={24} className="text-green-400" />}
              label="Languages"
              onClick={() => handleOpenFolder('languages')}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Music size={24} className="text-purple-400" />}
              label="Hobbies"
              onClick={() => handleOpenFolder('hobbies')}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Globe size={24} className="text-blue-400" />}
              label="Browser"
              onClick={handleOpenBrowser}
              className="hover:bg-white/20 hover:scale-105"
            />
            
            <DesktopIcon
              icon={<Terminal size={24} className="text-green-400" />}
              label="Terminal"
              onClick={handleOpenTerminal}
              className="hover:bg-white/20 hover:scale-105"
            />
          </div>

          {/* Windows 11 Style Taskbar - responsive */}
          <div className="absolute bottom-0 left-0 right-0 h-10 md:h-14 bg-black/40 backdrop-blur-md flex items-center justify-between px-2 md:px-3 z-10 shadow-lg">
            {/* Left area with Start Button and Quick Launch */}
            <div className="flex items-center h-full">
              {/* Start Button */}
              <div className="h-8 w-8 md:h-10 md:w-10 rounded-md flex items-center justify-center bg-blue-600/80 hover:bg-blue-600 transition-all cursor-pointer mx-0.5 md:mx-1">
                <div className="grid grid-cols-2 grid-rows-2 gap-0.5">
                  <div className="w-1.5 h-1.5 md:w-2 md:h-2 bg-white"></div>
                  <div className="w-1.5 h-1.5 md:w-2 md:h-2 bg-white"></div>
                  <div className="w-1.5 h-1.5 md:w-2 md:h-2 bg-white"></div>
                  <div className="w-1.5 h-1.5 md:w-2 md:h-2 bg-white"></div>
                </div>
              </div>
              
              {/* Search Button */}
              <div className="h-8 w-8 md:h-10 md:w-10 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer mx-0.5 md:mx-1">
                <Search size={16} className="text-white md:hidden" />
                <Search size={20} className="text-white hidden md:block" />
              </div>
              
              {/* Quick Launch Icons - hide some on mobile */}
              <div className="h-8 md:h-10 flex items-center space-x-0.5 md:space-x-1 px-0.5 md:px-1">
                <div className="w-8 h-8 md:w-10 md:h-10 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer" onClick={handleOpenBrowser}>
                  <Globe size={16} className="text-white md:hidden" />
                  <Globe size={20} className="text-white hidden md:block" />
                </div>
                <div className="w-8 h-8 md:w-10 md:h-10 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer hidden sm:flex" onClick={() => handleOpenFolder('work')}>
                  <Folder size={16} className="text-white md:hidden" />
                  <Folder size={20} className="text-white hidden md:block" />
                </div>
                <div className="w-8 h-8 md:w-10 md:h-10 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer hidden md:flex">
                  <File size={16} className="text-white md:hidden" />
                  <File size={20} className="text-white hidden md:block" />
                </div>
                <div className="w-8 h-8 md:w-10 md:h-10 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer">
                  <Settings size={16} className="text-white md:hidden" />
                  <Settings size={20} className="text-white hidden md:block" />
                </div>
              </div>
            </div>
            
            {/* System Tray */}
            <div className="h-full flex items-center">
              {/* Contact Icons */}
              <div className="hidden md:flex items-center mr-3 space-x-2">
                <div className="w-8 h-8 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer" onClick={() => openExternalLink("mailto:contact@ihebchebbi.pro")}>
                  <Mail size={18} className="text-white" />
                </div>
                <div className="w-8 h-8 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer" onClick={() => openExternalLink("tel:+21629249512")}>
                  <Phone size={18} className="text-white" />
                </div>
                <div className="w-8 h-8 rounded-md flex items-center justify-center hover:bg-white/10 transition-colors cursor-pointer" onClick={() => openExternalLink("https://maps.google.com?q=Tunisia")}>
                  <MapPin size={18} className="text-white" />
                </div>
              </div>
              
              {/* Clock and Date */}
              <div className="text-xs md:text-sm text-white px-1 md:px-3 font-medium flex flex-col items-end">
                <div>{currentTime}</div>
                <div className="text-xs text-white/80 hidden md:block">{currentDate}</div>
              </div>
            </div>
          </div>
          
          {/* Folder Windows */}
          <SkillsFolder 
            isOpen={openFolder === 'skills'} 
            onClose={handleCloseFolder} 
          />
          <EducationFolder 
            isOpen={openFolder === 'education'} 
            onClose={handleCloseFolder} 
          />
          <WorkFolder 
            isOpen={openFolder === 'work'} 
            onClose={handleCloseFolder} 
          />
          <LanguagesFolder 
            isOpen={openFolder === 'languages'} 
            onClose={handleCloseFolder} 
          />
          <HobbiesFolder 
            isOpen={openFolder === 'hobbies'} 
            onClose={handleCloseFolder} 
          />
        </div>
      )}
    </div>
  );
};

export default BrowserContent;
