import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "axios";
import { FaEye, FaEyeSlash } from "react-icons/fa"; // N'oublie pas d'importer les icônes pour l'affichage du mot de passe

const AgentDetails = () => {
  const { id } = useParams(); // Récupère l'ID depuis l'URL
  const navigate = useNavigate(); // Pour rediriger après suppression
  const [agent, setAgent] = useState(null); // Etat pour un Agent spécifique
  const [passwords, setPasswords] = useState({}); // Etat pour gérer les mots de passe des Agents
  const [showPassword, setShowPassword] = useState({}); // Etat pour gérer la visibilité des mots de passe
  const [successMessage, setSuccessMessage] = useState("");

  // Récupère les détails d'un Agent
  useEffect(() => {
    if (id) {
      fetchAgentById(id);
    }
  }, [id]);

  const fetchAgentById = async (id) => {
    try {
      const token = localStorage.getItem("token");
      const config = {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      };
      const response = await axios.get(`http://localhost:8000/api/responsable/users/${id}`, config);
      setAgent(response.data);
    } catch (error) {
      console.error("Erreur lors de la récupération des informations de l'Agent:", error);
    }
  };

  const deleteAgent = async (id_user) => {
    try {
      const token = localStorage.getItem("token");
      await axios.delete(`http://localhost:8000/api/responsable/users/${id_user}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      navigate("/ResponsablePage"); // Redirige après la suppression
    } catch (error) {
      console.error("Erreur lors de la suppression d’un Agent:", error);
    }
  };

  const handlePasswordChange = (e, id_user) => {
    const updatedPassword = e.target.value;
    setPasswords((prevPasswords) => ({
      ...prevPasswords,
      [id_user]: updatedPassword,
    }));
  };

  const togglePasswordVisibility = (id_user) => {
    setShowPassword((prev) => ({
      ...prev,
      [id_user]: !prev[id_user],
    }));
  };

  const handleUpdate = async (id_user) => {
    const nom = document.getElementById(`nom_user-${id_user}`).value;
    const prenom = document.getElementById(`prenom_user-${id_user}`).value;
    const email = document.getElementById(`email_user-${id_user}`).value;
    const password = document.getElementById(`password-${id_user}`).value;

    if (!nom.trim() || !prenom.trim() || !email.trim()) {
      alert("Les données doivent être renseignées");
      return;
    }

    const updatedData = {
      nom_user: nom,
      prenom_user: prenom,
      email_user: email,
      password: password,
    };

    try {
      const token = localStorage.getItem("token");
      const config = {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      };
      await axios.put(
        `http://localhost:8000/api/responsable/users/${id_user}`,
        updatedData,
        config
      );
      setSuccessMessage("Mise à jour réussie !");
      setTimeout(() => {
        setSuccessMessage("");
      }, 3000);
      navigate("/ResponsablePage"); // Redirige après la mise à jour
    } catch (error) {
      console.error("Erreur lors de la mise à jour de l'Agent:", error);
    }
  };

  return (
    <div>
      <h2>Détails de l'Agent</h2>
      {agent ? (
        <div>
          <p>
            <strong>Nom:</strong>{" "}
            <input
              id={`nom_user-${agent.id_user}`}
              type="text"
              defaultValue={agent.nom_user}
            />
          </p>
          <p>
            <strong>Prénom:</strong>{" "}
            <input
              id={`prenom_user-${agent.id_user}`}
              type="text"
              defaultValue={agent.prenom_user}
            />
          </p>
          <p>
            <strong>Email:</strong>{" "}
            <input
              id={`email_user-${agent.id_user}`}
              type="text"
              defaultValue={agent.email_user}
            />
          </p>
          <p>
            <strong>Nouveau mot de passe:</strong>
            <div className="form-group input-group">
              <input
                id={`password-${agent.id_user}`}
                type={showPassword[agent.id_user] ? "text" : "password"}
                value={passwords[agent.id_user] || ""}
                onChange={(e) => handlePasswordChange(e, agent.id_user)}
                className="form-control"
                placeholder="Nouveau mot de passe"
              />
              <span
                className="input-group-text"
                onClick={() => togglePasswordVisibility(agent.id_user)}
                style={{ cursor: "pointer" }}
              >
                {showPassword[agent.id_user] ? <FaEyeSlash /> : <FaEye />}
              </span>
            </div>
          </p>
          <button
            className="btn btn-primary me-2"
            onClick={() => handleUpdate(agent.id_user)}
          >
            Mettre à jour
          </button>
          <button
            className="btn btn-danger"
            onClick={() => deleteAgent(agent.id_user)}
          >
            Supprimer
          </button>
        </div>
      ) : (
        <p>Chargement...</p>
      )}

      {successMessage && <div className="alert alert-success">{successMessage}</div>}

      <button className="btn btn-secondary" onClick={() => navigate("/ResponsablePage")}>
        Retour
      </button>
    </div>
  );
};

export default AgentDetails;