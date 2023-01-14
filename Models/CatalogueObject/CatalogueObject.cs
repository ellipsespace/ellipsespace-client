using System.Text.Json;
using System.Text.Json.Serialization;

namespace EllipseSpaceClient.Models.CatalogueObject
{
	public class CatalogueObject
    {
		// Название спутника
		[JsonPropertyName("name")]
		public string Name { get; set; }
		// Подробное описание
		[JsonPropertyName("description")]
		public string Description { get; set; }
		// Дата обнаружения EllipseSpace
		[JsonPropertyName("o-date-time")]
		public string OpeningDateTime { get; set; }
        // Сидерический период обращения
        [JsonPropertyName("s-conversion-period")]
		public float SidericConversionPeriod { get; set; }
		// Орбитальная скорость
		[JsonPropertyName("orbital-vel")]
		public float BodyOrbitalVelocity { get; set; }
		// Наклонение
		[JsonPropertyName("inclination")]
		public float Inclination { get; set; }
		// Спутники
		[JsonPropertyName("satelites")]
		public string[] Satelites { get; set; }
		// Чей спутник
		[JsonPropertyName("whose-satelite")]
		public string WhoseSatelite { get; set; }
		// Экваториальный радиус
		[JsonPropertyName("equator-radius")]
		public float EquatorialRadius { get; set; }
		// Полярный радиус
		[JsonPropertyName("polar-radius")]
		public float PolarRadius { get; set; }
		// Средний радиус
		[JsonPropertyName("avg-radius")]
		public float AverageRadius { get; set; }
		// Площадь
		[JsonPropertyName("s")]
		public decimal Square { get; set; }
		// Объем
		[JsonPropertyName("v")]
		public decimal Volume { get; set; }
		// Масса
		[JsonPropertyName("m")]
		public decimal Weight { get; set; }
		// Средняя плотность
		[JsonPropertyName("p")]
		public float AverageDensity { get; set; }
		// Ускорение свободного падения
		[JsonPropertyName("g")]
		public float GravityAcceleration { get; set; }
		// Первая космическая скорость
		[JsonPropertyName("v1")]
		public float FirstSpaceVelocity { get; set; }
		// Вторая космическая скорость
		[JsonPropertyName("v2")]
		public float SecondSpaceVelocity { get; set; }
		//Фотографии
		[JsonPropertyName("photos")]
		public string[] Photos { get; set; }

		public CatalogueObject()
		{

		}

		internal string Marshal()
		{
			return JsonSerializer.Serialize(this);
		}

		internal static CatalogueObject? Unmarshal(string json)
		{
			return JsonSerializer.Deserialize<CatalogueObject>(json);
		}

		internal static CatalogueObject[]? UnmarshalArray(string json)
		{
			return JsonSerializer.Deserialize<CatalogueObject[]>(json);
		}
    }
}
