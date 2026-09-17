dotnet ef migrations add AddUniqueIndexToUrl --project Motofushin.App.Infrastructure --startup-project Motofushin.App.Api

dotnet ef database update --project Motofushin.App.Infrastructure --startup-project Motofushin.App.Api