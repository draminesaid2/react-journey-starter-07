
import React, { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { ArrowLeft, ArrowRight, RefreshCw, Search, Globe, History } from 'lucide-react';

interface BrowserBarProps {
  url: string;
  onNavigate: (url: string) => void;
  onSearch: (query: string) => void;
  onToggleHistory: () => void;
  isLoading: boolean;
}

const BrowserBar = ({ url, onNavigate, onSearch, onToggleHistory, isLoading }: BrowserBarProps) => {
  const [inputValue, setInputValue] = useState<string>(url);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    if (inputValue.trim() === '') return;
    
    // Check if input is a URL or search query
    const isUrl = /^(http|https):\/\//.test(inputValue) || 
                  /^[a-zA-Z0-9][a-zA-Z0-9-]{1,61}[a-zA-Z0-9]\.[a-zA-Z]{2,}/.test(inputValue);
    
    if (isUrl) {
      onNavigate(inputValue);
    } else {
      onSearch(inputValue);
    }
  };

  // Update input when URL changes externally
  React.useEffect(() => {
    setInputValue(url);
  }, [url]);

  const handleRefresh = () => {
    onNavigate(url);
  };

  return (
    <div className="bg-gray-800/90 backdrop-blur-md p-1.5 sm:p-2.5 shadow-lg border-b border-gray-700/50">
      <div className="flex items-center gap-1 sm:gap-2">
        <Button 
          onClick={() => window.history.back()}
          variant="ghost" 
          size="icon"
          className="text-gray-400 hover:text-gray-100 hover:bg-gray-700/50 h-7 w-7 sm:h-8 sm:w-8"
        >
          <ArrowLeft size={16} className="sm:hidden" />
          <ArrowLeft size={18} className="hidden sm:inline" />
        </Button>
        
        <Button 
          onClick={() => window.history.forward()}
          variant="ghost" 
          size="icon"
          className="text-gray-400 hover:text-gray-100 hover:bg-gray-700/50 h-7 w-7 sm:h-8 sm:w-8"
        >
          <ArrowRight size={16} className="sm:hidden" />
          <ArrowRight size={18} className="hidden sm:inline" />
        </Button>
        
        <Button 
          onClick={handleRefresh}
          variant="ghost" 
          size="icon"
          className={`text-gray-400 hover:text-gray-100 hover:bg-gray-700/50 h-7 w-7 sm:h-8 sm:w-8 ${isLoading ? 'animate-spin' : ''}`}
        >
          <RefreshCw size={16} className="sm:hidden" />
          <RefreshCw size={18} className="hidden sm:inline" />
        </Button>
        
        <form onSubmit={handleSubmit} className="flex-1 relative">
          <div className="absolute left-2 sm:left-3 top-1/2 transform -translate-y-1/2 text-gray-400">
            <Search size={14} className="sm:hidden" />
            <Search size={15} className="hidden sm:inline" />
          </div>
          <Input 
            type="text"
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
            className="pl-7 sm:pl-9 pr-7 sm:pr-9 bg-gray-700/50 border-none focus:ring-1 focus:ring-blue-500 text-white w-full h-8 sm:h-9 rounded-full text-xs sm:text-sm"
            placeholder="Search or enter address..."
          />
          <Button 
            type="submit"
            size="sm"
            variant="ghost"
            className="absolute right-1 sm:right-1.5 top-1/2 transform -translate-y-1/2 h-6 w-6 p-0 text-gray-400 hover:text-gray-100"
          >
            <Globe size={12} className="sm:hidden" />
            <Globe size={14} className="hidden sm:inline" />
          </Button>
        </form>
        
        <Button 
          onClick={onToggleHistory}
          variant="ghost"
          size="icon"
          className="bg-transparent hover:bg-gray-700/50 text-gray-300 text-xs rounded-md h-7 w-7 sm:h-8 sm:w-8 md:hidden"
        >
          <History size={16} />
        </Button>
        
        <Button 
          onClick={onToggleHistory}
          variant="outline" 
          size="sm"
          className="bg-transparent hover:bg-gray-700/50 border border-gray-600/50 hover:border-gray-500 text-gray-300 text-xs rounded-md hidden md:inline-flex"
        >
          History
        </Button>
      </div>
    </div>
  );
};

export default BrowserBar;
