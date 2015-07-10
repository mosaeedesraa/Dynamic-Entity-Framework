using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DataLayer.IDataLayer;

// Eng : Mohammad Alsaid 

namespace DataLayer
{

    public class Tables<T> : ITables<T>
    {

        #region Variable

        SqlConnection conn;
        SqlCommand cmd;

        #endregion


        #region Constructor

        public Tables(string connection)
        {
            conn = new SqlConnection();
            conn.ConnectionString = connection;
            cmd = new SqlCommand();
            cmd.Connection = conn;
        }

        public Tables()
        {
            // TODO: Complete member initialization
        }

        #endregion


        #region Method

        /// <summary>
        /// Insert Row to Table
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Insert(T param)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                string query = string.Empty;
                Type TableType = typeof(T);

                string parameters = string.Empty;
                string p2 = string.Empty;
                string colnames = string.Empty;
                for (int i = 0; i < TableType.GetProperties().Length; i++)
                {
                    if (i != 0)
                    {
                        p2 += "@p" + i.ToString() + ",";
                        parameters += " '" + param.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null) + "' ,";
                        colnames += TableType.GetProperties()[i].Name + ",";
                    }
                }
                parameters = parameters.TrimEnd(',');
                p2 = p2.TrimEnd(',');
                colnames = colnames.TrimEnd(',');
                query = String.Format("INSERT INTO  {0} ({2}) VALUES({1} )", TableType.Name, p2, colnames);

                cmd.CommandText = query;
                for (int i = 0; i < TableType.GetProperties().Length; i++)
                {
                    if (i != 0)
                    {
                        cmd.Parameters.AddWithValue("@p" + i.ToString(), param.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null));
                    }
                }
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;

            }
            catch
            {
                return false;
            }

            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }


        /// <summary>
        /// Update row for table T
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool Update(T param)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                string query = string.Empty;
                Type TableType = typeof(T);

                string parameters = string.Empty;
                for (int i = 0; i < TableType.GetProperties().Length; i++)
                {
                    var value = param.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null) != null ?
                        param.GetType().GetProperty(TableType.GetProperties()[i].Name).GetValue(param, null).ToString()
                        : null;
                    value = value != null ? value.Replace(',', '.') : value;
                    if (i != 0)
                    {
                        parameters += TableType.GetProperties()[i].Name + " = '" +
                                       value + "' ,";

                    }
                }
                parameters = parameters.TrimEnd(',');

                string Wherecond = TableType.GetProperties()[0].Name + " = " + param.GetType().
                    GetProperty(TableType.GetProperties()[0].Name).GetValue(param, null);

                query = String.Format("Update {0} Set  {1} Where {2}", TableType.Name, parameters, Wherecond);

                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;

            }
            catch
            {
                return false;
            }

            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Select Data from Tables
        /// </summary>
        /// <param name="StoredProcedureName"></param>
        /// <param name="ParametersValues"></param>
        /// <param name="ParametersNames"></param>
        /// <returns></returns>
        public List<T> Select(string StoredProcedureName, ArrayList ParametersValues, string[] ParametersNames)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = StoredProcedureName;
                if (ParametersValues != null)
                {
                    for (int i = 0; i < ParametersValues.Count; i++)
                    {
                        SqlParameter spData = new SqlParameter();
                        spData.ParameterName = ParametersNames[i];
                        spData.Value = ParametersValues[i];
                        cmd.Parameters.Add(spData);
                    }
                }
                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results;


            }
            catch (Exception ee)
            {
                return null;
            }
        }

        /// <summary>
        /// Get All Rows from Table
        /// </summary>
        /// <returns></returns>
        public List<T> AllData()
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T); 
                var d = TableType.GetProperties().Count();
                string Query = string.Format("select * from  {0} ", TableType.Name);
                cmd.CommandText = Query;
                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results;


            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Insert - Update - Delete from Tables
        /// </summary>
        /// <param name="StoredProcedureName"></param>
        /// <param name="parametersValues"></param>
        /// <param name="parametersNames"></param>
        /// <returns></returns>
        public bool Execute(string StoredProcedureName, ArrayList parametersValues, string[] parametersNames)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = StoredProcedureName;
                if (parametersNames != null)
                {
                    for (int i = 0; i < parametersValues.Count; i++)
                    {
                        SqlParameter spData = new SqlParameter();
                        spData.ParameterName = parametersNames[i];
                        spData.Value = parametersValues[i] != null ? parametersValues[i] : "";
                        cmd.Parameters.Add(spData);
                    }
                }
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;

            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// First Record in Table
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public T FirstOrDefault()
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;


                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select top(1)* from {0}", TableType.Name);

                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results.FirstOrDefault();


            }
            catch
            {
                return default(T);
            }
        }


        public bool Delete(Int32 ID)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;


                Type TableType = typeof(T);
                cmd.CommandText = String.Format("delete from {0} where {1} = '{2}'", TableType.Name, TableType.GetProperties()[0].Name, ID);

                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }



        /// <summary>
        /// Last Record in Table
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T LastOrDefault()
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);

                cmd.CommandText = String.Format("select top(1)* from {0} ORDER BY {1} DESC", TableType.Name, TableType.GetProperties()[0].Name);

                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results.FirstOrDefault();


            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Find Record by ID
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T Find(Int32 ID)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select * from {0} where {1} = {2}", TableType.Name, TableType.GetProperties()[0].Name, ID);

                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results.FirstOrDefault();


            }
            catch
            {
                return default(T);
            }
        }


        /// <summary>
        /// Max value in column
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public decimal Max(string ColumnName)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);

                cmd.CommandText = String.Format("select Max({0}) as Result from {1} ", ColumnName, TableType.Name);

                SqlDataReader dr = cmd.ExecuteReader();
                decimal result = 0;
                while (dr.Read())
                {
                    result = Convert.ToDecimal(dr["Result"].ToString());
                }

                conn.Close();

                return result;


            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Min value in column
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public decimal Min(string ColumnName)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select Min({0}) as Result from {1} ", ColumnName, TableType.Name);

                SqlDataReader dr = cmd.ExecuteReader();
                decimal result = 0;
                while (dr.Read())
                {
                    result = Convert.ToDecimal(dr["Result"].ToString());
                }

                conn.Close();

                return result;


            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Sum values inside column
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public decimal Sum(string ColumnName)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select Sum({0}) as Result from {1} ", ColumnName, TableType.Name);

                SqlDataReader dr = cmd.ExecuteReader();
                decimal result = 0;
                while (dr.Read())
                {
                    result = Convert.ToDecimal(dr["Result"].ToString());
                }

                conn.Close();

                return result;


            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// Number rows in specific column
        /// </summary>
        /// <returns></returns>
        public Int32 Count()
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);

                cmd.CommandText = String.Format("select Count(*) as Result from {0} ", TableType.Name);

                SqlDataReader dr = cmd.ExecuteReader();
                Int32 result = 0;
                while (dr.Read())
                {
                    result = Convert.ToInt32(dr["Result"].ToString());
                }

                conn.Close();

                return result;


            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// Get data from table according to column asc
        /// </summary>
        /// <returns></returns>
        public List<T> OrderBy()
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select * from {0} order by {1} asc", TableType.Name, TableType.GetProperties()[0].Name);

                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results;


            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Get data from table according to column desc
        /// </summary>
        /// <returns></returns>
        public List<T> OrderByDescending()
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select * from {0} order by {1} desc", TableType.Name, TableType.GetProperties()[0].Name);

                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results;


            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Get all rows that a specefic word inside cell
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public List<T> Contains(string ColumnName, string Value)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select * from {0} where {1} Like '%{2}%'", TableType.Name, ColumnName, Value);

                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results;


            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Execute condition and return result data
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="Condition"></param>
        /// <returns></returns>
        public List<T> Where(string ColumnName, string Condition)
        {
            try
            {
                cmd.Parameters.Clear();
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                Type TableType = typeof(T);
                cmd.CommandText = String.Format("select * from {0} where {1} = '{2}'", TableType.Name, ColumnName, Condition);

                SqlDataReader dr = cmd.ExecuteReader();

                List<T> results = new List<T>().FromDataReader(dr).ToList();

                conn.Close();

                return results;


            }
            catch
            {
                return null;
            }
        }

        #endregion

    }
}
