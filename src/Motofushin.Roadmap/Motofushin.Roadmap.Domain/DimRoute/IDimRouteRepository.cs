using Motofushin.Roadmap.Domain.DimRoute.Entities;

namespace Motofushin.Roadmap.Domain.DimRoute;

/// <summary>
/// Read access to <see cref="DimRoute"/> rows of the scraper data warehouse.
/// </summary>
public interface IDimRouteRepository
{
	/// <summary>
	/// Returns all routes ordered by <see cref="DimRoute.Id"/>.
	/// </summary>
	Task<IReadOnlyList<IDimRoute>> GetAllAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the route with the given id, or <see langword="null"/> when it does not exist.
	/// </summary>
	Task<IDimRoute?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the route with the given id together with its points ordered by
	/// <see cref="RoutePoint.Seq"/>, or <see langword="null"/> when it does not exist.
	/// </summary>
	Task<IDimRoute?> GetWithPointsByIdAsync(int id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the route identified by its source system and external id, or
	/// <see langword="null"/> when it does not exist.
	/// </summary>
	Task<IDimRoute?> GetBySourceAndExternalIdAsync(string source, string externalId, CancellationToken cancellationToken = default);
}
