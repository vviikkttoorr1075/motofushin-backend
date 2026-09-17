namespace Motofushin.Roadmap.Application.Features.Gpx;

/// <summary>
/// Read access to GPX tracks persisted by the application.
/// </summary>
public interface IGpxTrackStore
{
	/// <summary>
	/// Returns the names of all available GPX tracks,
	/// or <see langword="null"/> when the storage is unavailable.
	/// </summary>
	Task<IReadOnlyList<string>?> GetNamesAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the raw GPX content of the track with the given name,
	/// or <see langword="null"/> when it does not exist.
	/// </summary>
	Task<string?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}