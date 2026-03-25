using ApiFinanceiro.DataContexts;
using ApiFinanceiro.Dtos;
using ApiFinanceiro.Exceptions;
using ApiFinanceiro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiFinanceiro.Services
{
    public class DespesaService
    {
        private readonly AppDbContext _context;

        public DespesaService(AppDbContext context) 
        { 
            _context = context;
        }

        public async Task<ICollection<Despesa>> FindAll()
        {
            try
            {
                return await _context.Despesas.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Despesa> Create(DespesaDto novaDespesa)
        {
            try
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

                return despesa;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task<Despesa> FindById(int id)
        {
            try
            {
                var despesa = await _context.Despesas.FirstOrDefaultAsync(x => x.Id == id);

                if (despesa is null)
                {
                    throw new ErrorServiceException($"Despesa ${id} não encontrada", 
                        c => c.NotFound(new { message = $"Despesa ${id} não encontrada" }));
                }

                return despesa;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Despesa> Update(int id, DespesaUpdateDto despesaDto)
        {
            try
            {
                var despesa = await FindById(id);

                var dataVencimento = new DateTime(despesa.DataVencimento.Year, despesa.DataVencimento.Month, despesa.DataVencimento.Day);
                var dataPagamento = new DateTime(despesaDto.DataPagamento.Year, despesaDto.DataPagamento.Month, despesaDto.DataPagamento.Day);

                // TODO: adicionar data de emissão
                if(dataPagamento < dataVencimento)
                {
                    throw new ErrorServiceException("Somente é possível realizar o pagamento no dia de vencimento", 
                        c => c.Conflict(new { message = "Somente é possível realizar o pagamento no dia de vencimento" }));
                }

                despesa.Descricao = despesaDto.Descricao;
                despesa.Valor = despesaDto.Valor;
                despesa.DataVencimento = despesaDto.DataVencimento;
                despesa.Categoria = despesaDto.Categoria;
                despesa.Situacao = despesaDto.Situacao;
                despesa.DataPagamento = dataPagamento;

                

                _context.Despesas.Update(despesa);
                await _context.SaveChangesAsync();

                return despesa;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task Remove(int id)
        {
            try
            {
                var despesa = await FindById(id);

                _context.Despesas.Remove(despesa);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
