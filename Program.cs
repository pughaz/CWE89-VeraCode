using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace CWE89Demos
{
	class Program
	{
		static string  connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnection"].ConnectionString;

		DataHelper _dataHelper = new DataHelper(connectionString);

		public static string UpdateDateUpdatedSql
		{
			get { return "DateUpdated = GETDATE()"; }
		}
		static void Main(string[] args)
		{
			string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnection"].ConnectionString;
			Approach1(connectionString);

			bool isMoved=MoveUserCode(200, "ABC", 1000, "1");

			UpdateForms("", "", 0, 0, 0);

		}

		/// <summary>
		/// $"EXEC SPName {param1} ,{param1}";
		/// </summary>
		/// <param name="connectionString"></param>
	


		#region Approach 1

		public static bool MoveUserCode(int codeType, string code, int newSegment, string approach)
		{
			//var result = new MessageResult { Result = true };

			try
			{
				DataHelper dataHelper = new DataHelper(connectionString);
				//string sql = "update tblUserFieldCodes set IdSegment = " + newSegment +
				//				 " where IdField = " + codeType +
				//				 " and Code = " + code;
				// Example 2: Executing a SQL statement with parameters
				string sqlQueryWithParams = "UPDATE SampleTableTestCWE89 SET IdSegment = @newSegment WHERE IdField = @codeType AND Code= @code";
				SqlParameter[] parameters = new SqlParameter[]
				                            {
					                            new SqlParameter("@newSegment", SqlDbType.Int) { Value = newSegment },
					                            new SqlParameter("@codeType", SqlDbType.Int) { Value = codeType },
					                            new SqlParameter("@code", SqlDbType.NVarChar) { Value = code }
				                            };
				int rowsUpdated = dataHelper.ExecuteNonQueryString(sqlQueryWithParams, parameters);
				Console.WriteLine($"Rows updated: {rowsUpdated}");
				return true;
			}
			catch (Exception ex)
			{
				return false;

			}

			return true;
		}



		#endregion

		#region "Approach 2"
		public static void Approach1(string connectionString)
		{
			int IDField = 1;
			int IDEdition = 1;

			string query = $"EXEC TestSPCWE89 {IDField} ,{IDEdition}";

			DataHelper _dataHelper = new DataHelper(connectionString);


			int rowsaffected = _dataHelper.ExecuteNonQueryString(query);

		}


		#endregion

		public static string UpdateForms(string topNode, string mode, int fieldID, int fromID, int toID)
		{
			topNode = "EditMode";
			mode = "EDIT";
			fieldID = 213;
			fromID = 100001301;
			toID = 10;


		string updateSql = "update tblTMSForms " +
									 "set FormXml.modify('replace value of (" + topNode + "/" + mode +
									 "/Tab/Zones/Zone/Groups/Group/Fields/Field[@FieldId=\"" + fieldID + "\"]/@DefaultValue)[1] with \"" + toID + "\"'), " +
									 UpdateDateUpdatedSql + " " +
									 "where FormXml.exist('(//Field[@FieldId=\"" + fieldID + "\" and @DefaultValue=\"" + fromID + "\"])[1]') = 1";
			return updateSql;
		}



	}


	}
