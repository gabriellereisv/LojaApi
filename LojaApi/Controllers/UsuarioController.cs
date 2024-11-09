using LojaApi.Models;
using LojaApi.Repositories;
using LojaApi;
using LojaApi.Models;
using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LojaApi.Models;

namespace LojaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // POST api/<UsuarioController>
        [HttpPost("cadastrar-usuario")]
        public async Task<IActionResult> CadastrarUsuarioDB([FromBody] Usuario usuario)
        {
            var usuarioId = await _usuarioRepository.CadastrarUsuarioDB(usuario);
            return Ok(new { mensagem = "Usuário registrado com sucesso.", usuarioId });
        }

        // GET api/<UsuarioController>/listar-usuario
        [HttpGet("listar-usuario")]
        public async Task<IActionResult> ListarUsuarioDB()
        {
            var usuarios = await _usuarioRepository.ListarUsuarioDB();
            return Ok(usuarios);
        }
    }
}
