import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import GestionAgent from '../responsable/GestionAgent';
import ErrorBoundary from '../../ErrorBoundary'; 
import { logout } from '../logout';
import '../supadmin/SupAdminPage.css';

const ResponsablePage = () => {
  const [activeTab, setActiveTab] = useState('agent');

  return (
    <ErrorBoundary>
      <div className="container-fluid">
        <div className="row">
          {/* Sidebar */}
          <nav className="col-md-3 d-none d-md-block bg-light sidebar">
            <ul className="nav flex-column text-start">
              <li className="nav-item">
                <button className="nav-link btn btn-link" onClick={() => setActiveTab('agent')}>
                  Gestion des agents
                </button>
              </li>
            </ul>
          </nav>

          {/* Contenu principal */}
          <main className="col-md-8 mx-auto w-75 text-center p-4 bg-white shadow-sm rounded">
            {/* Barre du haut avec bouton Déconnexion à droite */}
            <div className="d-flex justify-content-between align-items-center mb-3">
              
              {/* Bouton Déconnexion bien aligné à droite */}
              <button onClick={logout} className="btn btn-danger ms-auto">Déconnexion</button>
            </div>

            {/* Contenu dynamique */}
            <div className="card shadow-sm p-4 bg-white rounded">
              {activeTab === 'agent' && <GestionAgent />}
            </div>
          </main>
        </div>
      </div>
    </ErrorBoundary>
  );
};

export default ResponsablePage;


