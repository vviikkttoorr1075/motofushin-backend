# Правила для C# / .NET

## Контекст и токены
- Не читай целые файлы .cs, если нужна только структура. Сначала запрашивай сигнатуры типов и членов (outline), и только потом — конкретные строки (start_line/end_line).
- Для получения структуры C#-файла используй Roslyn-подход: перечисли типы, методы, свойства с их строками, без тел.
- Не загружай автоматически файлы из bin/, obj/, .vs/, packages/ (они в .clineignore) и не включай их в regex-поиск.
- При изменении кода генерируй минимальные диффы, а не полные перезаписи файлов.
- Не перечитывай один и тот же файл в рамках задачи; группируй независимые чтения/поиски в один запрос.
- Ответ держи кратким: в чат — только изменённые строки и итог, не дублируй большие куски кода.

## Карта репозитория (ориентируйся по ней, а не по листингу каталогов)
```
motofushin-backend/
├── motofushin-backend.sln        ← в solution 4 проекта: Motofushin.Roadmap.{Api,Application,Domain,Infrastructure}
├── src/
│   └── Motofushin.Roadmap/       ← эталонный модуль (Clean Architecture)
│       ├── ...Api/            ← Program.cs, Endpoints/, Filters/, Middleware/ — тонкий web-слой (net9.0)
│       ├── ...Application/    ← Features/<Фича>/ (напр. Features/Gpx/IGpxTrackStore.cs)
│       ├── ...Domain/         ← DimRoute/{Entities,ValueObjects}, Enums/, Exceptions/, Repositories/, Common/
│       └── ...Infrastructure/ ← Persistence/ (ScraperDwhDbContext), Migrations/, External/, Services/, DependencyInjection/
├── tests/                        ← тестовые проекты (сейчас пуст)
├── README.md                     ← команды EF-миграций
└── .clineignore, .editorconfig, Dockerfile
motofushin-plans/     ← требования и планы продукта (см. раздел «Планы»)
```

## Стратегия чтения и поиска
- Сначала точечный regex-поиск (имена типов/методов, строка из ошибки), и только затем чтение найденного по диапазонам строк.
- Ограничивай поиск каталогами src/ и tests/ — поиск по корню ловит мусор из .vs/ и motofushin-plans/.
- Никогда не читай целиком: Migrations/*.cs и *.Designer.cs (сгенерированы EF), *.g.cs, содержимое bin/obj/.vs.
- appsettings*.json, launchSettings.json, планы в motofushin-plans/ — читай только нужные секции/диапазоны.

## Правки
- Точечные правки (поиск-замена, вставка по строке); не переформатируй код, не относящийся к задаче.
- Предпочитай небольшие сфокусированные файлы и функции.
- Следуй существующим конвенциям проекта и .editorconfig.
- Не добавляй placeholder-логику и TODO в production-код.

## Сборка и проверка
- Собирай точечно: `dotnet build <путь>/<Проект>.csproj`; весь solution — только финальная проверка.
- Если пакеты не менялись, используй `--no-restore`.
- Тесты: `dotnet test <ТестовыйПроект>.csproj`; тестовых проектов пока нет — не запускай пустые команды.

## EF Core / миграции
- Схему смотри в Persistence/ и в DbContext; сгенерированные файлы миграций не читай.
- Миграции добавляй только при реальном изменении схемы; формат команд — в README.md (startup-проект в README устарел: сейчас это Motofushin.Roadmap.Api).
- Один DbContext на bounded context (сейчас: ScraperDwhDbContext в Roadmap.Infrastructure).

## ASP.NET Core / архитектура
- Каноническая структура: Domain ← Application ← Infrastructure; Api — тонкий слой без бизнес-логики.
- DI-регистрации модуля — extension-методы в DependencyInjection/, не Program.cs.
- Сначала читаемость, потом оптимизация.
- Конфигурация — централизованная и безопасная для окружений (User Secrets / env-переменные, секреты не в репозитории).

## Рост проекта (MVP мото-роутпланнера)
Проект разрастётся: профиль мотоцикла, радиусный планировщик, POI из OSM, покрытие (surface/smoothness), AI-планировщик, AI-описатель треков, GPS-запись трека, фото с гео-привязкой. Где что размещать:

| Фича MVP | Где код |
| --- | --- |
| Профиль мотоцикла | Motofushin.Motorcycle (Domain/Application/Infrastructure по образцу Roadmap) |
| Радиусный планировщик, GPX/треки | Motofushin.Roadmap — расширять Application/Features/, Domain/DimRoute |
| POI из OSM | отдельный модуль (напр. Motofushin.Poi) по образцу Roadmap |
| Покрытие (surface/smoothness) | OSM-клиент в Infrastructure/External, SurfaceType — ValueObject в Domain |
| AI-планировщик / описатель треков | LLM-клиент в Infrastructure/External; промпты и оркестрация — Application/Features |
| GPS-запись, фото с гео-привязкой | отдельный модуль (напр. Motofushin.Tracks/Media); приём треков — Application/Features/Gpx |

- Клиенты внешних систем (Overpass/OSM, OSRM, LLM) — только в Infrastructure/External; их контракты — интерфейсы в Application/Features.
- Общие гео-типы (координаты и т.п.) — один канонический источник; не дублируй модели между модулями (мотивация — в Docs/architecture.md).
- Domain — поведение и инварианты; аналитические/DWH-таблицы — плоские POCO без поведения.

## Планы и документы
- motofushin-plans/Product/MVP.md, Roadmap.md, Vision.md — источник требований; Docs/architecture.md — архитектурные решения.
- Код — истина по реализации, планы — по требованиям. Читай только раздел под текущую задачу, не все файлы подряд.
