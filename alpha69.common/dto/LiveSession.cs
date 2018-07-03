using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class LiveSession
    {
        private int _id;
        public int Id { get { return _id; } }

        public int HostModelId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public double RequiredUserScore { get; set; }


        public int EndedRating { get; set; }
        public string EndedRemark { get; set; }


        protected DateTime _startedAt;
        public DateTime StartedAt { get { return _startedAt; } }

        public DateTime EndedAt { get; set; }

        public bool Abandoned { get; set; }

        protected Model _hostModel;
        public Model HostModel
        {
            get { return _hostModel; }
        }


        public LiveSession()
        {

        }

        public LiveSession(DataRow row)
        {
            this._id = Convert.ToInt32(row["id"]);
            this.HostModelId= Convert.ToInt32(row["host_model_id"]);
            this.Title = row["title"] as String;
            this.Description = row["description"] as String;
            this.Tags = row["tags"] as String;
            this.RequiredUserScore = Convert.ToDouble(row["req_user_score"]);
            this._startedAt = Convert.ToDateTime(row["started_at"]);

            if (!row.IsNull("ended_at"))
                this.EndedAt = Convert.ToDateTime(row["ended_at"]);

            if (!row.IsNull("ended_rating"))
                this.EndedRating = Convert.ToInt16(row["ended_rating"]);

            if (!row.IsNull("ended_remark"))
                this.EndedRemark = (row["ended_remark"]) as string;

            this.Abandoned = Convert.ToBoolean(row["abandoned"]);

            
        }


        public static List<LiveSession> LoadOpenAll(MySqlConnection conn)
        {
            var list = new List<LiveSession>();

            var da = new MySqlDataAdapter($"SELECT id,host_model_id,title,description,tags,req_user_score,ended_at,ended_rating,ended_remark,started_at,abandoned FROM live_sessions WHERE (ended_at IS NULL) ORDER BY id DESC", conn);
            var ds = new DataSet("live_sessions");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count> 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var item = new LiveSession(row);
                    item._hostModel = Model.Load(item.HostModelId,true,conn);
                    list.Add(item);
                }
            }



            return list;
        }

        public static List<LiveSession> LoadAllByModel(int modelId, MySqlConnection conn)
        {
            var list = new List<LiveSession>();
            var da = new MySqlDataAdapter($"SELECT id,host_model_id,title,description,tags,req_user_score,ended_at,ended_rating,ended_remark,started_at,abandoned FROM live_sessions WHERE (host_model_id={modelId}) ORDER BY id DESC", conn);
            var ds = new DataSet("live_sessions");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new LiveSession(row));
                }
            }

            return list;
        }

       



        public static LiveSession LoadOpenById(int liveSessionId, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id,host_model_id,title,description,tags,req_user_score,ended_at,ended_rating,ended_remark,started_at,abandoned FROM live_sessions WHERE id={liveSessionId}  AND (ended_at IS NULL)", conn);
            var ds = new DataSet("live_sessions");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new LiveSession(ds.Tables[0].Rows[0]);
            else
                return null;

        }

        public static LiveSession LoadOpenForModel(int modelId, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id,host_model_id,title,description,tags,req_user_score,ended_at,ended_rating,ended_remark,started_at,abandoned FROM live_sessions WHERE (host_model_id={modelId}) AND (ended_at IS NULL) ORDER BY id DESC LIMIT 1", conn);
            var ds = new DataSet("live_sessions");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new LiveSession(ds.Tables[0].Rows[0]);
            else
                return null;

        }



        public void EndSession(MySqlConnection conn)
        {
            var cmd = new MySqlCommand($"UPDATE live_sessions SET ended_at=now(), ended_rating={EndedRating}, ended_remark=@ended_remark, abandoned={Abandoned} WHERE id={Id};COMMIT", conn);
            cmd.Parameters.Add("@ended_remark", MySqlDbType.VarChar);
            cmd.Parameters["@ended_remark"].Value = this.EndedRemark;

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Save(MySqlConnection conn)
        {
            var closeCmd=new MySqlCommand($"UPDATE live_sessions SET ended_at=now(), ended_rating=NULL, ended_remark=NULL, abandoned=1 WHERE (host_model_id={HostModelId}) AND (ended_at IS NULL);COMMIT", conn);

            var cmd = new MySqlCommand($"INSERT INTO live_sessions(host_model_id,title,description,req_user_score,tags,abandoned) VALUES ({HostModelId},@title,@description,{RequiredUserScore},@tags,0);COMMIT;SELECT LAST_INSERT_ID();", conn);
            cmd.Parameters.Add("@title", MySqlDbType.VarChar);
            cmd.Parameters.Add("@description", MySqlDbType.VarChar);
            cmd.Parameters.Add("@tags", MySqlDbType.VarChar);

            cmd.Parameters["@title"].Value = Title;

            if (string.IsNullOrEmpty(Description))
                cmd.Parameters["@description"].Value = DBNull.Value;
            else
                cmd.Parameters["@description"].Value = Description;

            if (string.IsNullOrEmpty(Tags))
                cmd.Parameters["@tags"].Value = DBNull.Value;
            else
                cmd.Parameters["@tags"].Value = Tags;

            conn.Open();
            closeCmd.ExecuteNonQuery(); //closes abandoned sessions for this model
            var o = cmd.ExecuteScalar(); //insert a new session and returns the id
            this._id = Convert.ToInt32(o);
            conn.Close();

        }
    }
}
