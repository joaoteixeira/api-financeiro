using ApiFinanceiro.Controllers.Filters;
using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Dtos;
using ApiFinanceiro.Exceptions;
using ApiFinanceiro.Helpers.Paginated;
using ApiFinanceiro.Models;
using ApiFinanceiro.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Controllers
{
    
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/despesas")]
    //[Authorize]
    public class DespesaController : ControllerBase
    {
        private readonly DespesaService _service;

       public DespesaController(DespesaService service)
        {
            _service = service;
        }

        [HttpGet()]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var despesas = await _service.FindAll();

                return Ok(despesas);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet()]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> FindAll([FromQuery] DespesaFilter filter)
        {
            try
            {
                var despesas = await _service.FindAllV2(filter);

                return Ok(despesas);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var despesa = await _service.FindById(id);

                return Ok(despesa);
            }
            catch(ErrorServiceException e)
            {
                return e.ToActionResult(this);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] DespesaDto novaDespesa)
        {
            try
            {
                var despesa = await _service.Create(novaDespesa);

                return Created("", despesa);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DespesaUpdateDto despesaDto)
        {
            try
            {
                var despesa = await _service.Update(id, despesaDto);

                return Ok(despesa);

            } catch (ErrorServiceException e)
            {
                return e.ToActionResult(this);
            } 
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AddTags(int id, [FromBody] DespesaTagsDto tag)
        {
            try
            {
                var despesa = await _service.AddTags(id, tag);

                return Ok(despesa);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                await _service.Remove(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }            
        }
    }
}
