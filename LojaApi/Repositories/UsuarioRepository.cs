using BibliotecaAPI.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BibliotecaAPI.Repositories
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection Connection =>
            new MySqlConnection(_connectionString);

        public async Task<int> CadastrarUsuarioDB(Usuario dados)
        {
            var sql = "INSERT INTO Usuarioos (Nome, Email, Endereco) VALUES (@Nome, @Email, @Endereco)";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, new
                {
                    Nome = dados.Nome,
                    Email = dados.Email,
                    Endereco = dados.Endereco,
                });
            }
        }

        public async Task<IEnumerable<Usuario>> BuscarUsuarios(string? nome = null, string? email = null, string? endereco = null)
        {
            var sql = "SELECT * FROM Usuarioos WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(nome))
            {
                sql += " AND Nome = @Nome";
                parameters.Add("Nome", nome);
            }

            if (!string.IsNullOrEmpty(email))
            {
                sql += " AND Email = @Email";
                parameters.Add("Email", email);
            }

            if (!string.IsNullOrEmpty(endereco))
            {
                sql += " AND Endereco = @Endereco";
                parameters.Add("Endereco", endereco);
            }

            using (var conn = Connection)
            {
                return await conn.QueryAsync<Usuario>(sql, parameters);
            }
        }
    }
}
