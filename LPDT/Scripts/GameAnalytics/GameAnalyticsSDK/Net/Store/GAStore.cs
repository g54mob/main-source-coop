using System;
using System.Collections.Generic;
using System.IO;
using GameAnalyticsSDK.Net.Device;
using GameAnalyticsSDK.Net.Logging;
using GameAnalyticsSDK.Net.Utilities;
using Mono.Data.Sqlite;

namespace GameAnalyticsSDK.Net.Store
{
	internal class GAStore
	{
		public const bool InMemory = false;

		private const long MaxDbSizeBytes = 6291456L;

		private const long MaxDbSizeBytesBeforeTrim = 5242880L;

		private static readonly GAStore _instance = new GAStore();

		private string dbPath = "";

		private bool _tableReady;

		private static GAStore Instance => _instance;

		private SqliteConnection SqlDatabase { get; set; }

		private bool DbReady { get; set; }

		public static bool IsTableReady
		{
			get
			{
				return Instance._tableReady;
			}
			private set
			{
				Instance._tableReady = value;
			}
		}

		public static bool IsDbTooLargeForEvents => DbSizeBytes > 6291456;

		public static long DbSizeBytes => new FileInfo(Instance.dbPath).Length;

		private GAStore()
		{
		}

		public static JSONArray ExecuteQuerySync(string sql)
		{
			return ExecuteQuerySync(sql, new Dictionary<string, object>());
		}

		public static JSONArray ExecuteQuerySync(string sql, Dictionary<string, object> parameters)
		{
			return ExecuteQuerySync(sql, parameters, useTransaction: false);
		}

		public static JSONArray ExecuteQuerySync(string sql, Dictionary<string, object> parameters, bool useTransaction)
		{
			if (GAUtilities.StringMatch(sql.ToUpperInvariant(), "^(UPDATE|INSERT|DELETE)"))
			{
				useTransaction = true;
			}
			SqliteConnection sqlDatabase = Instance.SqlDatabase;
			JSONArray jSONArray = new JSONArray();
			SqliteTransaction sqliteTransaction = null;
			SqliteCommand sqliteCommand = null;
			try
			{
				if (useTransaction)
				{
					sqliteTransaction = sqlDatabase.BeginTransaction();
				}
				sqliteCommand = sqlDatabase.CreateCommand();
				if (useTransaction)
				{
					sqliteCommand.Transaction = sqliteTransaction;
				}
				sqliteCommand.CommandText = sql;
				sqliteCommand.Prepare();
				if (parameters.Count != 0)
				{
					foreach (KeyValuePair<string, object> parameter in parameters)
					{
						sqliteCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
					}
				}
				using (SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						int fieldCount = sqliteDataReader.FieldCount;
						JSONObject jSONObject = new JSONObject();
						for (int i = 0; i < fieldCount; i++)
						{
							string name = sqliteDataReader.GetName(i);
							if (!string.IsNullOrEmpty(name))
							{
								jSONObject[name] = sqliteDataReader.GetValue(i).ToString();
							}
						}
						jSONArray.Add(jSONObject);
					}
				}
				if (useTransaction)
				{
					sqliteTransaction.Commit();
				}
			}
			catch (Exception ex)
			{
				GALogger.E("SQLITE3 ERROR: " + ex);
				jSONArray = null;
				if (useTransaction && sqliteTransaction != null)
				{
					try
					{
						sqliteTransaction.Rollback();
					}
					catch (Exception ex2)
					{
						GALogger.E("SQLITE3 ROLLBACK ERROR: " + ex2);
					}
					finally
					{
						sqliteTransaction.Dispose();
					}
				}
			}
			finally
			{
				sqliteCommand?.Dispose();
				sqliteTransaction?.Dispose();
			}
			return jSONArray;
		}

		public static bool EnsureDatabase(bool dropDatabase, string key)
		{
			if (string.IsNullOrEmpty(Instance.dbPath))
			{
				Instance.dbPath = Path.Combine(Path.Combine(GADevice.WritablePath, key), "ga.sqlite3");
				string path = Path.Combine(GADevice.WritablePath, key);
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				GALogger.D("Database path set to: " + Instance.dbPath);
			}
			try
			{
				Instance.SqlDatabase = new SqliteConnection("URI=file:" + Instance.dbPath + ";Version=3");
				Instance.SqlDatabase.Open();
				Instance.DbReady = true;
				GALogger.I("Database opened: " + Instance.dbPath);
			}
			catch (Exception ex)
			{
				Instance.DbReady = false;
				GALogger.W("Could not open database: " + Instance.dbPath + " " + ex);
				return false;
			}
			if (dropDatabase)
			{
				GALogger.D("Drop tables");
				ExecuteQuerySync("DROP TABLE ga_events");
				ExecuteQuerySync("DROP TABLE ga_state");
				ExecuteQuerySync("DROP TABLE ga_session");
				ExecuteQuerySync("DROP TABLE ga_progression");
				ExecuteQuerySync("VACUUM");
			}
			string sql = "CREATE TABLE IF NOT EXISTS ga_events(status CHAR(50) NOT NULL, category CHAR(50) NOT NULL, session_id CHAR(50) NOT NULL, client_ts CHAR(50) NOT NULL, event TEXT NOT NULL);";
			string sql2 = "CREATE TABLE IF NOT EXISTS ga_session(session_id CHAR(50) PRIMARY KEY NOT NULL, timestamp CHAR(50) NOT NULL, event TEXT NOT NULL);";
			string sql3 = "CREATE TABLE IF NOT EXISTS ga_state(key CHAR(255) PRIMARY KEY NOT NULL, value TEXT);";
			string sql4 = "CREATE TABLE IF NOT EXISTS ga_progression(progression CHAR(255) PRIMARY KEY NOT NULL, tries CHAR(255));";
			if (ExecuteQuerySync(sql) == null)
			{
				return false;
			}
			if (ExecuteQuerySync("SELECT status FROM ga_events LIMIT 0,1") == null)
			{
				GALogger.D("ga_events corrupt, recreating.");
				ExecuteQuerySync("DROP TABLE ga_events");
				if (ExecuteQuerySync(sql) == null)
				{
					GALogger.W("ga_events corrupt, could not recreate it.");
					return false;
				}
			}
			if (ExecuteQuerySync(sql2) == null)
			{
				return false;
			}
			if (ExecuteQuerySync("SELECT session_id FROM ga_session LIMIT 0,1") == null)
			{
				GALogger.D("ga_session corrupt, recreating.");
				ExecuteQuerySync("DROP TABLE ga_session");
				if (ExecuteQuerySync(sql2) == null)
				{
					GALogger.W("ga_session corrupt, could not recreate it.");
					return false;
				}
			}
			if (ExecuteQuerySync(sql3) == null)
			{
				return false;
			}
			if (ExecuteQuerySync("SELECT key FROM ga_state LIMIT 0,1") == null)
			{
				GALogger.D("ga_state corrupt, recreating.");
				ExecuteQuerySync("DROP TABLE ga_state");
				if (ExecuteQuerySync(sql3) == null)
				{
					GALogger.W("ga_state corrupt, could not recreate it.");
					return false;
				}
			}
			if (ExecuteQuerySync(sql4) == null)
			{
				return false;
			}
			if (ExecuteQuerySync("SELECT progression FROM ga_progression LIMIT 0,1") == null)
			{
				GALogger.D("ga_progression corrupt, recreating.");
				ExecuteQuerySync("DROP TABLE ga_progression");
				if (ExecuteQuerySync(sql4) == null)
				{
					GALogger.W("ga_progression corrupt, could not recreate it.");
					return false;
				}
			}
			TrimEventTable();
			IsTableReady = true;
			GALogger.D("Database tables ensured present");
			return true;
		}

		public static void SetState(string key, string value)
		{
			if (value == null)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("$key", key);
				ExecuteQuerySync("DELETE FROM ga_state WHERE key = $key;", dictionary);
			}
			else
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("$key", key);
				dictionary2.Add("$value", value);
				ExecuteQuerySync("INSERT OR REPLACE INTO ga_state (key, value) VALUES($key, $value);", dictionary2, useTransaction: true);
			}
		}

		private static void TrimEventTable()
		{
			if (DbSizeBytes <= 5242880)
			{
				return;
			}
			JSONArray jSONArray = ExecuteQuerySync("SELECT session_id, Max(client_ts) FROM ga_events GROUP BY session_id ORDER BY client_ts LIMIT 3");
			if (!(jSONArray != null) || jSONArray.Count <= 0)
			{
				return;
			}
			string text = "";
			for (int i = 0; i < jSONArray.Count; i++)
			{
				text += jSONArray[i].Value;
				if (i < jSONArray.Count - 1)
				{
					text += ",";
				}
			}
			string sql = "DELETE FROM ga_events WHERE session_id IN (\"" + text + "\");";
			GALogger.W("Database too large when initializing. Deleting the oldest 3 sessions.");
			ExecuteQuerySync(sql);
			ExecuteQuerySync("VACUUM");
		}
	}
}
