using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace SiteReservas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly ILogger<ReservasController> _logger;

        private readonly IConfiguration Configuration;

        public ReservasController(IConfiguration configuration, ILogger<ReservasController> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Reservas> Get()
        {

            var response = HttpRequestHelper.TryGetHTTPResponse($"{Configuration["ApiReferences:Reservas"]}reservas", 3);
            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;

            var arrayJson = JArray.Parse(responseBody);

            var listaReservas = new List<Reservas>();

            foreach (JObject v in arrayJson)
            {
                listaReservas.Add(new Reservas()
                {
                    Cliente = FormatarCliente(int.Parse(v["idCliente"].ToString())),
                    IdPagamento = int.Parse(v["idPagamento"].ToString()),
                    Localizador = v["localizador"].ToString(),
                    Carro = FormatarPlaca(v["placa"].ToString()),
                    Status = v["status"].ToString()
                });
            }

            return listaReservas;
        }

        private string FormatarPlaca(string placa)
        {
            var response = HttpRequestHelper.TryGetHTTPResponse($"{Configuration["ApiReferences:Carros"]}carros?placa={placa}", 3);
            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;

            var arrayJson = JArray.Parse(responseBody);

            if (arrayJson.Count() > 0)
            {
                var carro = JObject.Parse(arrayJson[0].ToString());
                return $"{carro["loja"]} - {carro["placa"]} - {carro["descricao"]}";
            }

            return placa;
        }

        private string FormatarCliente(int id)
        {
            var response = HttpRequestHelper.TryGetHTTPResponse($"{Configuration["ApiReferences:Clientes"]}clientes?id={id}", 3);
            response.EnsureSuccessStatusCode();

            var responseBody = response.Content.ReadAsStringAsync().Result;

            var arrayJson = JArray.Parse(responseBody);

            if (arrayJson.Count() > 0)
            {
                var cliente = JObject.Parse(arrayJson[0].ToString());
                return $"{cliente["id"]} - {cliente["nome"]}";
            }

            return id.ToString();
        }
    }
}
