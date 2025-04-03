import axios from 'axios';

// Fonction pour gérer la déconnexion
export const logout = async () => {
  try {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('Aucun token trouvé');
      return;
    }

    const config = {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    };

    // Appel de l'API de déconnexion
    await axios.post('http://127.0.0.1:8000/api/logout', {}, config); // Remplacez par l'URL de votre API

    // Supprimer le token du localStorage
    localStorage.removeItem('token');
    alert('Vous êtes déconnecté(e)');
    window.location.href = '/'; // Redirigez l'utilisateur après la déconnexion
  } catch (error) {
    console.error('Erreur lors de la déconnexion:', error);
  }
};
