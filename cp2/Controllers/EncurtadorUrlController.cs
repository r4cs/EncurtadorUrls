using cp2.Entities;
using cp2.Models;
using cp2.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace cp2.Controllers
{
    public class EncurtadorController : BaseApiController
    {
        private readonly ApiService _apiService;

        public EncurtadorController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Encurta uma URL.", Description = "Encurta uma URL.")]
        [SwaggerResponse(StatusCodes.Status201Created, "A URL foi encurtada com sucesso.", typeof(EncurtadorUrl))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A URL requisitada é inválida.")]
        public async Task<IActionResult> EncurtarUrl(EncurtadorUrlRequest request)
        {
            var encurtadorUrl = await _apiService.Create(request, HttpContext);

            if (encurtadorUrl == null)
            {
                return BadRequest("A url requisitada é inválida");
            }
            return Created(encurtadorUrl.UrlCurta, encurtadorUrl);
        }
        
        [HttpGet("{codigo}")]
        [SwaggerOperation(Summary = "Obtém informações sobre a URL encurtada associada ao código.", Description = "Obtém informações sobre a URL encurtada associada ao código.")]
        [SwaggerResponse(StatusCodes.Status200OK, "As informações sobre a URL encurtada foram obtidas com sucesso.", typeof(EncurtadorUrl))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "O código de URL encurtada não foi encontrado.")]
        public async Task<IActionResult> ObterInformacoesUrl(string codigo)
        {
            var encurtadorUrl = await _apiService.Read(codigo);

            if (encurtadorUrl == null)
            {
                return NotFound();
            }

            return Ok(encurtadorUrl);
        }

        [HttpGet("r/{codigo}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [SwaggerOperation(Summary = "Redireciona para a URL original associada ao código de URL encurtada.", Description = "Redireciona para a URL original associada ao código de URL encurtada.")]
        [SwaggerResponse(StatusCodes.Status302Found, "O redirecionamento foi efetuado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "O código de URL encurtada não foi encontrado.")]
        public async Task<IActionResult> RedirecionarUrl(string codigo)
        {
            var encurtadorUrl = await _apiService.Read(codigo);

            if (encurtadorUrl == null)
            {
                return NotFound();
            }

            return Redirect(encurtadorUrl.UrlLonga);
        }

        // [HttpGet]
        // [SwaggerOperation(Summary = "Retorna todas as urls salvas", Description = "Retorna todas as urls salvas")]
        // [SwaggerResponse(StatusCodes.Status200OK)]
        // [SwaggerResponse(StatusCodes.Status404NotFound)]
        // public async Task<IActionResult> ObtemTodasUrls()
        // {
        //     var urls = await _apiService.GetAll();
        //     if (urls == null || urls.Count == 0)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(urls);
        // }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todas as URLs salvas", Description = "Retorna todas as URLs salvas")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de URLs retornada com sucesso", typeof(List<EncurtadorUrl>))]
        public async Task<IActionResult> ObtemTodasUrls([FromQuery] string orderBy = null)
        {
            var urls = await _apiService.GetAll(orderBy);
            if (urls == null || urls.Count == 0)
            {
                return NotFound();
            }
            return Ok(urls);
        }


        [HttpDelete("{codigo}")]
        [SwaggerOperation(Summary = "Exclui uma URL encurtada.", Description = "Exclui uma URL encurtada.")]
        [SwaggerResponse(StatusCodes.Status200OK, "A URL encurtada foi excluída com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "O código de URL encurtada não foi encontrado.")]
        public async Task<IActionResult> ExcluirUrl(string codigo)
        {
            var result = await _apiService.Delete(codigo);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Url encurtada excluída com sucesso");
        }

        [HttpPut("{codigo}")]
        [SwaggerOperation(Summary = "Atualiza a URL original associada ao código de URL encurtada.", Description = "Atualiza a URL original associada ao código de URL encurtada.")]
        [SwaggerResponse(StatusCodes.Status200OK, "A URL encurtada foi atualizada com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "O código de URL encurtada não foi encontrado.")]
        public async Task<IActionResult> AtualizarUrl(string codigo, [FromBody] EncurtadorUrlRequest request)
        {
            var result = await _apiService.Update(codigo, request.Url);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Url encurtada atualizada com sucesso");
        }
    }
}

