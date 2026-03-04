using ApiFinanceiro.Dtos;
using ApiFinanceiro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanceiro.Controllers
{
    [Route("/despesas")]
    [ApiController]
    public class DespesaController : ControllerBase
    {
        private static List<Despesa> listaDespesas = new()
        {
            new Despesa {
                Descricao = "Internet",
                Valor = 150, Categoria = "Moradia",
                DataVencimento = new DateOnly(2026, 03, 15),
                Situacao = "Aberto"
            },
            new Despesa {
                Descricao = "Caerd",
                Valor = 45, Categoria = "Moradia",
                DataVencimento = new DateOnly(2026, 03, 10),
                Situacao = "Aberto"
            },
            new Despesa {
                Descricao = "Energisa",
                Valor = 245, Categoria = "Moradia",
                DataVencimento = new DateOnly(2026, 03, 09),
                Situacao = "Aberto"
            }
        };

        [HttpGet()]
        public ActionResult FindAll()
        {
            return Ok(listaDespesas);
        }

        [HttpPost()]
        public ActionResult Create([FromBody]DespesaDto novaDespesa)
        {
            var despesa = new Despesa
            {
                Descricao = novaDespesa.Descricao,
                Valor = novaDespesa.Valor,
                Categoria = novaDespesa.Categoria,
                DataVencimento = novaDespesa.DataVencimento,
                Situacao = "Pendente"
            };

            listaDespesas.Add(despesa);

            return Created("", despesa);
        }

        [HttpGet("{id}")]
        public ActionResult FindById(Guid id)
        {
            var despesa = listaDespesas.FirstOrDefault(d => d.Id == id);

            if (despesa is null)
            {
                return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });
            }

            return Ok(despesa);
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, [FromBody] DespesaUpdateDto despesaDto)
        {
            var despesa = listaDespesas.FirstOrDefault(d => d.Id == id);

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

            return Ok(despesa);
        }

        [HttpDelete("{id}")]
        public ActionResult Remove(Guid id)
        {
            var despesa = listaDespesas.FirstOrDefault(d => d.Id == id);

            if (despesa is null)
            {
                return NotFound(new { mensagem = $"Despesa #{id} não encontrada" });
            }

            listaDespesas.Remove(despesa);

            return NoContent();
        }
    }
}
