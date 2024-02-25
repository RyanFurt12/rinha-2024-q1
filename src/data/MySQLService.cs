using rinha_2024_q1.model;
using MySql.Data.MySqlClient;

namespace rinha_2024_q1.data
{
    public class MySQLService
    {
        private readonly string _connectionString;

        public MySQLService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySQLConnection") ?? "";
        }


        public bool CheckIfClientExists(int clientId)
        {
            var _connection = new MySqlConnection(_connectionString);
            _connection.Open();

            string query = $"SELECT COUNT(*) FROM clientes WHERE Id = {clientId}";
            var command = new MySqlCommand(query, _connection);

            return (long)command.ExecuteScalar() > 0;
        }

        public object? GetClientExtract(int clientId)
        {
            var _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        
            string query = $@"
                SELECT 
                    c.valor, c.limite, t.valor, t.tipo, t.descricao, t.criada_em
                FROM 
                    clientes c
                LEFT JOIN 
                    transacoes t ON c.id = t.cliente_id
                WHERE 
                    c.id = {clientId}
                ORDER BY 
                    t.criada_em DESC
                LIMIT 10
            ";
            
            var dateNow = new MySqlCommand("SELECT NOW()", _connection).ExecuteScalar();
            var command = new MySqlCommand(query, _connection);
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var total = reader.GetDecimal(0);
                var limite = reader.GetDecimal(1);

                var extrato = new
                {
                    saldo = new
                    {
                        total,
                        data_extrato = dateNow,
                        limite
                    },
                    ultimas_transacoes = new List<object>()
                };

                do
                {
                    if(!reader.IsDBNull(2))
                    {
                        var valor = reader.GetDecimal(2);
                        var tipo = reader.GetString(3);
                        var descricao = reader.GetString(4);
                        var realizada_em = reader.GetDateTime(5);

                        var transacao = new
                        {
                            valor,
                            tipo,
                            descricao,
                            realizada_em
                        };

                        extrato.ultimas_transacoes.Add(transacao);
                    }
                } 
                while (reader.Read());

                return extrato;
            }
            else
            {
                return null;
            }
        }

        public object PostTransaction(int clientId, Transaction transaction)
        {
            var _connection = new MySqlConnection(_connectionString);
            _connection.Open();

            string query = @"INSERT INTO transacoes (`cliente_id`, `valor`, `tipo`, `descricao`, `criada_em`)
                     VALUES (@clientId, @valor, @tipo, @descricao, NOW())";

            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@clientId", clientId);
            command.Parameters.AddWithValue("@valor", transaction.valor);
            command.Parameters.AddWithValue("@tipo", transaction.tipo);
            command.Parameters.AddWithValue("@descricao", transaction.descricao);

            command.ExecuteNonQuery();
            var response = UpdateClientBalance(clientId, transaction, _connection);

            return response;
        }

        private static object UpdateClientBalance(int clientId, Transaction transaction, MySqlConnection connection)
        {
            string query = $"SELECT valor FROM clientes c WHERE c.id = {clientId}";
            var command = new MySqlCommand(query, connection);
            int value = transaction.tipo == "c" ? (int)command.ExecuteScalar() + transaction.valor : (int)command.ExecuteScalar() - transaction.valor;

            query = $"SELECT limite FROM clientes c WHERE c.id = {clientId}";
            command = new MySqlCommand(query, connection);
            int limite = (int)command.ExecuteScalar();

            query = $"UPDATE clientes SET valor = {value} WHERE (id = {clientId});";
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();

            return new
            {
                limite,
                saldo = value
            };
        }

        public bool TestClientHasBalance(int clientId, int value)
        {
            var _connection = new MySqlConnection(_connectionString);
            _connection.Open();

            string query = $"SELECT valor FROM clientes c WHERE c.id = {clientId}";
            var command = new MySqlCommand(query, _connection);
            value = (int)command.ExecuteScalar() - value;

            query = $"SELECT limite FROM clientes c WHERE c.id = {clientId}";
            command = new MySqlCommand(query, _connection);
            var limit = (int)command.ExecuteScalar();

            return (value * -1) <= limit;
        }
    }

}
