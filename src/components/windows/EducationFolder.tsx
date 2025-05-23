
import React from 'react';
import FolderWindow from '../FolderWindow';
import { School } from 'lucide-react';

interface EducationItem {
  degree: string;
  institution: string;
  year: string;
  description: string;
}

interface EducationFolderProps {
  isOpen: boolean;
  onClose: () => void;
}

const EducationFolder = ({ isOpen, onClose }: EducationFolderProps) => {
  const educationItems: EducationItem[] = [
    {
      degree: "Master's Degree in Computer Science",
      institution: "Higher Institute of Computer Science",
      year: "2018-2020",
      description: "Specialized in Web development and AI implementation in web applications."
    },
    {
      degree: "Bachelor's Degree in Computer Science",
      institution: "Higher Institute of Technological Studies",
      year: "2015-2018",
      description: "Focused on software engineering and web development fundamentals."
    },
    {
      degree: "Web Development Certification",
      institution: "FreeCodeCamp",
      year: "2017",
      description: "Completed the full stack development certification focusing on modern JavaScript frameworks."
    }
  ];

  return (
    <FolderWindow
      isOpen={isOpen}
      onClose={onClose}
      title="Education"
      icon={<School size={18} />}
    >
      <div className="space-y-6">
        {educationItems.map((item, index) => (
          <div 
            key={index}
            className="bg-gray-800/40 rounded-lg p-5 backdrop-blur-sm border border-gray-700/30 hover:border-blue-500/30 transition-colors"
          >
            <div className="flex justify-between items-start mb-2">
              <h3 className="text-lg font-semibold text-white">{item.degree}</h3>
              <span className="text-sm px-3 py-1 bg-blue-500/20 text-blue-300 rounded-full">
                {item.year}
              </span>
            </div>
            <div className="text-blue-400 mb-2">{item.institution}</div>
            <p className="text-gray-300 text-sm">{item.description}</p>
          </div>
        ))}
        
        <div className="p-4 bg-gray-800/40 rounded-lg mt-4">
          <h3 className="text-lg font-medium text-white mb-3">Certifications</h3>
          <ul className="space-y-2">
            <li className="flex items-center gap-2">
              <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
              <span className="text-gray-300">AWS Certified Solutions Architect</span>
            </li>
            <li className="flex items-center gap-2">
              <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
              <span className="text-gray-300">Google Cloud Professional Developer</span>
            </li>
            <li className="flex items-center gap-2">
              <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
              <span className="text-gray-300">Microsoft Certified: Azure Developer Associate</span>
            </li>
          </ul>
        </div>
      </div>
    </FolderWindow>
  );
};

export default EducationFolder;
