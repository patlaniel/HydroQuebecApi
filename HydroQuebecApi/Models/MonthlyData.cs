using System;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class MonthlyData: IEquatable<MonthlyData>
    {
        [JsonPropertyName("dateDebutMois")] public DateTime StartDate { get; set; }
        [JsonPropertyName("dateFinMois")] public DateTime EndDate { get; set; }

        [JsonPropertyName("codeConsoMois")] public string ConsumptionCode { get; set; }
        [JsonPropertyName("nbJourCalendrierMois")] public int DayCount { get; set; }
        [JsonPropertyName("presenceTarifDTmois")] public bool IsDualEnergyMonthlyRate { get; set; }
        [JsonPropertyName("tempMoyenneMois")] public int AverageTemperature { get; set; }
        [JsonPropertyName("moyenneKwhJourMois")] public double AverageKwhDay { get; set; }
        [JsonPropertyName("affichageTarifFlexMois")] public bool IsFlexRate { get; set; }
        [JsonPropertyName("consoRegMois")] public int RegularPriceConsumption { get; set; }
        [JsonPropertyName("consoHautMois")] public int HighPriceConsumption { get; set; }
        [JsonPropertyName("consoTotalMois")] public int TotalConsumption { get; set; }
        [JsonPropertyName("zoneMessageHTMLMois")] public string HtmlMessageZone { get; set; }
        [JsonPropertyName("indPresenceCodeEvenementMois")] public bool isEventPresent { get; set; }

        public bool Equals(MonthlyData other) => GetHashCode() == other.GetHashCode();
        public override bool Equals(object other) => Equals(other as MonthlyData);
        public override int GetHashCode() => StartDate.GetHashCode() ^ EndDate.GetHashCode();

    }
}
