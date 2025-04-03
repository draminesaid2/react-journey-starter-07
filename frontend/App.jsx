import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import ErrorBoundary from './ErrorBoundary.jsx';
import SupAdminPage from './pages/supadmin/SupAdminPage';
import AdminPage from './pages/admin/AdminPage';
import ResponsablePage from './pages/responsable/ResponsablePage';
import AgentPage from './pages/agent/AgentPage.jsx';
import ProtectedRoute from './components/ProtectedRoute.jsx';
import GestionAdmin from './pages/supadmin/GestionAdmin';
import GestionEtablissement from './pages/supadmin/GestionEtablissement';
import EtablissementDetails from "./pages/supadmin/EtablissementDetails";
import AdminDetails from "./pages/supadmin/AdminDetails";
import AgentDetails from "./pages/responsable/AgentDetails";
import ResponsableDetails from "./pages/admin/ResponsableDetails";
import AgenceDetails from "./pages/admin/AgenceDetails";

const App = () => {
  return (
    <ErrorBoundary> {/* Gère les erreurs globalement */}
      <Router>
        <Routes>
          {/* Page publique */}
          <Route path="/login" element={<Login />} />

          {/* Page d'accueil non protégée */}
          <Route path="/" element={<Home />} />

          {/* Routes protégées */}
          <Route element={<ProtectedRoute />}>
            <Route path="/SupAdminPage" element={<SupAdminPage />} />
            <Route path="/AdminPage" element={<AdminPage />} />
            <Route path="/ResponsablePage" element={<ResponsablePage />} />
            <Route path="/AgentPage" element={<AgentPage />} />
            <Route path="/SupAdminPage" element={<GestionAdmin />} />
            <Route path="/SupAdminPage" element={<GestionEtablissement/>} />
            <Route path="/etablissement/:id" element={<EtablissementDetails />} />
            <Route path="/admin/:id" element={<AdminDetails />} />
            <Route path="/agent/:id" element={<AgentDetails />} />
            <Route path="/responsable/:id" element={<ResponsableDetails />} />
            <Route path="/agence/:id" element={<AgenceDetails />} />
           
          </Route>

        </Routes>
      </Router>
    </ErrorBoundary>
  );
};

export default App;




