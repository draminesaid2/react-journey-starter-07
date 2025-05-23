
import React from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { ScrollArea } from '@/components/ui/scroll-area';
import { X, Minus, Square, ChevronLeft, ChevronRight, Search, RefreshCcw } from 'lucide-react';

interface FolderWindowProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  icon?: React.ReactNode;
  children: React.ReactNode;
}

const FolderWindow = ({ isOpen, onClose, title, icon, children }: FolderWindowProps) => {
  return (
    <Dialog open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <DialogContent className="sm:max-w-[700px] p-0 gap-0 shadow-2xl border border-gray-600/30 bg-gray-900/95 backdrop-blur-md rounded-lg overflow-hidden">
        {/* Window title bar */}
        <div className="bg-gray-800/70 flex items-center justify-between px-3 py-2 select-none drag-handle">
          <div className="flex items-center gap-2">
            {icon && <div className="text-white">{icon}</div>}
            <DialogTitle className="text-sm text-white font-medium">{title}</DialogTitle>
          </div>
          <div className="flex items-center">
            <button className="w-8 h-6 flex items-center justify-center rounded hover:bg-white/10 transition-colors">
              <Minus size={16} className="text-white" />
            </button>
            <button className="w-8 h-6 flex items-center justify-center rounded hover:bg-white/10 transition-colors">
              <Square size={14} className="text-white" />
            </button>
            <button 
              onClick={onClose}
              className="w-8 h-6 flex items-center justify-center rounded hover:bg-red-500/80 transition-colors"
            >
              <X size={16} className="text-white" />
            </button>
          </div>
        </div>
        
        {/* Folder navigation bar */}
        <div className="bg-gray-800/50 border-b border-gray-700/50 px-3 py-2 flex items-center gap-2">
          <button className="w-6 h-6 flex items-center justify-center rounded-full bg-gray-700/50 hover:bg-gray-700 transition-colors">
            <ChevronLeft size={16} className="text-white" />
          </button>
          <button className="w-6 h-6 flex items-center justify-center rounded-full bg-gray-700/50 hover:bg-gray-700 transition-colors">
            <ChevronRight size={16} className="text-white" />
          </button>
          <div className="flex-1 bg-gray-700/30 rounded-md px-3 py-1 text-sm text-white flex items-center gap-2">
            {icon && <div>{icon}</div>}
            <span>{title}</span>
          </div>
          <div className="relative">
            <Search size={14} className="text-gray-400 absolute left-2 top-1/2 transform -translate-y-1/2" />
            <input 
              type="text" 
              placeholder="Search" 
              className="bg-gray-700/30 rounded-md pl-8 pr-2 py-1 text-sm text-white w-32 focus:outline-none focus:ring-1 focus:ring-blue-500"
            />
          </div>
          <button className="w-6 h-6 flex items-center justify-center rounded-full bg-gray-700/50 hover:bg-gray-700 transition-colors">
            <RefreshCcw size={14} className="text-white" />
          </button>
        </div>
        
        {/* Folder content */}
        <ScrollArea className="p-4 h-[400px]">
          {children}
        </ScrollArea>
      </DialogContent>
    </Dialog>
  );
};

export default FolderWindow;
