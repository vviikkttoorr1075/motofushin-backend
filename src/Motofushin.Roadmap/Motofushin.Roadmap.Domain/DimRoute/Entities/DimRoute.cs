namespace Motofushin.Roadmap.Domain.DimRoute.Entities;

/// <summary>
/// A route imported into the scraper data warehouse.
/// </summary>
public class DimRoute: IDimRoute
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
	List<IRoutePoint> IDimRoute.Points {
		get => Points.Cast<IRoutePoint>().ToList();
		set => Points = value.Cast<RoutePoint>().ToList();
	}
}

