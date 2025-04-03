import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";


const EtablissementDetails = () => {
  const { id } = useParams(); // Récupère l'ID depuis l'URL
  const navigate = useNavigate(); // Pour rediriger après suppression
  const [etablissement, setEtablissement] = useState(null);

  useEffect(() => {
    fetchEtablissementById();
  }, []);

  const fetchEtablissementById = async () => {
    try {
      const token = localStorage.getItem("token");
      const config = { headers: { Authorization: `Bearer ${token}` } };

      const response = await axios.get(
        `http://localhost:8000/api/supadmin/etablissement/${id}`,
        config
      );
      setEtablissement(response.data);
    } catch (error) {
      console.error("Erreur lors de la récupération des informations:", error);
    }
  };

  const deleteEtablissement = async () => {
    try {
      const token = localStorage.getItem("token");
      await axios.delete(
        `http://localhost:8000/api/supadmin/etablissement/${id}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      navigate("/"); // Redirige vers la liste après suppression
    } catch (error) {
      console.error("Erreur lors de la suppression:", error);
    }
  };

  const handleUpdate = async () => {
    const nom = document.getElementById("nom").value;
    const adresse = document.getElementById("adresse").value;

    if (!nom.trim() || !adresse.trim()) {
      alert("Le nom et l'adresse doivent être renseignés");
      return;
    }

    try {
      const token = localStorage.getItem("token");
      await axios.put(
        `http://localhost:8000/api/supadmin/etablissement/${id}`,
        { nom_etabl: nom, adresse_etabl: adresse },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      alert("Mise à jour réussie !");
      fetchEtablissementById(); // Recharger les infos après modification
    } catch (error) {
      console.error("Erreur lors de la mise à jour:", error);
    }
  };

  return (
    <div>
      <h2>Détails de l'Établissement</h2>
      {etablissement ? (
        <div>
          <p>
            <strong>Nom:</strong>{" "}
            <input id="nom" type="text" defaultValue={etablissement.nom_etabl} />
          </p>
          <p>
            <strong>Adresse:</strong>{" "}
            <input
              id="adresse"
              type="text"
              defaultValue={etablissement.adresse_etabl}
            />
          </p>
          <button className="btn btn-primary me-2" onClick={async () => {
  await handleUpdate();
  navigate("/SupAdminPage");
}}>
  Mettre à jour
</button>

          <button className="btn btn-danger" onClick={async () => {
  await deleteEtablissement();
  navigate("/SupAdminPage");
}}>
  Supprimer
</button>

          <button className="btn btn-secondary" onClick={() => navigate("/SupAdminPage")}>
  Retour
</button>

        </div>
      ) : (
        <p>Chargement...</p>
      )}
    </div>
  );
};

export default EtablissementDetails;

