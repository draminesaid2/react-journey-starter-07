
import React from 'react';
import { Button } from '@/components/ui/button';
import { ScrollArea } from '@/components/ui/scroll-area';
import { type HistoryItem } from '../pages/Index';
import { Clock, Search, X } from 'lucide-react';

interface BrowserHistoryProps {
  history: HistoryItem[];
  onSelect: (item: HistoryItem) => void;
  onClose: () => void;
}

const BrowserHistory = ({ history, onSelect, onClose }: BrowserHistoryProps) => {
  const formatTime = (timestamp: number) => {
    return new Date(timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  };

  const formatDate = (timestamp: number) => {
    const now = new Date();
    const date = new Date(timestamp);
    
    if (date.toDateString() === now.toDateString()) {
      return 'Today';
    }
    
    if (new Date(now.setDate(now.getDate() - 1)).toDateString() === date.toDateString()) {
      return 'Yesterday';
    }
    
    return date.toLocaleDateString(undefined, { weekday: 'long', month: 'short', day: 'numeric' });
  };

  // Group history by date
  const groupedHistory = history.reduce((acc, item) => {
    const dateGroup = formatDate(item.timestamp);
    if (!acc[dateGroup]) {
      acc[dateGroup] = [];
    }
    acc[dateGroup].push(item);
    return acc;
  }, {} as Record<string, HistoryItem[]>);

  return (
    <div className="absolute inset-0 z-10 bg-gray-900/95 backdrop-blur-md p-4 border border-gray-700/50 rounded-md shadow-2xl animate-fade-in">
      <div className="flex justify-between items-center mb-6 border-b border-gray-700/50 pb-3">
        <div className="flex items-center gap-2">
          <Clock className="h-5 w-5 text-blue-400" />
          <h2 className="text-xl font-semibold text-white">History</h2>
        </div>
        <Button 
          onClick={onClose}
          variant="ghost"
          size="icon"
          className="rounded-full hover:bg-white/10"
        >
          <X className="h-5 w-5 text-gray-300" />
        </Button>
      </div>
      
      {history.length === 0 ? (
        <div className="flex flex-col items-center justify-center h-64 text-gray-400">
          <Search className="h-10 w-10 mb-3 text-gray-500" />
          <p>No browsing history yet</p>
        </div>
      ) : (
        <ScrollArea className="h-[calc(100vh-140px)]">
          {Object.entries(groupedHistory).map(([date, items]) => (
            <div key={date} className="mb-6">
              <h3 className="text-sm font-medium text-gray-400 mb-2 px-2">{date}</h3>
              <div className="space-y-1">
                {items.map((item, index) => (
                  <div 
                    key={index}
                    onClick={() => onSelect(item)}
                    className="flex items-center p-3 rounded-md hover:bg-white/10 cursor-pointer group transition-colors"
                  >
                    <div className="w-8 h-8 rounded-full bg-blue-500/20 flex items-center justify-center mr-3 text-blue-400">
                      {item.url.startsWith('Search of') ? 
                        <Search className="h-4 w-4" /> : 
                        item.url.charAt(8).toUpperCase()}
                    </div>
                    <div className="flex-1 min-w-0">
                      <p className="text-white font-medium truncate">
                        {item.title}
                      </p>
                      <p className="text-gray-400 text-sm truncate">
                        {item.url}
                      </p>
                    </div>
                    <span className="text-gray-500 text-xs ml-4 group-hover:text-blue-400 transition-colors">
                      {formatTime(item.timestamp)}
                    </span>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </ScrollArea>
      )}
    </div>
  );
};

export default BrowserHistory;
