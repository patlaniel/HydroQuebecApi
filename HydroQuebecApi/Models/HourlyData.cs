using HydroQuebecApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class HourlyData
    {
        [JsonPropertyName("heure")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan Hour { get; set; }
        [JsonPropertyName("consoReg")]
        public double RegularPriceConsumption { get; set; }
        [JsonPropertyName("consoHaut")]
        public double HighPriceConsumption { get; set; }
        [JsonPropertyName("consoTotal")]
        public double TotalConsumption { get; set; }
        [JsonPropertyName("codeConso")]
        public string ConsumptionCode { get; set; }
        [JsonPropertyName("codeEvemenentEnergie")]
        public string EnergyEventCode { get; set; }
        [JsonPropertyName("zoneMessageHTMLEnergie")]
        public string EnergyHtmlMessageZone { get; set; }
    }

    public class HourlyCombinedData
    {
        [JsonPropertyName("codeTarif")]
        public string RateCode { get; set; }
        [JsonPropertyName("affichageTarifFlex")]
        public bool IsFlexRate { get; set; }
        [JsonPropertyName("dateJour")]
        public DateTime Date { get; set; }
        [JsonPropertyName("echelleMinKwhHeureParJour")]
        public int KwhScaleMin { get; set; }
        [JsonPropertyName("echelleMaxKwhHeureParJour")]
        public int KwhScaleMax { get; set; }
        [JsonPropertyName("zoneMsgHTMLNonDispEnergie")]
        public string EnergyNotAvailHtmlZoneMsge  { get; set; }
        [JsonPropertyName("zoneMsgHTMLNonDispPuissance")]
        public string PowerNotAvailHtmlZoneMsge { get; set; }
        [JsonPropertyName("indErreurJourneeEnergie")]
        public bool IsEnergyErrorIndicatorPresent { get; set; }
        [JsonPropertyName("indErreurJourneePuissance")]
        public bool IsPowerErrorIndicatorPresent { get; set; }
        [JsonPropertyName("listeDonneesConsoEnergieHoraire")]
        public IList<HourlyData> HourlyData { get; set; }

        public HourlyTemperatureData TemperatureData { get; set; }
    }
}
