
import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import './index.css'

// Hide default cursor throughout the app
document.documentElement.style.cursor = 'none';

createRoot(document.getElementById("root")!).render(<App />);
