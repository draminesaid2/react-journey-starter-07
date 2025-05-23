
import React from 'react';
import FolderWindow from '../FolderWindow';
import { Music, Code, Book, Plane, Camera, Gamepad, Bike } from 'lucide-react';

interface Hobby {
  name: string;
  icon: React.ReactNode;
  description: string;
}

interface HobbiesFolderProps {
  isOpen: boolean;
  onClose: () => void;
}

const HobbiesFolder = ({ isOpen, onClose }: HobbiesFolderProps) => {
  const hobbies: Hobby[] = [
    {
      name: 'Music Production',
      icon: <Music size={24} />,
      description: 'Creating electronic music and experimenting with different genres in my home studio.'
    },
    {
      name: 'Coding Side Projects',
      icon: <Code size={24} />,
      description: 'Building small utilities and apps to solve personal problems or learn new technologies.'
    },
    {
      name: 'Reading',
      icon: <Book size={24} />,
      description: 'Science fiction, technical books, and biographies of tech innovators.'
    },
    {
      name: 'Traveling',
      icon: <Plane size={24} />,
      description: 'Exploring new cultures, foods, and meeting people from around the world.'
    },
    {
      name: 'Photography',
      icon: <Camera size={24} />,
      description: 'Capturing urban landscapes and natural scenery during travels.'
    },
    {
      name: 'Gaming',
      icon: <Gamepad size={24} />,
      description: 'Playing strategy games and RPGs in my free time.'
    },
    {
      name: 'Cycling',
      icon: <Bike size={24} />,
      description: 'Weekend rides exploring local trails and city routes.'
    }
  ];

  return (
    <FolderWindow
      isOpen={isOpen}
      onClose={onClose}
      title="Hobbies & Interests"
      icon={<Music size={18} />}
    >
      <div className="grid grid-cols-2 gap-4">
        {hobbies.map((hobby, index) => (
          <div 
            key={index}
            className="bg-gray-800/40 rounded-lg p-4 hover:bg-gray-700/40 transition-colors border border-gray-700/30 hover:border-blue-500/30"
          >
            <div className="flex items-center gap-3 mb-2">
              <div className="w-10 h-10 rounded-full bg-blue-500/20 flex items-center justify-center text-blue-300">
                {hobby.icon}
              </div>
              <h3 className="text-lg font-medium text-white">{hobby.name}</h3>
            </div>
            <p className="text-gray-300 text-sm">{hobby.description}</p>
          </div>
        ))}
      </div>
      
      <div className="mt-6 p-4 bg-gray-800/40 rounded-lg text-center">
        <h3 className="text-lg font-medium text-white mb-2">Fun Fact</h3>
        <p className="text-gray-300 italic">
          "When I'm not coding, you'll probably find me hiking with my camera or experimenting with new recipes in the kitchen."
        </p>
      </div>
    </FolderWindow>
  );
};

export default HobbiesFolder;
