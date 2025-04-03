<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Laravel\Sanctum\HasApiTokens;

class Citoyen extends Model
{
    use HasFactory;
    use HasApiTokens;

    //tableau Indique les champs qui peuvent être remplis (mass assignable)
    protected $fillable = ['identifiant'];

    // Si tu veux que le modèle ne gère pas les timestamps (created_at, updated_at)
    // tu peux le faire en ajoutant cette ligne
    public $timestamps = true;
}

