<?php
namespace App\Http\Controllers;

use App\Models\Agence;
use App\Models\Services;
use App\Models\AgenceServices;
use App\Models\AdminServices;
use App\Models\Admins;
use App\Models\Files;
use App\Models\Agent;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Validation\Rule;
use Illuminate\Support\Facades\Validator;
use Carbon\Carbon;
/**
 * Contrôleur pour la gestion des files d'attente.
 * Ce contrôleur gère la création, mise à jour, suppression et affichage des files d'attente.
 */
class FilesController extends Controller
{
    /**
     * Crée une nouvelle file d'attente.
     * 
     * Cette méthode permet à un agent de créer une file d'attente pour un service spécifique
     * ou d'incrémenter une file d'attente existante.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int|null  $id
     * @return \Illuminate\Http\Response
     */
    public function creat(Request $request, $id = null)
    {
        // Vérification de l'authentification
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Erreur d'authentification"], 401);
        }
        
        // Vérification du rôle de l'utilisateur
        if ($user->role !== 'agent') {
            return response()->json(['message' => "Erreur d'accès: seuls les agents peuvent créer des files d'attente"], 403);
        }
        
        // Validation des données 
        $validatedData = $request->validate([
            'id_service' => 'required|string|max:255',
        ]);
   
        // Récupération des informations de l'agent
        $agent = Agent::where('id_user', $user->id_user)->first();
        if (!$agent) {
            return response()->json(['message' => "Agent non trouvé"], 404);
        }

        // Récupération des informations du service et de l'agence
        $serviceInfo = Services::findOrFail($request->id_service);
        $agenceInfo = Agence::findOrFail($agent->id_agence);
        $date = now()->format('Y-m-d');
        $nom_file = $serviceInfo->nom_service.'-'.$agenceInfo->nom_agence.'-'.$date;
        
        // Vérification si une file existe déjà pour ce service dans cette agence
        $verify_file = Files::whereIn('status', ['ouvert', 'pause'])
                      ->where('id_agence', $agent->id_agence)
                      ->where('id_service', $request->id_service)
                      ->first();

        if ($verify_file) {
            // Si la file existe, on l'ouvre et on incrémente le numéro
            $verify_file->status = 'ouvert';
            $verify_file->numero += 1;
            $verify_file->save();
            return response()->json(['message' => 'File déjà existante, numéro incrémenté', 'file' => $verify_file], 201); 
        } else {
            // Sinon, on crée une nouvelle file
            $file = new Files();
            $file->nom_file = $nom_file;
            $file->id_service = $request->id_service;
            $file->id_agent = $agent->id_agent;
            $file->id_agence = $agent->id_agence;
            $file->status = "ouvert";
            $file->numero = 1;
            $file->date = $date;
            $file->save();
    
            return response()->json(['message' => 'File créée avec succès', 'file' => $file], 201); 
        }
    }

    /**
     * Met à jour le statut d'une file d'attente (ouvert, pause, fermer).
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function updateStatus(Request $request, $id)
    {
        // Vérification de l'authentification
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Erreur d'authentification"], 401);
        }
        
        // Vérification du rôle de l'utilisateur
        if ($user->role !== 'agent') {
            return response()->json(['message' => "Erreur d'accès: vous n'avez pas les droits nécessaires"], 403);
        }
        
        // Validation des données 
        $validatedData = $request->validate([
            'status' => 'required|string|in:ouvert,pause,fermer',
        ]);
   
        // Récupération des informations de l'agent
        $agent = Agent::where('id_user', $user->id_user)->first();
        if (!$agent) {
            return response()->json(['message' => "Agent non trouvé"], 404);
        }

        // Récupération de la file d'attente
        $file = Files::where('id_file', $id)->where('id_agence', $agent->id_agence)->first();
        
        if (!$file) {
            return response()->json(['message' => "File non trouvée"], 404);
        }
        
        if ($file) {
            // Vérification si la file est déjà fermée
            if ($file->status == 'fermer') {
                return response()->json(['message' => "File d'attente déjà fermée", 'file' => $file], 406);
            }
            
            // Mise à jour du statut
            $file->status = $request->status;
            $file->save();
            return response()->json(['message' => "Statut mis à jour avec succès", 'file' => $file], 200);
        }

        return response()->json(['message' => "Erreur d'accès"], 403);
    }

    /**
     * Affiche les détails d'une file d'attente spécifique.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function show($id)
    {
        // Vérification de l'authentification
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Erreur d'authentification"], 401);
        }
        
        // Récupération de la file avec ses relations
        $file = Files::findOrFail($id);
        $file->load('service', 'agent', 'agence');

        return response()->json(['message' => 'Détails de la file', 'file' => $file], 200); 
    }

    /**
     * Affiche les files d'attente de l'agence pour la journée en cours.
     * 
     * Méthode utilisant GET conformément au standard REST pour les opérations de lecture.
     *
     * @return \Illuminate\Http\Response
     */
    public function showByAgency()
    {
        // Vérification de l'authentification
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Erreur d'authentification"], 401);
        }
        
        // Vérification du rôle de l'utilisateur
        if ($user->role !== 'agent') {
            return response()->json(['message' => "Erreur d'accès: vous n'avez pas les droits nécessaires"], 403);
        }

        $date = now()->format('Y-m-d');
        
        // Récupération des informations de l'agent
        $agent = Agent::where('id_user', $user->id_user)->first();
        if (!$agent) {
            return response()->json(['message' => "Agent non trouvé"], 404);
        }
        
        // Récupération des files de l'agence pour aujourd'hui
        $files = Files::where('id_agence', $agent->id_agence)
                      ->where('date', $date)
                      ->with(['service', 'agent', 'agence'])
                      ->get();

        return response()->json(['message' => "Détails des files de l'agence", 'files' => $files], 200); 
    }
    
    /**
     * Met à jour les détails d'une file d'attente.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, $id)
    {
        // Vérification de l'authentification
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Erreur d'authentification"], 401);
        }
        
        // Vérification du rôle de l'utilisateur
        if ($user->role !== 'agent') {
            return response()->json(['message' => "Erreur d'accès: vous n'avez pas les droits nécessaires"], 403);
        }        
        
        // Validation des données 
        $validatedData = $request->validate([
            'nom_file' => 'required|string|max:255',
            'date' => 'required|string|max:255',
        ]);
        
        // Récupération des informations de l'agent
        $agent = Agent::where('id_user', $user->id_user)->first();
        if (!$agent) {
            return response()->json(['message' => "Agent non trouvé"], 404);
        }

        // Récupération de la file d'attente
        $file = Files::where('id_file', $id)->where('id_agence', $agent->id_agence)->first();
        
        if (!$file) {
            return response()->json(['message' => "File non trouvée"], 404);
        }
        
        if ($file) {
            // Mise à jour des informations
            $file->nom_file = $request->nom_file;
            $file->date = $request->date;
            $file->save();

            return response()->json(['message' => 'File mise à jour avec succès', 'file' => $file], 200); 
        }

        return response()->json(['message' => "Erreur d'accès"], 403);
    }

    /**
     * Supprime une file d'attente.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function destroy($id)
    {
        // Vérification de l'authentification
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Erreur d'authentification"], 401);
        }
        
        // Vérification du rôle de l'utilisateur
        if ($user->role !== 'agent') {
            return response()->json(['message' => "Erreur d'accès: vous n'avez pas les droits nécessaires"], 403);
        }
    
        // Récupération des informations de l'agent
        $agent = Agent::where('id_user', $user->id_user)->first();
        if (!$agent) {
            return response()->json(['message' => "Agent non trouvé"], 404);
        }

        // Récupération de la file d'attente
        $file = Files::where('id_file', $id)->where('id_agence', $agent->id_agence)->first();
        
        if (!$file) {
            return response()->json(['message' => "File non trouvée"], 404);
        }
        
        if ($file) {
            // Suppression de la file
            $file->delete();
            return response()->json(['message' => 'File supprimée avec succès'], 200);
        }
        
        return response()->json(['message' => "Erreur d'accès"], 403);
    }

    /**
     * Incrémente le numéro de la file d'attente.
     *
     * @param  int  $id
     * @return \Illuminate\Http\Response
     */
    public function countFile($id)
    {
        // Vérification de l'authentification
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Erreur d'authentification"], 401);
        }
        
        // Vérification du rôle de l'utilisateur
        if ($user->role !== 'agent') {
            return response()->json(['message' => "Erreur d'accès: vous n'avez pas les droits nécessaires"], 403);
        } 

        // Récupération de la file d'attente
        $file = Files::findOrFail($id);
        
        // Vérification du statut de la file
        if ($file->status == 'ouvert') {
            // Incrémentation du numéro
            $file->numero += 1;
            $file->save();
            return response()->json(['message' => 'Numéro de file incrémenté avec succès', 'file' => $file], 200); 
        } else {
            return response()->json(['message' => "Impossible d'incrémenter: la file d'attente doit être ouverte", 'file' => $file], 400); 
        }
    }
}
