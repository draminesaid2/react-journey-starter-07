<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class AgenceServices extends Model
{
    // Spécifie le nom de la table associée
    protected $table = 'agence_service';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_agence,id_service';


    // Relation avec la table 'agences' (un établissement peut avoir plusieurs agences)
    public function agences()
    {
        return $this->hasMany(Agence::class, 'id_agence');
    }

    public function services()
    {
        return $this->hasMany(Services::class, 'id_service');
    } 

}
