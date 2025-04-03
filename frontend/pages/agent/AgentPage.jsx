
import React, { useEffect, useState, useCallback } from "react";
import axios from "axios";
import './AgentPage.css';
import { useParams, useNavigate } from 'react-router-dom';

const API_BASE_URL = "http://127.0.0.1:8000/api/agent/files";

// Global authentication for Axios
axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;

const AgentPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [fileId, setFileId] = useState("");
  const [singleFile, setSingleFile] = useState(null);
  const [successMessage, setSuccessMessage] = useState("");
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [serviceId, setServiceId] = useState("");
  const [agencyFiles, setAgencyFiles] = useState([]);
  const [showAgencyFiles, setShowAgencyFiles] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // Fetch file by ID
  const fetchFileById = useCallback(async (id) => {
    if (!id) return;
    setLoading(true);
    try {
      const response = await axios.get(`${API_BASE_URL}/${id}`);
      if (response.data.file) {
        setSingleFile(response.data.file);
      }
    } catch (error) {
      console.error("Erreur lors de la récupération du file:", error.response?.data || error);
      setError("Erreur lors de la récupération du file");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    if (fileId) {
      fetchFileById(fileId);
    }
  }, [fileId, fetchFileById]);

  // Fetch all files by agency for current day
  const fetchFilesByAgency = async () => {
    setLoading(true);
    try {
      const token = localStorage.getItem("token");
      // Changed from POST to GET as per the API route configuration
      const response = await axios.get(`${API_BASE_URL}/agence`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      
      if (response.data && Array.isArray(response.data.files)) {
        setAgencyFiles(response.data.files);
      } else if (response.data && Array.isArray(response.data.agences)) {
        setAgencyFiles(response.data.agences); // Alternative property name
      } else {
        console.log("Réponse API:", response.data);
        setAgencyFiles([]);
      }
      setError("");
    } catch (error) {
      console.error("Erreur lors de la récupération des files:", error.response?.data || error);
      setError("Erreur lors de la récupération des files");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchFilesByAgency();
  }, []);

  const handleFileClick = (id) => {
    setFileId(id);
    fetchFileById(id);
  };

  // Create new file
  const createFile = async () => {
    if (!serviceId.trim()) {
      alert("Veuillez remplir le champ 'ID du Service'.");
      return;
    }
    try {
      // Fixed the endpoint to match the backend route '/creat' instead of '/create'
      const response = await axios.post(`${API_BASE_URL}/creat`, { id_service: serviceId });
      setServiceId("");
      setFileId(response.data.id);
      setShowCreateForm(false);
      setSuccessMessage("File créée avec succès");
      setTimeout(() => setSuccessMessage(""), 3000);
      fetchFilesByAgency(); // Refresh the list
    } catch (error) {
      console.error("Erreur lors de la création du file:", error.response?.data || error);
      setSuccessMessage("Erreur lors de la création de la file.");
    }
  };

  // Update file status
  const updateStatus = async (id, newStatus) => {
    try {
      await axios.put(`${API_BASE_URL}/update_status/${id}`, { status: newStatus });
      fetchFileById(id);
      fetchFilesByAgency(); // Refresh the list
      setSuccessMessage(`Statut mis à jour: ${newStatus}`);
      setTimeout(() => setSuccessMessage(""), 3000);
    } catch (error) {
      console.error("Erreur lors de la mise à jour du statut:", error.response?.data || error);
    }
  };

  // Delete file
  const deleteFile = async (id) => {
    if (confirm("Êtes-vous sûr de vouloir supprimer cette file?")) {
      try {
        await axios.delete(`${API_BASE_URL}/${id}`);
        setSingleFile(null);
        setSuccessMessage("Suppression avec succès");
        setTimeout(() => setSuccessMessage(""), 3000);
        fetchFilesByAgency(); // Refresh the list
      } catch (error) {
        console.error("Erreur lors de la suppression du file:", error.response?.data || error);
      }
    }
  };

  // Call next citizen (increment file number)
  const callNextCitizen = async (id) => {
    try {
      await axios.post(`${API_BASE_URL}/nunmber/${id}`);
      fetchFileById(id);
      fetchFilesByAgency(); // Refresh the list
      setSuccessMessage("Citoyen suivant appelé avec succès");
      setTimeout(() => setSuccessMessage(""), 3000);
    } catch (error) {
      console.error("Erreur lors de l'appel du citoyen suivant:", error.response?.data || error);
    }
  };

  // Status style for files
  const getStatusStyle = (status) => {
    switch(status) {
      case 'ouvert': return 'bg-success text-white';
      case 'fermer': return 'bg-danger text-white';
      case 'pause': return 'bg-warning text-dark';
      default: return 'bg-secondary text-white';
    }
  };

  return (
    <div className="agent-page">
      <h1>Gestion des Files d'Attente</h1>
      <button className="create-file-btn" onClick={() => setShowCreateForm(!showCreateForm)}>
        {showCreateForm ? "Annuler la Création" : "Créer un File"}
      </button>

      {showCreateForm && (
        <div className="form-container">
          <input
            className="input-field"
            type="text"
            placeholder="ID du Service"
            value={serviceId}
            onChange={(e) => setServiceId(e.target.value)}
          />
          <button className="submit-btn" onClick={createFile}>Créer un Nouveau File</button>
        </div>
      )}

      <div className="fetch-single-file">
        <input
          className="input-field"
          type="text"
          placeholder="Entrez l'ID du file à récupérer"
          value={fileId}
          onChange={(e) => setFileId(e.target.value)}
        />
      </div>

      {loading && <p className="text-center">Chargement en cours...</p>}
      {error && <p className="text-center text-danger">{error}</p>}

      {showAgencyFiles && agencyFiles.length > 0 && (
        <div className="agency-files-list">
          <h3>Files pour l'Agence:</h3>
          <div className="files-grid">
            {agencyFiles.map(file => (
              <div 
                key={file.id_file} 
                className="file-item" 
                onClick={() => handleFileClick(file.id_file)}
              >
                <span className="file-id">ID: {file.id_file}</span>
                <span className={`file-status ${file.status}`}>
                  {file.status}
                </span>
                <span className="file-number">Numéro: {file.numero}</span>
              </div>
            ))}
          </div>
        </div>
      )}

      {agencyFiles.length === 0 && !loading && (
        <p className="text-center">Aucun file trouvé pour cette agence</p>
      )}

      {singleFile && (
        <div className="file-details">
          <h3>File Spécifique:</h3>
          <p>ID: {singleFile.id_file}</p>
          <p>Status: {singleFile.status}</p>
          <div className="button-group">
            <button className="status-btn ouvert" onClick={() => updateStatus(singleFile.id_file, "ouvert")}>Ouvert</button>
            <button className="status-btn fermer" onClick={() => updateStatus(singleFile.id_file, "fermer")}>Fermer</button>
            <button className="status-btn pause" onClick={() => updateStatus(singleFile.id_file, "pause")}>Pause</button>
          </div>
          <div className="citizen-number">{singleFile.numero}</div>
          <button className="call-next-btn" onClick={() => callNextCitizen(singleFile.id_file)}>Numéro Suivant</button>
          <button className="delete-btn" onClick={() => deleteFile(singleFile.id_file)}>Supprimer</button>
        </div>
      )}

      {successMessage && <p className="success-message">{successMessage}</p>}
    </div>
  );
};

export default AgentPage;
