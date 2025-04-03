import React, { useState, useEffect } from "react";
import axios from "axios";
import "./GestionServices.css";

const GestionServices = ({ id_agence = null }) => {
  const [services, setServices] = useState([]);
  const [editServiceId, setEditServiceId] = useState(null);
  const [editServiceName, setEditServiceName] = useState("");
  const [newServiceName, setNewServiceName] = useState(""); // Utiliser cette variable
  const [addingService, setAddingService] = useState(false);

  useEffect(() => {
    if (!id_agence) {
      console.error("Erreur : id_agence est indéfini.");
      alert("L'id de l'agence est requis pour charger les services.");
      return;
    }

    fetchServicesByAgence(id_agence);
  }, [id_agence]);

  const fetchServicesByAgence = async (id_agence) => {
    if (!id_agence) return;

    try {
      const token = localStorage.getItem("token");
      if (!token) {
        alert("Veuillez vous connecter pour accéder aux services.");
        return;
      }

      const config = { headers: { Authorization: `Bearer ${token}` } };
      const response = await axios.get(
        `http://127.0.0.1:8000/api/admin/agences/services/${id_agence}`,
        config
      );

      setServices(response.data.services || []);
    } catch (error) {
      console.error("Erreur lors de la récupération des services :", error.response?.data || error.message);
      alert("Une erreur est survenue lors de la récupération des services.");
    }
  };

  const deleteService = async (id_service) => {
    try {
      const token = localStorage.getItem("token");
      if (!token) {
        console.error("Token non trouvé.");
        return;
      }

      const config = { headers: { Authorization: `Bearer ${token}` } };

      await axios.delete(
        `http://127.0.0.1:8000/api/admin/agences/services/${id_service}`,
        config
      );

      fetchServicesByAgence(id_agence);
    } catch (error) {
      console.error("Erreur lors de la suppression du service :", error.response?.data || error.message);
    }
  };

  const startEditing = (service) => {
    setEditServiceId(service.id_service);
    setEditServiceName(service.nom_service);
  };

  const cancelEditing = () => {
    setEditServiceId(null);
    setEditServiceName("");
  };

  const updateService = async (id_service) => {
    if (!editServiceName.trim()) {
      alert("Le nom du service est requis.");
      return;
    }

    try {
      const token = localStorage.getItem("token");
      if (!token) {
        console.error("Token non trouvé.");
        return;
      }

      const config = {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      };

      await axios.put(
        `http://127.0.0.1:8000/api/admin/agences/services/${id_service}`,
        { nom_service: editServiceName },
        config
      );

      setEditServiceId(null);
      fetchServicesByAgence(id_agence);
    } catch (error) {
      console.error("Erreur lors de la mise à jour du service :", error.response?.data || error.message);
    }
  };

  // Ajouter un service
  const addService = async () => {
    if (!newServiceName.trim()) {  // Utiliser newServiceName ici
      alert("Le nom du service est requis.");
      return;
    }
    try {
      const token = localStorage.getItem("token");
      if (!token) {
        console.error("Token non trouvé.");
        return;
      }
      const config = {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      };
      await axios.post(
        `http://127.0.0.1:8000/api/admin/agences/services/${id_agence}`,
        { nom_service: newServiceName }, // Utiliser newServiceName ici
        config
      );
      setNewServiceName("");  // Réinitialiser la variable
      setAddingService(false); // Cacher le formulaire après l'ajout
      fetchServicesByAgence(id_agence);
    } catch (error) {
      console.error("Erreur lors de l'ajout du service :", error.message);
    }
  };

  return (
    <div className="gestion-services">
      <h2>Liste Services</h2>

      <ul>
        {services.length > 0 ? (
          services.map((service) => (
            <li key={service.id_service} className="service-item">
              {editServiceId === service.id_service ? (
                <input
                  type="text"
                  value={editServiceName}
                  onChange={(e) => setEditServiceName(e.target.value)}
                />
              ) : (
                <span className="service-name">{service.nom_service}</span>
              )}

              <div className="service-actions">
                {editServiceId === service.id_service ? (
                  <>
                    <button className="save-btn" onClick={() => updateService(service.id_service)}>Enregistrer</button>
                    <button className="cancel-btn" onClick={cancelEditing}>Annuler</button>
                  </>
                ) : (
                  <>
                    <button className="edit-btn" onClick={() => startEditing(service)}>Modifier</button>
                    <button className="delet-btn" onClick={() => deleteService(service.id_service)}>Supprimer</button>
                  </>
                )}
              </div>
            </li>
          ))
        ) : (
          <p>Aucun service trouvé.</p>
        )}
      </ul>

      {/* Bouton Ajouter un service */}
      {!addingService ? (
        <button className="add-service-btn" onClick={() => setAddingService(true)}>Ajouter un service</button>
      ) : (
        <div className="add-service">
          <input
            type="text"
            placeholder="Nom du service"
            value={newServiceName} // Utiliser newServiceName ici
            onChange={(e) => setNewServiceName(e.target.value)}
          />
          <button className="save-btn" onClick={addService}>Enregistrer</button>
          <button className="cancel-btn" onClick={() => setAddingService(false)}>Annuler</button>
        </div>
      )}
    </div>
  );
};

export default GestionServices;














