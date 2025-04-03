<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Agence extends Model
{
    // Spécifie le nom de la table associée
    protected $table = 'agences';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_agence';

    // Définir les attributs qui peuvent être remplis via la masse d'assignation
    protected $fillable = [
        'nom_agence', 'adresse', 'ville', 'latitude', 'longitude', 'horaires', 'id_etabl', 'id_admin'
    ];

    // Relation avec le modèle Etablissement (une agence appartient à un établissement)
    public function etablissement()
    {
        return $this->belongsTo(Etablissement::class, 'id_etabl', 'id_etabl');
    }

    // Relation avec le modèle Admin (une agence appartient à un administrateur)
    public function admin()
    {
        return $this->belongsTo(Admin::class, 'id_admin', 'id_admin');
    }

    public function agents()
    {
        return $this->hasMany(Agent::class, 'id_agence');
    }
}
