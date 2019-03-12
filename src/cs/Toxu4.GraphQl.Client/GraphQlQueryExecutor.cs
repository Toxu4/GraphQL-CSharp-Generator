using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Toxu4.GraphQl.Client
{
    internal class GraphQlQueryExecutor : IGraphQlQueryExecutor
    {
        private readonly HttpClient _httpClient;        
        private readonly string _endpoint;
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new AbstractClassConverter()
            }
        };

        public GraphQlQueryExecutor(IOptions<GraphQlApiSettings> settings, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _endpoint = settings.Value.Endpoint;
        }        
        
        public async Task<TResult> Run<TQuery, TResult>(TQuery query) where TQuery : IGraphQlQuery
        {
            var queryParamsBuilder = new StringBuilder($"query={query.QueryText}", 2);
            if (query.Variables.Any())
            {
                queryParamsBuilder.Append($"&variables={JsonConvert.SerializeObject(query.Variables)}");
            }

            var str = await _httpClient.GetStringAsync($"{_endpoint}?{queryParamsBuilder}");
                
            return JsonConvert.DeserializeObject<TResult>(str, _jsonSerializerSettings);
        }
    }
}
