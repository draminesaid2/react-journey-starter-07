
import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import App from './App.tsx'
import Dashboard from './pages/Dashboard.tsx'
import DateFarcie from './pages/DateFarcie.tsx'
import './index.css'
import './i18n.ts' // Import i18n configuration
import { Toaster } from './components/ui/toaster.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path="/statistique" element={<Dashboard />} />
        <Route path="/technical-products" element={<App />} />
        <Route path="/dattes-farcies" element={<DateFarcie />} />
        <Route path="/products" element={<Navigate to="/" replace />} />
        <Route path="/products-all" element={<Navigate to="/" replace />} />
        <Route path="/about" element={<Navigate to="/" replace />} />
        <Route path="/contact" element={<Navigate to="/" replace />} />
        <Route path="/partners" element={<Navigate to="/" replace />} />
        <Route path="/resellers" element={<Navigate to="/" replace />} />
        <Route path="/certifications" element={<Navigate to="/" replace />} />
      </Routes>
      <Toaster />
    </BrowserRouter>
  </React.StrictMode>,
)
