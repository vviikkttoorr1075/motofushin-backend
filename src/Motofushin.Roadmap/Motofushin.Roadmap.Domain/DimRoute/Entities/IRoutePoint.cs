namespace Motofushin.Roadmap.Domain.DimRoute.Entities;

/// <summary>
/// A single recorded point of a route, ordered by <see cref="Seq"/>.
/// </summary>
public interface IRoutePoint
{
    int Id { get; set; }

    IDimRoute Route { get; set; }

    int Seq { get; set; }

    decimal Lat { get; set; }

    decimal Lon { get; set; }

    //Высота точки над уровнем моря в метрах
    decimal? ElevationM { get; set; }

    DateTimeOffset? RecordedAt { get; set; }
}
