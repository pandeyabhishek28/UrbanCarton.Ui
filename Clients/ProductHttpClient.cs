﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using UrbanCarton.Mvc.Models;

namespace UrbanCarton.Mvc.Clients
{
    public class ProductHttpClient
    {
        private readonly HttpClient _httpClient;

        public ProductHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<ProductsContainer>> GetProducts()
        {
            var response = await _httpClient.GetAsync(@"
                            ?query=
                            {
                            products
                            {
                            id name price rating photoFileName
                            }
                            }
                            ");

            var stringResult = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Response<ProductsContainer>>(stringResult);
        }
    }
}
