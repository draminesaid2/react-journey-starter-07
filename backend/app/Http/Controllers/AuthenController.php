<?php
namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\Models\User;
use App\Models\Admins;
use App\Models\Responsable;
use App\Models\Agent;

use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Auth;
use Laravel\Sanctum\HasApiTokens;
use Illuminate\Support\Facades\Validator;

class AuthenController extends Controller
{
    // Méthode d'enregistrement d'un utilisateur
    public function register(Request $request)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }

        // Create a validator instance
        $validator = Validator::make($request->all(), [
            'nom_user' => 'required|string|max:255',
            'prenom_user' => 'required|string|max:255',
            'email_user' => 'required|string|email|max:255|unique:users,email_user',
            'password' => 'required|string|min:6|confirmed',
            'id_etabl' => 'nullable|string',
            'id_service'=> 'nullable|string',
            'id_agence'=> 'nullable|string',
            'role' => 'required|string|in:admin,agent,responsable,supadmin,user',
        ]);

        // Check if validation fails
        if ($validator->fails()) {
            return response()->json(['errors' => $validator->errors()], 422);
        }

        // Role hierarchy
        $roleHierarchy = [
            'supadmin' =>  'admin',
            'admin' => 'responsable',
            'responsable' => 'agent',
        ];

        $requestedRole = $request->input('role');
        $authUserRole = $user->role;
      
        if (!array_key_exists($authUserRole, $roleHierarchy) || $roleHierarchy[$authUserRole] !== $requestedRole) {
            return response()->json([
                'message' => "You are not authorized to create a user with role: $requestedRole"
            ], 403);
        }

        // Create the user
        $newUser = User::create([
            'nom_user' => $request->nom_user,
            'prenom_user' => $request->prenom_user,
            'email_user' => $request->email_user,
            'password' => Hash::make($request->password),
            'role' => $requestedRole,
        ]);
     
       

        if($requestedRole == 'admin'){
            $admin = New Admins();
            $admin->id_user = $newUser->id_user;
            $admin->id_etabl  = $request->id_etabl;
            $admin->save();
        }
        
        if($requestedRole == 'responsable'){
            $admin_creator = Admins::where('id_user',$user->id_user)->first();
            if(!$admin_creator){
                return response()->json(['message' => "You are not authorized (admin)"], 403);
            }
            $responsable = New Responsable();
            $responsable->id_admin = $admin_creator->id_admin;
            $responsable->id_user = $newUser->id_user;
            $responsable->save();
        }

        if($requestedRole == 'agent'){
            $responsable_creator = Responsable::where('id_user',$user->id_user)->first();
            if(!$responsable_creator){
                return response()->json(['message' => "You are not authorized (responsable)"], 403);
            }
            $agent = New Agent();
            $agent->id_responsable = $responsable_creator->id_responsable;
            $agent->id_user = $newUser->id_user;
            $agent->id_agence = $request->id_agence;
            $agent->save();
        }

        $token = $newUser->createToken('Personal Access Token')->plainTextToken;

        return response()->json([
            'message' => "Bienvenue {$newUser->nom_user}, votre compte a été créé avec succès en tant que {$newUser->role}.",
            'user' => $newUser,
            'token' => $token,
        ], 201);
    }

    // Méthode de connexion d'un utilisateur
    public function login(Request $request)
    {
        $creds = $request->only(['email_user', 'password']); // Utilisation de 'email_user'
    
        // Validation des champs
        if (empty($creds['email_user']) || empty($creds['password'])) {
            return response()->json(['error' => 'Email et mot de passe sont requis'], 400);
        }

        // Tentative d'authentification
        if (Auth::attempt($creds)) {
            $user = Auth::user();
            $token = $user->createToken('Personal Access Token')->plainTextToken;

            return response()->json([
                'message' => "Bienvenue {$user->nom_user}, vous êtes connecté.",
                'token' => $token,
                'user' => [
                    'nom_user' => $user->nom_user,
                    'prenom_user' => $user->prenom_user,
                    'email_user' => $user->email_user,
                    'role' => $user->role
                ]
            ]);
        } else {
            return response()->json(['error' => 'Email ou mot de passe incorrect'], 401);
        }
    }

    // Méthode de déconnexion de l'utilisateur
    public function logout()
    {
        // Vérifier si l'utilisateur est authentifié
        if (auth()->check()) {
            // Révocation du token actuel
            auth()->user()->tokens->each(function ($token) {
                $token->delete();
            });

            return response()->json(['message' => 'Déconnexion réussie']);
        }

        return response()->json(['error' => 'Utilisateur non authentifié'], 401);
    }

}





                            