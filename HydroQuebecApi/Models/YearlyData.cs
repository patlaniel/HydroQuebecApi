using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HydroQuebecApi.Models
{
    public class YearlyData
    {
        [JsonPropertyName("dateDebutAnnee")] public DateTime StartDate { get; set; }
        [JsonPropertyName("dateFinAnnee")] public DateTime EndDate { get; set; }
        [JsonPropertyName("nbJourCalendrierAnnee")] public int DayCount { get; set; }
        [JsonPropertyName("moyenneKwhJourAnnee")] public double AverageDaylyKwh { get; set; }
        [JsonPropertyName("consoRegAnnee")] public int RegularPriceConsumption { get; set; }
        [JsonPropertyName("consoHautAnnee")] public int HighPriceConsumption { get; set; }
        [JsonPropertyName("consoTotalAnnee")] public int TotalConsumption { get; set; }
        [JsonPropertyName("montantFactureAnnee")] public double Bill { get; set; }
        [JsonPropertyName("moyenneDollarsJourAnnee")] public double AverageDaylyCost { get; set; }
        [JsonPropertyName("coutCentkWh")] public double CostCentKWh { get; set; }


        public YearlyData Clone() => (YearlyData)this.MemberwiseClone();
    }
}
