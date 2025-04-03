import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import GestionResponsable from '../admin/GestionResponsable';
import GestionAgence from '../admin/GestionAgence';

import ErrorBoundary from '../../ErrorBoundary'; 
import { logout } from '../logout';
import '../supadmin/SupAdminPage.css';

const AdminPage = () => {
  const role = localStorage.getItem('userRole');
  const [activeTab, setActiveTab] = useState('responsable');

  return (
    <ErrorBoundary>
      <div className="container-fluid">
        <div className="row">
          {/* Sidebar */}
          <nav className="col-md-2 d-none d-md-block bg-light sidebar">
            <ul className="nav flex-column text-start">
              <li className="nav-item">
                <button className="nav-link btn btn-link" onClick={() => setActiveTab('responsable')}>Gestion des responsables</button>
              </li>
              <li className="nav-item">
                <button className="nav-link btn btn-link" onClick={() => setActiveTab('agence')}>Gestion des agences</button>
              </li>
             
            </ul>
          </nav>

          {/* Contenu principal */}
          <main className="col-md-8 mx-auto w-75 text-center p-4 bg-white shadow-sm rounded">
            {/* Barre du haut avec bouton Déconnexion */}
            <div className="d-flex justify-content-between align-items-center mb-3">

              
              {/* Bouton Déconnexion bien à droite */}
              <button onClick={logout} className="btn btn-danger ms-auto">Déconnexion</button>
            </div>

            {/* Affichage des composants en fonction de l'onglet actif */}
            <div className="card shadow-sm p-4 bg-white rounded">
              {activeTab === 'responsable' && <GestionResponsable />}
              {activeTab === 'agence' && <GestionAgence />}
              
            </div>
          </main>
        </div>
      </div>
    </ErrorBoundary>
  );
};

export default AdminPage;

