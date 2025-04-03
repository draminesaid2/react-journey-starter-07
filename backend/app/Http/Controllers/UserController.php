<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use app\Models\User;
use Illuminate\Support\Facades\Validator;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Hash;

class UserController extends Controller
{


    // 1. Récupérer la liste des utilisateurs
   
    public function index()
    {

        $user = Auth::user();
      
        if (!$user) {
            return response()->json(['error' => 'Utilisateur non authentifié'], 401);
        }
    
        // SupAdmin : voir admin
        if ($user->isSupAdmin()) {
            $users = User::where('role', 'admin')->get();
            $message = "Liste des admis.";
        }
        // Admin : voit les responsables 
        elseif ($user->isAdmin()) {
            $users = User::where('role', 'responsable')->get();
            $message = "Liste des responsables.";
        }
        // Responsable : voit  les agents
        elseif ($user->isResponsable()) {
            $users = User::where('role', 'agent')->get();
            $message = "Liste des agents.";
        }
        // Agent : n'a pas le droit de voir les utilisateurs
        elseif ($user->role == 'agent') {
            return response()->json(['error' => 'Accès refusé'], 403);
        }
        // Cas par défaut (ne devrait jamais arriver)
        else {
            return response()->json(['error' => 'Accès refusé'], 403);
        }
    
        return response()->json([
            'message' => $message,
            'users' => $users
        ]);
    }
    
    
    

    // 2. Récupérer un utilisateur par son ID
    public function show($id)
    {
        $userAuth = Auth::user();
        if (!$userAuth) {
            return response()->json(['message' => "Error Auth"], 401);
        }

        $user = User::find($id);
        if (!$user) {
            return response()->json(['error' => 'Utilisateur non trouvé'], 404);
        }
    
        $authUser = Auth::user();
        
        // Define role-based permissions in a cleaner way
        $allowedRoles = [
            'supadmin' => ['admin'], // Can see all
            'admin' => ['responsable'],
            'responsable' => ['agent']
        ];
    
        // Get current user's role
        $authRole = strtolower($authUser->role ?? '');
  
        // Check if user has permission based on their role
        if (!isset($allowedRoles[$authRole]) || 
            !in_array($user->role, $allowedRoles[$authRole])) {
            return response()->json(['error' => 'Accès refusé'], 403);
        }
    
        return response()->json($user);
    }
    public function search($q)
    {
       
        $userAuth = Auth::user();
        if (!$userAuth) {
            return response()->json(['message' => "Error Auth"], 401);
        }

        $authUser = Auth::user();
        
        // Define role-based permissions in a cleaner way
        $allowedRoles = [
            'supadmin' => ['admin'], // Can see all
            'admin' => ['responsable'],
            'responsable' => ['agent']
        ];
        

        // Get current user's role
        $authRole = strtolower($authUser->role ?? '');
        
        $user = User::where(function ($query) use ($q) {
                        $query->where('nom_user', 'LIKE', '%' . $q . '%') 
                        ->orWhere('prenom_user', 'LIKE', '%' . $q . '%');
                    })
                    ->where('role',  $allowedRoles[$authRole])
                    ->get();
      
        return response()->json($user);
    }
    

    
    public function update(Request $request, $id)
    {
        $authUser = Auth::user();
        if (!$authUser) {
            return response()->json(['message' => "Error Auth"], 401);
        }

        // Trouver l'utilisateur par ID
        $user = User::findOrFail($id);
    
        // Valider les données envoyées
        $validator = Validator::make($request->all(), [
            'nom_user' => '|string|max:255',
            'prenom_user' => '|string|max:255',
            'email_user' => '|string|email|max:255|unique:users,email_user,' . $user->id_user . ',id_user',
            'password' => 'nullable|string|max:255',
        ]);
    
        if ($validator->fails()) {
            return response()->json(['error' => $validator->errors()], 400);
        }
    
        // Define role-based permissions in a cleaner way
        $allowedRoles = [
            'supadmin' => ['supadmin','admin'], // Can see all
            'admin' => ['responsable'],
            'responsable' => ['agent']
        ];
    
        // Get current user's role
        $authRole = strtolower($authUser->role ?? '');
  
        // Check if user has permission based on their role
        if (!isset($allowedRoles[$authRole]) || 
            !in_array($user->role, $allowedRoles[$authRole])) {
            return response()->json(['error' => 'Accès refusé'], 403);
        }
        
        $data = $request->only(['nom_user', 'prenom_user', 'email_user']);
        if ($request->filled('password')) {
            $data['password'] = Hash::make($request->password);
        }
        $user->update($data);
    
        return response()->json(['message' => 'Utilisateur mis à jour avec succès', 'user' => $user]);
    }
    

    // 4. Supprimer un utilisateur
    public function destroy($id)
    {
        $user = User::where('id_user', $id)->first();

        if (!$user) {
            return response()->json(['error' => 'Utilisateur non trouvé'], 404);
        }

        $authUser = Auth::user();

        // Contrôles des permissions
        if ($authUser->isSupAdmin()) {
          
            $user->delete();
        } elseif ($authUser->isAdmin()) {
            
            if (in_array($user->role, ['responsable', 'agent'])) {
                $user->delete();
            } else {
                return response()->json(['error' => 'Accès refusé'], 403);
            }
        } elseif ($authUser->isResponsable()) {
            
            if ($user->role == 'agent') {
                $user->delete();
            } else {
                return response()->json(['error' => 'Accès refusé'], 403);
            }
        }

        return response()->json(['message' => 'Utilisateur supprimé avec succès']);
    }
} 

