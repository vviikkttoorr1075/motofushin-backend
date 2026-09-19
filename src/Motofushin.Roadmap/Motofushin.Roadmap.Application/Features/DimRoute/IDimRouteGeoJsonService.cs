namespace Motofushin.Roadmap.Application.Features.DimRoute;

/// <summary>
/// Read access to <see cref="Domain.DimRoute.Entities.IDimRoute"/> data
/// rendered as GeoJSON documents.
/// </summary>
public interface IDimRouteGeoJsonService
{
	/// <summary>
	/// Returns every route as a GeoJSON <c>Feature</c> string, ordered by <see cref="Domain.DimRoute.Entities.IDimRoute.Id"/>.
	/// Routes without points yield an empty <c>coordinates</c> array.
	/// </summary>
	Task<IReadOnlyList<string>> GetAllAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the GeoJSON <c>Feature</c> string of the route with the given id
	/// (with its points ordered by <see cref="Domain.DimRoute.Entities.RoutePoint.Seq"/>),
	/// or <see langword="null"/> when it does not exist.
	/// </summary>
	Task<string?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
