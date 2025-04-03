<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class AgenceAdmin extends Model
{
    // Spécifie le nom de la table associée
    protected $table = 'admin_agence';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_agence,id_admin';


    // Relation avec la table 'agences' (un établissement peut avoir plusieurs agences)
    public function agences()
    {
        return $this->hasMany(Agence::class, 'id_agence');
    }

    public function admin()
    {
        return $this->hasMany(Admins::class, 'id_admin');
    } 

}
