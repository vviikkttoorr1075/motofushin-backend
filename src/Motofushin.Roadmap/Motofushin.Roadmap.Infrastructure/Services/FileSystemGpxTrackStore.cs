using Motofushin.Roadmap.Application.Features.Gpx;

namespace Motofushin.Roadmap.Infrastructure.Services;

/// <summary>
/// GPX track store reading track files (*.gpx) from a configured assets directory.
/// </summary>
public sealed class FileSystemGpxTrackStore() : IGpxTrackStore
{
	public async Task<IReadOnlyList<string>?> GetNamesAsync(CancellationToken cancellationToken = default)
	{
		return new List<string>([""]).AsReadOnly();
	}

	public async Task<string?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
	{
		return "";
	}

	private static bool ContainsPathTraversal(string name) =>
		name.Contains('/') || name.Contains('\\') || name.Contains("..");
}