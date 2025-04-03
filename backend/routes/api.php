<?php
use Laravel\Sanctum\Sanctum;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\CitoyenController;
use App\Http\Controllers\AuthenController;
use App\Http\Controllers\EtablissementController;
use App\Http\Controllers\UserController;
use App\Http\Controllers\AgenceController;
use App\Http\Controllers\ServicesController;
use Laravel\Sanctum\Http\Controllers\CsrfCookieController;
use App\Http\Controllers\FilesController;
use App\Http\Controllers\TicketController;

// Route pour obtenir un cookie CSRF (important pour les requêtes d'authentification)
Route::get('/sanctum/csrf-cookie', [CsrfCookieController::class, 'show']);

///http://127.0.0.1:8000/api/login (terminé)
Route::post('/login', [AuthenController::class, 'login'])->name('login');

///http://127.0.0.1:8000/api/logout (terminé)
Route::middleware('auth:sanctum')->post('logout', [AuthenController::class, 'logout']);

Route::middleware('auth:sanctum')->prefix('supadmin')->group(function () {
    Route::prefix('/etablissement')->group(function () { 
        /// 1* http://localhost:8000/api/supadmin/etablissement (get) (afficher tous les établissements) (terminé)
        Route::get('/', [EtablissementController::class, 'index']);
        
        /// 2* http://localhost:8000/api/supadmin/etablissement/1 (récupérer les informations d'un établissement par id) (terminé)
        Route::get('/{id}', [EtablissementController::class, 'show']);
        
        /// 3* http://localhost:8000/api/supadmin/etablissement (post) (Création d'un établissement) (terminé)
        Route::post('/', [EtablissementController::class, 'store']);

        /// 4* http://localhost:8000/api/supadmin/etablissement/2 (put) (Mise à jour d'un établissement) (terminé)
        Route::put('/{id}', [EtablissementController::class, 'update']);

        /// 5* http://localhost:8000/api/supadmin/etablissement/2 (delete) (Suppression d'un établissement) (terminé)
        Route::delete('/{id}', [EtablissementController::class, 'destroy']);

        /// 6* http://localhost:8000/api/supadmin/etablissement/search/ben arous (Recherche d'un établissement) (terminé)
        Route::get('/search/{q}', [EtablissementController::class, 'search']);
    });

    Route::prefix('/users')->group(function () { 

        /// 0* http://127.0.0.1:8000/api/supadmin/users/register (créer un nouveau utilisateur (admin..) (terminé)
        Route::post('register', [AuthenController::class, 'register']);

        /// 1* http://127.0.0.1:8000/api/supadmin/users (Récupérer la liste des utilisateurs) (terminé)
        Route::get('/', [UserController::class, 'index']);

        /// 2* http://127.0.0.1:8000/api/supadmin/users/5 (Récupérer un utilisateur par id_user) (terminé)
        Route::get('/{id}', [UserController::class, 'show']);

        /// 3* http://127.0.0.1:8000/api/supadmin/users/5 (Mettre à jour un utilisateur) (terminé)
        Route::put('/{id_user}', [UserController::class, 'update']);

        /// 4* http://127.0.0.1:8000/api/supadmin/users/5 (Supprimer un utilisateur) (terminé)
        Route::delete('/{id}', [UserController::class, 'destroy']);

        
        /// 5* http://127.0.0.1:8000/api/supadmin/users/search/{q} (terminé)
        Route::get('/search/{q}', [UserController::class, 'search']); 
    }); 
});

Route::middleware('auth:sanctum')->prefix('admin')->group(function (){
    Route::prefix('/users')->group(function () {
        /// http://127.0.0.1:8000/api/admin/users/register (créer un nouveau utilisateur (admin..) (terminé)
        Route::post('register', [AuthenController::class, 'register']);
            
        /// 1* http://127.0.0.1:8000/api/admin/users (Récupérer la liste des utilisateurs) (terminé)
        Route::get('/', [UserController::class, 'index']);

        /// 2* http://127.0.0.1:8000/api/admin/users/5 (Récupérer un utilisateur par ID) (terminé)
        Route::get('/{id}', [UserController::class, 'show']);
            
        /// 3* http://127.0.0.1:8000/api/admin/users/5 (Mettre à jour un utilisateur) (terminé)
        Route::put('/{id_user}', [UserController::class, 'update']);

        /// 4* http://127.0.0.1:8000/api/admin/users/5 (Supprimer un utilisateur) (terminé)
        Route::delete('/{id}', [UserController::class, 'destroy']);

         /// 5* http://127.0.0.1:8000/api/admin/users/search/{q} (terminé)
         Route::get('/search/{q}', [UserController::class, 'search']);
    });
    Route::prefix('/agences')->group(function(){
        /// 0* http://127.0.0.1:8000/api/admin/agences Créer une agence (terminé)
        Route::post('/', [AgenceController::class, 'create']);

        /// 1* http://127.0.0.1:8000/api/admin/agences Récupérer toutes les agences (terminé)
        Route::get('/', [AgenceController::class, 'index']);

        /// 2* http://127.0.0.1:8000/api/admin/agences/{id} Récupérer une agence par ID (terminé)
        Route::get('/{id}', [AgenceController::class, 'show']);

        /// 3* http://127.0.0.1:8000/api/admin/agences/{id} Mettre à jour une agence (terminé)
        Route::put('/{id}', [AgenceController::class, 'update']);

        /// 4* http://127.0.0.1:8000/api/admin/agences/{id} Supprimer une agence (terminé)
        Route::delete('/{id}', [AgenceController::class, 'destroy']);

        /// 5* http://127.0.0.1:8000/api/admin/agences/search/{q} (terminé)
        Route::get('/search/{q}', [AgenceController::class, 'search']);

        Route::prefix('/services')->group(function(){
            /// 0* http://127.0.0.1:8000/api/admin/agences/services/{id} Récupérer toutes les services (1 agence)(id_agence)
            Route::get('/{id}', [ServicesController::class, 'index']);
            
            /// 1* http://127.0.0.1:8000/api/admin/agences/services/{id} (id_agence) (création)
            Route::post('/{id}', [ServicesController::class, 'store']);

            /// 2* http://127.0.0.1:8000/api/admin/agences/services/{id} 
            Route::put('/{id}', [ServicesController::class, 'update']);

            // 3* http://127.0.0.1:8000/api/admin/agences/services/{id} 
            Route::delete('/{id}', [ServicesController::class, 'destroy']);

            // http://127.0.0.1:8000/api/admin/agences/services/details/{id_service} (Récupérer un service spécifique d'une agence)
            Route::get('/details/{id}', [ServicesController::class, 'show']);
            
        });

    });
});

Route::middleware('auth:sanctum')->prefix('responsable')->group( function (){
    Route::prefix('/users')->group(function (){
        /// http://127.0.0.1:8000/api/responsable/users/register (créer un nouveau utilisateur (terminé)
        Route::post('register', [AuthenController::class, 'register']);

        /// 1* http://127.0.0.1:8000/api/reponsable/users (Récupérer la liste des utilisateurs) (terminé)
        Route::get('/', [UserController::class, 'index']);

        /// 2* http://127.0.0.1:8000/api/reponsable/users/5 (Récupérer un utilisateur par ID) (terminé)
        Route::get('/{id}', [UserController::class, 'show']);

        /// 3* http://127.0.0.1:8000/api/reponsable/users/5 (Mettre à jour un utilisateur) (terminé)
        Route::put('/{id_user}', [UserController::class, 'update']);

        /// 4* http://127.0.0.1:8000/api/reponsable/users/5 (Supprimer un utilisateur) (terminé)
        Route::delete('/{id}', [UserController::class, 'destroy']);

         /// 5* http://127.0.0.1:8000/api/admin/users/search/{q} (terminé)
         Route::get('/search/{q}', [UserController::class, 'search']);
    }); 
});

Route::middleware('auth:sanctum')->prefix('agent')->group(function (){
    Route::prefix('/files')->group(function (){
        /// http://127.0.0.1:8000/api/agent/files/creat (créer un nouveau file 
        Route::post('/creat', [FilesController::class, 'creat']);

        /// 1* http://127.0.0.1:8000/api/agent/files/update_status/{id} (mise a jour status (teminé/ en attente) file d attent) (terminé)
        Route::put('/update_status/{id}', [FilesController::class, 'updateStatus']);

        /// 2* http://127.0.0.1:8000/api/agent/files/5 (Récupérer un file par ID) 
        Route::get('/{id}', [FilesController::class, 'show']);

        /// 3* http://127.0.0.1:8000/api/agent/files/5 (Mettre à jour un file) 
        Route::put('/{id}', [FilesController::class, 'update']);

        /// 4* http://127.0.0.1:8000/api/agent/files/5 (Supprimer un file) (terminé)
        Route::delete('/{id}', [FilesController::class, 'destroy']);

        /// 5* http://127.0.0.1:8000/api/agent/files/nunmber/4 (incrémentation par 1)(terminé)
        Route::post('/nunmber/{id}', [FilesController::class, 'countFile']);

        /// 6* http://127.0.0.1:8000/api/agent/files/agence (Récupérer un file par Agence)
        /// Utilisons explicitement GET comme standard REST pour les opérations de lecture
        Route::get('/agence', [FilesController::class, 'showByAgency']);
    });  
});

Route::middleware('auth:sanctum')->prefix('citoyen')->group(function (){
    Route::prefix('/agences')->group(function (){
        /// 0* http://127.0.0.1:8000/api/citoyen/agences/villes Récupérer toutes les villes
        Route::get('/villes', [AgenceController::class, 'villes']);

        /// 1* http://127.0.0.1:8000/api/citoyen/agences Récupérer toutes les agences (pour maps marker)
        Route::get('/', [AgenceController::class, 'index']);
        
        /// 2* http://127.0.0.1:8000/api/citoyen/agences/ville/{ville} Récupérer toutes les agences par ville
        Route::get('/ville/{ville}', [AgenceController::class, 'getAgencyByVille']);

        /// 3* http://127.0.0.1:8000/api/citoyen/agences/services/{id} Récupérer toutes les services (1 agence)
        Route::get('/services/{id}', [ServicesController::class, 'getAgencyServices']);

    });

    Route::prefix('/etablissement')->group(function (){ 
        /// 1* http://localhost:8000/api/citoyen/etablissement (get) (afficher tous les établissements)
        Route::get('/', [EtablissementController::class, 'index']);

        /// 2* http://localhost:8000/api/citoyen/etablissement/1 (récupérer les informations d'un établissement par id)
        Route::get('/{id}', [EtablissementController::class, 'show']);

        /// 3* http://localhost:8000/api/citoyen/etablissement/search/ben arous (Recherche d'un établissement)
        Route::get('/search/{q}', [EtablissementController::class, 'search']);
    });

    Route::prefix('/ticket')->group(function (){
        /// 0* http://localhost:8000/api/citoyen/ticket/add (post) (creat ticket)
        Route::post('/add', [TicketController::class, 'creat']);

        /// 1* http://localhost:8000/api/citoyen/ticket/{id} (delete) (delete ticket)
        Route::delete('/{id}', [TicketController::class, 'destroy']);

    });

});
Route::post('/ticket/add', [TicketController::class, 'createst']);

/// http://127.0.0.1:8000/api/creer-citoyen (id aléatoire)
Route::post('/creer-citoyen', [CitoyenController::class, 'creerCitoyen']);
