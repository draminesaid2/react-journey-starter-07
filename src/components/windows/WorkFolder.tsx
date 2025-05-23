
import React from 'react';
import FolderWindow from '../FolderWindow';
import { Briefcase } from 'lucide-react';

interface WorkExperience {
  position: string;
  company: string;
  period: string;
  description: string;
  technologies: string[];
}

interface WorkFolderProps {
  isOpen: boolean;
  onClose: () => void;
}

const WorkFolder = ({ isOpen, onClose }: WorkFolderProps) => {
  const experiences: WorkExperience[] = [
    {
      position: "Senior Full Stack Developer",
      company: "Tech Innovations Ltd",
      period: "2020 - Present",
      description: "Leading development of modern web applications, designing system architecture, and mentoring junior developers.",
      technologies: ["React", "Node.js", "AWS", "MongoDB", "GraphQL"]
    },
    {
      position: "Frontend Developer",
      company: "Digital Solutions Agency",
      period: "2018 - 2020",
      description: "Built responsive web applications and implemented complex UI components for various clients.",
      technologies: ["JavaScript", "React", "Next.js", "Tailwind CSS"]
    },
    {
      position: "Web Developer Intern",
      company: "Startup Incubator",
      period: "2017 - 2018",
      description: "Collaborated with designers and senior developers to implement UI designs and improve website performance.",
      technologies: ["HTML", "CSS", "JavaScript", "PHP"]
    }
  ];

  return (
    <FolderWindow
      isOpen={isOpen}
      onClose={onClose}
      title="Work Experience"
      icon={<Briefcase size={18} />}
    >
      <div className="space-y-6">
        {experiences.map((exp, index) => (
          <div 
            key={index}
            className="bg-gray-800/40 rounded-lg p-5 backdrop-blur-sm border border-gray-700/30 hover:border-blue-500/30 transition-colors"
          >
            <div className="flex justify-between items-start mb-1">
              <h3 className="text-lg font-semibold text-white">{exp.position}</h3>
              <span className="text-sm px-3 py-1 bg-blue-500/20 text-blue-300 rounded-full">
                {exp.period}
              </span>
            </div>
            <div className="text-blue-400 mb-3">{exp.company}</div>
            <p className="text-gray-300 text-sm mb-3">{exp.description}</p>
            
            <div className="flex flex-wrap gap-2">
              {exp.technologies.map((tech, i) => (
                <span 
                  key={i}
                  className="px-2 py-1 text-xs rounded-full bg-gray-700/70 text-gray-300"
                >
                  {tech}
                </span>
              ))}
            </div>
          </div>
        ))}
        
        <div className="p-4 bg-blue-500/10 border border-blue-500/20 rounded-lg text-center">
          <h3 className="text-lg font-medium text-white mb-2">Professional Summary</h3>
          <p className="text-gray-300">
            "Full Stack Developer with over 5 years of experience specializing in React, Node.js, and modern JavaScript. 
            Passionate about building scalable applications and integrating AI solutions."
          </p>
        </div>
      </div>
    </FolderWindow>
  );
};

export default WorkFolder;
