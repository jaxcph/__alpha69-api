using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace alpha69.common.dto
{
    public class ModelProduct
    {
        public int ModelId { get; set; }
        public int ProductId { get; set; }


        protected DateTime _createdAt;
        public DateTime CreatedAt { get { return _createdAt; } }


       
        public ModelProduct()
        {
            
        }

        public ModelProduct(DataRow row)
        {
            ModelId = Convert.ToInt32(row["model_id"]);
            ProductId = Convert.ToInt32(row["product_id"]);
            this._createdAt = Convert.ToDateTime(row["created_at"]);
          
        }


        public void Save(MySqlConnection conn)
        {
            var cmd = new MySqlCommand($"INSERT INTO model_products(model_id,product_id) VALUES ({ModelId},{ProductId});COMMIT;", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Delete(MySqlConnection conn)
        {
            var cmd = new MySqlCommand($"DELETE FROM model_products where model_id={ModelId} AND product_id={ProductId};COMMIT;", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }


    }
}
