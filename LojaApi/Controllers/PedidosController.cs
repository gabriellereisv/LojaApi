using LojaApi.Models;
using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidosRepository _pedidoRepository;

        public PedidoController(PedidosRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet("listar-pedidos")]
        public async Task<IActionResult> ListarPedidos(int usuarioId)
        {
            var pedidos = await _pedidoRepository.ListarPedidosPorUsuario(usuarioId);
            return Ok(pedidos);
        }

        [HttpPost("criar-pedido")]
        public async Task<IActionResult> CriarPedido([FromBody] Pedido pedido)
        {
            var novoPedido = await _pedidoRepository.CriarPedido(pedido);
            return Ok(new { mensagem = "O pedido foi criado com sucesso.", pedidoId = novoPedido.Id });
        }

        [HttpGet("status-pedidos")]
        public async Task<IActionResult> ConsultarStatus(int pedidoId)
        {
            var status = await _pedidoRepository.ConsultarStatusPedido(pedidoId);
            return Ok(new { pedidoId, status });
        }

        [HttpPost("finalizar-pedido/{id}")]
        public async Task<IActionResult> FinalizarPedido(int id, int q, int p)
        {
            var resultado = await _pedidoRepository.FinalizarPedido(id, q, p);
            if (resultado)
            {
                return Ok(new { mensagem = "O pedido foi finalizado com sucesso." });
            }
            else
            {
                return Ok(new { mensagem = "Não há estoque suficiente para finalizar o pedido." });
            }
        }


    }

}