using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {

	// values to match the database columns
	int id, score;
	string fbid;

	bool saving = false;
	bool loading = false;

	// MySQL data
	string constr = "Server=localhost;Database=test;User ID=demo;Password=0806;Pooling=true";
	MySqlConnection con = null;
	MySqlCommand cmd = null;
	MySqlDataReader rdr = null;
	MySqlError err = null;
	GameObject[] bodies;

	//object definitions
	public struct data
	{
		public int id, score;
		public string fbid;
		public DateTime dt;
	}
	// collection container
	List<data>_items;

	void Awake()
	{
		try
		{
			// setup the connection element
			con = new MySqlConnection(constr);

			//lets see if we can open the connection
			con.Open();
			Debug.Log("Connection State : " + con.State);
		}catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
	}

	void OnApplicationQuit()
	{
		Debug.Log ("killing con");
		if(con != null)
		{
			Debug.Log ("con is not null");
			if(con.State != ConnectionState.Closed)
				con.Close();
			con.Dispose();
			Debug.Log ("con is kiied");
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InsertDB()
	{
		saving = true;
		InsertRankData("test100", 1500);

		LogItems();
		saving = false;
	}
	public void UpdateDB()
	{
		saving = true;
		UpdateRankData("test100", 8000);

		LogItems();
		saving = false;
	}
	public void SerchDB()
	{
		loading = true;
		bool result = SearchRankData("test100");
		if(result)
			Debug.Log("test100 is exist in DB");
		loading = false;
	}
	public void ReadAllDB()
	{
		loading = true;
		// lets read the items from the database
		ReadRankData();
		// now display what is known about them to our log
		LogItems();
		loading = false;
	}

	void InsertRankData(string _fbid, int _score)
	{
		string query = String.Empty;
		// Error trapping in the simplest form
		try
		{
			query = "INSERT INTO rank (fbid, score) VALUES (?fbid, ?score)";
			if(con.State != ConnectionState.Open)
				con.Open();
			using(con)
			{
				using(cmd = new MySqlCommand(query,con))
				{
					MySqlParameter oParam1 = cmd.Parameters.Add("?fbid", MySqlDbType.VarChar);
					oParam1.Value = _fbid;
					MySqlParameter oParam2 = cmd.Parameters.Add("?score", MySqlDbType.UInt16);
					oParam2.Value = _score;
					cmd.ExecuteNonQuery();
				}
			}
		}
		catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
		finally
		{
		}
	}
	void UpdateRankData(string _fbid, int _score)
	{
		string query = string.Empty;

		// Error trapping in the simplest form
		try
		{
			query = "UPDATE rank SET score=?score WHERE fbid=?fbid";
			if(con.State != ConnectionState.Open)
				con.Open();
			using(con)
			{
				using(cmd = new MySqlCommand(query,con))
				{
					MySqlParameter oParam1 = cmd.Parameters.Add("?fbid", MySqlDbType.VarChar);
					oParam1.Value = _fbid;
					MySqlParameter oParam2 = cmd.Parameters.Add("?score", MySqlDbType.UInt16);
					oParam2.Value = _score;
					cmd.ExecuteNonQuery();
				}
			}
		}
		catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
		finally
		{
		}
	}

	bool SearchRankData(string _fbid)
	{
		bool result = false;
		string query = string.Empty;
		// Error trapping in the simplest form
		try
		{
			query = "SELECT * FROM rank WHERE fbid=?fbid";
			if(con.State != ConnectionState.Open)
				con.Open();
			using(con)
			{
				using(cmd = new MySqlCommand(query,con))
				{
					MySqlParameter oParam1 = cmd.Parameters.Add("?fbid", MySqlDbType.VarChar);
					oParam1.Value = _fbid;
					rdr = cmd.ExecuteReader();
					if(rdr.HasRows)
						result = true;
					rdr.Dispose();
				}
			}
		}
		catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
		finally
		{
		}
		return result;
	}

	void ReadRankData()
	{
		string query = string.Empty;
		if(_items == null)
			_items = new List<data>();
		if(_items.Count > 0)
			_items.Clear();
		// Error trapping in the simplest form
		try
		{
			query = "SELECT * FROM rank";
			if(con.State != ConnectionState.Open)
				con.Open();
			using(con)
			{
				using(cmd = new MySqlCommand(query,con))
				{
					rdr = cmd.ExecuteReader();
					if(rdr.HasRows)
					{
						while(rdr.Read())
						{
							data itm = new data();
							itm.id = int.Parse(rdr["id"].ToString());
							itm.fbid = rdr["fbid"].ToString();
							itm.score = int.Parse(rdr["score"].ToString());
							itm.dt = DateTime.Parse(rdr["dt"].ToString());
							_items.Add(itm);
						}
					}
					rdr.Dispose();
				}
			}
		}catch(Exception ex)
		{
			Debug.Log(ex.ToString());
		}
		finally
		{
		}
	}

	/// <summary>
	/// Lets show what was read back to the log window
	/// </summary>
	void LogItems()
	{
		if(_items != null)
		{
			if(_items.Count > 0)
			{
				foreach(data itm in _items)
				{
					Debug.Log("===" + itm.id + "===");
					Debug.Log("fbid : " + itm.fbid);
					Debug.Log("score : " + itm.score);
					Debug.Log("dateTime : " + itm.dt);
				}
			}
		}
	}


}
