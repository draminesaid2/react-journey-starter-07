
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'

// Add style to hide default cursor when on the app
const style = document.createElement('style');
style.textContent = `
  body { 
    cursor: none !important; 
    overflow-x: hidden;
  }
`;
document.head.appendChild(style);

createRoot(document.getElementById("root")!).render(<App />);
