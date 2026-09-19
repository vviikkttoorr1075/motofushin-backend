using Motofushin.Roadmap.Domain.DimRoute.ValueObjects;

namespace Motofushin.Roadmap.Domain.DimRoute.Entities;

/// <summary>
/// A single recorded point of a route, ordered by <see cref="Seq"/>.
/// </summary>
public interface IRoutePoint
{
    int Id { get; set; }

    IDimRoute Route { get; set; }

    int Seq { get; set; }

    GeoPoint Position { get; set; }

    DateTimeOffset? RecordedAt { get; set; }
}
