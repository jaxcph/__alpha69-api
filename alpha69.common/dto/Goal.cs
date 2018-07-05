using System;
using System.Collections.Generic;
using System.Data;
using System.Security.AccessControl;
using System.Text;
using MySql.Data.MySqlClient;

namespace alpha69.common.dto
{
    public class Goal
    {

        private int _id;
        public int Id { get { return _id; } }



        protected DateTime _createdAt;
        public DateTime CreatedAt { get { return _createdAt; } }

        protected DateTime _completedAt;
        public DateTime CompletedAt { get { return _completedAt; } }

        protected DateTime _abortedAt;
        public DateTime AbortedAt { get { return   _abortedAt; } }

        public int LiveSessionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public double GoalAmount { get; set; }
        public double GoalAmountLeft { get; set; }



        public Goal()
        {
            
        }

        public Goal(DataRow row)
        {
            this._id = Convert.ToInt32(row["id"]);
            this._createdAt = Convert.ToDateTime(row["created_at"]);
            this._abortedAt = Convert.ToDateTime(row["aborted_at"]);
            this._completedAt = Convert.ToDateTime(row["completed_at"]);

            this.LiveSessionId = Convert.ToInt32(row["live_session_id"]);
            this.GoalAmount = Convert.ToDouble(row["goal_amount"]);
            this.GoalAmountLeft = Convert.ToDouble(row["goal_left"]);

            this.Title = row["title"] as String;
            this.Description = row["description"] as String;
            this.Tags = row["tags"] as String;
        }

        public static Goal LoadOpenByLiveSession(int livesessionId, MySqlConnection conn)
        {
            var da = new MySqlDataAdapter($"SELECT id, live_session_id, title, description, goal_amount, goal_left, created_at, completed_at, aborted_at, tags  FROM live_sessions_goals WHERE (live_session_id={livesessionId}) AND ( completed_at IS NULL AND aborted_at IS NULL) ORDER BY id DESC LIMIT 1", conn);
            var ds = new DataSet("live_sessions_goals");
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count == 1)
                return new Goal(ds.Tables[0].Rows[0]);
            else
                return null;

        }

        public static void SetAmountLeft(int id, double amount, MySqlConnection conn)
        {

            var cmd = new MySqlCommand($"UPDATE live_session_goals SET amount_left={amount} WHERE (id={id})", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public static void SetCompleted(int id, MySqlConnection conn)
        {
            var cmd = new MySqlCommand($"UPDATE live_session_goals SET completed_at=now() WHERE (id={id})", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void Abort(int id, MySqlConnection conn)
        {
            var cmd = new MySqlCommand($"UPDATE live_session_goals SET aborted_at=now() WHERE (id={id})", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Save(MySqlConnection conn)
        {
            //abort exisiting open of any
            var cmdExisting=new MySqlCommand($"UPDATE live_session_goals SET aborted_at=now() WHERE (live_session_id={LiveSessionId}) AND (completed_at IS NULL AND aborted_at IS NULL)",conn);


            //insert new
            var cmdInsert =new MySqlCommand($"INSERT INTO live_session_goals(live_session_id, title, description, goal_amount, goal_left, tags) VALUES({LiveSessionId},@title,@description,{GoalAmount},{GoalAmountLeft},@tags);COMMIT;SELECT LAST_INSERT_ID()",conn);

            cmdInsert.Parameters.Add("@title", MySqlDbType.VarChar);
            cmdInsert.Parameters.Add("@description", MySqlDbType.VarChar);
            cmdInsert.Parameters.Add("@tags", MySqlDbType.VarChar);

            cmdInsert.Parameters["@title"].Value = Title;

            if (string.IsNullOrEmpty(Description))
                cmdInsert.Parameters["@description"].Value = DBNull.Value;
            else
                cmdInsert.Parameters["@description"].Value = Description;

            if (string.IsNullOrEmpty(Tags))
                cmdInsert.Parameters["@tags"].Value = DBNull.Value;
            else
                cmdInsert.Parameters["@tags"].Value = Tags;

            conn.Open();
            cmdExisting.ExecuteNonQuery(); //closes abandoned goals
            var o = cmdInsert.ExecuteScalar(); //insert a new goal
            this._id = Convert.ToInt32(o);
            conn.Close();

        }



    }
}
