using System.Data;
using System.Threading.Tasks;
using MySqlConnector;

namespace bank_project
{
    public class account
    {
        public int idaccount { get; set; }
        public int owner { get; set; }
        public double balance { get; set; }

        internal AppDb Db { get; set; }

        public account()
        {
        }

        internal account(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `account` (`owner`, `balance`) VALUES (@owner, @balance);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            idaccount = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `account` SET `owner` = @owner, `balance` = @balance WHERE `idaccount` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `account` WHERE `idaccount` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = idaccount,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@owner",
                DbType = DbType.String,
                Value = owner,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@balance",
                DbType = DbType.Double,
                Value = balance,
            });
        }
    }
}