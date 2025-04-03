<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Responsable extends Model
{
    use HasFactory;
    protected $table = 'responsables';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_responsable';

    // Définir les attributs qui peuvent être remplis via la masse d'assignation
    protected $fillable = [
        'id_user',      
        'id_admin',  
    ];
    
    // Relation avec le modèle Admin (une agence appartient à un administrateur)
    public function admin()
    {
        return $this->hasMany(Admins::class, 'id_admin');
    }

    // Relation avec le modèle Admin (une agence appartient à un administrateur)
    public function user()
    {
        return $this->hasMany(User::class, 'id_user');
    }
}
