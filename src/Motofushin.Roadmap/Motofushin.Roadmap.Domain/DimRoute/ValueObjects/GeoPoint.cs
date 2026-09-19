namespace Motofushin.Roadmap.Domain.DimRoute.ValueObjects;

/// <summary>
/// Geographic position of a route point: latitude/longitude in decimal degrees
/// and the elevation above sea level in meters (optional).
/// Immutable value object; equality is structural.
/// Invariants enforced at construction:
/// <list type="bullet">
/// <item><see cref="Lat"/> within [-90, +90] degrees;</item>
/// <item><see cref="Lon"/> within [-180, +180] degrees;</item>
/// <item><see cref="ElevationM"/>, when present, within [-430, 9000] meters.</item>
/// </list>
/// </summary>
public sealed record GeoPoint
{
	/// <summary>Наибольшая широта в градусах (северный полюс).</summary>
	public const decimal MaxLat = 90m;

	/// <summary>Наибольшая долгота в градусах.</summary>
	public const decimal MaxLon = 180m;


	private readonly decimal _lat;
	private readonly decimal _lon;
	private readonly decimal? _elevationM;

	public decimal Lat
	{
		get => _lat;
		init
		{
			Validate(value, _lon, _elevationM);
			_lat = value;
		}
	}

	public decimal Lon
	{
		get => _lon;
		init
		{
			Validate(_lat, value, _elevationM);
			_lon = value;
		}
	}

	public decimal? ElevationM
	{
		get => _elevationM;
		init
		{
			Validate(_lat, _lon, value);
			_elevationM = value;
		}
	}

	private GeoPoint(decimal lat, decimal lon, decimal? elevationM = null)
	{
		if (!IsValid(lat, lon, elevationM))
			throw new ArgumentOutOfRangeException($"Invalid GeoPoint: {lat}, {lon}, {elevationM}");
		_lat = lat;
		_lon = lon; 
		_elevationM = elevationM;
	}

	/// <summary>Есть ли у точки известная высота.</summary>
	public bool HasElevation => ElevationM is not null;

	/// <summary>
	/// Пытается создать точку; возвращает <c>false</c> вместо исключения,
	/// если координаты выходят за допустимые границы.
	/// </summary>
	public static bool TryCreate(decimal lat, decimal lon, decimal? elevationM, out GeoPoint? point)
	{
		if (!IsValid(lat, lon, elevationM))
		{
			point = null; 
			return false;
		}
		point = new GeoPoint(lat, lon, elevationM);
		return true;
	}

	/// <summary>Проверка инвариантов без создания объекта.</summary>
	private static bool IsValid(decimal lat, decimal lon, decimal? elevationM)
	{
		return lat is >= -MaxLat and <= MaxLat
			&& lon is >= -MaxLon and <= MaxLon
			&& (elevationM is null or >= -430m and <= 9000m);
	}

	/// <summary>Расстояние до другой точки по ортодромии, в метрах (формула гаверсинуса).</summary>
	public decimal DistanceTo(GeoPoint other)
	{
		ArgumentNullException.ThrowIfNull(other);
		return HaversineM(this, other);
	}

	/// <summary>Расстояние до другой точки, если оно не превышает <paramref name="maxM"/>, иначе <c>null</c>.</summary>
	public decimal? DistanceWithin(GeoPoint other, decimal maxM)
	{
		var d = DistanceTo(other);
		return d <= maxM ? d : null;
	}

	/// <summary>Находится ли другая точка не дальше <paramref name="maxM"/> метров (упрощённая плоская оценка).</summary>
	public bool IsNear(GeoPoint other, decimal maxM) => FlatDistanceM(this, other) <= maxM;

	/// <summary>Начальный азимут (bearing) до другой точки, в градусах [0, 360).</summary>
	public decimal BearingTo(GeoPoint other)
	{
		ArgumentNullException.ThrowIfNull(other);

		var lat1 = ToRad(Lat);
		var lat2 = ToRad(other.Lat);
		var dLon = ToRad(other.Lon - Lon);

		var y = Math.Sin(dLon) * Math.Cos(lat2);
		var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

		var deg = ToDeg(Math.Atan2(y, x));
		return (decimal)Math.Round((deg % 360 + 360) % 360, 1);
	}

	/// <summary>Компактное строковое представление: "lat, lon" (+ " @N м" при известной высоте).</summary>
	public override string ToString()
		=> ElevationM is { } e ? $"{Lat:0.####}, {Lon:0.####} @ {e:0.#} м" : $"{Lat:0.####}, {Lon:0.####}";

	private static void Validate(decimal lat, decimal lon, decimal? elevationM)
	{
		if (!IsValid(lat, lon, elevationM))
			throw new ArgumentOutOfRangeException(
				nameof(lat),
				$"Invalid GeoPoint: lat={lat}, lon={lon}, elevationM={elevationM}. " +
				$"Expected lat in [-90, 90], lon in [-180, 180], elevation in [-430, 9000] m.");
	}

	// формула гаверсинуса — точный расчёт расстояния по большому кругу
	private static decimal HaversineM(GeoPoint a, GeoPoint b)
	{
		//  средний радиус Земли, м
		const decimal EarthRadiusM = 6_371_000m;

		var dLat = ToRad(b.Lat - a.Lat);
		var dLon = ToRad(b.Lon - a.Lon);

		var h = Math.Pow(Math.Sin((double)dLat / 2), 2)
			+ Math.Cos(ToRad(a.Lat)) * Math.Cos(ToRad(b.Lat)) * Math.Pow(Math.Sin((double)dLon / 2), 2);
		var h1 = Math.Min(1.0, h);
		return EarthRadiusM * (decimal)(2 * Math.Asin(Math.Sqrt(h1)));
	}

	/// <summary>Приблизительное плоское расстояние (equirectangular), достаточно точное для коротких дистанций.</summary>
	private static decimal FlatDistanceM(GeoPoint a, GeoPoint b)
	{
		//  метров в одном градусе широты (постоянно с хорошей точностью)
		const decimal MetersPerDegLat = 111_320m;

		var midLatRad = ToRad((a.Lat + b.Lat) / 2m);
		var metersPerDegLon = MetersPerDegLat * (decimal)Math.Cos(midLatRad);

		var dLatM = (b.Lat - a.Lat) * MetersPerDegLat;
		var dLonM = (b.Lon - a.Lon) * metersPerDegLon;

		return (decimal)Math.Sqrt((double)(dLatM * dLatM + dLonM * dLonM));
	}

	private static double ToRad(decimal deg) => (double)deg * Math.PI / 180.0;
	private static double ToDeg(double rad) => rad * 180.0 / Math.PI;

	public void Deconstruct(out decimal lat, out decimal lon, out decimal? elevationM)
		=> (lat, lon, elevationM) = (_lat, _lon, _elevationM);
}
