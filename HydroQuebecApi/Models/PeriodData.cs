using HydroQuebecApi.Infrastructure;
using System;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class PeriodData
    {
        [JsonPropertyName("numeroContrat")]
        public string ContractNumber { get; set; }
        [JsonPropertyName("dateFinPeriode")]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("dateDebutPeriode")] // "2021-11-20",
        public DateTime StartDate { get; set; }
        [JsonPropertyName("dateDerniereLecturePeriode")] // "2021-12-08",
        public DateTime? LastMeterReadDate { get; set; }
        [JsonPropertyName("heureDerniereLecturePeriode")] // "23:59:00",
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan LastMeterReadTime { get; set; }
        [JsonPropertyName("nbJourLecturePeriode")] // 19,
        public int DayReadCount { get; set; }
        [JsonPropertyName("nbJourPrevuPeriode")] // 63,
        public int DayCountForecast     { get; set; }
        [JsonPropertyName("consoTotalProjetePeriode")] // 7401,
        public int? TotalConsumptionForecast { get; set; }
        [JsonPropertyName("consoTotalPeriode")] // 2136,
        public int TotalConsumption { get; set; }
        [JsonPropertyName("consoHautPeriode")] // 0,
        public int HighPriceConsumption { get; set; }
        [JsonPropertyName("consoRegPeriode")] // 2136,
        public int RegularPriceConsumption { get; set; }
        [JsonPropertyName("moyenneKwhJourPeriode")] // 112.4,
        public double AverageDailyConsumption { get; set; }
        [JsonPropertyName("montantFacturePeriode")] // 213.11,
        public double Bill { get; set; }
        [JsonPropertyName("moyenneDollarsJourPeriode")] // 11.22,
        public double AverageDailyPrice { get; set; }
        [JsonPropertyName("codeConsPeriode")] // "A",
        public string ConsumptionCode;
        [JsonPropertyName("indMVEPeriode")] // Mode de versements égaux false,
        public bool IsEqualizedPaymentsPlan { get; set; }
        [JsonPropertyName("tempMoyennePeriode")] // -5,
        public int AverageTemperature;
        [JsonPropertyName("dateDebutPeriodeComparable")] // null,
        public DateTime? ComparePeriodStartDate { get; set; }
        [JsonPropertyName("indConsoAjusteePeriode")] // false,
        public bool IsAdjustedConsumption { get; set; }
        [JsonPropertyName("presenceCodeTarifDTPeriode")] // false,
        public bool IsDualEnergyRate;
        [JsonPropertyName("dernierTarif")] // "D",
        public string LastRateCode;
        [JsonPropertyName("presencePiscineExterieursPeriode")] // false,
        public bool IsOutsidePoolPresent { get; set; }
        [JsonPropertyName("indPresenceCodeEvenementPeriode")] // false,
        public bool IsEventCodePresent { get; set; }
        [JsonPropertyName("montantProjetePeriode")] // 741.5,
        public double? ForecastCost { get; set; }
        [JsonPropertyName("multiplicateurFacturation")] // 1.00,
        public double BillMultiplier    { get; set; }
        [JsonPropertyName("coutCentkWh")] // 9.98,
        public double CostCentKWh { get; set; }
    }
}
