namespace Motofushin.App.Infrastructure.Persistence.Entities;

/// <summary>
/// A route imported into the scraper data warehouse.
/// </summary>
public class DimRoute
{
	public int Id { get; set; }

	public string Source { get; set; } = null!;

	public string ExternalId { get; set; } = null!;

	public string? Name { get; set; }

	//  длина трека в метрах
	public decimal? LengthM { get; set; }

	//  количество точек в треке
	public int? PointCount { get; set; }

	public DateTimeOffset CreatedAt { get; set; }

	public List<RoutePoint> Points { get; set; } = [];
}

/// <summary>
/// A single recorded point of a route, ordered by <see cref="Seq"/>.
/// </summary>
public class RoutePoint
{
	public int Id { get; set; }

	public DimRoute Route { get; set; } = null!;

	public int Seq { get; set; }

	public decimal Lat { get; set; }

	public decimal Lon { get; set; }

	//Высота точки над уровнем моря в метрах
	public decimal? ElevationM { get; set; }

	public DateTimeOffset? RecordedAt { get; set; }
}
