dotnet ef migrations add AddUniqueIndexToUrl --project Motofushin.Roadmap.Infrastructure --startup-project Motofushin.Roadmap.Api

dotnet ef database update --project Motofushin.Roadmap.Infrastructure --startup-project Motofushin.Roadmap.Api