Name: Library
Host: aws-0-ap-southeast-1.pooler.supabase.com
Port: 5432
Database: postgres

COMMANDS:

dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:Supabase" "Host=aws-0-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username= ;Password= "