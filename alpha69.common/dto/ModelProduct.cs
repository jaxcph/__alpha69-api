using MySql.Data.MySqlClient;
using System;
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

        
       /* public static ModelProduct Load(int id, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id,user_id,name,description,website,facebook,twitter,instagram,snapchat, created_at FROM models WHERE id={id}", conn);
            var ds = new DataSet("models");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new Model(ds.Tables[0].Rows[0]);
            else
                return null;
        }

        public static ModelProduct LoadByUser(int userId, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id,user_id,name,description,website,facebook,twitter,instagram,snapchat, created_at FROM models WHERE user_id={userId}", conn);
            var ds = new DataSet("models");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new Model(ds.Tables[0].Rows[0]);
            else
                return null;
        }


        public static ModelProduct LoadByName(string name, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id,user_id,name,description,website,facebook,twitter,instagram,snapchat, created_at FROM models WHERE name=@name", conn);
            da.SelectCommand.Parameters.Add("@name",MySqlDbType.VarChar);
            da.SelectCommand.Parameters[0].Value = name;

            var ds = new DataSet("models");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new Model(ds.Tables[0].Rows[0]);
            else
                return null;
        }
        */

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
