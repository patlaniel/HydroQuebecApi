using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class HourlyTemperatureData
    {
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
        [JsonPropertyName("tempMoyJour")]
        public double AverageTemperature { get; set; }
        [JsonPropertyName("tempMinJour")]
        public double MinTemperature { get; set; }
        [JsonPropertyName("tempMaxJour")]
        public double MaxTemperature { get; set; }
        [JsonPropertyName("listeTemperaturesHeure")]
        public IList<int> HourlyTemperatures { get; set; }
        [JsonPropertyName("temperatureEstimee")]
        public bool IsTemperatureEstimated { get; set; }

    }
}
