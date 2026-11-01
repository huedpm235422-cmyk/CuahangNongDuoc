/* =========================================================
 * Mục tiêu: Chuyển đổi tầng truy xuất dữ liệu từ Microsoft Access (cuahang.dll) 
 *           sang Microsoft SQL Server (QL_CuaHangNongDuoc).
 * Kỹ thuật áp dụng: Refactoring - Encapsulate Field & Replace Database Provider
 * ========================================================= */

using System;
using System.Data;
using System.Data.SqlClient;

namespace CuahangNongduoc
{
    public class DataService : DataTable
    {
        // 1. Quản lý kết nối SQL Server duy nhất
        private static SqlConnection m_Connection;

        // 2. Chuỗi kết nối trực tiếp đến Database trên máy của bạn
        public static string m_ConnectString = @"Data Source=DESKTOP-S1V4MEI\SQLEXPRESS;Initial Catalog=QL_CuaHangNongDuoc;Integrated Security=True;";

        // 3. Command và DataAdapter của SqlClient
        private SqlCommand m_Command;
        private SqlDataAdapter m_DataAdapter;

        public DataService() { }

        public SqlCommand Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        public void Load(SqlCommand command)
        {
            OpenConnection();
            m_Command = command;
            try
            {
                m_Command.Connection = m_Connection;

                m_DataAdapter = new SqlDataAdapter();
                m_DataAdapter.SelectCommand = m_Command;

                this.Clear();
                m_DataAdapter.Fill(this);
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
        }

        public static bool OpenConnection()
        {
            try
            {
                if (m_Connection == null)
                    m_Connection = new SqlConnection(m_ConnectString);

                if (m_Connection.State == ConnectionState.Closed)
                    m_Connection.Open();
                return true;
            }
            catch (Exception)
            {
                if (m_Connection != null && m_Connection.State == ConnectionState.Open)
                    m_Connection.Close();
                return false;
            }
        }

        /// <summary>
        /// Đóng kết nối cơ sở dữ liệu
        /// </summary>
        public void CloseConnection()
        {
            if (m_Connection != null && m_Connection.State == ConnectionState.Open)
                m_Connection.Close();
        }

        /// <summary>
        /// Cập nhật thay đổi của DataTable vào SQL Server
        /// </summary>
        public int ExecuteNoneQuery()
        {
            int result = 0;
            SqlTransaction tr = null;
            try
            {
                tr = m_Connection.BeginTransaction();

                m_Command.Connection = m_Connection;
                m_Command.Transaction = tr;

                m_DataAdapter = new SqlDataAdapter();
                m_DataAdapter.SelectCommand = m_Command;

                SqlCommandBuilder builder = new SqlCommandBuilder(m_DataAdapter);

                result = m_DataAdapter.Update(this);

                tr.Commit();
            }
            catch (Exception)
            {
                if (tr != null) tr.Rollback();
            }
            return result;
        }

        /// <summary>
        /// Thực thi một SqlCommand (Insert / Update / Delete)
        /// </summary>
        public int ExecuteNoneQuery(SqlCommand cmd)
        {
            int result = 0;
            SqlTransaction tr = null;

            try
            {
                tr = m_Connection.BeginTransaction();

                cmd.Connection = m_Connection;
                cmd.Transaction = tr;

                result = cmd.ExecuteNonQuery();

                this.AcceptChanges();

                tr.Commit();
            }
            catch (Exception)
            {
                if (tr != null) tr.Rollback();
                throw;
            }
            return result;
        }

        public object ExecuteScalar(SqlCommand cmd)
        {
            object result = null;
            SqlTransaction tr = null;

            try
            {
                tr = m_Connection.BeginTransaction();

                cmd.Connection = m_Connection;
                cmd.Transaction = tr;

                result = cmd.ExecuteScalar();

                this.AcceptChanges();

                tr.Commit();

                if (result == DBNull.Value)
                {
                    result = null;
                }
            }
            catch
            {
                if (tr != null) tr.Rollback();
                throw;
            }
            return result;
        }
    }
}