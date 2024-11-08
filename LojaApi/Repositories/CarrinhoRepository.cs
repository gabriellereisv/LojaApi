using Dapper;
using LojaApi.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LojaApi.Repositories
{
    public class CarrinhoRepository
    {
        private readonly string _connectionString;

        public CarrinhoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<bool> AdicionarProduto(Carrinho carrinho)
        {
            using (var conn = Connection)
            {
                var estoqueSql = "SELECT QuantidadeEstoque FROM Produtos WHERE Id = @ProdutoId";
                var quantidadeEstoque = await conn.ExecuteScalarAsync<int>(estoqueSql, new { ProdutoId = carrinho.ProdutoId });

                if (quantidadeEstoque < carrinho.Quantidade)
                {
                    return false;
                }

                var sql = "INSERT INTO Carrinho (UsuarioId, ProdutoId, Quantidade) " +
                          "VALUES (@UsuarioId, @ProdutoId, @Quantidade);" +
                          "SELECT LAST_INSERT_ID();";

                await conn.ExecuteScalarAsync<int>(sql, new
                {
                    UsuarioId = carrinho.UsuarioId,
                    ProdutoId = carrinho.ProdutoId,
                    Quantidade = carrinho.Quantidade
                });

                return true;
            }
        }


        public async Task<bool> RemoverProduto(int usuarioId, int produtoId)
        {
            using (var conn = Connection)
            {
                var sql = "DELETE FROM Carrinho WHERE UsuarioId = @UsuarioId AND ProdutoId = @ProdutoId;";
                await conn.ExecuteAsync(sql, new { UsuarioId = usuarioId, ProdutoId = produtoId });
                return true; 
            }
        }

        public async Task<(List<Carrinho>, decimal)> ConsultarCarrinho(int usuarioId)
        {
            using (var conn = Connection)
            {
                var sql = @"
            SELECT c.ProdutoId, c.Quantidade, p.Preco
            FROM Carrinho c
            JOIN Produtos p ON c.ProdutoId = p.Id
            WHERE c.UsuarioId = @UsuarioId;";

                var itens = await conn.QueryAsync(sql, new { UsuarioId = usuarioId });

                var carrinhoLista = new List<Carrinho>();
                decimal total = 0;

                foreach (var item in itens)
                {
                    var carrinhoItem = new Carrinho
                    {
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade,
                        Preco = item.Preco 
                    };

                    carrinhoLista.Add(carrinhoItem);
                    total += carrinhoItem.Preco * carrinhoItem.Quantidade;
                }

                return (carrinhoLista, total);
            }
        }
    }

}
