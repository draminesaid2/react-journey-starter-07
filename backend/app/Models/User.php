<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Notifications\Notifiable;
use Laravel\Sanctum\HasApiTokens;

class User extends Authenticatable
{
    use HasApiTokens, HasFactory, Notifiable;

    protected $table = 'users'; // Spécifie que la table s'appelle 'users', pas 'user'
    protected $primaryKey = 'id_user'; // Clé primaire est 'id_user'
    public $incrementing = true;
    protected $keyType = 'int';

    const ROLE_ADMIN = 'admin';
    const ROLE_AGENT = 'agent';
    const ROLE_RESPONSABLE = 'responsable';
    const ROLE_SUPADMIN = 'supadmin';

    protected $fillable = [
        'nom_user',        // Correspond à 'name'
        'prenom_user',     // Nouveau champ pour le prénom
        'email_user',      // Correspond à 'email'
        'password',
        'role',
    ]; 

    protected $hidden = [
        'password',
        'remember_token',
    ];

    protected $casts = [
        'email_verified_at' => 'datetime',
    ];

    // Méthodes pour vérifier les rôles
    public function isAdmin()
    {
        return $this->role === self::ROLE_ADMIN;
    }

    public function isAgent()
    {
        return $this->role === self::ROLE_AGENT;
    }

    public function isResponsable()
    {
        return $this->role === self::ROLE_RESPONSABLE;
    }

    public function isSupadmin()
    {
        return $this->role === self::ROLE_SUPADMIN;
    }

    public function getAuthIdentifierName()
    {
        return 'id_user'; // S'assure que Laravel utilise 'id_user' pour l'authentification
    }
}



   
