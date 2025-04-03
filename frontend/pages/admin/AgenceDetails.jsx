import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import GestionServices from "./GestionService";

const AgenceDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [agence, setAgence] = useState(null);

  // Références pour les champs de formulaire
  const nomAgenceRef = useRef(null);
  const adresseRef = useRef(null);
  const villeRef = useRef(null);
  const horairesRef = useRef(null);

  useEffect(() => {
    if (id) {
      fetchAgenceById(id);
    }
  }, [id]);

  const fetchAgenceById = async (id) => {
    try {
      const token = localStorage.getItem("token");
      const config = { headers: { Authorization: `Bearer ${token}` } };

      const response = await axios.get(
        `http://localhost:8000/api/admin/agences/${id}`,
        config
      );
      setAgence(response.data.agences); // Assurez-vous que l'API renvoie un objet sous 'agences'
    } catch (error) {
      console.error("Erreur lors de la récupération des informations:", error);
    }
  };

  const deleteAgence = async () => {
    try {
      const token = localStorage.getItem("token");
      await axios.delete(`http://localhost:8000/api/admin/agences/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      navigate("/AdminPage");
    } catch (error) {
      console.error("Erreur lors de la suppression:", error);
    }
  };

  const handleUpdate = async () => {
    // Utilisation des références pour obtenir les valeurs des champs
    const nom_agence = nomAgenceRef.current.value;
    const adresse = adresseRef.current.value;
    const ville = villeRef.current.value;
    const horaires = horairesRef.current.value;

    // Vérification des champs
    if (!nom_agence.trim() || !adresse.trim() || !ville.trim() || !horaires.trim()) {
      alert("Les données doivent être renseignées");
      return;
    }

    try {
      const token = localStorage.getItem("token");
      const response = await axios.put(
        `http://localhost:8000/api/admin/agences/${id}`,
        { nom_agence, adresse, ville, horaires },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setAgence(response.data.agences); // Met à jour l'état avec les nouvelles données
      alert("Mise à jour réussie !");
    } catch (error) {
      console.error("Erreur lors de la mise à jour:", error);
      alert("Erreur lors de la mise à jour !");
    }
  };

  return (
    <div>
      <h2>Détails de l'agence</h2>
      {agence ? (
        <div>
          <p>
            <strong>Nom:</strong>{" "}
            <input
              ref={nomAgenceRef}
              type="text"
              defaultValue={agence.nom_agence}
            />
          </p>
          <p>
            <strong>Adresse:</strong>{" "}
            <input
              ref={adresseRef}
              type="text"
              defaultValue={agence.adresse}
            />
          </p>
          <p>
            <strong>Ville:</strong>{" "}
            <input ref={villeRef} type="text" defaultValue={agence.ville} />
          </p>
          <p>
            <strong>Horaires:</strong>{" "}
            <input
              ref={horairesRef}
              type="text"
              defaultValue={agence.horaires}
            />
          </p>

{/* Section pour gérer les services de l'agence */}


          <button
            className="btn btn-primary me-2"
            onClick={async () => {
              await handleUpdate();
              navigate("/AdminPage");
            }}
          >
            Mettre à jour
          </button>

          <button
            className="btn btn-danger"
            onClick={async () => {
              await deleteAgence();
              navigate("/AdminPage");
            }}
          >
            Supprimer
          </button>

          <button className="btn btn-secondary" onClick={() => navigate("/AdminPage")}>
            Retour
          </button>

          <div>
            
            <GestionServices id_agence={id} /> {/* Passer l'ID de l'agence pour récupérer ses services */}
          </div>

        </div>
      ) : (
        <p>Chargement...</p>
      )}
    </div>
  );
};

export default AgenceDetails;

