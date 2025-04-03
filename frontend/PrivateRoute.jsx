import React from 'react';
import { Navigate } from 'react-router-dom';

const PrivateRoute = ({ element, ...rest }) => {
  const token = localStorage.getItem('token');
  const role = localStorage.getItem('userRole');

  // Si pas de token, redirige vers login
  if (!token) {
    return <Navigate to="/login" replace />;
  }

  // Si le rôle n'est pas admin, redirige vers la page d'accueil
  if (role !== 'admin') { // Tu peux ajuster cette condition pour chaque rôle
    return <Navigate to="/" replace />;
  }

  // Si tout est valide, rend l'élément de la route protégée
  return element;
};

export default PrivateRoute;

