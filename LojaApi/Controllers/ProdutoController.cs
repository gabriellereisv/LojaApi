using LojaApi.Models;
using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoController(ProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet("produtos-cadastrados")]
        public async Task<IActionResult> ListagemProdutos()
        {
            var produtos = await _produtoRepository.Listar();
            return Ok(produtos);
        }

        [HttpPost("registrar-produto")]
        public async Task<IActionResult> CadastrarProduto([FromBody] Produto produto)
        {
            await _produtoRepository.CadastrarProduto(produto);
            return Ok(new { mensagem = "O produto foi cadastrado com sucesso." });
        }

        [HttpPut("atualizar-produto/{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] Produto produto)
        {
            produto.Id = id;
            await _produtoRepository.AtualizarProduto(produto);
            return Ok(new { mensagem = "O produto foi atualizado com sucesso." });
        }

        [HttpDelete("deletar-produto/{id}")]
        public async Task<IActionResult> DeletarPorId(int id)
        {
            var resultado = await _produtoRepository.DeletarPorId(id);
            if (resultado > 0)
            {
                return Ok(new { mensagem = "O produto foi deletado com sucesso." });
            }
            else
            {
                return Ok(new { mensagem = "O produto não foi encontrado." });
            }
        }
    }
}
