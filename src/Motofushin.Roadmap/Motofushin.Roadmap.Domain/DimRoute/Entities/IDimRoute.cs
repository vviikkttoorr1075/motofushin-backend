namespace Motofushin.Roadmap.Domain.DimRoute.Entities;

/// <summary>
/// A route imported into the scraper data warehouse.
/// </summary>
public interface IDimRoute
{
	int Id { get; set; }

	string Source { get; set; }

	string ExternalId { get; set; }

	string? Name { get; set; }

	//  длина трека в метрах
	decimal? LengthM { get; set; }

	//  количество точек в треке
	int? PointCount { get; set; }

	DateTimeOffset CreatedAt { get; set; }

	List<IRoutePoint> Points { get; set; }
}

