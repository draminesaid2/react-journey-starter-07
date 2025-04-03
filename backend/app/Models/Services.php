<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Services extends Model
{
    use HasFactory;

    protected $primaryKey = 'id_service';

    protected $fillable = [
        'nom_service',      
    ];

}
