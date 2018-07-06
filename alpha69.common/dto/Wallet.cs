using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class Wallet
    {
        protected DateTime _createdAt;


        public Wallet()
        {
        }

        public Wallet(DataRow row)
        {
            Id = Convert.ToInt32(row["id"]);
            UserId = Convert.ToInt32(row["user_id"]);
            ProductId = Convert.ToInt32(row["product_id"]);
            Balance = Convert.ToDouble(row["balance"]);
            TotalTipped = Convert.ToDouble(row["total_tipped"]);
            TotalPurchased = Convert.ToDouble(row["total_purchased"]);
            _createdAt = Convert.ToDateTime(row["created_at"]);
        }

        public int Id { get; private set; }


        public int ProductId { get; set; }
        public int UserId { get; set; }

        public double Balance { get; set; }
        public double TotalPurchased { get; set; }
        public double TotalTipped { get; set; }
        public DateTime CreatedAt => _createdAt;


        public static Wallet Load(int id, MySqlConnection conn)
        {
            throw new NotImplementedException();
        }


        public static Wallet LoadByUserProduxt(int userId, int productId, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter(
                $"SELECT id,user_id,product_id,balance,total_tipped,total_purchased,created_at FROM wallets WHERE user_id={userId} AND product_id={productId}",
                conn);
            var ds = new DataSet("wallets");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new Wallet(ds.Tables[0].Rows[0]);
            return null;
        }


        public void Save(MySqlConnection conn)
        {
            var cmd = new MySqlCommand(
                $"INSERT INTO wallets(user_id, product_id, balance, total_tipped, total_purchased) VALUES({UserId},{ProductId},{Balance},{TotalTipped},{TotalPurchased});COMMIT;",
                conn);
            var cmdSelect =
                new MySqlCommand($"SELECT id FROM wallets WHERE user_id={UserId} AND product_id={ProductId}", conn);

            conn.Open();

            cmd.ExecuteNonQuery();
            var o = cmdSelect.ExecuteScalar();
            Id = Convert.ToInt32(o);

            conn.Close();
        }

        public void Update(MySqlConnection conn)
        {
            var cmd = new MySqlCommand(
                $"UPDATE wallets SET balance={Balance}, total_tipped={TotalTipped}, total_purchased={TotalPurchased} WHERE id={Id};COMMIT;",
                conn);
            var cmdSelect =
                new MySqlCommand($"SELECT id FROM wallets WHERE user_id={UserId} AND product_id={ProductId}", conn);

            conn.Open();

            cmd.ExecuteNonQuery();
            var o = cmdSelect.ExecuteScalar();
            Id = Convert.ToInt32(o);

            conn.Close();
        }
    }
}