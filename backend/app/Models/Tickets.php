<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Tickets extends Model
{
    use HasFactory;
    protected $table = 'tickets';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_ticket';

    // Définir les attributs qui peuvent être remplis via la masse d'assignation
    protected $fillable = [
        'id_identifiant',      
        'id_file',  
        'numero',
    ];
    
    public function citoyen()
    {
        return $this->belongsTo(Citoyen::class, 'id_identifiant');
    }

    // An Agent belongs to one User
    public function file()
    {
        return $this->belongsTo(Files::class, 'id_file');
    }

}
