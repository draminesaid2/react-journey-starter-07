
import React, { useState, useEffect } from 'react';
import BrowserBar from '../components/BrowserBar';
import BrowserContent from '../components/BrowserContent';
import BrowserHistory from '../components/BrowserHistory';
import { useToast } from '@/hooks/use-toast';

export type HistoryItem = {
  url: string;
  title: string;
  timestamp: number;
};

const Index = () => {
  const [url, setUrl] = useState<string>('Search of Iheb Chebbi');
  const [showHistory, setShowHistory] = useState<boolean>(false);
  const [history, setHistory] = useState<HistoryItem[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const { toast } = useToast();
  
  useEffect(() => {
    // Load history from localStorage when component mounts
    const savedHistory = localStorage.getItem('browserHistory');
    if (savedHistory) {
      setHistory(JSON.parse(savedHistory));
    }
  }, []);

  const handleNavigate = (newUrl: string) => {
    // Process the URL
    let processedUrl = newUrl;
    
    // Only add protocol if it's not a search query and doesn't already have a protocol
    if (!processedUrl.startsWith('http') && !processedUrl.startsWith('Search of')) {
      processedUrl = `https://${processedUrl}`;
    }
    
    setUrl(processedUrl);
    setIsLoading(true);
    
    // Add to history
    const newHistoryItem = {
      url: processedUrl,
      title: processedUrl, // We'll update this when page loads
      timestamp: Date.now()
    };
    
    const updatedHistory = [newHistoryItem, ...history].slice(0, 30); // Keep only last 30 items
    setHistory(updatedHistory);
    
    // Save to localStorage
    localStorage.setItem('browserHistory', JSON.stringify(updatedHistory));
  };

  const handleSearch = (query: string) => {
    // For search queries, use the Search of format
    if (!query.startsWith('Search of')) {
      const searchUrl = `Search of ${query}`;
      handleNavigate(searchUrl);
    } else {
      handleNavigate(query);
    }
  };

  const handlePageLoad = (title: string) => {
    setIsLoading(false);
    
    // Update history with page title
    if (history.length > 0) {
      const updatedHistory = [...history];
      updatedHistory[0].title = title;
      setHistory(updatedHistory);
      localStorage.setItem('browserHistory', JSON.stringify(updatedHistory));
    }
  };

  const handleHistoryClick = (historyItem: HistoryItem) => {
    setShowHistory(false);
    handleNavigate(historyItem.url);
  };

  const handleError = (error: string) => {
    setIsLoading(false);
    toast({
      title: "Navigation Error",
      description: error,
      variant: "destructive"
    });
  };

  return (
    <div className="flex flex-col h-screen bg-browser-DEFAULT overflow-hidden">
      {/* Keep the browser navigation bar at the top */}
      <BrowserBar 
        url={url}
        onNavigate={handleNavigate}
        onSearch={handleSearch}
        onToggleHistory={() => setShowHistory(!showHistory)}
        isLoading={isLoading}
      />
      
      <div className="relative flex-1 overflow-hidden">
        {showHistory && (
          <div className="absolute inset-0 z-10">
            <BrowserHistory
              history={history}
              onSelect={handleHistoryClick}
              onClose={() => setShowHistory(false)}
            />
          </div>
        )}
        
        <BrowserContent 
          url={url}
          onTitleChange={handlePageLoad}
          onError={handleError}
        />
      </div>
    </div>
  );
};

export default Index;
