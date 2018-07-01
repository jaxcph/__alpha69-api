using System;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class Purchase
    {
        private int _id;
        public int Id  { get { return _id; } }

        private int _userId;
        public int UserId { get { return _userId; } }


        private int _walletId;
        public int WalletId {get { return _walletId; }  }

        public double Amount { get; set; }
        public string Note { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PaymentProcessor { get; set; }

        protected DateTime _createdAt;
        public DateTime CreatedAt { get { return _createdAt; } }



        public Purchase()
        {
        }

        public Purchase(int userId, int walletId)
        {
            this._userId = userId;
            this._walletId = walletId;
        }
        
        public void Load(int id,MySqlConnection conn)
        {
            throw new NotImplementedException();
        }

        public void Save(MySqlConnection conn)
        {

            var cmd = new MySqlCommand($"INSERT INTO purchases(wallet_id, amount, payment_trans_id, payment_processor, note) VALUES({WalletId},{Amount},@PaymentTransactionId,@PaymentProcessor,@Note);COMMIT;SELECT LAST_INSERT_ID();", conn);
            cmd.Parameters.Add("@PaymentTransactionId", MySqlDbType.VarChar);
            cmd.Parameters.Add("@PaymentProcessor", MySqlDbType.VarChar);
            cmd.Parameters.Add("@Note", MySqlDbType.VarChar);

            if (!string.IsNullOrEmpty(PaymentTransactionId))
                cmd.Parameters["@PaymentTransactionId"].Value = PaymentTransactionId;
            else
                cmd.Parameters["@PaymentTransactionId"].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(PaymentProcessor))
                cmd.Parameters["@PaymentProcessor"].Value = PaymentProcessor;
            else
                cmd.Parameters["@PaymentProcessor"].Value = DBNull.Value;


            if (!string.IsNullOrEmpty(Note))
                cmd.Parameters["@Note"].Value = Note;
            else
                cmd.Parameters["@Note"].Value = DBNull.Value;
            

            conn.Open();

            var o = cmd.ExecuteScalar();
            this._id = Convert.ToInt32(o);

            conn.Close();
        }
    }
}
