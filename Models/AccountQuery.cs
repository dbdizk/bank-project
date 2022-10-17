using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;

namespace bank_project
{
    public class AccountQuery
    {
        public AppDb Db { get; }

        public AccountQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<account> FindOneAsync(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `idaccount`, `owner`, `balance` FROM `account` WHERE `idaccount` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<account>> LatestAccountAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `idaccount`, `owner`, `balance` FROM `account` ORDER BY `idaccount` DESC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            using var txn = await Db.Connection.BeginTransactionAsync();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `account`";
            await cmd.ExecuteNonQueryAsync();
            await txn.CommitAsync();
        }

        private async Task<List<account>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<account>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new account(Db)
                    {
                        idaccount = reader.GetInt32(0),
                        owner = reader.GetInt32(1),
                        balance = reader.GetDouble(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}