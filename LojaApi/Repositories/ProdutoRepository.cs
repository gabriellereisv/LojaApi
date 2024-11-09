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
            using (var conn = Connection)
            {
                var sql = "INSERT INTO Produtos (Nome, Descricao, Preco, QuantidadeEstoque) " +
                      "VALUES (@Nome, @Descricao, @Preco, @QuantidadeEstoque) SELECT LAST_INSERT_ID()";

                return await conn.ExecuteAsync(sql, produto);
            }
        }

        public async Task<int> AtualizarProduto(Produto produto)
        {
            using (var conn = Connection)
            {
                var sql = "UPDATE Produtos SET Nome = @Nome, " +
                      "Descricao = @Descricao, " +
                      "Preco = @Preco, " +
                      "QuantidadeEstoque = @QuantidadeEstoque " +
                      "WHERE Id = @Id";

                return await conn.ExecuteAsync(sql, produto);
            }
        }

        public async Task<int> DeletarPorId(int id)
        {
            using (var conn = Connection)
            {
                var carrinhoverSql = "SELECT COUNT(*) FROM Carrinho WHERE ProdutoId = @Id";

                var carrinhocount = await conn.ExecuteScalarAsync<int>(carrinhoverSql, new { Id = id });

                if (carrinhocount > 0)
                {
                    throw new InvalidOperationException("O produto está em um carrinho ativo.");
                }


                var sqlexcluir = "DELETE FROM Produtos WHERE Id = @Id";

                return await conn.ExecuteAsync(sqlexcluir, new { Id = id });
            }
        }



    }
}