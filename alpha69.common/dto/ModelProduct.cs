using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class ModelProduct
    {
        protected DateTime _createdAt;


        public ModelProduct()
        {
        }

        public ModelProduct(DataRow row)
        {
            ModelId = Convert.ToInt32(row["model_id"]);
            ProductId = Convert.ToInt32(row["product_id"]);
            _createdAt = Convert.ToDateTime(row["created_at"]);
        }

        public int ModelId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedAt => _createdAt;


        public void Save(MySqlConnection conn)
        {
            var cmd = new MySqlCommand(
                $"INSERT INTO model_products(model_id,product_id) VALUES ({ModelId},{ProductId});COMMIT;", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Delete(MySqlConnection conn)
        {
            var cmd = new MySqlCommand(
                $"DELETE FROM model_products where model_id={ModelId} AND product_id={ProductId};COMMIT;", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}