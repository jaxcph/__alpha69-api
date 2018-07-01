using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class User
    {
        private int _id;
        public int Id { get { return _id; } }


        public string SourceUserId { get; set; }
        public string SourceDomain { get; set; }

        protected string _hashKey;
        public string HashKey { get { return _hashKey; } }
        public string Login { get; set; }


        protected DateTime _createdAt;
        public DateTime CreatedAt { get { return _createdAt; } }
        

        public User()
        {

        }

        public User(DataRow row)
        {
            this._id = Convert.ToInt32(row["id"]);
            this._hashKey = row["hash_key"] as String;
            this._createdAt = Convert.ToDateTime(row["created_at"]);
            this.SourceDomain = row["src_domain"] as String;
            this.SourceUserId = row["src_user_id"] as String;
            this.Login = row["login"] as String;
        }

        public static User Load(int id, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id, hash_key, src_domain, src_user_id, login, created_at FROM users WHERE id={id}", conn);
            var ds = new DataSet("users");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new User(ds.Tables[0].Rows[0]);
            else
                return null;
        }

        public static User LoadBySourceUser(SourceUser sourceUser, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id, hash_key, src_domain, src_user_id, login, created_at FROM users WHERE src_domain='{sourceUser.Domain}' and src_user_id='{sourceUser.Id}'", conn);
            var ds = new DataSet("users");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new User(ds.Tables[0].Rows[0]);
            else
                return null;

        }


        public void Save(MySqlConnection conn)
        {

            this.SourceDomain = this.SourceDomain.ToLower();
            this.SourceUserId = this.SourceUserId.ToLower();
            this.Login = this.Login.ToLower();
            this._hashKey = createMD5($"{this.SourceUserId}@{this.SourceDomain}");

            var cmd = new MySqlCommand($"INSERT INTO users(hash_key, src_domain, src_user_id, login) VALUES('{_hashKey}',@SourceDomain,@SourceUserId,@Login);COMMIT;", conn);
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
            this._id = Convert.ToInt32(o);

            conn.Close();
        }


        // ----------------------------------------------------------------------------- //

        internal string createMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }



    }
}
