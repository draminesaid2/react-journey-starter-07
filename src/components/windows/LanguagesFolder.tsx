
import React from 'react';
import FolderWindow from '../FolderWindow';
import { Languages } from 'lucide-react';
import { Progress } from '../ui/progress';

interface Language {
  name: string;
  level: string;
  percentage: number;
}

interface LanguagesFolderProps {
  isOpen: boolean;
  onClose: () => void;
}

const LanguagesFolder = ({ isOpen, onClose }: LanguagesFolderProps) => {
  const languages: Language[] = [
    { name: 'English', level: 'Professional', percentage: 90 },
    { name: 'Arabic', level: 'Native', percentage: 100 },
    { name: 'French', level: 'Fluent', percentage: 85 },
    { name: 'German', level: 'Basic', percentage: 30 },
  ];

  return (
    <FolderWindow
      isOpen={isOpen}
      onClose={onClose}
      title="Languages"
      icon={<Languages size={18} />}
    >
      <div className="grid gap-4 md:gap-6">
        {languages.map((lang, index) => (
          <div 
            key={index} 
            className="bg-gray-800/40 backdrop-blur-md rounded-lg p-3 md:p-5 flex flex-col md:flex-row md:items-center gap-3 md:gap-4 hover:bg-gray-800/60 transition-colors"
          >
            <div className="w-12 h-12 rounded-full bg-blue-500/20 flex items-center justify-center text-blue-300 font-bold text-lg shrink-0 mx-auto md:mx-0">
              {lang.name.substring(0, 2)}
            </div>
            
            <div className="flex-1">
              <div className="flex flex-col md:flex-row md:justify-between mb-2 text-center md:text-left">
                <h3 className="text-white font-medium">{lang.name}</h3>
                <span className="text-blue-400 text-sm md:text-base">{lang.level}</span>
              </div>
              
              <div className="w-full">
                <Progress value={lang.percentage} className="h-3 bg-gray-700/50" />
                <p className="text-right text-xs text-gray-400 mt-1">{lang.percentage}%</p>
              </div>
            </div>
          </div>
        ))}
        
        <div className="bg-gray-800/40 rounded-lg p-3 md:p-4 mt-2 md:mt-4 border-l-4 border-blue-500 backdrop-blur-md">
          <h3 className="text-base md:text-lg font-medium text-white mb-2">Language Certifications</h3>
          <ul className="space-y-2 text-gray-300 text-sm md:text-base">
            <li className="flex items-center gap-2">
              <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
              <span>TOEFL - English Proficiency (Score: 110/120)</span>
            </li>
            <li className="flex items-center gap-2">
              <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
              <span>DELF B2 - French Proficiency</span>
            </li>
          </ul>
        </div>
      </div>
    </FolderWindow>
  );
};

export default LanguagesFolder;
