import React, { Component } from 'react';

class ErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null, info: null };
  }

  static getDerivedStateFromError(error) {
    // Met à jour l'état pour afficher l'UI de secours
    return { hasError: true };
  }

  componentDidCatch(error, info) {
    // Enregistrer l'erreur et les informations de l'info
    this.setState({
      error: error,  // Pas besoin de redéclarer `error` ici, on utilise l'objet d'erreur passé en paramètre
      info: info,
    });
  }

  render() {
    const { error, info } = this.state; // Utilisation de l'objet error déjà dans l'état

    if (this.state.hasError) {
      // Vous pouvez rendre un fallback UI personnalisé ici
      return (
        <div>
          <h1>Oups ! Quelque chose s'est mal passé.</h1>
          <details style={{ whiteSpace: 'pre-wrap' }}>
            {error && error.toString()}  {/* Utilisation de `error` ici */}
            <br />
            {info ? info.componentStack : null} {/* Vérifiez que `info` existe */}
          </details>
        </div>
      );
    }

    return this.props.children;  // Retourner les enfants si pas d'erreur
  }
}

export default ErrorBoundary;




