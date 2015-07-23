using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {

	// singleton
	public static DataManager current = null;
	public static GameObject container = null;

	public static DataManager Instace
	{
		get{
			if(current==null)
			{
				container = new GameObject();
				container.name = "DataManager";
				current = container.AddComponent(typeof(DataManager)) as DataManager;
			}
			return current;
		}
	}

	// Web
	// delegate for event 
	public delegate void HttpRequestDelegate(int _id, WWW www);
	// event Handler
	public event HttpRequestDelegate OnHttpRequest;

	//bool result = false;
	//string url = "http://ilhaeye.tk/DBController.php";
	string baseURL = "http://192.168.219.161/DBController.php";
	string descURL;
	const int DEFAULT_GET_DATA_SIZE = 30;
	private Dictionary<string,string> urlLinkData = new Dictionary<string, string>();
	//public List<UserData> dataList = new List<UserData>();
	
	public void UpdateUserScore(int request_id, string _fb_id, int _score)
	{
		urlLinkData.Clear();
		urlLinkData.Add("?action=","updateScore");
		urlLinkData.Add("&fb_id=",_fb_id);
		urlLinkData.Add("&score=",""+_score);

		MakeURL();
		//MakeURL("updateScore",_fb_id,_score);
		StartCoroutine(Co_RequestURL(request_id));
	}
	public void GetUserRankData(int request_id, string _fb_id)
	{
		urlLinkData.Clear();
		urlLinkData.Add("?action=","getTargetRank");
		urlLinkData.Add("&fb_id=",_fb_id);

		MakeURL();
		//MakeURL("getTargetRank",_fb_id);
		StartCoroutine(Co_RequestURL(request_id));
	}
	public void GetUserAreaRankData(int request_id, string _fb_id, int _getDataSize = DEFAULT_GET_DATA_SIZE)
	{
		urlLinkData.Clear();
		urlLinkData.Add("?action=","getTargetAreaRank");
		urlLinkData.Add("&fb_id=",_fb_id);
		urlLinkData.Add("&getDataSize=",""+_getDataSize);
		
		MakeURL();
		//MakeURL("getTargetAreaRank",_fb_id);
		StartCoroutine(Co_RequestURL(request_id));
	}
	public void GetUserAllRankData(int request_id, int _pageIndex = 0, int _getDataSize = DEFAULT_GET_DATA_SIZE)
	{
		urlLinkData.Clear();
		urlLinkData.Add("?action=","getAllRank");
		urlLinkData.Add("&pageIndex=",""+_pageIndex);
		urlLinkData.Add("&getDataSize=",""+_getDataSize);
		
		MakeURL();
		//MakeURL("getAllRank", _pageIndex);
		StartCoroutine(Co_RequestURL(request_id));
	}

	void MakeURL()
	{
		descURL = baseURL;
		foreach(var pair in urlLinkData)
			descURL += pair.Key + pair.Value;
	}

//	void MakeURL(string action, int _pageIndex)
//	{
//		descURL = baseURL + "?action=" + action + "&pageIndex=" + _pageIndex;
//	}
////	void MakeURL(string action, int _pageIndex, int _getDataSize)
////	{
////		descURL = baseURL + "?action=" + action + "&pageIndex=" + _pageIndex + "&getDataSize=" + _getDataSize;
////	}
//	void MakeURL(string action, string _fb_id)
//	{
//		descURL = baseURL + "?action=" + action + "&fb_id=" + _fb_id;
//	}
////	void MakeURL(string action, string _fb_id, int _getDataSize)
////	{
////		descURL = baseURL + "?action=" + action + "&fb_id=" + _fb_id + "&getDataSize=" + _getDataSize;
////	}
//	void MakeURL(string action, string _fb_id, int _score)
//	{
//		string scoreStr = "" + _score;
//		descURL = baseURL + "?action=" + action + "&fb_id=" + _fb_id + "&score=" + scoreStr;
//	}

	IEnumerator Co_RequestURL(int request_id)
	{
		WWW www = new WWW(descURL);
		yield return www;
		if(www.error == null)
		{
//			result = true;
			Debug.Log("WWW OK! : " + www.text);
			if(OnHttpRequest != null)
			{
				OnHttpRequest(request_id, www);
			}
		}else{
//			result = false;
			Debug.Log("WWW Error : " + www.error);
		}
		www.Dispose();
	}
}