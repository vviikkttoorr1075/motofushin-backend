using Motofushin.Roadmap.Domain.DimRoute.Entities;
using Motofushin.Roadmap.Domain.DimRoute.ValueObjects;

namespace Motofushin.Roadmap.Application.Features.DimRoute.Visitors;

/// <summary>
/// External visitor over the <see cref="IDimRoute"/> object graph, which consists of
/// the route itself, its <see cref="IDimRoute.Points"/> and the <see cref="GeoPoint"/>
/// position of every <see cref="IRoutePoint"/>.
/// The domain types expose no <c>Accept</c> methods, so implementations decide
/// themselves how to traverse the graph and what to produce for each node.
/// </summary>
/// <typeparam name="TResult">The type produced for each visited node.</typeparam>
public interface IRouteVisitor<out TResult>
{
	/// <summary>
	/// Visits a route.
	/// </summary>
	TResult Visit(IDimRoute route);

	/// <summary>
	/// Visits a single route point.
	/// </summary>
	TResult Visit(IRoutePoint point);

	/// <summary>
	/// Visits the geographic position of a route point.
	/// </summary>
	TResult Visit(GeoPoint position);
}
