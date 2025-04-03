<?php

namespace App\Http\Controllers;

use Illuminate\Support\Str;
use App\Models\Citoyen;
use Laravel\Sanctum\PersonalAccessToken;

class CitoyenController extends Controller
{
    public function creerCitoyen()
    {
        try {
            // Générer un identifiant aléatoire (UUID)
            $identifiant = Str::uuid()->toString();

            // Créer le citoyen dans la base de données
            $citoyen = Citoyen::create([
                'identifiant' => $identifiant,
            ]);

            // Générer un token Sanctum
            $tokenResult = $citoyen->createToken('Citoyen Access Token');
            $plainTextToken = $tokenResult->plainTextToken; // e.g., "38|ASnizFEOCrcjyaUAzFQFNnrDhiSeoDzYeBxy4sUh"

            // Récupérer le token hashé depuis la table personal_access_tokens
            $hashedToken = PersonalAccessToken::where('tokenable_id', $citoyen->id)
                ->where('tokenable_type', Citoyen::class)
                ->where('id', explode('|', $plainTextToken)[0])
                ->value('token'); // e.g., "374a03c25adcc74ef1f0a507beaefcb79c28625f21b3891887..."

            // Log pour débogage
            \Log::info('Citoyen créé avec token', [
                'citoyen_id' => $citoyen->id,
                'identifiant' => $identifiant,
                'plain_text_token' => $plainTextToken,
                'hashed_token' => $hashedToken,
            ]);

            // Retourner le token correct (plain text) pour l'authentification
            return response()->json([
                'message' => 'Citoyen créé avec succès',
                'citoyen' => [
                    'id' => $citoyen->id,
                    'identifiant' => $citoyen->identifiant,
                    'created_at' => $citoyen->created_at,
                ],
                'token' => $plainTextToken,          // Correct token for authentication
                'hashed_token' => $hashedToken,      // Hashed token for reference (optional)
            ], 201);

        } catch (\Exception $e) {
            \Log::error('Erreur lors de la création du citoyen', ['error' => $e->getMessage()]);
            return response()->json([
                'message' => 'Erreur lors de la création du citoyen',
                'error' => $e->getMessage(),
            ], 500);
        }
    }
}