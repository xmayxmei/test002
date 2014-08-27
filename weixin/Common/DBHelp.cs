using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace bgweb
{
    public class DBHelp 
    {
        private string  connString = "";
        private SqlConnection cnn =null;
        private SqlTransaction trans = null;
        private string _name = String.Empty;

        public DBHelp()
        {
            connString = ConfigurationManager.ConnectionStrings["BgEntities"].ConnectionString;
        }
       
        public string ProfileName
        {
            get { return _name; }
            set { _name = value; }
        }

        public string GetUserID()
        {
            if (HttpContext.Current.Session["CurrentUser"] == null)
                return "";

            return HttpContext.Current.Session["CurrentUser"].ToString(); 
         }

        public object GetSessionValue(string name)
        {
            return HttpContext.Current.Session[name];
        }


        public string GetParameterName(string name)
        {
            return "@" + name;
        }

        public SqlConnection Open()
        {
            if (cnn == null)
            {
                cnn = new SqlConnection(connString);
                cnn.Open();
            }
              return cnn;
        }

        public IDbConnection GetDbConnection()
        {
            if (cnn == null)
                return this.Open();

            return cnn;
        }

        public void Close()
        {
            if(cnn!=null && cnn.State == ConnectionState.Open)
            {
                cnn.Close();
                cnn =null;
            }
        }

        public void Commit()
        {
            if (trans != null)
                trans.Commit();

        }

        public void Rollback()
        {
            if (trans != null)
                trans.Rollback();
        }

        public SqlCommand GetCommand(string sql)
        {
            SqlCommand cmd;
            if (trans == null)
                cmd = new SqlCommand(sql, cnn);
            else
                cmd = new SqlCommand(sql, cnn, trans);
            return cmd; 
        }
    }
}
