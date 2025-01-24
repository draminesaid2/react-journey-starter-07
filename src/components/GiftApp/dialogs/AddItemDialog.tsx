import React from 'react';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Product } from '@/types/product';
import SizeSelector from '../../product-detail/SizeSelector';
import { canItemBePersonalized, getPersonalizationMessage } from '@/utils/personalizationConfig';
import { Textarea } from "@/components/ui/textarea";
import { toast } from "@/hooks/use-toast";
import { needsSizeSelection, getDefaultSize } from '@/utils/sizeUtils';

interface AddItemDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  droppedItem: Product | null;
  selectedSize: string;
  personalization: string;
  onSizeSelect: (size: string) => void;
  onPersonalizationChange: (text: string) => void;
  onConfirm: () => void;
}

const AddItemDialog = ({
  open,
  onOpenChange,
  droppedItem,
  selectedSize,
  personalization,
  onSizeSelect,
  onPersonalizationChange,
  onConfirm,
}: AddItemDialogProps) => {
  const getAvailableSizes = (product: Product | null): string[] => {
    if (!product || !product.sizes) return [];
    
    // For items that don't need size selection
    if (!needsSizeSelection(product.itemgroup_product)) {
      // Automatically select a default size
      if (!selectedSize) {
        onSizeSelect('unique');
      }
      return [];
    }

    return product.itemgroup_product === 'costumes'
      ? Object.entries(product.sizes)
          .filter(([key, stock]) => ['48', '50', '52', '54', '56', '58'].includes(key) && stock > 0)
          .map(([size]) => size)
      : Object.entries(product.sizes)
          .filter(([key, stock]) => ['s', 'm', 'l', 'xl', 'xxl', '3xl'].includes(key.toLowerCase()) && stock > 0)
          .map(([size]) => size.toUpperCase());
  };

  const availableSizes = getAvailableSizes(droppedItem);
  const canPersonalize = droppedItem ? canItemBePersonalized(droppedItem.itemgroup_product) : false;
  const personalizationMessage = droppedItem ? getPersonalizationMessage(droppedItem.itemgroup_product) : undefined;
  const requiresSizeSelection = droppedItem ? needsSizeSelection(droppedItem.itemgroup_product) : false;
  const isChemise = droppedItem?.itemgroup_product === 'chemises';
  const maxLength = isChemise ? 4 : 100;
  const remainingChars = maxLength - (personalization?.length || 0);

  const handlePersonalizationChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    const newText = e.target.value;
    if (newText.length <= maxLength) {
      onPersonalizationChange(newText);
    } else {
      toast({
        title: "Limite de caractères atteinte",
        description: isChemise 
          ? "La personnalisation est limitée à 4 caractères pour les chemises"
          : "La personnalisation est limitée à 100 caractères",
        variant: "destructive",
      });
    }
  };

  const canConfirm = () => {
    if (!requiresSizeSelection) return true;
    if (requiresSizeSelection && !selectedSize) return false;
    if (requiresSizeSelection && availableSizes.length === 0) return false;
    if (isChemise && personalization && personalization.length > 4) return false;
    return true;
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px] bg-white/95">
        <DialogHeader>
          <DialogTitle className="text-xl font-serif text-[#6D0201] mb-4">
            {requiresSizeSelection ? 'Personnalisez votre article' : 'Confirmer la sélection'}
          </DialogTitle>
        </DialogHeader>
        <div className="space-y-6">
          {requiresSizeSelection && availableSizes.length > 0 && (
            <SizeSelector
              selectedSize={selectedSize}
              sizes={availableSizes}
              onSizeSelect={onSizeSelect}
              isCostume={droppedItem?.itemgroup_product === 'costumes'}
            />
          )}
          
          {canPersonalize && (
            <div className="space-y-2">
              <div className="flex justify-between items-center">
                <label className="text-sm font-medium text-gray-700">Votre texte de personnalisation</label>
                <span className={`text-sm ${remainingChars === 0 ? 'text-red-500' : 'text-gray-500'}`}>
                  {remainingChars} caractères restants
                </span>
              </div>
              <Textarea
                placeholder={isChemise 
                  ? "Maximum 4 caractères (ex: IHEB)"
                  : "Ajoutez votre texte personnalisé ici..."}
                value={personalization}
                onChange={handlePersonalizationChange}
                maxLength={maxLength}
                className="min-h-[120px] p-4 text-gray-800 bg-gray-50 border-2 border-gray-200 focus:border-[#700100] focus:ring-[#700100] rounded-lg resize-none transition-all duration-300"
              />
              <p className="text-sm text-gray-500 italic">
                {isChemise 
                  ? "Pour les chemises, la personnalisation est limitée à 4 caractères"
                  : "Maximum 100 caractères"}
              </p>
            </div>
          )}

          {requiresSizeSelection && availableSizes.length === 0 && (
            <p className="text-red-500">Aucune taille disponible pour ce produit</p>
          )}

          <button
            onClick={onConfirm}
            className={`w-full py-4 rounded-xl text-white font-medium ${
              !canConfirm()
                ? 'bg-gray-400 cursor-not-allowed'
                : 'bg-[#6D0201] hover:bg-[#590000]'
            }`}
            disabled={!canConfirm()}
          >
            Confirmer
          </button>
        </div>
      </DialogContent>
    </Dialog>
  );
};

export default AddItemDialog;