// GestionAdmin.jsx
import React, { useEffect, useState , useCallback} from 'react';
import axios from 'axios';
import { FaEye, FaEyeSlash } from "react-icons/fa";
import { Link } from 'react-router-dom';


const GestionAdmin = () => {
 const [admins,setAdmins] = useState([]);
 const [newAdmin, setNewAdmin] = useState({ nom_user: '',prenom_user: '', email_user: '', password: '' ,id_etabl:'' ,role:'',password_confirmation:''});
const [showForm, setShowForm] = useState(false);
const [showPassword, setShowPassword] = useState(false);
//const [successMessage, setSuccessMessage] = useState("");
//const [adminDetails, setAdminDetails] = useState(null);
const [searchQuery, setSearchQuery] = useState('');




  useEffect(() => {
    fetchAdmins();
  }, []);

  const fetchAdmins = async () => {
    try {
      const token = localStorage.getItem('token');
      const config = {
        headers: { Authorization: `Bearer ${token}` },
      };
      const response = await axios.get(`http://localhost:8000/api/supadmin/users`, config);
      console.log(response.data);
      setAdmins(response.data.users);
    } catch (error) {
      console.error('Erreur de récupération des admins:', error);
    }
  };

  const addAdmin = async () => {
    try {
      if (!newAdmin.nom_user || !newAdmin.prenom_user || !newAdmin.email_user || !newAdmin.password || !newAdmin.id_etabl || !newAdmin.role || !newAdmin.password_confirmation) {
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

      await axios.post(`http://localhost:8000/api/supadmin/users/register`, newAdmin, config);
      fetchAdmins();
      setNewAdmin({ nom_user: '',prenom_user: '', email_user: '', password: '' ,id_etabl:'' ,role:'',password_confirmation:''});
      setShowForm(false);
    } catch (error) {
      console.error('Erreur lors de l’ajout d’un admin:', error.response ? error.response.data : error.message);
    }
  };

 

 

 


  const searchAdmin = useCallback(
    async (query) => { 
      setSearchQuery(query); 
    
      if (!query.trim()) {
        fetchAdmins(); 
        return;
      }
    
      try {
        const token = localStorage.getItem('token');
        const config = {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        };
    
        const response = await axios.get(`http://localhost:8000/api/supadmin/users/search/${query}`, {
          params: { query },
          ...config,
        });
    
        setAdmins(response.data); // Mettre à jour la liste des admins selon la recherche
      } catch (error) {
        console.error("Erreur lors de la recherche:", error.response ? error.response.data : error.message);
      }
    },
    [] 
  );

  // Effet avec délai de 500ms avant de lancer la recherche
  useEffect(() => {
    const timer = setTimeout(() => {
      searchAdmin(searchQuery); 
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
    <h2>Liste des Admins</h2>
    <input
      type="text"
      placeholder="Nom ou Prénom"
      value={searchQuery}
      onChange={handleSearchChange} // Recherche dynamique à chaque modification
    />
    <button className="btn btn-secondary" onClick={searchAdmin}>
      Rechercher
    </button>

    {/* Table des admins */}
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
        {admins.length > 0 ? (
          admins.map((admin) => (
            <tr key={admin.id_user}>
              <td>{admin.id_user}</td>
              <td>{admin.nom_user}</td>
              <td>{admin.prenom_user}</td>
              <td>{admin.email_user}</td>
              <td>
                <Link to={`/admin/${admin.id_user}`} className="btn btn-info">
                  Voir Détails
                </Link>
              </td>
            </tr>
          ))
        ) : (
          <tr>
            <td colSpan="5">Aucun administrateur trouvé.</td>
          </tr>
        )}
      </tbody>
    </table>

    {/* Ajouter un admin */}
    <button className="btn btn-primary mb-3" onClick={() => setShowForm(!showForm)}>
      {showForm ? "Fermer" : "Ajouter Admin"}
    </button>

    {showForm && (
      <div className="card p-3 mb-4">
        <h3>Ajouter un admin</h3>

        {/* Formulaire d'ajout d'un admin */}
        {["nom_user", "prenom_user", "email_user", "id_etabl", "role"].map((field) => (
          <div className="form-group" key={field}>
            <label>{field.replace("_", " ")}</label>
            <input
              type="text"
              className="form-control"
              name={field}
              value={newAdmin[field]}
              onChange={(e) =>
                setNewAdmin({ ...newAdmin, [e.target.name]: e.target.value })
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
                value={newAdmin[field]}
                onChange={(e) =>
                  setNewAdmin({ ...newAdmin, [e.target.name]: e.target.value })
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

        <button className="btn btn-success" onClick={addAdmin}>
          Ajouter Admin
        </button>
      </div>
    )}
  </div>
);}
export default GestionAdmin;
  
