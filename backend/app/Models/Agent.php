<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Agent extends Model
{
    use HasFactory;
    protected $table = 'agents';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_agent';

    // Définir les attributs qui peuvent être remplis via la masse d'assignation
    protected $fillable = [
        'id_user',      
        'id_responsable',  
        'id_agence',
    ];
    
    public function responsable()
    {
        return $this->belongsTo(Responsable::class, 'id_responsable');
    }

    // An Agent belongs to one User
    public function user()
    {
        return $this->belongsTo(User::class, 'id_user');
    }

    // An Agent belongs to one Agence (assuming you have an Agence model)
    public function agence()
    {
        return $this->belongsTo(Agence::class, 'id_agence');
    }

}
