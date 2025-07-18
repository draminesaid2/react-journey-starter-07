import { useState, useEffect } from 'react';
import { Dialog, DialogContent, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Separator } from '@/components/ui/separator';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { 
  Crown, 
  CreditCard, 
  Calendar, 
  CheckCircle, 
  XCircle, 
  AlertTriangle,
  Receipt,
  Star
} from 'lucide-react';
import { useAuth } from '@/hooks/useAuth';
import { userDataService, Payment } from '@/services/userDataService';
import { toast } from '@/hooks/use-toast';
import DeleteConfirmationModal from './admin/DeleteConfirmationModal';

interface SubscriptionManagementModalProps {
  isOpen: boolean;
  onClose: () => void;
  subscription: {
    isActive: boolean;
    plan: string;
    endDate?: string;
    price?: string;
    status?: string;
    nextBilling?: string;
  };
}

const SubscriptionManagementModal = ({ isOpen, onClose, subscription }: SubscriptionManagementModalProps) => {
  const { user } = useAuth();
  const [paymentHistory, setPaymentHistory] = useState<Payment[]>([]);
  const [isLoadingHistory, setIsLoadingHistory] = useState(false);
  const [cancelLoading, setCancelLoading] = useState(false);
  const [cancelConfirmOpen, setCancelConfirmOpen] = useState(false);

  useEffect(() => {
    const fetchPayments = async () => {
      if (user?.id) {
        setIsLoadingHistory(true);
        const res = await userDataService.getPayments(user.id);
        if (res.success) {
          setPaymentHistory(res.data);
        }
        setIsLoadingHistory(false);
      }
    };
    if (isOpen && user?.id) {
      fetchPayments();
    }
  }, [isOpen, user?.id]);

  const handleAskCancelSubscription = () => {
    setCancelConfirmOpen(true);
  };

  const handleCancelSubscription = async () => {
    setCancelConfirmOpen(false);
    if (!user?.id) {
      toast({
        title: "Erreur",
        description: "Utilisateur non authentifié",
        variant: "destructive",
      });
      return;
    }
    setCancelLoading(true);
    try {
      const res = await userDataService.cancelSubscription(user.id);
      if (res.success) {
        toast({
          title: "Abonnement annulé",
          description: "Votre abonnement a été annulé avec succès.",
        });
        onClose();
      } else {
        toast({
          title: "Erreur",
          description: res.message || "Erreur lors de l'annulation de l'abonnement",
          variant: "destructive",
        });
      }
    } catch (error) {
      toast({
        title: "Erreur",
        description: "Erreur de connexion au serveur.",
        variant: "destructive",
      });
    } finally {
      setCancelLoading(false);
    }
  };

  const handleUpgradeSubscription = () => {
    console.log('Upgrade subscription clicked');
    // Here you would integrate with Stripe to upgrade subscription
  };

  const handleUpdatePayment = () => {
    console.log('Update payment method clicked');
    // Here you would open Stripe customer portal for payment method update
  };

  const handleSubscribe = () => {
    console.log('Subscribe clicked');
    // Here you would start checkout process
  };

  return (
    <>
      <Dialog open={isOpen} onOpenChange={onClose}>
        <DialogContent className="sm:max-w-4xl max-h-[90vh] overflow-y-auto bg-white border-2 border-slate-200">
          <DialogHeader>
            <DialogTitle className="text-center text-2xl font-bold text-slate-800 flex items-center justify-center space-x-2">
              <Crown className="w-6 h-6 text-purple-500" />
              <span>Gestion de l'abonnement</span>
            </DialogTitle>
          </DialogHeader>
          
          <div className="space-y-6 p-2">
            {/* Current Subscription Status */}
            <Card className="bg-gradient-to-r from-purple-50 to-blue-50 border-2 border-purple-200">
              <CardHeader>
                <CardTitle className="flex items-center justify-between">
                  <div className="flex items-center space-x-2">
                    <Crown className="w-5 h-5 text-purple-500" />
                    <span>Statut de l'abonnement</span>
                  </div>
                  {subscription.isActive ? (
                    <Badge className="bg-green-100 text-green-800">
                      <CheckCircle className="w-3 h-3 mr-1" />
                      Actif
                    </Badge>
                  ) : (
                    <Badge variant="secondary" className="bg-gray-100 text-gray-800">
                      <XCircle className="w-3 h-3 mr-1" />
                      Inactif
                    </Badge>
                  )}
                </CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                {subscription.isActive ? (
                  <>
                    <div className="grid md:grid-cols-2 gap-4">
                      <div>
                        <p className="text-sm text-slate-600">Plan actuel</p>
                        <p className="text-xl font-bold text-slate-800">{subscription.plan}</p>
                      </div>
                      <div>
                        <p className="text-sm text-slate-600">Prix payé</p>
                        <p className="text-xl font-bold text-slate-800">
                          {paymentHistory.length > 0 
                            ? `${paymentHistory[0].total_amount} ${paymentHistory[0].currency}`
                            : subscription.price}
                          /mois
                        </p>
                      </div>
                    </div>
                    
                    {subscription.endDate && (
                      <div className="flex items-center space-x-2 text-slate-600">
                        <Calendar className="w-4 h-4" />
                        <span>
                          Prochain renouvellement : {subscription.endDate}
                        </span>
                      </div>
                    )}

                    <div className="flex flex-wrap gap-3 pt-4">
                      <Button 
                        onClick={handleAskCancelSubscription}
                        variant="outline" 
                        className="border-red-200 text-red-600 hover:bg-red-50"
                        disabled={cancelLoading}
                      >
                        <XCircle className="w-4 h-4 mr-2" />
                        {cancelLoading ? "Annulation..." : "Annuler l'abonnement"}
                      </Button>
                      <Button 
                        onClick={handleUpgradeSubscription}
                        className="bg-gradient-to-r from-purple-500 to-blue-500 hover:from-purple-600 hover:to-blue-600 text-white"
                      >
                        <Star className="w-4 h-4 mr-2" />
                        Changer de plan
                      </Button>
                      <Button 
                        onClick={handleUpdatePayment}
                        variant="outline" 
                        className="border-blue-200 text-blue-600 hover:bg-blue-50"
                      >
                        <CreditCard className="w-4 h-4 mr-2" />
                        Modifier le paiement
                      </Button>
                    </div>
                  </>
                ) : (
                  <div className="text-center py-6">
                    <AlertTriangle className="w-12 h-12 text-orange-500 mx-auto mb-4" />
                    <p className="text-slate-600 mb-4">Vous n'avez pas d'abonnement actif</p>
                    <p className="text-sm text-slate-500 mb-6">
                      Souscrivez à un abonnement pour profiter de toutes nos fonctionnalités premium
                    </p>
                  </div>
                )}
              </CardContent>
            </Card>

            <Separator />

            {/* Payment History */}
            <Card className="bg-white border-slate-200">
              <CardHeader>
                <CardTitle className="flex items-center space-x-2">
                  <Receipt className="w-5 h-5 text-green-500" />
                  <span>Historique des paiements</span>
                </CardTitle>
                <CardDescription>Vos dernières transactions</CardDescription>
              </CardHeader>
              <CardContent>
                {isLoadingHistory ? (
                  <div className="text-center py-8">
                    <div className="animate-spin rounded-full h-6 w-6 border-b-2 border-green-500 mx-auto mb-3"></div>
                    <p className="text-slate-500">Chargement des paiements...</p>
                  </div>
                ) : paymentHistory.length > 0 ? (
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Date</TableHead>
                        <TableHead>Plan</TableHead>
                        <TableHead>Montant</TableHead>
                        <TableHead>Statut</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {paymentHistory.map((payment) => (
                        <TableRow key={payment.id} className="hover:bg-slate-50">
                          <TableCell className="font-medium">
                            {new Date(payment.created_at).toLocaleDateString('fr-FR', { day:"2-digit", month:"short", year:"numeric" })}
                          </TableCell>
                          <TableCell>{payment.plan_type}</TableCell>
                          <TableCell className="font-semibold">{payment.total_amount} {payment.currency}</TableCell>
                          <TableCell>
                            <Badge 
                              className={
                                payment.status === 'paid' 
                                  ? 'bg-green-100 text-green-800' 
                                  : payment.status === 'pending'
                                  ? 'bg-yellow-100 text-yellow-800'
                                  : 'bg-red-100 text-red-800'
                              }
                            >
                              {payment.status === 'paid' 
                                ? 'Payé'
                                : payment.status === 'pending'
                                ? 'En attente'
                                : 'Annulé'}
                            </Badge>
                          </TableCell>
                        </TableRow>
                      ))}
                    </TableBody>
                  </Table>
                ) : (
                  <div className="text-center py-8">
                    <Receipt className="w-12 h-12 text-slate-300 mx-auto mb-4" />
                    <p className="text-slate-500">Aucun historique de paiement disponible</p>
                  </div>
                )}
              </CardContent>
            </Card>

            {/* Available Plans */}
            {!subscription.isActive && (
              <Card className="bg-white border-slate-200">
                <CardHeader>
                  <CardTitle className="flex items-center space-x-2">
                    <Star className="w-5 h-5 text-yellow-500" />
                    <span>Plans disponibles</span>
                  </CardTitle>
                  <CardDescription>Choisissez le plan qui vous convient</CardDescription>
                </CardHeader>
                <CardContent>
                  <div className="grid md:grid-cols-2 gap-4">
                    <Card className="border-2 border-blue-200 hover:border-blue-300 transition-colors cursor-pointer">
                      <CardContent className="p-6 text-center">
                        <h3 className="text-lg font-bold text-slate-800 mb-2">Plan Basic</h3>
                        <p className="text-3xl font-bold text-blue-600 mb-2">4.99€</p>
                        <p className="text-sm text-slate-600 mb-4">par mois</p>
                        <ul className="text-sm text-slate-600 space-y-1">
                          <li>• 2 livres par mois</li>
                          <li>• Support email</li>
                          <li>• Accès aux modèles de base</li>
                        </ul>
                      </CardContent>
                    </Card>
                    
                    <Card className="border-2 border-purple-200 hover:border-purple-300 transition-colors cursor-pointer bg-gradient-to-br from-purple-50 to-blue-50">
                      <CardContent className="p-6 text-center">
                        <div className="flex justify-between items-start mb-2">
                          <h3 className="text-lg font-bold text-slate-800">Plan Premium</h3>
                          <Badge className="bg-purple-100 text-purple-800">Populaire</Badge>
                        </div>
                        <p className="text-3xl font-bold text-purple-600 mb-2">9.99€</p>
                        <p className="text-sm text-slate-600 mb-4">par mois</p>
                        <ul className="text-sm text-slate-600 space-y-1">
                          <li>• Livres illimités</li>
                          <li>• Support prioritaire</li>
                          <li>• Tous les modèles</li>
                          <li>• Personnalisation avancée</li>
                        </ul>
                      </CardContent>
                    </Card>
                  </div>
                </CardContent>
              </Card>
            )}
          </div>

          <div className="flex justify-end pt-4">
            <Button variant="outline" onClick={onClose}>
              Fermer
            </Button>
          </div>
        </DialogContent>
      </Dialog>
      {/* Cancel confirmation modal */}
      <DeleteConfirmationModal
        isOpen={cancelConfirmOpen}
        onClose={() => setCancelConfirmOpen(false)}
        onConfirm={handleCancelSubscription}
        title="Annuler l'abonnement"
        description="Êtes-vous sûr de vouloir annuler votre abonnement ?"
      />
    </>
  );
};

export default SubscriptionManagementModal;
