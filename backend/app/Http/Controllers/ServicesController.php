<?php

namespace App\Http\Controllers;

use App\Models\Agence;
use App\Models\Services;
use App\Models\AgenceServices;
use App\Models\AdminServices;
use App\Models\Admins;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Validation\Rule;
use Illuminate\Support\Facades\Validator;

class ServicesController extends Controller
{

    public function index($id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        $serviceIds = AgenceServices::where('id_agence', $id)->pluck('id_service');

        $services = Services::whereIn('id_service',$serviceIds)->get();

        return response()->json(['message' => 'services Liste', 'services' => $services], 201); 
    }


    public function store(Request $request,$id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if ($user->role !== 'admin') {
            return response()->json(['message' => "Error Accee"], 403);
        }        
        // Validation des données 
        $validatedData = $request->validate([
            'nom_service' => 'required|string|max:255',
        ]);
        
        $admin = Admins::where('id_user',$user->id_user)->first();

        // Création de l'service
        $service = Services::create($validatedData);
        $agenceService = New AgenceServices();
        $agenceService->id_service = $service->id_service;
        $agenceService->id_agence = $id;
        $agenceService->save();

        $adminServices = New AdminServices();
        $adminServices->id_service = $service->id_service;
        $adminServices->id_admin = $admin->id_admin;
        $adminServices->save();

        return response()->json(['message' => 'Service créée avec succès', 'Service' => $agenceService], 201);  
    }

    public function show($id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        
        $service = Services::findOrFail($id);

        return response()->json(['message' => 'services Details', 'service' => $service], 201); 
    }


    public function update(Request $request,$id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if ($user->role !== 'admin') {
            return response()->json(['message' => "Error Accee"], 403);
        }        
        // Validation des données 
        $validatedData = $request->validate([
            'nom_service' => 'required|string|max:255',
        ]);
        $service = Services::findOrFail($id);
        
        $data = $request->only(['nom_service']);
        $service->update($data);

        return response()->json(['message' => 'service update', 'service' => $service], 201);
    }


    public function destroy($id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        if ($user->role !== 'admin') {
            return response()->json(['message' => "Error Accee"], 403);
        }

        $service = Services::findOrFail($id);
        $service->delete();

        return response()->json(['message' => 'service supprimée']);
    }

    public function getAgencyServices($id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }

        $agences = Agence::findOrFail($id);
        $services = Services::whereIn('id_service', function ($query) use ($id) {
            $query->select('id_service')
                  ->from('agence_service')
                  ->where('id_agence', $id);
        })->get();
        return response()->json(['message' => 'Agences show', 'services' => $services], 201);
    }
}
