using LojaApi.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace LojaApi.Repositories
{
    public class ProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<Produto>> Listar()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Produtos";
                return await conn.QueryAsync<Produto>(sql);
            }
        }

        public async Task<int> CadastrarProduto(Produto produto)
        {
            var sql = "INSERT INTO Produtos (Nome, Descricao, Preco, QuantidadeEstoque) " +
                      "VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque)";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, produto);
            }
        }

        public async Task<int> AtualizarProduto(Produto produto)
        {
            var sql = "UPDATE Produtos SET Nome = @Nome, " +
                      "Descricao = @Descricao, " +
                      "Preco = @Preco, " +
                      "QuantidadeEstoque = @QuantidadeEstoque " +
                      "WHERE Id = @Id";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, produto);
            }
        }

        public async Task<int> DeletarPorId(int id)
        {
            using (var conn = Connection)
            {
                    var carrinhoSql = @"
                SELECT COUNT(*) 
                FROM Carrinho c
                JOIN Pedidos p ON c.UsuarioId = p.UsuarioId
                WHERE c.ProdutoId = @Id
                AND p.StatusPedido = 'Em andamento';";

                var produtoEmCarrinho = await conn.ExecuteScalarAsync<int>(carrinhoSql, new { Id = id });

                if (produtoEmCarrinho > 0)
                {
                    return -1; 
                }

                    var pedidoSql = @"
                SELECT COUNT(*) 
                FROM PedidoProdutos pp
                JOIN Pedidos p ON pp.PedidoId = p.Id
                WHERE pp.ProdutoId = @Id
                AND p.StatusPedido = 'Em andamento';";

                var produtoEmPedido = await conn.ExecuteScalarAsync<int>(pedidoSql, new { Id = id });
                                     
                if (produtoEmPedido > 0)
                {
                    
                    return -1; 
                }

                var sql = "DELETE FROM Produtos WHERE Id = @Id";

                return await conn.ExecuteAsync(sql, new { Id = id });
            }
        }



    }
}