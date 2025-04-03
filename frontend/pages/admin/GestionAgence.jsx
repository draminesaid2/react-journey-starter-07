import React, { useEffect, useState, useCallback } from "react";
import axios from "axios";
import { Link, useNavigate } from "react-router-dom";


const GestionAgence = () => {
  const [agences, setAgences] = useState([]);
  const [searchQuery, setSearchQuery] = useState("");
  const navigate = useNavigate();
  const [newAgence, setNewAgence] = useState({
    nom_agence: "",
    adresse: "",
    ville: "",
    latitude: "",
    longitude: "",
    horaires: "",
    
  });
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    fetchAgences();
  }, []);

  const fetchAgences = async () => {
    try {
      const token = localStorage.getItem("token");
      if (!token) {
        console.error("Token non trouvé");
        return;
      }

      const config = { headers: { Authorization: `Bearer ${token}` } };
      const response = await axios.get(`http://localhost:8000/api/admin/agences`, config);

      console.log("Données reçues:", response.data.agences);

      // Vérifier si la réponse contient bien un tableau
      setAgences(Array.isArray(response.data.agences) ? response.data.agences : []);
    } catch (error) {
      console.error("Erreur de récupération des Agences:", error);
      setAgences([]); // Éviter une erreur si l’API échoue
    }
  };

  const addAgence = async () => {
    try {
      const { nom_agence, adresse, ville, latitude, longitude, horaires,id_etabl } = newAgence;
      if (!nom_agence || !adresse || !ville || !latitude || !longitude || !horaires || !id_etabl) {
        console.error("Tous les champs sont requis.");
        return;
      }

      const token = localStorage.getItem("token");
      if (!token) {
        console.error("Token non trouvé");
        return;
      }

      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };

      await axios.post(`http://localhost:8000/api/admin/agences`, newAgence, config);
      fetchAgences();
      setNewAgence({
        nom_agence: "",
        adresse: "",
        ville: "",
        latitude: "",
        longitude: "",
        horaires: "",
        id_etabl:"",
      });
      setShowForm(false);
    } catch (error) {
      console.error("Erreur lors de l’ajout d’une Agence:", error.response ? error.response.data : error.message);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login"); // Redirection vers la page de connexion
  };

 

  const searchAgence = useCallback(async (query) => {
    // Assurez-vous que query est bien une chaîne de caractères
    const validQuery = typeof query === "string" ? query : "";
  
    // Appliquez trim() uniquement si validQuery est une chaîne non vide
    if (!validQuery.trim()) {
      fetchAgences();
      return;
    }
  
    try {
      const token = localStorage.getItem("token");
      if (!token) {
        console.error("Token non trouvé");
        return;
      }
  
      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };
  
      const response = await axios.get(`http://localhost:8000/api/admin/agences/search/${validQuery}`, config);
  
      console.log("Résultats de recherche:", response.data.agences);
      setAgences(Array.isArray(response.data.agences) ? response.data.agences : []);
    } catch (error) {
      console.error("Erreur lors de la recherche:", error.response ? error.response.data : error.message);
    }
  }, []);
    

  useEffect(() => {
    const timer = setTimeout(() => {
      searchAgence(searchQuery);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchQuery, searchAgence]);

  const handleSearchChange = (event) => {
    const query = event.target.value;
     // Assurez-vous que query est une chaîne de caractères
  if (typeof query === "string") {
    setSearchQuery(query);
  } else {
    console.error("La valeur de la recherche n'est pas une chaîne de caractères.");
  }
};

  const handleChange = (event) => {
    const { name, value } = event.target;
    setNewAgence((prevState) => ({ ...prevState, [name]: value }));
  };

  return (
    <div>
      <h2>Gestion des Agences</h2>

      <input
        type="text"
        placeholder="Nom ou Ville"
        value={searchQuery}
        onChange={handleSearchChange} // Recherche dynamique à chaque modification
      />
      <button className="btn btn-secondary" onClick={searchAgence}>
      Rechercher
    </button>

      <table className="table mt-4">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nom agence</th>
            <th>Adresse</th>
            <th>Ville</th>
            <th>Horaires</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {Array.isArray(agences) && agences.length > 0 ? (
            agences.map((agence) => (
              <tr key={agence.id_agence}>
                <td>{agence.id_agence}</td>
                <td>{agence.nom_agence}</td>
                <td>{agence.adresse}</td>
                <td>{agence.ville}</td>
                <td>{agence.horaires}</td>
                
                <td>
                  <Link to={`/agence/${agence.id_agence}`} className="btn btn-info">
                    Voir Détails
                  </Link>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="6">Aucune agence trouvée.</td>
            </tr>
          )}
        </tbody>
      </table>

      <button className="btn btn-primary mb-3" onClick={() => setShowForm(!showForm)}>
        {showForm ? "Fermer" : "Ajouter agence"}
      </button>

      {showForm && (
        <div className="card p-3 mb-4">
          <h3>Ajouter une agence</h3>
          {["nom_agence", "adresse", "latitude", "longitude", "ville", "horaires" ,"id_etabl"].map((field) => (
            <div className="form-group" key={field}>
              <label>{field.replace("_", " ")}</label>
              <input
                type="text"
                className="form-control"
                name={field}
                value={newAgence[field]}
                onChange={handleChange}
                placeholder={field}
              />
            </div>
          ))}
          <button className="btn btn-success mt-2" onClick={addAgence}>
            Ajouter
          </button>
        </div>
      )}
    </div>
  );
};

export default GestionAgence;
