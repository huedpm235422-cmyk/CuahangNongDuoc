using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc.DataLayer
{
    public class DonViTinhFactory
    {
        DataService m_Ds = new DataService();

        public DataTable DanhsachDVT()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DON_VI_TINH");
            m_Ds.Load(cmd);

            return m_Ds;
        }


        public DataTable LayDVT(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM DON_VI_TINH WHERE ID = @id");
            cmd.Parameters.Add("id", SqlDbType.Int).Value = id;
            m_Ds.Load(cmd);
            return m_Ds;
        }
        public bool Save()
        {
            return m_Ds.ExecuteNoneQuery() > 0;
        }
    }
}
