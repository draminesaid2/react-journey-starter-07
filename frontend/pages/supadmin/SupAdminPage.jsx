import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import GestionAdmin from '../supadmin/GestionAdmin.jsx';
import GestionEtablissement from '../supadmin/GestionEtablissement';
import ErrorBoundary from '../../ErrorBoundary';
import { logout } from '../logout';
import '../supadmin/SupAdminPage.css';

const SupAdminPage = () => {
  const [activeTab, setActiveTab] = useState('admins');

  return (
    <ErrorBoundary>
      <div className="container-fluid">
        <div className="row">
          {/* Sidebar */}
          <nav className="col-md-3 d-none d-md-block bg-light sidebar">
            <ul className="nav flex-column text-start">
              <li className="nav-item">
                <button className="nav-link btn btn-link" onClick={() => setActiveTab('admins')}>
                  Gestion des Admins
                </button>
              </li>
              <li className="nav-item">
                <button className="nav-link btn btn-link" onClick={() => setActiveTab('etablissements')}>
                  Gestion des Établissements
                </button>
              </li>
            </ul>
          </nav>

          {/* Contenu principal avec largeur réduite */}
          <main className="col-md-8 mx-auto w-75 text-center p-4 bg-white shadow-sm rounded">
            {/* Barre du haut avec bouton Déconnexion à droite */}
            <div className="d-flex justify-content-between align-items-center mb-3">
              
              {/* Bouton Déconnexion bien aligné à droite */}
              <button onClick={logout} className="btn btn-danger ms-auto">Déconnexion</button>
            </div>

            {/* Contenu dynamique */}
            <div className="card shadow-sm p-4 bg-white rounded">
              {activeTab === 'admins' && <GestionAdmin />}
              {activeTab === 'etablissements' && <GestionEtablissement />}
            </div>
          </main>
        </div>
      </div>
    </ErrorBoundary>
  );
};

export default SupAdminPage;








