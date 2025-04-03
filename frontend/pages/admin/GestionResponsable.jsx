// GestionResponsable.jsx
import React, { useEffect, useState , useCallback} from 'react';
import axios from 'axios';
import { FaEye, FaEyeSlash } from "react-icons/fa";
import { Link } from 'react-router-dom';


const GestionResponsable = () => {
 const [responsables,setResponsables] = useState([]);
 const [newResponsable, setNewResponsable] = useState({ nom_user: '',prenom_user: '', email_user: '', password: '' ,role:'',password_confirmation:''});
const [showForm, setShowForm] = useState(false);
const [showPassword, setShowPassword] = useState(false);
const [searchQuery, setSearchQuery] = useState('');




  useEffect(() => {
    fetchResponsables();
  }, []);

  const fetchResponsables = async () => {
    try {
      const token = localStorage.getItem('token');
      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };
      const response = await axios.get(`http://localhost:8000/api/admin/users`, config);
      console.log(response.data);
      setResponsables(response.data.users);
    } catch (error) {
      console.error('Erreur de récupération des Responsables:', error);
    }
  };

  const addResponsable = async () => {
    try {
      if (!newResponsable.nom_user || !newResponsable.prenom_user || !newResponsable.email_user || !newResponsable.password || !newResponsable.role || !newResponsable.password_confirmation) {
        console.error('Les champs sont requis.');
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

      await axios.post(`http://localhost:8000/api/admin/users/register`, newResponsable, config);
      fetchResponsables();
      setNewResponsable({ nom_user: '',prenom_user: '', email_user: '', password: '' ,role:'',password_confirmation:''});
      setShowForm(false);
    } catch (error) {
      console.error('Erreur lors de l’ajout d’un Responsable:', error.response ? error.response.data : error.message);
    }
  };

 

 

 


  const searchResponsable = useCallback(
    async (query) => { 
      setSearchQuery(query); 
    
      if (!query.trim()) {
        fetchResponsables(); 
        return;
      }
    
      try {
        const token = localStorage.getItem('token');
        const config = {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        };
    
        const response = await axios.get(`http://localhost:8000/api/responsable/users/search/${query}`, {
          params: { query },
          ...config,
        });
    
        setResponsables(response.data); // Mettre à jour la liste des Responsables selon la recherche
      } catch (error) {
        console.error("Erreur lors de la recherche:", error.response ? error.response.data : error.message);
      }
    },
    [] 
  );

  // Effet avec délai de 500ms avant de lancer la recherche
  useEffect(() => {
    const timer = setTimeout(() => {
      searchResponsable(searchQuery); 
    }, 500); 
    
    return () => clearTimeout(timer); 
  }, [searchQuery]); 

  // Fonction de gestion du changement dans le champ de recherche
const handleSearchChange = (event) => {
  setSearchQuery(event.target.value); 
  
};
  
const togglePasswordVisibility = (field) => {
  setShowPassword((prev) => ({
    ...prev,
    [field]: !prev[field],
  }));
}; 
    
return (
  <div>
    <h2>Liste des Responsables</h2>
    <input
      type="text"
      placeholder="Nom ou Prénom"
      value={searchQuery}
      onChange={handleSearchChange} // Recherche dynamique à chaque modification
    />
    <button className="btn btn-secondary" onClick={searchResponsable}>
      Rechercher
    </button>

    {/* Table des Responsables */}
    <table className="table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nom</th>
          <th>Prénom</th>
          <th>Email</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {responsables.length > 0 ? (
          responsables.map((responsable) => (
            <tr key={responsable.id_user}>
              <td>{responsable.id_user}</td>
              <td>{responsable.nom_user}</td>
              <td>{responsable.prenom_user}</td>
              <td>{responsable.email_user}</td>
              <td>
                <Link to={`/responsable/${responsable.id_user}`} className="btn btn-info">
                  Voir Détails
                </Link>
              </td>
            </tr>
          ))
        ) : (
          <tr>
            <td colSpan="5">Aucun responsable trouvé.</td>
          </tr>
        )}
      </tbody>
    </table>

    {/* Ajouter un Responsable */}
    <button className="btn btn-primary mb-3" onClick={() => setShowForm(!showForm)}>
      {showForm ? "Fermer" : "Ajouter Responsable"}
    </button>

    {showForm && (
      <div className="card p-3 mb-4">
        <h3>Ajouter un responsable</h3>

        {/* Formulaire d'ajout d'un responsable */}
        {["nom_user", "prenom_user", "email_user", "role"].map((field) => (
          <div className="form-group" key={field}>
            <label>{field.replace("_", " ")}</label>
            <input
              type="text"
              className="form-control"
              name={field}
              value={newResponsable[field]}
              onChange={(e) =>
                setNewResponsable({ ...newResponsable, [e.target.name]: e.target.value })
              }
              placeholder={field.replace("_", " ")}
            />
          </div>
        ))}

        {/* Password and confirmation fields */}
        {["password", "password_confirmation"].map((field) => (
          <div className="form-group" key={field}>
            <label>{field.replace("_", " ")}</label>
            <div className="input-group">
              <input
                type={showPassword[field] ? "text" : "password"}
                className="form-control"
                name={field}
                value={newResponsable[field]}
                onChange={(e) =>
                  setNewResponsable({ ...newResponsable, [e.target.name]: e.target.value })
                }
                placeholder={field.replace("_", " ")}
              />
              <span
                className="input-group-text"
                onClick={() => togglePasswordVisibility(field)}
                style={{ cursor: "pointer" }}
              >
                {showPassword[field] ? <FaEyeSlash /> : <FaEye />}
              </span>
            </div>
          </div>
        ))}

        <button className="btn btn-success" onClick={addResponsable}>
          Ajouter Responsable
        </button>
      </div>
    )}
  </div>
);}
export default GestionResponsable;
