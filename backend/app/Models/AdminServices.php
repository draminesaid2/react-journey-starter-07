<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class AdminServices extends Model
{
    // Spécifie le nom de la table associée
    protected $table = 'admin_service';

    // Spécifie la clé primaire
    protected $primaryKey = 'id_admin,id_service';


    // Relation avec la table 'admin' (un service peut avoir plusieurs admin)
    public function admin()
    {
        return $this->hasMany(Admins::class, 'id_admin');
    }

    public function services()
    {
        return $this->hasMany(Services::class, 'id_service');
    } 

}
