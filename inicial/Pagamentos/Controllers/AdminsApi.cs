/*
 * Pagamentos API
 *
 * API de Pagamentos
 *
 * OpenAPI spec version: 1.0.0
 * Contact: michael.costa@localiza.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using IO.Swagger.Attributes;

using Microsoft.AspNetCore.Authorization;
using IO.Swagger.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;

namespace IO.Swagger.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class AdminsApiController : ControllerBase
    {
        private IMemoryCache Cache = null;

        private readonly IConfiguration Configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="cache"></param>
        public AdminsApiController(IConfiguration configuration, IMemoryCache cache)
        {
            Configuration = configuration;
            Cache = cache;
        }
        /// <summary>
        /// adiciona um pagamento
        /// </summary>
        /// <remarks>adiciona um pagamento</remarks>
        /// <param name="pagamento">Pagamento a adicionar</param>
        /// <response code="201">Pagamento criado</response>
        /// <response code="400">entrada inválida, objeto inválido</response>
        /// <response code="409">um pagamento com esses dados já existe</response>
        [HttpPost]
        [Route("/michaelcosta/Pagamentos/1.0.0/pagamentos")]
        [ValidateModelState]
        [SwaggerOperation("AdicionarParamento")]
        public virtual IActionResult AdicionarParamento([FromBody] Pagamento pagamento)
        {
            List<Pagamento> listaPagamento = (List<Pagamento>)Cache.Get("ListaPagamentos");

            if (listaPagamento == null)
            {
                listaPagamento = Startup.CarregarMockPagamento();

                Cache.Set("ListaPagamentos", listaPagamento, DateTimeOffset.Now.AddMinutes(10.0));
            }

            var pagamentos = from p in listaPagamento
                             where p.Id == pagamento.Id
                             select p;

            if (pagamentos.Count() > 0)
            {
                return StatusCode(409, "Já existe um pagamento com esse ID");
            }
            else
            {

                listaPagamento.Add(new Pagamento() { Id = pagamento.Id, Status = "AGUARDANDO PAGAMENTO" });
                Cache.Set("ListaPagamentos", listaPagamento, DateTimeOffset.Now.AddMinutes(10.0));
                return StatusCode(201, listaPagamento.LastOrDefault());
            }
        }

        /// <summary>
        /// altera um pagamento
        /// </summary>
        /// <remarks>altera dados de uma reserva existente</remarks>
        /// <param name="id">id do pagamento a alterar</param>
        /// <param name="pagamento">pagamento a alterar</param>
        /// <response code="200">pagamento alterado</response>
        /// <response code="400">entrada inválida, objeto inválido</response>
        /// <response code="404">nenhum pagamento encontrado com o id informado</response>
        [HttpPatch]
        [Route("/michaelcosta/Pagamentos/1.0.0/pagamentos/{id}")]
        [ValidateModelState]
        [SwaggerOperation("AlterarPagamento")]
        public virtual IActionResult AlterarPagamento([FromRoute][Required] string id, [FromBody] Pagamento pagamento)
        {

            List<Pagamento> listaPagamento = (List<Pagamento>)Cache.Get("ListaPagamentos");

            if (listaPagamento == null)
            {
                listaPagamento = Startup.CarregarMockPagamento();

                Cache.Set("ListaPagamentos", listaPagamento, DateTimeOffset.Now.AddMinutes(10.0));
            }

            var pagamentos = from p in listaPagamento
                             where p.Id.ToString() == id
                             select p;

            if (pagamentos.Count() == 0)
            {
                return StatusCode(404);
            }


            if (pagamentos.FirstOrDefault().Status != "AGUARDANDO PAGAMENTO")
            {
                return StatusCode(400, "Pagamento não está aguardando pagamento");
            }
            else
            {
                if ((pagamento.Status != "PAGO") && (pagamento.Status != "CANCELADO"))
                {
                    return StatusCode(400, "Status somente pode ser PAGO ou CANCELADO");
                }
                else
                {
                    foreach (Pagamento p in listaPagamento)
                    {
                        //atualiza cache
                        if (p.Id.ToString() == id)
                        {
                            p.Status = pagamento.Status;
                            Cache.Set("ListaPagamentos", listaPagamento, DateTimeOffset.Now.AddMinutes(10.0));
                            if (pagamento.Status == "PAGO")
                            {
                                #region Atualiza Status Reserva

                                //var payload = "{\"localizador\": \"WS" + p.Id + "\",\"status\": \"PAGAMENTO APROVADO\"}";
                                var payload = $"{{\"localizador\":\"WS{p.Id}\",\"placa\":\"0\",\"idCliente\":0,\"status\":\"PAGAMENTO APROVADO\"}}";

                                HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");

                                var pagamentoResponse = HttpRequestHelper.TryPatchHTTPResponse($"{Configuration["ApiReferences:Reservas"]}reservas/WS{p.Id}", c);
                                if (pagamentoResponse.StatusCode != HttpStatusCode.OK)
                                {
                                    return StatusCode(202, "Pagamento aprovado, entretanto não foi possível atualizar a reserva.");
                                }
                                #endregion

                            }
                            return StatusCode(200);
                        }
                    }
                }
            }

            return StatusCode(400);
        }
    }
}
