using HydroQuebecApi.Infrastructure;
using System;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class PeriodData
    {
        [JsonPropertyName("numeroContrat")] public string ContractNumber { get; set; }
        [JsonPropertyName("dateFinPeriode")] public DateTime EndDate { get; set; }
        [JsonPropertyName("dateDebutPeriode")] public DateTime StartDate { get; set; }
        [JsonPropertyName("dateDerniereLecturePeriode")] public DateTime? LastMeterReadDate { get; set; }
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        [JsonPropertyName("heureDerniereLecturePeriode")] public TimeSpan LastMeterReadTime { get; set; }
        [JsonPropertyName("nbJourLecturePeriode")] public int DayReadCount { get; set; }
        [JsonPropertyName("nbJourPrevuPeriode")] public int DayCountForecast     { get; set; }
        [JsonPropertyName("consoTotalProjetePeriode")] public int? TotalConsumptionForecast { get; set; }
        [JsonPropertyName("consoTotalPeriode")] public int TotalConsumption { get; set; }
        [JsonPropertyName("consoHautPeriode")] public int HighPriceConsumption { get; set; }
        [JsonPropertyName("consoRegPeriode")] public int RegularPriceConsumption { get; set; }
        [JsonPropertyName("moyenneKwhJourPeriode")] public double AverageDailyConsumption { get; set; }
        [JsonPropertyName("montantFacturePeriode")] public double Bill { get; set; }
        [JsonPropertyName("moyenneDollarsJourPeriode")] public double AverageDailyPrice { get; set; }
        [JsonPropertyName("codeConsPeriode")] public string ConsumptionCode;
        [JsonPropertyName("indMVEPeriode")] public bool IsEqualizedPaymentsPlan { get; set; }
        [JsonPropertyName("tempMoyennePeriode")] public int AverageTemperature;
        [JsonPropertyName("dateDebutPeriodeComparable")] public DateTime? ComparePeriodStartDate { get; set; }
        [JsonPropertyName("indConsoAjusteePeriode")] public bool IsAdjustedConsumption { get; set; }
        [JsonPropertyName("presenceCodeTarifDTPeriode")] public bool IsDualEnergyRate;
        [JsonPropertyName("dernierTarif")] public string LastRateCode;
        [JsonPropertyName("presencePiscineExterieursPeriode")] public bool IsOutsidePoolPresent { get; set; }
        [JsonPropertyName("indPresenceCodeEvenementPeriode")] public bool IsEventCodePresent { get; set; }
        [JsonPropertyName("montantProjetePeriode")] public double? ForecastCost { get; set; }
        [JsonPropertyName("multiplicateurFacturation")] public double BillMultiplier    { get; set; }
        [JsonPropertyName("coutCentkWh")] public double CostCentKWh { get; set; }
    }
}
