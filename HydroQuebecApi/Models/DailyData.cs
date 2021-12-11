using System;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class DailyData
    {
        [JsonPropertyName("dateJourConso")]
        public DateTime Date { get; set; }
        [JsonPropertyName("zoneMessageHTMLQuot")]
        public string HtmlMessageZone { get; set; }

        [JsonPropertyName("consoRegQuot")]
        public double RegularPriceConsumption { get; set; }

        [JsonPropertyName("consoHautQuot")]
        public double HighPriceConsumption { get; set; }
        [JsonPropertyName("consoTotalQuot")]
        public double TotalConsumption { get; set; }
        [JsonPropertyName("codeConsoQuot")]
        public string ConsumptionCode { get; set; }
        [JsonPropertyName("tempMoyenneQuot")]
        public int AverageTemperature { get; set; }
        [JsonPropertyName("codeTarifQuot")]
        public string RateCode { get; set; }
        [JsonPropertyName("affichageTarifFlexQuot")]
        public bool IsFlexRate { get; set; }

        [JsonPropertyName("codeEvenementQuot")]
        public string EventCode { get; set; }
    }
}
