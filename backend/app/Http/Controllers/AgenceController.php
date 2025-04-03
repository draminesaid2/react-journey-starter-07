<?php

namespace App\Http\Controllers;

use App\Models\Agence;
use App\Models\Service;
use App\Models\Admins;
use App\Models\User;
use App\Models\AgenceAdmin;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Validation\Rule;
use Illuminate\Support\Facades\Validator;

class AgenceController extends Controller
{

    // Vérifier si l'utilisateur est administrateur
    private function isAdmin()
    {
        $user = Auth::user();
        return $user && $user->role === 'admin';
    }

    // Créer une agence (accessible uniquement par l'admin)
    public function create(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if ($user->role !== 'admin') {
            return response()->json(['message' => "Error Accee"], 403);
        }
        $admin = Admins::where('id_user',$user->id_user)->first();
        
        if (!$this->isAdmin()) {
            return response()->json(['error' => 'Accès non autorisé'], 403);
        }

        // Validation des données avec vérification que l'id_user appartient bien à un admin
        $validatedData = $request->validate([
            'nom_agence' => 'required|string|max:255',
            'adresse' => 'required|string|max:255',
            'ville' => 'required|string|max:255',
            'latitude' => 'required|numeric',
            'longitude' => 'required|numeric',
            'horaires' => 'required|string|max:255',
            'id_etabl' => 'required|exists:etablissements,id_etabl',
        ]);
        
        $validatedData['id_admin'] = $admin->id_admin;
        // Création de l'agence
        $agence = Agence::create($validatedData);

        $agenceAdmin = New AgenceAdmin();
        $agenceAdmin->id_admin = $admin->id_admin;
        $agenceAdmin->id_agence = $agence->id_agence;
        $agenceAdmin->save();

        return response()->json(['message' => 'Agence créée avec succès', 'agence' => $agence], 201);
    }

    
    // index function
    public function index(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        
        $agences = Agence::all();

        return response()->json(['message' => 'Agences Liste', 'agences' => $agences], 201);
    }

    // villes
    public function villes()
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        
        $villes = Agence::distinct('ville')->pluck('ville');

        return response()->json(['message' => 'Ville Liste', 'villes' => $villes], 201);
    }

    public function getAgencyByVille($ville)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        
        $agences = Agence::where('ville',$ville)->get();

        return response()->json(['message' => 'Agences Liste', 'agences' => $agences], 201);
    }
    public function search($q)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        
        $agences = Agence::where(function ($query) use ($q) {
            $query->where('nom_agence', 'LIKE', '%' . $q . '%') 
            ->orWhere('ville', 'LIKE', '%' . $q . '%');
        })
        ->get();

        return response()->json(['message' => 'Agences Liste', 'agences' => $agences], 201);
    }
    
    // show agence by id
    public function show($id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if ($user->role !== 'admin') {
            return response()->json(['message' => "Error Accee"], 403);
        }
        $agences = Agence::findOrFail($id);

        return response()->json(['message' => 'Agences show', 'agences' => $agences], 201);
    }

    //update agence
    public function update(Request $request,$id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if ($user->role !== 'admin') {
            return response()->json(['message' => "Error Accee"], 403);
        }
        $validator = Validator::make($request->all(), [
            'nom_agence' => 'nullable|string|max:255',
            'adresse' => 'nullable|string|max:255',
            'ville' => 'nullable|string|max:255',
            'latitude' => 'nullable|string',
            'longitude' => 'nullable|string',
            'horaires' => 'nullable|string',
        ]);

        // Check if validation fails
        if ($validator->fails()) {
            return response()->json(['errors' => $validator->errors()], 422);
        }

        $agence = Agence::findOrFail($id);

        $data = $request->only(['nom_agence', 'adresse', 'ville', 'latitude', 'longitude', 'horaires']);
        $agence->update($data);

        return response()->json(['message' => 'Agences show', 'agences' => $agence], 201);
    }
    // Supprimer une agence (accessible uniquement par l'admin)
    public function destroy($id)
    {
        if (!$this->isAdmin()) {
            return response()->json(['error' => 'Accès non autorisé'], 403);
        }

        $agence = Agence::findOrFail($id);
        $agence->delete();

        return response()->json(['message' => 'Agence supprimée']);
    }

    // Créer un service pour une agence (accessible uniquement par l'admin)
    public function createService(Request $request, $agence_id)
    {
        if (!$this->isAdmin()) {
            return response()->json(['error' => 'Accès non autorisé'], 403);
        }

        $request->validate([
            'nom_service' => 'required|string|max:255',
        ]);

        $agence = Agence::findOrFail($agence_id);

        $service = Service::create([
            'nom_service' => $request->nom_service,
            'id_agence' => $agence->id_agence,
        ]);

        return response()->json($service, 201);
    }

    // Récupérer tous les services d'une agence
    public function services($agence_id)
    {
        $services = Service::where('id_agence', $agence_id)->get();
        return response()->json($services);
    }

    // Récupérer un service d'une agence
    public function showService($agence_id, $service_id)
    {
        $service = Service::where('id_agence', $agence_id)->findOrFail($service_id);
        return response()->json($service);
    }

    // Mettre à jour un service (accessible uniquement par l'admin)
    public function updateService(Request $request, $agence_id, $service_id)
    {
        if (!$this->isAdmin()) {
            return response()->json(['error' => 'Accès non autorisé'], 403);
        }

        $service = Service::where('id_agence', $agence_id)->findOrFail($service_id);

        $request->validate([
            'nom_service' => 'required|string|max:255',
        ]);

        $service->update(['nom_service' => $request->nom_service]);

        return response()->json($service);
    }

    // Supprimer un service (accessible uniquement par l'admin)
    public function destroyService($agence_id, $service_id)
    {
        if (!$this->isAdmin()) {
            return response()->json(['error' => 'Accès non autorisé'], 403);
        }

        $service = Service::where('id_agence', $agence_id)->findOrFail($service_id);
        $service->delete();

        return response()->json(['message' => 'Service supprimé']);
    }
}



