<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Etablissement extends Model
{
    use HasFactory;


    protected $table = 'etablissements';
    protected $primaryKey = 'id_etabl';

    protected $fillable = [
        'nom_etabl',
        'adresse_etabl',
        'created_at',
        'updated_at',
    ];

    // Relation avec la table 'agences' (un établissement peut avoir plusieurs agences)
    public function agences()
    {
        return $this->hasMany(Agence::class, 'id_etabl');
    }

    public function admins()
    {
        return $this->hasMany(Admin::class, 'id_etabl');
    }
}