using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Dtos;
using ApiFinanceiro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanceiro.Controllers
{
    [Route("/despesas")]
    [ApiController]
    public class DespesaController : ControllerBase
    {
        private readonly AppDbContext _context;

       public DespesaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<IActionResult> FindAll()
        {
            try
            {
                var listaDespesas = await _context.Despesas.ToListAsync();

                return Ok(listaDespesas);

            } catch(Exception e)
            {
                return Problem(e.Message);
            }
            
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] DespesaDto novaDespesa)
        {
            var despesa = new Despesa
            {
                Descricao = novaDespesa.Descricao,
                Valor = novaDespesa.Valor,
                Categoria = novaDespesa.Categoria,
                DataVencimento = novaDespesa.DataVencimento,
                Situacao = "pendente"
            };

            await _context.Despesas.AddAsync(despesa);
            await _context.SaveChangesAsync();

            return Created("", despesa);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindById(int id)
        {
            try
            {
                var despesa = await _context.Despesas.FirstOrDefaultAsync(x => x.Id == id);

                if (despesa is null)
                {
                    return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });
                }

                return Ok(despesa);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] DespesaUpdateDto despesaDto)
        {
            try
            {
                var despesa = await _context.Despesas.FirstOrDefaultAsync(x => x.Id == id);

                if (despesa is null)
                {
                    return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });
                }

                var dataPagamento = new DateTime(despesaDto.DataPagamento.Year, despesaDto.DataPagamento.Month, despesaDto.DataPagamento.Day);

                despesa.Descricao = despesaDto.Descricao;
                despesa.Valor = despesaDto.Valor;
                despesa.DataVencimento = despesaDto.DataVencimento;
                despesa.Categoria = despesaDto.Categoria;
                despesa.Situacao = despesaDto.Situacao;
                despesa.DataPagamento = dataPagamento;

                _context.Despesas.Update(despesa);
                await _context.SaveChangesAsync();

                return Ok(despesa);

            } catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            try
            {
                var despesa = await _context.Despesas.FirstOrDefaultAsync(x => x.Id == id);

                if (despesa is null)
                {
                    return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });
                }

                _context.Despesas.Remove(despesa);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }            
        }
    }
}
