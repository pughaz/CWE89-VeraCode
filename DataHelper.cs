using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CWE89Demos
{
	public class DataHelper: IDisposable
	{
		public DataHelper(string connectionString)
		{
			if (connectionString == string.Empty)
			{
				throw new System.ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
			}

			SqlConnection = new SqlConnection(connectionString);
			SqlConnection.Open();
		}

		public DataHelper(SqlConnection sqlConnection)
		{
			if (sqlConnection == null)
			{
				throw new System.ArgumentException("Sql Connection cannot be null.", nameof(sqlConnection));
			}

			SqlConnection = sqlConnection;
			if (SqlConnection.State == ConnectionState.Closed)
			{
				SqlConnection.Open();
			}
		}

		public SqlConnection SqlConnection { get; set; }


		public int ExecuteNonQueryString(string sql)
		{
			var command = new SqlCommand(sql, SqlConnection);
			return command.ExecuteNonQuery();
		}

		#region "New Approach"
		public int ExecuteNonQueryString(string sql, SqlParameter[] parameters)
		{
		
				using (SqlCommand command = new SqlCommand(sql, SqlConnection))
				{
					if (parameters is { Length: > 0})
					{
						command.Parameters.AddRange(parameters);
					}
					
					return command.ExecuteNonQuery();
				}
			
		}

		#endregion


		#region IDisposable implementation

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing || SqlConnection == null) return;

			if (SqlConnection.State != ConnectionState.Closed)
			{
				SqlConnection.Close();
			}

			SqlConnection = null;
		}

		#endregion IDisposable implementation
	}
}
