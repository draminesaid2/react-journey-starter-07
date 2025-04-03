<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateCitoyensTable extends Migration
{
    public function up()
    {
        Schema::create('citoyens', function (Blueprint $table) {
            $table->id();
            $table->string('token')->unique();
            $table->string('identifiant')->unique();
            $table->timestamps();
        });
    }
    public function down()
    {
        Schema::dropIfExists('citoyens');
    }
};
