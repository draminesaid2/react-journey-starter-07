
import React, { useState, useEffect } from 'react';
import { Terminal, X } from 'lucide-react';

interface FakeTerminalProps {
  onClose: () => void;
  autoCloseAfter?: number; // in milliseconds
}

const FakeTerminal = ({ onClose, autoCloseAfter }: FakeTerminalProps) => {
  const [visible, setVisible] = useState<boolean>(true);
  const [text, setText] = useState<string>('');
  const [textIndex, setTextIndex] = useState<number>(0);
  const [ipAddress, setIpAddress] = useState<string>('192.168.1.1');
  const [progress, setProgress] = useState<number>(0);
  
  // More impressive hacking texts with technical jargon
  const hackingTexts = [
    "> Initializing secure shell protocol...",
    "> Establishing SSH connection to server: ssh://root@45.33.32.156:22",
    "> Bypassing firewall protocols... [SUCCESS]",
    "> Running nmap scan on target network...",
    `> IP detected: ${ipAddress} | Port 443 OPEN`,
    "> Attempting RSA-4096 key decryption...",
    "> Accessing mainframe through encrypted tunnel...",
    "> Deploying reverse proxy to mask connection...",
    "> Breaching system security layers [|||||||--] 78%",
    "> Establishing secure connection to portfolio database...",
    "> Welcome to Iheb Chebbi's digital workspace",
    "> System access granted. Terminal ready..."
  ];

  // Generate random IP address
  useEffect(() => {
    const generateIP = () => {
      const octet = () => Math.floor(Math.random() * 255);
      return `${octet()}.${octet()}.${octet()}.${octet()}`;
    };
    setIpAddress(generateIP());
  }, []);

  // Text typing effect with variable speed
  useEffect(() => {
    if (textIndex < hackingTexts.length) {
      const typingSpeed = Math.random() * 150 + 250; // Variable typing speed for more realism
      const typingTimer = setTimeout(() => {
        setText((prev) => prev + hackingTexts[textIndex] + '\n');
        setTextIndex(textIndex + 1);
        setProgress((textIndex + 1) / hackingTexts.length * 100);
      }, typingSpeed);
      
      return () => clearTimeout(typingTimer);
    }
  }, [textIndex, hackingTexts.length]);

  // Auto-close terminal after specified time
  useEffect(() => {
    if (autoCloseAfter && textIndex >= hackingTexts.length) {
      const timer = setTimeout(() => {
        setVisible(false);
        setTimeout(onClose, 500); // Give time for animation to complete
      }, autoCloseAfter);
      
      return () => clearTimeout(timer);
    }
  }, [autoCloseAfter, onClose, textIndex, hackingTexts.length]);

  const handleClose = () => {
    setVisible(false);
    setTimeout(onClose, 300); // Give time for animation to complete
  };

  return (
    <div className={`fixed inset-0 z-50 flex items-center justify-center bg-black/90 backdrop-blur-sm transition-opacity ${visible ? 'opacity-100' : 'opacity-0'}`}>
      <div className={`w-full max-w-3xl h-[80vh] md:h-[60vh] bg-black border border-green-500/50 rounded-md shadow-lg shadow-green-500/20 transform transition-transform ${visible ? 'scale-100' : 'scale-95'} relative`}>
        <div className="flex items-center justify-between p-2 bg-gray-900/80 border-b border-green-500/30">
          <div className="flex space-x-2">
            <div className="w-3 h-3 rounded-full bg-red-500"></div>
            <div className="w-3 h-3 rounded-full bg-yellow-500"></div>
            <div className="w-3 h-3 rounded-full bg-green-500"></div>
          </div>
          <div className="text-green-300 text-sm font-mono flex items-center gap-2">
            <Terminal size={14} className="text-green-400" /> 
            <span>Terminal</span>
            <span className="px-1.5 py-0.5 text-xs bg-green-900/40 rounded-md">root@system</span>
          </div>
          <button 
            onClick={handleClose}
            className="text-gray-400 hover:text-white transition-colors"
          >
            <X size={18} />
          </button>
        </div>
        <div className="p-4 h-[calc(100%-2.5rem)] overflow-auto font-mono text-green-500 text-sm">
          <pre className="whitespace-pre-line">{text}</pre>
          <div className="inline-block w-2 h-4 bg-green-500 ml-1 animate-pulse"></div>
          
          {/* Matrix-like background effect */}
          <div className="absolute inset-0 overflow-hidden pointer-events-none opacity-10 z-[-1]">
            {Array.from({ length: 15 }).map((_, index) => (
              <div 
                key={index}
                className="absolute text-xs text-green-500 animate-fade-in"
                style={{
                  left: `${Math.random() * 100}%`,
                  top: `${Math.random() * 100}%`,
                  animationDuration: `${Math.random() * 3 + 1}s`,
                  animationDelay: `${Math.random() * 2}s`,
                  opacity: Math.random() * 0.5 + 0.5
                }}
              >
                {Math.random() > 0.5 ? '1' : '0'}
              </div>
            ))}
          </div>
        </div>
        
        {/* Progress bar at the bottom */}
        <div className="absolute bottom-0 left-0 w-full h-0.5 bg-green-900/50">
          <div 
            className="h-full bg-green-500/70 transition-all duration-300 backdrop-blur-sm"
            style={{ width: `${progress}%` }}
          ></div>
        </div>
      </div>
    </div>
  );
};

export default FakeTerminal;
