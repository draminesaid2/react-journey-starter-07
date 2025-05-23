
import React from 'react';
import FolderWindow from '../FolderWindow';
import { Award } from 'lucide-react';
import { Progress } from '@/components/ui/progress';

interface Skill {
  name: string;
  percentage: number;
  category: string;
}

interface SkillsFolderProps {
  isOpen: boolean;
  onClose: () => void;
}

const SkillsFolder = ({ isOpen, onClose }: SkillsFolderProps) => {
  const skills: Skill[] = [
    { name: 'JavaScript', percentage: 90, category: 'Frontend' },
    { name: 'React/Next.js', percentage: 85, category: 'Frontend' },
    { name: 'Node.js', percentage: 80, category: 'Backend' },
    { name: 'Python', percentage: 75, category: 'Backend' },
    { name: 'TensorFlow/ML', percentage: 70, category: 'AI' },
    { name: 'Cloud Hosting', percentage: 85, category: 'DevOps' },
    { name: 'Git/Github', percentage: 90, category: 'DevOps' },
    { name: 'UI/UX Design', percentage: 80, category: 'Design' }
  ];

  // Group skills by category
  const groupedSkills = skills.reduce((acc, skill) => {
    if (!acc[skill.category]) {
      acc[skill.category] = [];
    }
    acc[skill.category].push(skill);
    return acc;
  }, {} as Record<string, Skill[]>);

  return (
    <FolderWindow
      isOpen={isOpen}
      onClose={onClose}
      title="Skills"
      icon={<Award size={18} />}
    >
      <div className="grid grid-cols-2 gap-8">
        {Object.entries(groupedSkills).map(([category, skills]) => (
          <div key={category} className="bg-gray-800/40 rounded-lg p-4 backdrop-blur-sm">
            <h3 className="text-lg font-medium text-white mb-4">{category}</h3>
            <div className="space-y-4">
              {skills.map((skill) => (
                <div key={skill.name}>
                  <div className="flex justify-between text-sm mb-1">
                    <span className="text-white">{skill.name}</span>
                    <span className="text-gray-400">{skill.percentage}%</span>
                  </div>
                  <Progress value={skill.percentage} className="h-2 bg-gray-700">
                    <div
                      className="h-full bg-blue-500 transition-all duration-300 ease-in-out rounded-full"
                      style={{ width: `${skill.percentage}%` }}
                    />
                  </Progress>
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
      
      <div className="mt-6 p-4 bg-blue-500/10 border border-blue-500/20 rounded-lg">
        <div className="grid grid-cols-4 gap-4 text-center">
          <div>
            <div className="text-2xl font-bold text-white">2000+</div>
            <div className="text-sm text-gray-400">Clean Code</div>
          </div>
          <div>
            <div className="text-2xl font-bold text-white">30+</div>
            <div className="text-sm text-gray-400">Projects</div>
          </div>
          <div>
            <div className="text-2xl font-bold text-white">2+</div>
            <div className="text-sm text-gray-400">Awards Won</div>
          </div>
          <div>
            <div className="text-2xl font-bold text-white">25+</div>
            <div className="text-sm text-gray-400">Happy Clients</div>
          </div>
        </div>
      </div>
    </FolderWindow>
  );
};

export default SkillsFolder;
