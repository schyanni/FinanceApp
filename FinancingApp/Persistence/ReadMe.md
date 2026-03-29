# DB Migrations

- this project uses the Code-First approach

## Add new Migration
- open the Package Manager Console
- set the default project to 

`FinancingApp`

- use the command: 

`Add-Migration <MigrationName> -Context FinancingAppContext -OutPutDir Persistence/Migrations`