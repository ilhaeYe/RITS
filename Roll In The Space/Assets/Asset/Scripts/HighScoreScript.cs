using UnityEngine;
using System;
using System.Collections;
using LitJson;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour {

	private DataManager dataManager;
	private Text txt;
	public const int highScoreScriptID = 101;
	private int tempScore = -1;

	// Use this for initialization
	void Start () {
		dataManager = DataManager.Instace;
		dataManager.OnHttpRequest += OnHttpRequest;
		txt = GetComponent<Text>();
	}
	
	public void OnHttpRequest (int request_id, WWW www)
	{
		if(request_id == highScoreScriptID)
		{
			if(www.text.Trim() == string.Empty)
			{
				Debug.Log("empty data");
				ClientInfoData.score = 0;
			}else{
				Debug.Log("exist data");
				try{
					SetClientScoreData(www);
				}catch(Exception e){
					Debug.Log("Exception ( " + e + " )");
				}finally{
					Debug.Log ("finally doing");
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if( ClientInfoData.score != tempScore )
		{
			tempScore = ClientInfoData.score;
			SetScoreText(tempScore);
		}
	}

	private void SetClientScoreData(WWW www)
	{
		JsonData jData = JsonMapper.ToObject (www.text);
		for (int i=0; i<jData["rankData"].Count; i++) {

			//UserDataForDB obj = new UserDataForDB ();
			ClientInfoData.rank = int.Parse((string)jData ["rankData"] [i] ["rank"]);
			ClientInfoData.score = int.Parse((string)jData ["rankData"] [i] ["score"]);
			//SetScoreText(ClientInfoData.score);
		}
	}
	private void SetScoreText(int score)
	{
		txt.text = "High Score : " + score + " AU";
	}
}
