using LojaApi.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using Mysqlx.Crud;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace LojaApi.Repositories
{
    public class PedidosRepository
    {
        private readonly string _connectionString;

        public PedidosRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<Pedido>> ListarPedidosPorUsuario(int usuarioId)
        {
            var sql = "SELECT * FROM Pedidos WHERE UsuarioId = @UsuarioId";

            using (var conn = Connection)
            {
                return await conn.QueryAsync<Pedido>(sql, new { UsuarioId = usuarioId });
            }
        }


        public async Task<Pedido> CriarPedido(Pedido pedido)
        {
            var sql = "INSERT INTO Pedidos (UsuarioId, DataPedido, StatusPedido, ValorTotal) " +
                      "VALUES (@UsuarioId, @DataPedido, @StatusPedido, @ValorTotal); " +
                      "SELECT LAST_INSERT_ID();";

            using (var conn = Connection)
            {
                var id = await conn.QuerySingleAsync<int>(sql, pedido);
                pedido.Id = id;

                var updateSql = "UPDATE Pedidos SET StatusPedido = 'Enviado' WHERE Id = @Id;";
                await conn.ExecuteAsync(updateSql, new { Id = pedido.Id });

                return pedido;
            }
        }




        public async Task<string> ConsultarStatusPedido(int pedidoId)
        {
            var sql = "SELECT StatusPedido FROM Pedidos WHERE Id = @PedidoId";

            using (var conn = Connection)
            {
                return await conn.QuerySingleAsync<string>(sql, new { PedidoId = pedidoId });
            }
        }


        public async Task<bool> FinalizarPedido(int pedidoId, int _quantidade, int _produtoid)
        {
            using (var conn = Connection)
            {
                using (var transaction = conn.BeginTransaction())
                {
                    var sql = "SELECT ProdutoId, Quantidade FROM PedidoProdutos WHERE PedidoId = @PedidoId";
                    var carrinho = await conn.QueryAsync<Carrinho>(sql, new { PedidoId = pedidoId }, transaction);

                    var updateEstoqueSql = "UPDATE Produtos SET QuantidadeEstoque = QuantidadeEstoque - @Quantidade WHERE Id = @ProdutoId";
                    await conn.ExecuteAsync(updateEstoqueSql, new { QuantidadeEstoque = _quantidade, ProdutoId = _produtoid }, transaction);

                    transaction.Commit();
                    return true;
                }
            }
        }


    }
}
