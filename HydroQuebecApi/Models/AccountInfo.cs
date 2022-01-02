using System;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class AccountInfo : IEquatable<AccountInfo>
    {
        [JsonPropertyName("noPartenaireDemandeur")] public string accountId { get; set; } = null;
        [JsonPropertyName("noPartenaireTitulaire")] public string customerId { get; set; } = null;

        public AccountInfo(string accountId, string customerId)
        {
            this.accountId = accountId;
            this.customerId = customerId;
        }

        public override int GetHashCode() => accountId?.GetHashCode() ?? 0 ^ customerId?.GetHashCode() ?? 0;
        public bool Equals(AccountInfo other) => other.GetHashCode() == GetHashCode();
    }
}
