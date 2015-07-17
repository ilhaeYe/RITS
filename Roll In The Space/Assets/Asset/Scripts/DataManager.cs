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
	string constr = "Server=localhost;Database=RITS;User ID=user;Password=0806;Pooling=true";
	MySqlConnection con = null;
	MySqlCommand cmd = null;
	MySqlDataReader rdr = null;
	MySqlError err = null;
	GameObject[] bodies;

	//object definitions
	public struct data
	{
		public int rank_id, score;
		public string fb_id;
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

	public void InsertDB(string id, int score)
	{
		saving = true;
		InsertRankData(id, score);

		LogItems();
		saving = false;
	}
	public void UpdateDB(string id, int score)
	{
		saving = true;
		UpdateRankData(id, score);

		LogItems();
		saving = false;
	}
	public bool SearchDB(string id)
	{
		loading = true;
		bool result = SearchRankData(id);
		loading = false;
		return result;
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
			query = "INSERT INTO user_rank (fb_id, score) VALUES (?fb_id, ?score)";
			if(con.State != ConnectionState.Open)
				con.Open();
			using(con)
			{
				using(cmd = new MySqlCommand(query,con))
				{
					MySqlParameter oParam1 = cmd.Parameters.Add("?fb_id", MySqlDbType.VarChar);
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
			query = "UPDATE user_rank SET score=?score WHERE fb_id=?fb_id";
			if(con.State != ConnectionState.Open)
				con.Open();
			using(con)
			{
				using(cmd = new MySqlCommand(query,con))
				{
					MySqlParameter oParam1 = cmd.Parameters.Add("?fb_id", MySqlDbType.VarChar);
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
			query = "SELECT * FROM user_rank WHERE fb_id=?fb_id";
			if(con.State != ConnectionState.Open)
				con.Open();
			using(con)
			{
				using(cmd = new MySqlCommand(query,con))
				{
					MySqlParameter oParam1 = cmd.Parameters.Add("?fb_id", MySqlDbType.VarChar);
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
			query = "SELECT * FROM user_rank";
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
							itm.rank_id = int.Parse(rdr["rank_id"].ToString());
							itm.fb_id = rdr["fb_id"].ToString();
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
					Debug.Log("===" + itm.rank_id + "===");
					Debug.Log("fb_id : " + itm.fb_id);
					Debug.Log("score : " + itm.score);
					Debug.Log("dt : " + itm.dt);
				}
			}
		}
	}


}
