using Motofushin.Roadmap.Domain.DimRoute.ValueObjects;

namespace Motofushin.Roadmap.Domain.DimRoute.Entities;

/// <summary>
/// A single recorded point of a route, ordered by <see cref="Seq"/>.
/// </summary>
public class RoutePoint : IRoutePoint
{
    public int Id { get; set; }

    public DimRoute Route { get; set; } = null!;
    IDimRoute IRoutePoint.Route { get => Route; set => throw new NotImplementedException(); }

    public int Seq { get; set; }

    public GeoPoint Position { get; set; } = null!;

    public DateTimeOffset? RecordedAt { get; set; }
}
