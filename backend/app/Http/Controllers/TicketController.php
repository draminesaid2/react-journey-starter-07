<?php

namespace App\Http\Controllers;

use App\Models\Agence;
use App\Models\Services;
use App\Models\Files;
use App\Models\Tickets;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Validation\Rule;
use Illuminate\Support\Facades\Validator;

class TicketController extends Controller
{

    public function creat(Request $request,$id=null)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
        // Validation des données 
        $validatedData = $request->validate([
            'id_file' => 'required|string|max:255',
        ]);
   
        $file = Files::where('id_file',$request->id_file)->first();

        if (!$file) {
            return response()->json(['message' => "File non trouvé"], 404);
        }
        if($file->status !== 'ouvert'){
            return response()->json(['message' => "File non disponible"], 404); 
        }
        // Création de ticket
        $verify_ticket = Tickets::where('id_file', $file->id_file)
                    ->where('id_identifiant', $user->id)
                    ->first();

        if($verify_ticket){
            return response()->json(['message' => 'ticket deja exist, (suuprimer ticket) !!!!!', 'ticket' => $verify_ticket], 201); 
        } else {
            $get_number = Tickets::where('id_file', $file->id_file)
            ->latest()->first();
            if($get_number){
                $numero = $get_number->numero + 1;
            } else {
                $numero = 1 ;
            }
            $ticket = New Tickets();
            $ticket->id_identifiant =  $user->id;
            $ticket->id_file = $file->id_file;
            $ticket->numero = $numero;
            $ticket->save();
    
            return response()->json(['message' => 'ticket créée avec succès', 'ticket' => $ticket], 201); 
        }
         
    }

    public function createst(Request $request,$id=null)
    {
       
        $file = Files::where('id_file',$request->id_file)->first();

        if (!$file) {
            return response()->json(['message' => "File non trouvé"], 404);
        }
        if($file->status !== 'ouvert'){
            return response()->json(['message' => "File non disponible"], 404); 
        }
        // Création de ticket
        $verify_ticket = Tickets::where('id_file', $file->id_file)
                    ->where('id_identifiant', 35)
                    ->first();

        if($verify_ticket){
            return response()->json(['message' => 'ticket deja exist, (suuprimer ticket) !!!!!', 'ticket' => $verify_ticket], 201); 
        } else {
            $get_number = Tickets::where('id_file', $file->id_file)
            ->latest()->first();
            if($get_number){
                $numero = $get_number->numero + 1;
            } else {
                $numero = 1 ;
            }
            $ticket = New Tickets();
            $ticket->id_identifiant =  35;
            $ticket->id_file = $file->id_file;
            $ticket->numero = $numero;
            $ticket->save();
    
            return response()->json(['message' => 'ticket créée avec succès', 'ticket' => $ticket], 201); 
        }
         
    }
    public function destroy($id)
    {
        $user = Auth::user();
        if (!$user) {
            return response()->json(['message' => "Error Auth"], 401);
        }
    
        $ticket = Tickets::where('id_ticket',$id)->where('id_identifiant',$user->id)->first();

        if($ticket){
            $ticket->delete();
            return response()->json(['message' => 'ticket supprimée']);
        }
        return response()->json(['message' => "Error Accee"], 403);

    }

}
