/*
 * Contratos API
 *
 * API de Contratos
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

namespace IO.Swagger.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class DevelopersApiController : ControllerBase
    { 
        /// <summary>
        /// Busca Contratos
        /// </summary>
        /// <remarks>Retorna lista de reservas disponíveis </remarks>
        /// <param name="loja">código da loja (opcional)</param>
        /// <response code="200">resultados conforme critérios informados</response>
        /// <response code="400">entrada inválida</response>
        [HttpGet]
        [Route("/michaelcosta/Contratos/1.0.0/carros")]
        [ValidateModelState]
        [SwaggerOperation("BuscarContratos")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Contrato>), description: "resultados conforme critérios informados")]
        public virtual IActionResult BuscarContratos([FromQuery]string loja)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<Reserva>));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            string exampleJson = null;
            exampleJson = "[ {\n  \"localizadorReserva\" : \"WS12345\",\n  \"status\" : \"aberta\"\n}, {\n  \"localizadorReserva\" : \"WS12345\",\n  \"status\" : \"aberta\"\n} ]";
            
            var example = exampleJson != null
            ? JsonConvert.DeserializeObject<List<Contrato>>(exampleJson)
            : default(List<Contrato>);
            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}
