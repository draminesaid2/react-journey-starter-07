{% include '_meta.md' %}

{% include '_base.md' %}

# Anmeldung an das System {#login}

Sie starten die Anwendung durch Aufruf der Adresse unter der die Anwendung für Sie bereitgestellt wurde. Nachdem Sie die Seite in Ihrem Browser geöffnet haben erfolgt die Anmeldung mit Hilfe des bereitgestellten Anmeldeformulars.

![Anmeldeformular - integrierte Benutzerverwaltung](img/login.png "Anmeldeformular - integrierte Benutzerverwaltung")

---
__Hinweis__ Die Anmeldung erfolgt wahlweise über die integrierte L-mobile Benutzerverwaltung (via E-Mail und Passwort) oder über die Integration eines LDAP Servers. In diesem Fall erfolgt die Anmeldung mit Ihren bekannten Windows Anmeldedaten und dem auf dem LDAP System zugeordneten Passwort.

---

Wenn Ihr Benutzer Zugriff auf verschiedene Clients des L-mobile CRM/Service hat, gelangen Sie nach der Anmeldung in die Clientauswahl. Je nach aktiven Plugins können Sie Auswahl zwischen Mobiles CRM, Backend, Techniker-Client und der elektronischen Plantafel

![Clientauswahl](img/ClientSelection.png "Clientauswahl nach dem Login")

# Dashboard {#dashboard}

Das Dashboard ist die zentrale Anlaufstelle des Crm/Service Systems. Hier findet sich eine chronologische Auflistung der im System angefallenen Informationen. Dabei werden die Informationen in einen Aktivitätsstrom zusammengefasst. Dieser Aktivitätsstrom setzt sich aus einzelnen Informationen zusammen die auf dem Dashboard konsolidiert werden.

Zu jeder Information werden unterschiedliche Metadaten angezeigt:

- Zu welchem Objekt gehört die Information
- Welche Art von Information wurde erfasst
- Wer hat die Information erfasst/ausgelöst
- Wann wurde die Information erfasst/ausgelöst

![Alle Informationen laufen im Dashboard zusammen](img/dashboard.png "Alle Informationen laufen im Dashboard zusammen")

---
__Hinweis__ In der Liste der Informationen werden nur Informationen zu Objekten angezeigt auf die der aktuell angemeldete Benutzer Zugriff hat. Bei einer vorhandenen Einschränkung der Sichtbarkeit von Objekten erfolgt ebenso eine Einschränkung der Informationen im Dashboard.

---

Für die individuellen Informationsarten werden unterschiedliche Symbole verwendet. Je nach Menge der aktivierten Module im L-mobile Crm/Service System kann die Darstellung hier unterschiedlich ausfallen.

Symbol|Bedeutung
------:|:-----
![](img/note_company.png) | Notizen zu Firmen
![](img/note_person.png) | Notizen zu Personen
![](img/note_project.png) | Notizen zu Projekten
![](img/note_servicecase.png) | Notizen zu Servicefällen (bei aktivem Servicemodul)
![](img/note_serviceorder.png) | Notizen zu Serviceaufträgen (bei aktivem Servicemodul)
![](img/note_new.png) | Vermerk über neu angelegte Datensätze
![](img/note_status.png) | Statusänderungen
![](img/note_task.png) | Erledigte Aufgaben

Im Mobilen CRM (Material Client) gibt es diese chronologische Auflistung noch nicht.

Dort sieht das Dashboard so aus:

![Alle Informationen laufen im Dashboard zusammen](img/navigation_mobile_crm_dashboard_material_client.png)

<!-- BEGIN MANUAL -->

{% include 'contacts.md' %}

{% include 'contact_details.md' %}

{% include 'company.md' %}

{% include 'person.md' %}

{% include 'tags.md' %}

{% include 'tasks.md' %}

{% include 'apps.md' %}

{% include 'user_manager.md' %}

{% include 'my_info.md' %}

{% include 'mobile-crm.md' %}

<!-- END MANUAL -->
