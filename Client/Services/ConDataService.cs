
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace HouseholdAppliancesApp.Client
{
    public partial class ConDataService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public ConDataService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/ConData/");
        }


        public async System.Threading.Tasks.Task ExportHouseholdAppliancesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/householdappliances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/householdappliances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportHouseholdAppliancesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/householdappliances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/householdappliances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetHouseholdAppliances(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>> GetHouseholdAppliances(Query query)
        {
            return await GetHouseholdAppliances(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>> GetHouseholdAppliances(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"HouseholdAppliances");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetHouseholdAppliances(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>>(response);
        }

        partial void OnCreateHouseholdAppliance(HttpRequestMessage requestMessage);

        public async Task<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> CreateHouseholdAppliance(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance householdAppliance = default(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance))
        {
            var uri = new Uri(baseUri, $"HouseholdAppliances");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(householdAppliance), Encoding.UTF8, "application/json");

            OnCreateHouseholdAppliance(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>(response);
        }

        partial void OnDeleteHouseholdAppliance(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteHouseholdAppliance(int applianceId = default(int))
        {
            var uri = new Uri(baseUri, $"HouseholdAppliances({applianceId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteHouseholdAppliance(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetHouseholdApplianceByApplianceId(HttpRequestMessage requestMessage);

        public async Task<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance> GetHouseholdApplianceByApplianceId(string expand = default(string), int applianceId = default(int))
        {
            var uri = new Uri(baseUri, $"HouseholdAppliances({applianceId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetHouseholdApplianceByApplianceId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance>(response);
        }

        partial void OnUpdateHouseholdAppliance(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateHouseholdAppliance(int applianceId = default(int), HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance householdAppliance = default(HouseholdAppliancesApp.Server.Models.ConData.HouseholdAppliance))
        {
            var uri = new Uri(baseUri, $"HouseholdAppliances({applianceId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", householdAppliance.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(householdAppliance), Encoding.UTF8, "application/json");

            OnUpdateHouseholdAppliance(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}