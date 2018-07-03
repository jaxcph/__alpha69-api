using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class Product
    {
        private int _id;
        public int Id { get { return _id; } }
        protected DateTime _createdAt;
        public DateTime CreatedAt { get { return _createdAt; } }


        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool AllowTipping { get; set; }
        public bool AllowPayPerMinute { get; set; }


        public Product()
        {
            
        }

        public Product(DataRow row)
        {
            this._id = Convert.ToInt32(row["id"]);
            this.DisplayName = row["shortname"] as String;
            this.Description = row["descr"] as String;
            this._createdAt = Convert.ToDateTime(row["created_at"]);
            this.AllowTipping = Convert.ToBoolean(row["allow_tipping"]);
            this.AllowPayPerMinute = Convert.ToBoolean(row["allow_ppm"]);
        }

        

        public static List<Product> LoadForModel(int modelId, MySqlConnection conn)
        {
          

            var list = new List<Product>();

            var da = new MySqlDataAdapter($"SELECT p.id,p.shortname,p.descr,p.created_at,allow_tipping,allow_ppm FROM products p, model_products mp WHERE (mp.model_id={modelId}) AND (mp.product_id=p.id) AND (p.active=1) ORDER BY id", conn);
            var ds = new DataSet("products");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var item = new Product(row);
                    list.Add(item);
                }
            }

            return list;
        }


    }
}
