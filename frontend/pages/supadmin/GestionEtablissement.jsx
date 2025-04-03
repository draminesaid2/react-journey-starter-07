// GestionEtablissement.jsx
import React, { useEffect, useState ,useCallback} from "react";
import axios from "axios";
import { Link, useNavigate } from "react-router-dom";

const GestionEtablissement = () => {
  const [etablissements, setEtablissements] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');
  const navigate = useNavigate();
  const [newEtablissement, setNewEtablissement] = useState({ nom_etabl: '', adresse_etabl: '' });
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    fetchEtablissements();
  }, []);

  const fetchEtablissements = async () => {
    try {
      const token = localStorage.getItem("token");
      const config = { headers: { Authorization: `Bearer ${token}` } };
      const response = await axios.get(
        `http://localhost:8000/api/supadmin/etablissement`,
        config
      );
      setEtablissements(response.data);
    } catch (error) {
      console.error("Erreur de récupération des établissements:", error);
    }
  };


  const addEtablissement = async () => {
    try {
      if (!newEtablissement.nom_etabl || !newEtablissement.adresse_etabl) {
        console.error('Les champs "nom_etabl" et "adresse_etabl" sont requis.');
        return;
      }

      const token = localStorage.getItem('token');
      if (!token) {
        console.error('Token non trouvé');
        return;
      }

      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };

      await axios.post(`http://localhost:8000/api/supadmin/etablissement`, newEtablissement, config);
      fetchEtablissements();
      setNewEtablissement({ nom_etabl: '', adresse_etabl: '' });
      setShowForm(false);
    } catch (error) {
      console.error('Erreur lors de l’ajout d’un établissement:', error.response ? error.response.data : error.message);
    }
  };

 


  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login"); // Redirection vers la page de connexion
  };




   const searchEtablissement = useCallback(
      async (query) => { 
        setSearchQuery(query); 
      
        if (!query.trim()) {
          fetchEtablissements(); 
          return;
        }
      
        try {
          const token = localStorage.getItem('token');
          const config = {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          };
      
          const response = await axios.get(`http://localhost:8000/api/supadmin/etablissement/search/${query}`, {
            params: { query },
            ...config,
          });
      
          setEtablissements(response.data); // Mettre à jour la liste des etablissements selon la recherche
        } catch (error) {
          console.error("Erreur lors de la recherche:", error.response ? error.response.data : error.message);
        }
      },
      [] 
    );
  
    // Effet avec délai de 500ms avant de lancer la recherche
    useEffect(() => {
      const timer = setTimeout(() => {
        searchEtablissement(searchQuery); 
      }, 500); 
      
      return () => clearTimeout(timer); 
    }, [searchQuery]); 
  
    // Fonction de gestion du changement dans le champ de recherche
  const handleSearchChange = (event) => {
    setSearchQuery(event.target.value); 
    
  };

  const handleChange = (event) => {
    const { name, value } = event.target;
    setNewEtablissement((prevState) => ({ ...prevState, [name]: value }));
  };

  return (
    <div>
      <h2>Gestion des Établissements</h2>
    
      <input
        type="text"
        placeholder="Nom ou adresse"
        value={searchQuery}
        onChange={handleSearchChange} // Recherche dynamique à chaque modification
        
      />
      <button className="btn btn-secondary" onClick={searchEtablissement}>Rechercher</button>
      
      <table className="table mt-4">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nom</th>
            <th>Adresse</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {etablissements
            
            .map((etablissement) => (
              <tr key={etablissement.id_etabl}>
                <td>{etablissement.id_etabl}</td>
                <td>{etablissement.nom_etabl}</td>
                <td>{etablissement.adresse_etabl}</td>
                <td>
                  <Link to={`/etablissement/${etablissement.id_etabl}`} className="btn btn-info">
                    Voir Détails
                  </Link>
                </td>
              </tr>
            ))}
        </tbody>
      </table>


      <button className="btn btn-primary mb-3" onClick={() => setShowForm(!showForm)}>
        {showForm ? 'Fermer' : 'Ajouter Établissement'}
      </button>
      {showForm && (
        <div className="card p-3 mb-4">
          <h3>Ajouter un Établissement</h3>
          <div className="form-group">
            <label>Nom de l'Établissement</label>
            <input
              type="text"
              className="form-control"
              name="nom_etabl"
              value={newEtablissement.nom_etabl}
              onChange={handleChange}
              placeholder="Nom de l'établissement"
            />
          </div>
          <div className="form-group">
            <label>Adresse de l'Établissement</label>
            <input
              type="text"
              className="form-control"
              name="adresse_etabl"
              value={newEtablissement.adresse_etabl}
              onChange={handleChange}
              placeholder="Adresse de l'établissement"
            />
          </div>
          <button className="btn btn-success mt-2" onClick={addEtablissement}>Ajouter</button>
        </div>
      )}

     
    </div>
  );
};

export default GestionEtablissement;

