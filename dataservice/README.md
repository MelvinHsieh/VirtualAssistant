# Dataservice

## Installation Process

1. Bevestig dat je Microsoft Server Management Studio hebt geïnstalleerd.
2. Build de Dataservice solution.
3. Open de NuGet Package Manager console.
4. Voer het command `Update-Database`. Specificeer hier ook welke context als uitgangspunt gebruikt moet worden.
5. Alle migrations die nog niet in de EfMigrationHistory tabel staan, worden nu uitgevoerd.