<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Admins extends Model
{
    use HasFactory;
    protected $table = 'admins';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_admin';

    // Définir les attributs qui peuvent être remplis via la masse d'assignation
    protected $fillable = [
        'id_user'
    ];

    // Relation avec le modèle Admin (une agence appartient à un administrateur)
    public function user()
    {
        return $this->hasMany(User::class, 'id_admin');
    }

    public function etablissement()
    {
        return $this->belongsTo(Etablissement::class, 'id_etabl');
    }
}
