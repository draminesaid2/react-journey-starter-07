import React from 'react';
import ErrorBoundary from './components/ErrorBoundary'; // Le chemin vers votre fichier ErrorBoundary
import App from './App'; // Votre composant principal

const Root = () => {
  return (
    <ErrorBoundary>
      <App />
    </ErrorBoundary>
  );
};

export default Root;
