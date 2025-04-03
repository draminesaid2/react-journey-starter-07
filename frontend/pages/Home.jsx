import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './Home.css';  // Assurez-vous que le fichier CSS existe et est bien importé
import Ticket from '../assets/ticket.png';

const Home = () => {
  const navigate = useNavigate();

  useEffect(() => {
    setTimeout(() => {
      navigate('/login');
    }, 3000);  // Redirige après 3 secondes
  }, [navigate]);

  return (
    <div className="home-container">
      <div className="logo-container">
      <img src={Ticket} alt="Ticket" />

        <p className="app-name">Ticket</p>
      </div>
    </div>
  );
};

export default Home;


