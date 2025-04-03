<?php

namespace App\Http\Controllers;
use Illuminate\Http\Request;
use App\Models\Etablissement;  // Importation du modèle Etablissement
use Illuminate\Support\Facades\Auth;

class EtablissementController extends Controller {
    public function index() 
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        // Utilisation du nom de classe correctement avec une majuscule
        return response()->json(Etablissement::all());
    }

    public function show($id) 
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        // Utilisation du nom de classe correctement avec une majuscule
        return response()->json(Etablissement::findOrFail($id));
    }

     //  Création d'un établissement 
     public function store(Request $request)
     {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if($user->role !== 'supadmin'){
            return response()->json(['message' => "Error Access"], 403);
        }
         $validated = $request->validate([
             'nom_etabl' => 'required|string|max:255',
             'adresse_etabl' => 'required|string',
             
         ]);
 
         $etablissement = Etablissement::create($validated);
 
         return response()->json($etablissement, 201);
     }

     //Mise à jour d'un établissement
    public function update(Request $request, $id) 
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if($user->role !== 'supadmin'){
            return response()->json(['message' => "Error Access"], 403);
        }
        $etablissement = Etablissement::findOrFail($id);
        $validated = $request->validate([
            'nom_etabl' => 'required|string|max:255',
            'adresse_etabl' => 'required|string',
            
        ]);
    
        $etablissement->update($validated);
    
        return response()->json($etablissement, 200);
    } 

//Suppression d'un établissement 
    public function destroy($id) {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if($user->role !== 'supadmin'){
            return response()->json(['message' => "Error Access"], 403);
        }

        $etablissement = Etablissement::findOrFail($id);
        try {
            $etablissement->delete();
        }  catch (\Exception $e) {
            \Log::error('Erreur lors de la suppr Etab', ['error' => $e->getMessage()]);
            return response()->json([
                'message' => 'Erreur lors de la suppr du Etab',
                'error' => $e->getMessage(),
            ], 500);
        }
        
    
        return response()->json(['message' => 'Établissement supprimé avec succès'], 200);
    }

    //recherche d'un établissement
    public function search($q)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        $etablissements = Etablissement::where('nom_etabl', 'LIKE', "%$q%")
                                       ->orWhere('adresse_etabl', 'LIKE', "%$q%")
                                       ->get();

        return response()->json($etablissements, 200);
    }
    
}

