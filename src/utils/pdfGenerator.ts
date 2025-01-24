import jsPDF from 'jspdf';

export const generateOrderPDF = async (orderData: any) => {
  const doc = new jsPDF();
  const pageWidth = doc.internal.pageSize.getWidth();
  
  // Set default font
  doc.setFont("helvetica");
  
  // Add logo with fallback
  const addLogoWithFallback = async () => {
    try {
      const img = new Image();
      img.crossOrigin = "Anonymous";
      
      await new Promise((resolve, reject) => {
        img.onload = resolve;
        img.onerror = reject;
        img.src = 'https://www.fioriforyou.com/backfiori/logo.png';
      });
      
      const canvas = document.createElement('canvas');
      canvas.width = img.width;
      canvas.height = img.height;
      const ctx = canvas.getContext('2d');
      ctx?.drawImage(img, 0, 0);
      const dataUrl = canvas.toDataURL('image/jpeg');
      
      doc.addImage(dataUrl, 'JPEG', 20, 10, 50, 20);
    } catch (error) {
      console.warn('Failed to add logo to PDF:', error);
    }
  };

  await addLogoWithFallback();
  
  // Security notice and document header
  doc.setFontSize(8);
  doc.setTextColor(128);
  doc.text('Document officiel - Fiori For You SARL', pageWidth - 80, 15);
  doc.text('Pour votre sécurité, ce document est numérisé et archivé', pageWidth - 100, 20);
  
  // Main header
  doc.setFillColor(112, 1, 0); // #700100
  doc.rect(0, 35, pageWidth, 15, 'F');
  doc.setTextColor(255, 255, 255);
  doc.setFontSize(16);
  doc.setFont("helvetica", "bold");
  doc.text('CONFIRMATION DE COMMANDE', pageWidth / 2, 45, { align: 'center' });
  
  // Order details
  doc.setTextColor(0);
  doc.setFontSize(12);
  doc.text(`N° de commande: ${orderData.order_id}`, 20, 70);
  doc.text(`Date: ${new Date().toLocaleDateString('fr-FR')}`, pageWidth - 60, 70);
  
  // Customer Details section
  doc.setDrawColor(112, 1, 0);
  doc.setLineWidth(0.5);
  doc.roundedRect(15, 85, 180, 55, 3, 3);
  doc.setFillColor(112, 1, 0);
  doc.rect(15, 85, 180, 8, 'F');
  doc.setTextColor(255, 255, 255);
  doc.setFontSize(11);
  doc.text('INFORMATIONS CLIENT', 20, 90);
  
  // Customer details content
  doc.setTextColor(0);
  doc.setFontSize(10);
  doc.setFont("helvetica", "normal");
  
  // Left column
  doc.text('Nom complet:', 20, 100);
  doc.text('Adresse:', 20, 110);
  doc.text('Code postal:', 20, 120);
  doc.text('Pays:', 20, 130);
  
  // Right column
  doc.text('Téléphone:', 110, 100);
  doc.text('Email:', 110, 110);
  
  // Values
  doc.setFont("helvetica", "bold");
  doc.text(`${orderData.user_details.first_name} ${orderData.user_details.last_name}`, 50, 100);
  doc.text(orderData.user_details.address, 50, 110);
  doc.text(orderData.user_details.zip_code, 50, 120);
  doc.text(orderData.user_details.country, 50, 130);
  doc.text(orderData.user_details.phone, 140, 100);
  doc.text(orderData.user_details.email, 140, 110);
  
  // Products section
  let yPosition = 155;
  
  // Products header
  doc.setFillColor(112, 1, 0);
  doc.roundedRect(15, yPosition - 10, 180, 8, 2, 2, 'F');
  doc.setTextColor(255, 255, 255);
  doc.text('DÉTAIL DE LA COMMANDE', 20, yPosition - 4);
  
  // Products table header
  yPosition += 5;
  doc.setFillColor(240, 240, 240);
  doc.rect(15, yPosition, 180, 8, 'F');
  doc.setTextColor(80, 80, 80);
  doc.setFontSize(9);
  doc.text('Article', 55, yPosition + 5);
  doc.text('Qté', 130, yPosition + 5);
  doc.text('Prix', 160, yPosition + 5);
  
  yPosition += 15;
  
  // Products list with detailed information
  for (const item of orderData.items) {
    if (yPosition > 250) {
      doc.addPage();
      yPosition = 20;
    }
    
    // Product box with shadow effect
    doc.setFillColor(248, 248, 248);
    doc.roundedRect(15, yPosition - 2, 180, 60, 2, 2, 'F');
    doc.setDrawColor(220, 220, 220);
    doc.roundedRect(15, yPosition - 2, 180, 60, 2, 2);
    
    try {
      // Add product image
      const img = new Image();
      img.crossOrigin = "Anonymous";
      await new Promise((resolve, reject) => {
        img.onload = resolve;
        img.onerror = reject;
        img.src = item.image;
      });
      
      const canvas = document.createElement('canvas');
      canvas.width = img.width;
      canvas.height = img.height;
      const ctx = canvas.getContext('2d');
      ctx?.drawImage(img, 0, 0);
      const dataUrl = canvas.toDataURL('image/jpeg');
      
      doc.setFillColor(255, 255, 255);
      doc.roundedRect(19, yPosition, 32, 32, 2, 2, 'F');
      doc.addImage(dataUrl, 'JPEG', 20, yPosition, 30, 30);
    } catch (error) {
      console.warn('Failed to add product image:', error);
    }
    
    // Item details with improved layout
    doc.setTextColor(112, 1, 0);
    doc.setFont("helvetica", "bold");
    doc.setFontSize(11);
    doc.text(item.name, 55, yPosition + 5);
    
    // Reference number
    doc.setTextColor(128);
    doc.setFontSize(8);
    doc.text(`Réf: ${item.item_id.toString().padStart(6, '0')}`, 55, yPosition + 12);
    
    // Price and quantity
    doc.setTextColor(0);
    doc.setFontSize(10);
    doc.text(`Quantité: ${item.quantity}`, 55, yPosition + 22);
    doc.text(`Prix unitaire: ${item.price.toFixed(2)} TND`, 130, yPosition + 22);
    doc.setFont("helvetica", "bold");
    doc.text(`Total: ${(item.price * item.quantity).toFixed(2)} TND`, 130, yPosition + 32);
    
    // Product details grid
    doc.setFont("helvetica", "normal");
    doc.setFontSize(9);
    let detailsY = yPosition + 35;
    
    // Left column details
    if (item.size !== '-') {
      doc.text(`Taille: ${item.size}`, 55, detailsY);
    }
    if (item.color !== '-') {
      doc.text(`Couleur: ${item.color}`, 55, detailsY + 7);
    }
    
    // Right column details
    if (item.pack !== 'aucun') {
      doc.text(`Pack: ${item.pack}`, 130, detailsY);
    }
    if (item.box !== 'Sans box') {
      doc.text('Box cadeau incluse', 130, detailsY + 7);
    }
    
    // Personalization (if present)
    if (item.personalization && item.personalization !== '-') {
      doc.setFillColor(240, 240, 240);
      doc.roundedRect(55, detailsY + 12, 135, 8, 1, 1, 'F');
      doc.text(`Personnalisation: ${item.personalization}`, 58, detailsY + 17);
    }
    
    yPosition += 65;
  }
  
  // Total section with professional design
  yPosition += 5;
  doc.setFillColor(248, 248, 248);
  doc.roundedRect(15, yPosition, 180, 50, 3, 3, 'F');
  doc.setDrawColor(112, 1, 0);
  doc.roundedRect(15, yPosition, 180, 50, 3, 3);
  
  // Totals with right alignment
  const rightAlign = (text: string, y: number) => {
    const textWidth = doc.getTextWidth(text);
    doc.text(text, 185 - textWidth, y);
  };
  
  doc.setTextColor(80, 80, 80);
  doc.setFont("helvetica", "normal");
  doc.setFontSize(10);
  
  rightAlign(`Sous-total: ${orderData.price_details.subtotal.toFixed(2)} TND`, yPosition + 15);
  
  if (orderData.price_details.box_total > 0) {
    rightAlign(`Box cadeau: ${orderData.price_details.box_total.toFixed(2)} TND`, yPosition + 25);
  }
  
  rightAlign(`Livraison: ${orderData.price_details.shipping_cost.toFixed(2)} TND`, yPosition + 35);
  
  // Final total with accent color
  doc.setFillColor(112, 1, 0);
  doc.roundedRect(100, yPosition + 40, 95, 10, 2, 2, 'F');
  doc.setTextColor(255, 255, 255);
  doc.setFont("helvetica", "bold");
  doc.text(`Total: ${orderData.price_details.final_total.toFixed(2)} TND`, 105, yPosition + 46);
  
  // Professional footer with company information
  const pageCount = doc.getNumberOfPages();
  for(let i = 1; i <= pageCount; i++) {
    doc.setPage(i);
    doc.setDrawColor(112, 1, 0);
    doc.setLineWidth(0.5);
    doc.line(15, doc.internal.pageSize.getHeight() - 20, pageWidth - 15, doc.internal.pageSize.getHeight() - 20);
    
    doc.setFont("helvetica", "italic");
    doc.setFontSize(8);
    doc.setTextColor(128);
    doc.text(
      'Fiori For You - Votre boutique de confiance',
      15,
      doc.internal.pageSize.getHeight() - 15
    );
    doc.text(
      `Page ${i}/${pageCount}`,
      pageWidth - 40,
      doc.internal.pageSize.getHeight() - 15
    );
  }
  
  return doc;
};