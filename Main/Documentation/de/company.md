# Objektdetails - Firma
Zusätzlich zu den im Kapitel Objektdetails aufgeführten Funktionen stehen für Firmen noch weitere Funktionen zur Verfügung die hier im weiteren vorgestellt werden sollen.

## Kopfdarstellung - Zugehörigkeit
Wurde bei der Anlage der Firma ein *Mutterunternehmen* angegeben findet sich in der Kopfdarstellung der *Tochter* ein Verweis auf die Mutter. Mit Hilfe des Verweis kann die Detaildarstellung der Mutter aufgerufen werden.

![Zugehörigkeit zu einer Mutter](img/company/header_parent.png "Zugehörigkeit zu einer Mutter")

## Anwendungsbereich - Beziehungen
Aktive Beziehungen werden nach Ihrem Beziehungstyp gruppiert und in einer Liste dargestellt. Vorhandene Beziehungen können bearbeitet oder entfernt werden. 

![Listendarstellung vorhandener Beziehungen einer Firma](img/company/relationship_list.png "Listendarstellung vorhandener Beziehungen einer Firma")

Neue Beziehungen werden mit Hilfe der Kontextfunktion *Geschäftsbeziehung hinzufügen* eingerichet.

![Kontextfunktion - Geschäftsbeziehung anlegen](img/company/relationship_action_create.png "Kontextfunktion - Geschäftsbeziehung anlegen")

Im angebotenen Formular haben Sie die Möglichkeit Informationen zur Gegenseite, Art der Beziehung und weiterführende Informationen zu erfassen.

![Formulardarstellung für Beziehungen](img/company/relationship_form.png "Geschäftsbeziehung Formulardarstellung")



Hier ein Beispiel wie Beziehungen im Mobilen CRM (Client) hinzugefügt werden können:

![Mobiler Client Geschäfts- oder Projektbeziehung hinzufügen](img/company/realtionship_mobile_crm_material_client.png "Mobiler Client Geschäfts- oder Projektbeziehung hinzufügen")

----
Die angebotenen Beziehungsarten werden in einer Zuordnungstabelle pro Objekt gepflegt und können auf Ihre Bedürfnisse angepasst werden.

----

## Beziehungen {#relationships}
Beziehungen zwischen Objekten können genutzt werden um zusätzlich zu einer streng hierarchischen Struktur ein Netzwerk von Objekten zu bilden. Dabei kann innerhalb der Beziehung unterschieden werden:

- Beziehungen die in beide Richtungen gleich wirken z.B. Wettbewerber
- Beziehungen die unterschiedlich in beide Richtungen wirken z.B. Kunde / Lieferant
- Beziehungen die nur in eine Richtung wirken

![Beziehungen zwischen Objekten](img/relationships.png "Beziehungen zwischen Objekten")

## Kontext - Kontaktinformationen
Zu einer Firma kann eine Liste von Adressen gespeichert werden. Die Adressliste bildet die Grundlage um z.B. diesen Adressen einzelne Personen zuzuweisen.

![Liste von Adressen der Firma](img/company/address_list.png "Liste von Adressen der Firma")

Neue Adressen können mit Hilfe der Funktion *Hinzufügen* zur geöffneten Firma erfasst werden.

![Adresse hinzufügen - Formularansicht](img/company/address_form.png "Adresse hinzufügen - Formularansicht")

Jeder Adresse kann eine Reihe von Kommunikationsdaten zugeordnet werden um z.B. verschiedene Kontaktmöglichkeiten an den unterschiedlichen Adressen abzubilden. Per Definition kann jeder Firma genau eine __Standardadresse__ zugewiesen werden. Verfügt eine Firma über mehr als eine Adresse kann die Standardadresse mit Hilfe der Funktion *Zur Standardadresse machen* verändert werden.

![Funktion zur Standardadresse machen](img/company/address_action_default_address.png "Funktion zur Standardadresse machen")

Vorhandene Adressen können mit dem Verweis auf die *VCard* pro Adresse heruntergeladen werden und so z.B. in Outlook importiert werden.

So sieht es im Mobilen CRM aus:

![Adresse hinzufügen - im Mobilen CRM](img/company/new_address_mobile_crm_material_client.png "Adresse hinzufügen - im Mobilen CRM")

![Adresse zur Standardadresse machen - im Mobilen CRM](img/company/new_standard_address_mobile_crm_material_client.png "Adresse zur Standardadresse machen - im Mobilen CRM")

## Kontext - Personen bei dieser Firma
Im Kontext der Firma wird eine Liste von Personen angeboten. Diese Personen sind sowohl der Firma als auch einer der gespeicherten Adressen der Firma zugeordnet. Die Darstellung kann zwischen aktiven und inaktiven (ausgeschiedenen) Personen umgeschaltet werden.

![Personen bei dieser Firma](img/company/context_staff.png "Personen bei dieser Firma")

Zu jeder Person wird ein Verweis auf die Detaildarstellung sowie weiterführende Informationen und primäre Kommunikationsdaten angeboten. Besonders relevante Personen können mit Hilfe einer Kennzeichnung von anderen unterschieden werden.

## Kontext - Zugehörige Firmen
Wurde bei der Anlage einer Firma ein *Mutterunternehmen* angegeben, so erscheint in der Mutter eine Liste mit zugehörigen Firmen im Kontextbereich. Diese Liste bietet für jede *Tochter* einen Verweis an um direkt in die Detaildarstellung der Tochter zu gelangen.

![Zugehörige Firmen](img/company/context_child_companies.png "Zugehörige Firmen")

## Aktion - Zusammenfassung
Die Zusammenfassung einer Firma dient dazu einen Besuch bei der Firma vorzubereiten. Es werden relevante Inhalte in einer kompakten Form bereitgestellt. Die Inhalte können für die Verwendung ohne Zugriff auf die L-mobile Crm/Service Anwendung ausgedruckt oder gespeichert werden.

Sie erreichen die Zusammenfassung über die Aktion im Kontextbereich einer Firma.

![Zusammenfassung für eine Firma erstellen](img/company/context_child_companies.png "Zusammenfassung für eine Firma erstellen")

Dabei haben Sie anschliessend die Möglichkeit den Aufbau sowie die Inhalte der Zusammenfassung anzupassen bis der Bericht Ihren Vorstellungen entspricht. Dazu nutzen Sie die vier verfügbaren Layout-Bereiche:

- Kopfbereich
- Seitenleiste
- Hauptbereich
- Fußbereich

![Layout für die Zusammenfassung ändern](img/company/summary_layout.png "Layout für die Zusammenfassung ändern")


und verteilen die Inhalte via Drag and Drop auf den gewünschten Layoutbereich. Mit Hilfe der Checkbox können einzelne Inhalte ein- oder ausgeblendet werden. Wenn Sie mit dem gewünschten Inhalt zufrieden sind können Sie die aktuellen Inhalte mit der Funktion *Drucken* unterhalb der Layoutzone auf Ihren Drucker ausgeben.

![Inhalte mit der Checkbox ein- oder ausblenden](img/company/summary_layout_item.png "Inhalte mit der Checkbox ein- oder ausblenden")

----
Viele Browser verfügen heute über die Möglichkeit die Druckausgabe direkt in ein Pdf Dokument umzuleiten. Damit sind Sie in der Lage die angezeigten Inhalte auch auf elektronischem Wege weiterzugeben bzw. dauerhaft auf Ihrem Computer zu speichern

Hinweis: Die Zusammenfassung bietet je nach Konfigurationsumfang Ihrer L-mobile Anwendung weitere Inhalte zur Ausgabe an. 

----