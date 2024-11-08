using LojaApi.Models;
using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LojaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly CarrinhoRepository _carrinhoRepository;

        public CarrinhoController(CarrinhoRepository carrinhoRepository)
        {
            _carrinhoRepository = carrinhoRepository;
        }

        // POST api/carrinho/adicionar
        [HttpPost("adicionar")]
        public async Task<IActionResult> AdicionarProduto([FromBody] Carrinho carrinho)
        {
            var produtoAdicionado = await _carrinhoRepository.AdicionarProduto(carrinho);

            if (!produtoAdicionado)
            {
                return Ok(new { mensagem = "Quantidade solicitada não disponível em estoque." });
            }

            return Ok(new { mensagem = "O produto foi adicionado ao carrinho." });
        }


        // DELETE api/carrinho/remover
        [HttpDelete("remover")]
        public async Task<IActionResult> RemoverProduto(int usuarioId, int produtoId)
        {
            var resultado = await _carrinhoRepository.RemoverProduto(usuarioId, produtoId);
            if (resultado)
            {
                return Ok(new { mensagem = "O produto foi removido do carrinho." });
            }
            return NotFound(new { mensagem = "O produto não foi encontrado no carrinho." });
        }

        // GET api/carrinho/consultar

        [HttpGet("consultar")]
        public async Task<IActionResult> ConsultarCarrinho(int usuarioId)
        {
            var (itens, total) = await _carrinhoRepository.ConsultarCarrinho(usuarioId);
            return Ok(new { Itens = itens, Total = total, Mensagem = "Carrinho consultado." });
        }

    }
}
