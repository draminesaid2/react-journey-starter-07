
import React from 'react';
import { cn } from '@/lib/utils';

interface DesktopIconProps {
  icon: React.ReactNode;
  label: string;
  onClick?: () => void;
  className?: string;
}

const DesktopIcon = ({ icon, label, onClick, className }: DesktopIconProps) => {
  return (
    <div 
      className={cn(
        "flex flex-col items-center justify-center p-2 w-20 cursor-pointer rounded-lg transition-all text-center group",
        className
      )}
      onClick={onClick}
    >
      <div className="w-12 h-12 flex items-center justify-center mb-1.5 text-white drop-shadow-lg group-hover:scale-110 transition-transform backdrop-blur-sm rounded-lg bg-gradient-to-br from-white/10 to-transparent">
        {icon}
      </div>
      <span className="text-white text-xs font-medium text-center break-words w-full px-1 drop-shadow-[0_1px_2px_rgba(0,0,0,0.8)] bg-black/20 backdrop-blur-sm rounded-md py-0.5">
        {label}
      </span>
    </div>
  );
};

export default DesktopIcon;
