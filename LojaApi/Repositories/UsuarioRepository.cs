using LojaApi.Models;
using Dapper;
using LojaApi.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace LojaApi.Repositories
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

        public async Task<int> CadastrarUsuarioDB(Usuario usuario)
        {
            using (var conn = Connection)
            {
                var sql = "INSERT INTO Usuarioos (Nome, Email, Endereco) VALUES (@Nome, @Email, @Endereco) SELECT LAST_INSERT_ID();";

                return await conn.ExecuteScalarAsync<int>(sql, usuario);
            }
        }


        public async Task<IEnumerable<Usuario>> ListarUsuarioDB()
        {
            using (var conn = Connection)
            {
                var sql = "SELECT * FROM Usuarios";
                return await conn.QueryAsync<Usuario>(sql);
            }
        }
    }
}
