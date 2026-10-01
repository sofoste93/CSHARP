using System.Globalization;

namespace TaskManagerApp.Services;

public sealed class AppText
{
    private static readonly Dictionary<string, Dictionary<string, string>> Languages = new()
    {
        ["en"] = new()
        {
            ["AppName"] = "Violet Pulsar", ["Tagline"] = "Focus in motion", ["Home"] = "Home", ["Tasks"] = "Mission board", ["Privacy"] = "Privacy", ["Settings"] = "Settings",
            ["Language"] = "Language", ["Theme"] = "Appearance", ["ThemeHelp"] = "Choose the light level of your cockpit.", ["Dark"] = "Deep space", ["Light"] = "Starlight", ["System"] = "System",
            ["Density"] = "Compact layout", ["DensityHelp"] = "Show more tasks with less spacing.", ["Motion"] = "Reduced motion", ["MotionHelp"] = "Disable orbital interface animation.", ["SaveSettings"] = "Save settings", ["Close"] = "Close",
            ["Welcome"] = "Welcome back", ["HeroTitle"] = "Turn intent into momentum.", ["HeroText"] = "A private mission board for the work that matters. Plan clearly, move deliberately, finish strong.",
            ["OpenBoard"] = "Open mission board", ["CreateAccount"] = "Create free account", ["SignIn"] = "Sign in", ["LocalPrivate"] = "Private workspace", ["LocalPrivateText"] = "Every account sees only its own missions.",
            ["Fast"] = "Built for flow", ["FastText"] = "Search, filter and update without losing context.", ["CrossPlatform"] = "Runs everywhere", ["CrossPlatformText"] = "Bundled releases for Windows, Linux and macOS.",
            ["Dashboard"] = "Mission control", ["DashboardEyebrow"] = "PERSONAL ORBIT", ["DashboardIntro"] = "Capture work, set its gravity and keep the next move visible.", ["NewTask"] = "New mission",
            ["Total"] = "Total", ["Open"] = "Open", ["Completed"] = "Completed", ["Overdue"] = "Overdue", ["All"] = "All", ["Search"] = "Search missions", ["SearchPlaceholder"] = "Title or description…",
            ["Newest"] = "Newest", ["DueSoon"] = "Due soon", ["PrioritySort"] = "Priority", ["Apply"] = "Apply", ["NoTasks"] = "Your orbit is clear.", ["NoTasksText"] = "Create your first mission and give today a direction.",
            ["Created"] = "Created", ["Due"] = "Due", ["NoDueDate"] = "No due date", ["Edit"] = "Edit", ["Details"] = "Details", ["Delete"] = "Delete", ["MarkDone"] = "Complete", ["Reopen"] = "Reopen",
            ["CreateTitle"] = "Create a mission", ["EditTitle"] = "Edit mission", ["DetailsTitle"] = "Mission details", ["DeleteTitle"] = "Delete mission", ["Title"] = "Title", ["Description"] = "Description",
            ["Priority"] = "Priority", ["Status"] = "Status", ["Save"] = "Save mission", ["Update"] = "Update mission", ["Cancel"] = "Cancel", ["Back"] = "Back to board", ["ConfirmDelete"] = "Delete this mission permanently?",
            ["DeleteWarning"] = "This action cannot be undone.", ["Low"] = "Low", ["Normal"] = "Normal", ["High"] = "High", ["Critical"] = "Critical", ["TaskCreated"] = "Mission created.", ["TaskUpdated"] = "Mission updated.",
            ["TaskDeleted"] = "Mission deleted.", ["TaskCompleted"] = "Mission completed.", ["TaskReopened"] = "Mission reopened.", ["RequiredTitle"] = "A title is required.", ["SignedInAs"] = "Signed in as", ["Account"] = "Account",
            ["Logout"] = "Sign out", ["Register"] = "Register", ["Login"] = "Sign in", ["Footer"] = "Private task orbit", ["PrivacyTitle"] = "Privacy by design",
            ["PrivacyText"] = "Violet Pulsar stores account and task data only in its local SQLite database. It includes no analytics, advertising or third-party tracking.", ["DataControl"] = "Your data",
            ["DataControlText"] = "Tasks are isolated per account. Delete a mission at any time from your board.", ["Network"] = "Network", ["NetworkText"] = "The application can run entirely on your device and does not require a cloud service."
        },
        ["de"] = new()
        {
            ["AppName"] = "Violet Pulsar", ["Tagline"] = "Fokus in Bewegung", ["Home"] = "Start", ["Tasks"] = "Missionsboard", ["Privacy"] = "Datenschutz", ["Settings"] = "Einstellungen",
            ["Language"] = "Sprache", ["Theme"] = "Darstellung", ["ThemeHelp"] = "Wähle die Helligkeit deines Cockpits.", ["Dark"] = "Tiefer Raum", ["Light"] = "Sternenlicht", ["System"] = "System",
            ["Density"] = "Kompaktes Layout", ["DensityHelp"] = "Zeigt mehr Aufgaben mit weniger Abstand.", ["Motion"] = "Weniger Bewegung", ["MotionHelp"] = "Deaktiviert die orbitalen Animationen.", ["SaveSettings"] = "Einstellungen speichern", ["Close"] = "Schließen",
            ["Welcome"] = "Willkommen zurück", ["HeroTitle"] = "Aus Absicht wird Dynamik.", ["HeroText"] = "Ein privates Missionsboard für die Arbeit, die zählt. Klar planen, bewusst handeln, stark abschließen.",
            ["OpenBoard"] = "Missionsboard öffnen", ["CreateAccount"] = "Konto erstellen", ["SignIn"] = "Anmelden", ["LocalPrivate"] = "Privater Arbeitsraum", ["LocalPrivateText"] = "Jedes Konto sieht nur seine eigenen Missionen.",
            ["Fast"] = "Für den Flow gebaut", ["FastText"] = "Suchen, filtern und aktualisieren, ohne den Kontext zu verlieren.", ["CrossPlatform"] = "Überall verfügbar", ["CrossPlatformText"] = "Pakete für Windows, Linux und macOS.",
            ["Dashboard"] = "Missionskontrolle", ["DashboardEyebrow"] = "PERSÖNLICHER ORBIT", ["DashboardIntro"] = "Arbeit erfassen, Gewicht vergeben und den nächsten Schritt sichtbar halten.", ["NewTask"] = "Neue Mission",
            ["Total"] = "Gesamt", ["Open"] = "Offen", ["Completed"] = "Erledigt", ["Overdue"] = "Überfällig", ["All"] = "Alle", ["Search"] = "Missionen suchen", ["SearchPlaceholder"] = "Titel oder Beschreibung…",
            ["Newest"] = "Neueste", ["DueSoon"] = "Bald fällig", ["PrioritySort"] = "Priorität", ["Apply"] = "Anwenden", ["NoTasks"] = "Dein Orbit ist frei.", ["NoTasksText"] = "Erstelle deine erste Mission und gib dem Tag eine Richtung.",
            ["Created"] = "Erstellt", ["Due"] = "Fällig", ["NoDueDate"] = "Kein Termin", ["Edit"] = "Bearbeiten", ["Details"] = "Details", ["Delete"] = "Löschen", ["MarkDone"] = "Erledigen", ["Reopen"] = "Wieder öffnen",
            ["CreateTitle"] = "Mission erstellen", ["EditTitle"] = "Mission bearbeiten", ["DetailsTitle"] = "Missionsdetails", ["DeleteTitle"] = "Mission löschen", ["Title"] = "Titel", ["Description"] = "Beschreibung",
            ["Priority"] = "Priorität", ["Status"] = "Status", ["Save"] = "Mission speichern", ["Update"] = "Mission aktualisieren", ["Cancel"] = "Abbrechen", ["Back"] = "Zurück zum Board", ["ConfirmDelete"] = "Diese Mission dauerhaft löschen?",
            ["DeleteWarning"] = "Diese Aktion kann nicht rückgängig gemacht werden.", ["Low"] = "Niedrig", ["Normal"] = "Normal", ["High"] = "Hoch", ["Critical"] = "Kritisch", ["TaskCreated"] = "Mission erstellt.", ["TaskUpdated"] = "Mission aktualisiert.",
            ["TaskDeleted"] = "Mission gelöscht.", ["TaskCompleted"] = "Mission abgeschlossen.", ["TaskReopened"] = "Mission wieder geöffnet.", ["RequiredTitle"] = "Ein Titel ist erforderlich.", ["SignedInAs"] = "Angemeldet als", ["Account"] = "Konto",
            ["Logout"] = "Abmelden", ["Register"] = "Registrieren", ["Login"] = "Anmelden", ["Footer"] = "Privater Aufgabenorbit", ["PrivacyTitle"] = "Datenschutz als Prinzip",
            ["PrivacyText"] = "Violet Pulsar speichert Konto- und Aufgabendaten ausschließlich in der lokalen SQLite-Datenbank. Keine Analyse, Werbung oder Drittanbieter-Verfolgung.", ["DataControl"] = "Deine Daten",
            ["DataControlText"] = "Aufgaben sind pro Konto getrennt. Jede Mission kann jederzeit gelöscht werden.", ["Network"] = "Netzwerk", ["NetworkText"] = "Die Anwendung läuft vollständig auf deinem Gerät und benötigt keinen Cloud-Dienst."
        }
    };

    public string this[string key]
    {
        get
        {
            var language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (!Languages.TryGetValue(language, out var selected)) selected = Languages["en"];
            return selected.TryGetValue(key, out var value) ? value : Languages["en"].GetValueOrDefault(key, key);
        }
    }
}
