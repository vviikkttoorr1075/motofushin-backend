using System.Globalization;
using System.Text;
using Motofushin.Roadmap.Domain.DimRoute.Entities;
using Motofushin.Roadmap.Domain.DimRoute.ValueObjects;

namespace Motofushin.Roadmap.Application.Features.DimRoute.Visitors;

/// <summary>
/// Renders an <see cref="IDimRoute"/> as a GeoJSON <c>Feature</c> string whose
/// <c>LineString</c> geometry is built from the positions of its points:
/// <code>
/// {
///   "type": "Feature",
///   "properties": {},
///   "geometry": {
///     "type": "LineString",
///     "coordinates": [
///       [37.6173, 55.7558],
///       [37.6208, 55.7590]
///     ]
///   }
/// }
/// </code>
/// Coordinates follow the GeoJSON order <c>[lon, lat]</c> and are formatted
/// with the invariant culture; elevation is not included.
/// Line breaks are always <c>'\n'</c>, so the output is deterministic across platforms.
/// A route without points yields an empty <c>coordinates</c> array.
/// </summary>
public sealed class RouteGeoJsonVisitor : IRouteVisitor<string>
{
	/// <summary>Indent of a coordinate pair inside the <c>coordinates</c> array.</summary>
	private const string CoordinateIndent = "      ";

	/// <summary>
	/// Builds the GeoJSON <c>Feature</c> string for the whole route.
	/// </summary>
	public string Visit(IDimRoute route)
	{
		ArgumentNullException.ThrowIfNull(route);

		var builder = new StringBuilder();
		builder.Append("{\n");
		builder.Append("  \"type\": \"Feature\",\n");
		builder.Append("  \"properties\": {},\n");
		builder.Append("  \"geometry\": {\n");
		builder.Append("    \"type\": \"LineString\",\n");
		builder.Append("    \"coordinates\": [\n");
		builder.Append(
			string.Join(",\n", route.Points.Select(point => CoordinateIndent + Visit(point))));
		builder.Append('\n');
		builder.Append("    ]\n");
		builder.Append("  }\n");
		builder.Append('}');

		return builder.ToString();
	}

	/// <summary>
	/// Renders a single route point as its <c>[lon, lat]</c> coordinate pair.
	/// </summary>
	public string Visit(IRoutePoint point)
	{
		ArgumentNullException.ThrowIfNull(point);
		return Visit(point.Position);
	}

	/// <summary>
	/// Renders a geographic position as a <c>[lon, lat]</c> coordinate pair
	/// formatted with the invariant culture.
	/// </summary>
	public string Visit(GeoPoint position)
	{
		ArgumentNullException.ThrowIfNull(position);
		return string.Create(CultureInfo.InvariantCulture, $"[{position.Lon}, {position.Lat}]");
	}
}
