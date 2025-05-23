
import React, { useState, useEffect } from 'react';
import { Terminal, X } from 'lucide-react';

interface FakeTerminalProps {
  onClose: () => void;
  autoCloseAfter?: number | null; // in milliseconds, null for no auto-close
  isHacking?: boolean;
  terminalTitle?: string;
  finalMessage?: string;
}

const FakeTerminal = ({ 
  onClose, 
  autoCloseAfter = null, 
  isHacking = false,
  terminalTitle = "Terminal",
  finalMessage = ""
}: FakeTerminalProps) => {
  const [visible, setVisible] = useState<boolean>(true);
  const [text, setText] = useState<string>('');
  const [textIndex, setTextIndex] = useState<number>(0);
  const [ipAddress, setIpAddress] = useState<string>('192.168.1.1');
  const [progress, setProgress] = useState<number>(0);
  
  // More impressive hacking texts with technical jargon
  const standardTexts = [
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

  // Different hacking texts for each terminal in the sequence
  const hackingTexts = [
    [
      "> Initiating system breach protocol...",
      "> Scanning network vulnerabilities...",
      "> Identified weak points in firewall...",
      "> Executing buffer overflow attack...",
      "> Creating memory dump...",
      "> Bypassing authentication layer 1/3..."
    ],
    [
      "> Loading kernel modules...",
      "> Searching for open ports...",
      `> Discovered services: SSH(22), HTTP(80), HTTPS(443)`,
      "> Attempting SQL injection on admin portal...",
      "> Brute-forcing admin credentials...",
      "> Gained access to user authentication database..."
    ],
    [
      "> Decompiling system binaries...",
      "> Analyzing network traffic patterns...",
      "> Injecting malicious payload...",
      "> Creating backdoor access...",
      "> Elevating user privileges...",
      "> Achieved root access to main server..."
    ],
    [
      "> Running final security checks...",
      "> Cleaning up digital footprints...",
      "> Removing access logs...",
      "> Finalizing system breach...",
      "> Operation completed successfully..."
    ]
  ];
  
  // Select text array based on props
  const textToShow = isHacking ? 
    (hackingTexts[Math.min(activeTerminalIndex(), 3)]) : 
    (finalMessage ? [`> ${finalMessage}`] : standardTexts);

  function activeTerminalIndex() {
    if (terminalTitle?.includes("1/4")) return 0;
    if (terminalTitle?.includes("2/4")) return 1;
    if (terminalTitle?.includes("3/4")) return 2;
    return 3;
  }

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
    if (textIndex < textToShow.length) {
      const typingSpeed = Math.random() * 100 + (isHacking ? 100 : 250); // Faster typing for hacking terminals
      const typingTimer = setTimeout(() => {
        setText((prev) => prev + textToShow[textIndex] + '\n');
        setTextIndex(textIndex + 1);
        setProgress((textIndex + 1) / textToShow.length * 100);
      }, typingSpeed);
      
      return () => clearTimeout(typingTimer);
    }
  }, [textIndex, textToShow.length, isHacking]);

  // Auto-close terminal after specified time
  useEffect(() => {
    if (autoCloseAfter && textIndex >= textToShow.length) {
      const timer = setTimeout(() => {
        setVisible(false);
        setTimeout(onClose, 500); // Give time for animation to complete
      }, autoCloseAfter);
      
      return () => clearTimeout(timer);
    }
  }, [autoCloseAfter, onClose, textIndex, textToShow.length]);

  const handleClose = () => {
    setVisible(false);
    setTimeout(onClose, 300); // Give time for animation to complete
  };

  return (
    <div className={`fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-sm transition-opacity ${visible ? 'opacity-100' : 'opacity-0'}`}>
      <div className={`w-full max-w-3xl h-[80vh] md:h-[60vh] bg-black border border-green-500/50 rounded-md shadow-lg shadow-green-500/20 transform transition-transform ${visible ? 'scale-100' : 'scale-95'} relative`}>
        <div className="flex items-center justify-between p-2 bg-gray-900/80 border-b border-green-500/30">
          <div className="flex space-x-2">
            <div className="w-3 h-3 rounded-full bg-red-500"></div>
            <div className="w-3 h-3 rounded-full bg-yellow-500"></div>
            <div className="w-3 h-3 rounded-full bg-green-500"></div>
          </div>
          <div className="text-green-300 text-sm font-mono flex items-center gap-2">
            <Terminal size={14} className="text-green-400" /> 
            <span>{terminalTitle}</span>
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
            {Array.from({ length: 25 }).map((_, index) => (
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
