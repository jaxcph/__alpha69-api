using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class User
    {
        protected DateTime _createdAt;

        protected string _hashKey;


        public User()
        {
        }

        public User(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            _hashKey = row["hash_key"] as string;
            _createdAt = Convert.ToDateTime(row["created_at"]);
            SourceDomain = row["src_domain"] as string;
            SourceUserId = row["src_user_id"] as string;
            Login = row["login"] as string;
        }

        public int Id { get; private set; }


        public string SourceUserId { get; set; }
        public string SourceDomain { get; set; }
        public string HashKey => _hashKey;
        public string Login { get; set; }
        public DateTime CreatedAt => _createdAt;

        public static User Load(int id, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter(
                $"SELECT id, hash_key, src_domain, src_user_id, login, created_at FROM users WHERE id={id}", conn);
            var ds = new DataSet("users");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new User(ds.Tables[0].Rows[0]);
            return null;
        }

        public static User LoadBySourceUser(SourceUser sourceUser, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter(
                $"SELECT id, hash_key, src_domain, src_user_id, login, created_at FROM users WHERE src_domain='{sourceUser.Domain}' and src_user_id='{sourceUser.Id}'",
                conn);
            var ds = new DataSet("users");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new User(ds.Tables[0].Rows[0]);
            return null;
        }


        public void Save(MySqlConnection conn)
        {
            SourceDomain = SourceDomain.ToLower();
            SourceUserId = SourceUserId.ToLower();
            Login = Login.ToLower();
            _hashKey = createMD5($"{SourceUserId}@{SourceDomain}");

            var cmd = new MySqlCommand(
                $"INSERT INTO users(hash_key, src_domain, src_user_id, login) VALUES('{_hashKey}',@SourceDomain,@SourceUserId,@Login);COMMIT;",
                conn);
            cmd.Parameters.Add("@SourceDomain", MySqlDbType.VarChar);
            cmd.Parameters.Add("@SourceUserId", MySqlDbType.VarChar);
            cmd.Parameters.Add("@Login", MySqlDbType.VarChar);
            cmd.Parameters["@SourceDomain"].Value = SourceDomain;
            cmd.Parameters["@SourceUserId"].Value = SourceUserId;
            cmd.Parameters["@Login"].Value = Login;


            var cmdSelect = new MySqlCommand($"SELECT id FROM users WHERE hash_key='{_hashKey}'", conn);

            conn.Open();

            cmd.ExecuteNonQuery();
            var o = cmdSelect.ExecuteScalar();
            Id = Convert.ToInt32(o);

            conn.Close();
        }


        // ----------------------------------------------------------------------------- //

        internal string createMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
    }
}