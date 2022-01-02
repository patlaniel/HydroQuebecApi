using HydroQuebecApi.Infrastructure;
using HydroQuebecApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace HydroQuebecApi
{

    public class Client : IDisposable
    {
        #region URL constants
        private static readonly string HOST_LOGIN = "https://connexion.hydroquebec.com";
        private static readonly string HOST_SESSION = "https://session.hydroquebec.com";
        private static readonly string HOST_SERVICES = "https://cl-services.idp.hydroquebec.com";
        private static readonly string HOST_SPRING = "https://cl-ec-spring.hydroquebec.com";

        // HOST_LOGIN
        private static readonly string LOGIN_URL_1 = HOST_LOGIN + "/hqam/XUI/";
        private static readonly string LOGIN_URL_2 = HOST_LOGIN + "/hqam/json/serverinfo/*";
        private static readonly string LOGIN_URL_3 = HOST_LOGIN + "/hqam/json/realms/root/realms/clients/authenticate";
        private static readonly string LOGIN_URL_5 = HOST_LOGIN + "/hqam/oauth2/authorize";

        // HOST_SESSION
        private static readonly string LOGIN_URL_4 = HOST_SESSION + "/config/security.json";

        // HOST_SERVICES
        private static readonly string LOGIN_URL_6 = HOST_SERVICES + "/cl/prive/api/v3_0/conversion/codeAcces";
        private static readonly string LOGIN_URL_7 = HOST_SERVICES + "/cl/prive/api/v1_0/relations";

        // Contract URLs
        private static readonly string CONTRACT_URL_1 = HOST_SERVICES + "/cl/prive/api/v3_0/partenaires/infoBase";
        private static readonly string CONTRACT_URL_2 = HOST_SPRING + "/portail/prive/maj-session/";
        private static readonly string CONTRACT_URL_3 = HOST_SPRING + "/portail/fr/group/clientele/gerer-mon-compte/";

        // Data URLs
        private static readonly string CONSUMPTION_PORTAL_BASE_URL = HOST_SPRING + "/portail/fr/group/clientele/portrait-de-consommation/";
        private static readonly string PERIOD_DATA_URL = CONSUMPTION_PORTAL_BASE_URL + "resourceObtenirDonneesPeriodesConsommation";
        private static readonly string MONTHLY_DATA_URL = CONSUMPTION_PORTAL_BASE_URL + "resourceObtenirDonneesConsommationMensuelles";
        private static readonly string YEARLY_DATA_URL = CONSUMPTION_PORTAL_BASE_URL + "resourceObtenirDonneesConsommationAnnuelles";
        private static readonly string DAILY_DATA_URL = CONSUMPTION_PORTAL_BASE_URL + "resourceObtenirDonneesQuotidiennesConsommation";
        private static readonly string HOURLY_DATA_URL = CONSUMPTION_PORTAL_BASE_URL + "resourceObtenirDonneesConsommationHoraires";
        private static readonly string HOURLY_TEMPERATURE_DATA_URL = CONSUMPTION_PORTAL_BASE_URL + "resourceObtenirDonneesMeteoHoraires";
        #endregion

        private List<string> customers = new List<string>();
        private HttpClient httpClient = new HttpClient();
        private string currentAccoutId = null;
        private string currentCustomerId = null;
        private Guid guid = Guid.NewGuid();

        public IList<AccountInfo> Accounts { get; private set; } = new List<AccountInfo>();

        public Client()
        {
            httpClient.Timeout = TimeSpan.FromMinutes(3);
        }
        public void Dispose()
        {
            if (httpClient is not null)
            {
                httpClient.Dispose();
            }
        }

        public async Task LoginAsync(string username, string password)
        {
            Reset();

            try
            {
                //Get the callback template
                Dictionary<string, string> headers = new Dictionary<string, string>()
                {
                    ["X-NoSession"] = "true",
                    ["X-Password"] = "anonymous",
                    ["X-Requested-With"] = "XMLHttpRequest",
                    ["X-Username"] = "anonymous"
                };

                string responseStr = await httpClient.HttpRequest<string>(HttpMethod.Post, LOGIN_URL_3);
                const string IDToken1Value = "\"name\":\"IDToken1\",\"value\"";
                const string IDToken2Value = "\"name\":\"IDToken2\",\"value\"";

                var requestStr = responseStr.Replace($"{{{IDToken1Value}:\"\"}}", $"{{{IDToken1Value}:\"{username}\"}}")
                                            .Replace($"{{{IDToken2Value}:\"\"}}", $"{{{IDToken2Value}:\"{password}\"}}");


                responseStr = await httpClient.HttpRequest<string>(HttpMethod.Post, LOGIN_URL_3, headers, new StringContent(requestStr, Encoding.UTF8, "application/json"));

                if (!JsonDocument.Parse(responseStr).RootElement.TryGetProperty("tokenId", out JsonElement tokenId))
                {
                    throw new Exception("Unable to authenticate - No tokenId in response");
                }

                // Find settings for the authorize
                var responseJson = await httpClient.HttpGetRequest<SecConfig>(LOGIN_URL_4, null, null);

                var state = String.Empty.AddRandom(40);
                Dictionary<string, string> queries = new Dictionary<string, string>()
                {
                    ["response_type"] = "id_token token",
                    ["client_id"] = responseJson.oauth2[0].clientId,
                    ["state"] = state,
                    ["redirect_uri"] = responseJson.oauth2[0].redirectUri,
                    ["scope"] = responseJson.oauth2[0].scope,
                    ["nonce"] = state,
                    ["locale"] = "en"
                };

                var redirectedRequest = await httpClient.HttpGetRequest<HttpRequestMessage>(LOGIN_URL_5, queries, null);

                // Get the token from redirected URL
                // The previous HTTP request has automatically been redirected to an URL that contains 
                // the access token in the query string
                if (String.IsNullOrEmpty(redirectedRequest.RequestUri.Fragment))
                {
                    throw new Exception("Query string empty - unable to get the access token");
                }
                var returnedQueries = HttpUtility.ParseQueryString(redirectedRequest.RequestUri.Fragment);
                if (string.IsNullOrEmpty(returnedQueries["#access_token"]))
                {
                    throw new Exception("Access token not returned!");
                }
                httpClient.SetAccessToken(returnedQueries["#access_token"]);


                await httpClient.HttpGetRequest<string>(LOGIN_URL_6, null, null);

                // Get customers
                Accounts = await httpClient.HttpGetRequest<IList<AccountInfo>>(LOGIN_URL_7, null, null);
                if (Accounts.Count == 0)
                {
                    throw new Exception("No account found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed  - reason: {ex.Message}");
            }

        }


        public async Task SelectCustomerByIndexAsync(int index, bool force = false)
        {
            if (Accounts.Count == 0)
            {
                throw new Exception("Not logged in (account list empty)");
            }

            if (index >= Accounts.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            await SelectCustomerAsync(Accounts[index].accountId, Accounts[index].customerId, force);
        }

        /// <summary>
        /// Select a customer on the Home page. Equivalent to a click on the Home page customer box
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="customerId"></param>
        public async Task SelectCustomerAsync(string accountId, string customerId, bool force = false)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                throw new ArgumentNullException(nameof(accountId));
            }

            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            if (Accounts.Count == 0)
            {
                throw new Exception("Not logged in (account list empty)");
            }

            if (!Accounts.Contains(new AccountInfo(accountId, customerId)))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (accountId.Equals(currentAccoutId) && customerId.Equals(currentCustomerId) && !force)
            {
                return;
            }

            var now = DateTime.Now;

            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                ["NO_PARTENAIRE_DEMANDEUR"] = accountId,
                ["NO_PARTENAIRE_TITULAIRE"] = customerId,
                ["DATE_DERNIERE_VISITE"] = $"{now.ToString("yyyy-MM-dd")}T{now.ToString("HH:mm:ss.00+0000")}",  //.strftime("%Y-%m-%dT%H:%M:%S.000+0000"),
                ["GUID_SESSION"] = guid.ToString()
            };

            await httpClient.HttpGetRequest<string>(CONTRACT_URL_1, null, headers);
            await httpClient.HttpGetRequest<string>(CONTRACT_URL_2, new Dictionary<string, string>() { ["mode"] = "web" }, headers);

            //load overview page
            await httpClient.HttpGetRequest<string>(CONTRACT_URL_3, null, null);
            // load consumption profile page
            await httpClient.HttpGetRequest<string>(CONSUMPTION_PORTAL_BASE_URL, null, null);

            currentAccoutId = accountId;
            currentCustomerId = customerId;
        }
        public async Task<IList<PeriodData>> fetchPeriodDataAsync()
        {
            var results = await httpClient.HttpGetRequest<ResultArrayTemplate<PeriodData>>(PERIOD_DATA_URL, null, null);
            return results.results;
        }

        public async Task<IList<ResultWithComparison<YearlyData>>> FetchYearlyDataAsync() => await FetchDataWithComparisonAsync<YearlyData>(YEARLY_DATA_URL);
        public async Task<IList<ResultWithComparison<MonthlyData>>> FetchMonthlyDataAsync() => await FetchDataWithComparisonAsync<MonthlyData>(MONTHLY_DATA_URL);

        public async Task<IList<ResultWithComparison<DailyData>>> FetchDailyDataAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (endDate.HasValue && !startDate.HasValue)
            {
                throw new ArgumentNullException(nameof(startDate));
            }
            Dictionary<string, string> queries = new Dictionary<string, string>()
            {
                ["dateDebut"] = (startDate ?? DateTime.Today.AddDays(-1)).ToString("yyyy-MM-dd"),
                ["dateFin"] = (endDate ?? DateTime.Today).ToString("yyyy-MM-dd")
            };
            return await FetchDataWithComparisonAsync<DailyData>(DAILY_DATA_URL, queries);
        }

        public async Task<HourlyCombinedData> FetchHourlyDataAsync(DateTime? date = null)
        {
            // Get the hourly consumption data
            Dictionary<string, string> queries = new Dictionary<string, string>()
            {
                ["date"] = (date ?? DateTime.Today.AddDays(-1)).ToString("yyyy-MM-dd")
            };
            var results1 = await httpClient.HttpGetRequest<ResultTemplate<HourlyCombinedData>>(HOURLY_DATA_URL, queries, null);

            // Get the hourly temperature data
            queries["dateDebut"] = queries["date"];
            queries["dateFin"] = queries["date"];
            queries["date"] = null;
            var results2 = await httpClient.HttpGetRequest<ResultArrayTemplate<HourlyTemperatureData>>(HOURLY_TEMPERATURE_DATA_URL, queries, null);

            // Combine both results
            if (results2.results.Count > 0)
            {
                results1.results.TemperatureData = results2.results[0];
            }

            return results1.results;
        }
        private async Task<IList<ResultWithComparison<T>>> FetchDataWithComparisonAsync<T>(string url, Dictionary<string, string> queries = null)
        {
            if (currentCustomerId == null || currentAccoutId == null)
            {
                throw new Exception("Custom and account must be selected");
            }
            var results = await httpClient.HttpGetRequest<ResultArrayTemplateWithComparison<T>>(url, queries, null);
            return results.results;
        }
        private void Reset()
        {
            httpClient.SetAccessToken(null);
            Accounts.Clear();
            currentAccoutId = null;
            currentCustomerId = null;
        }
    }
}
