import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './Login.css';

const Login = () => {
  const [email_user, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post('http://127.0.0.1:8000/api/login', {
        email_user,  // Vérifie si ton API attend "email" ou "email_user"
        password,
      });

      console.log('Réponse API:', response.data);
      localStorage.setItem('token', response.data.token);
      localStorage.setItem('userRole', response.data.user.role);
      console.log("Token reçu :", response.data.token);
      console.log("Rôle reçu :", response.data.user.role);

      const userRole = response.data.user.role;
      console.log("Rôle reçu :", userRole);

      // Vérification stricte pour éviter les erreurs
      switch (userRole) {
        case 'supadmin':
          navigate('/SupAdminPage');
          break;
        case 'admin':
          navigate('/AdminPage');
          break;
        case 'responsable':
          navigate('/ResponsablePage');
          break;
        case 'agent':
          navigate('/AgentPage');
          break;
        default:
          setError('Rôle inconnu, accès refusé');
          break;
      }
    } catch (err) {
      if (err.response && err.response.data.message) {
        setError(err.response.data.message);
      } else {
        setError('Échec de la connexion, vérifie tes identifiants');
      }
    }
  };

  return (
    <div className="login-container">
      <h2 className="login-title">Se connecter</h2>

      {error && <p className="error-message">{error}</p>}

      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label>Email</label>
          <input
            type="email"
            value={email_user}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="input-field"
          />
        </div>

        <div className="input-group">
          <label>Mot de passe</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="input-field"
          />
        </div>

        <button type="submit" className="login-btn">Se connecter</button>
      </form>
    </div>
  );
};

export default Login;



