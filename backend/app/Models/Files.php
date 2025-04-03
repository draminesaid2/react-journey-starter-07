<?php
namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Files extends Model
{
    use HasFactory;
    protected $table = 'files';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_file';

    // Définir les attributs qui peuvent être remplis via la masse d'assignation
    protected $fillable = [
        'nom_file',      
        'id_service',  
        'id_agent',
        'id_agence',
        'status',
        'date',
        'numero',
    ];
    
    public function service()
    {
        return $this->belongsTo(Services::class, 'id_service', 'id_service');
    }

    // An Agent belongs to one User
    public function agent()
    {
        return $this->belongsTo(Agent::class, 'id_agent', 'id_agent');
    }

    // An Agent belongs to one Agence (assuming you have an Agence model)
    public function agence()
    {
        return $this->belongsTo(Agence::class, 'id_agence', 'id_agence');
    }
}
