using HiberusAPIEntidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HiberusAPIDatosContext
{
    /// <summary>
    /// Clase estática para el acceso a la API de Hiberus
    /// </summary>
    public static class ConexionAPIHiberus
    {
        /// <summary>
        /// Método asíncrono de acceso al JSON de rates de Hiberus
        /// </summary>
        /// <returns>Ienumerable(RateEnt)</returns>
        async public static Task<IEnumerable<RateEnt>> GetRatesAPI()
        {
            
            List<RateEnt> ratesList = new List<RateEnt>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://quiet-stone-2094.herokuapp.com/rates.json");
            HttpContent content = response.Content;

            string data = await content.ReadAsStringAsync();

            ratesList = JsonConvert.DeserializeObject<List<RateEnt>>(data);

            return ratesList;
        }

        /// <summary>
        /// Método asíncrono de acceso al JSON de transactions de Hiberus
        /// </summary>
        /// <returns>Ienumerable(Transaction)</returns>
        async public static Task<IEnumerable<Transaction>> GetTransactionAPI()
        {
            List<Transaction> transactionList = new List<Transaction>();

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://quiet-stone-2094.herokuapp.com/transactions.json");
            HttpContent content = response.Content;

            string data = await content.ReadAsStringAsync();

            transactionList = JsonConvert.DeserializeObject<List<Transaction>>(data);

            return transactionList;
        }

    }
}
