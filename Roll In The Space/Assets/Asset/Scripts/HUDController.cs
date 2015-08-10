using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HUDController : MonoBehaviour {
	public GameObject player;
	public Text distanceText;
	public Text highScoreText;
	public AudioClip tada;
	
	private PlayerController playerController;
	public int highScore;
	public int distance;
	private AudioSource source;
	private bool emphasize = false;
	private bool moveScore = false;
	private int scoreMoveSpeed = 10;
	private int tempScore;

	private DataManager dataManager;
	public const int HUDControllerID = 102;

	void Awake()
	{
		source = GetComponent<AudioSource> ();
		playerController = player.GetComponent<PlayerController>();
	}

	void Start () {
		SetDistance ();
		LoadScoreData ();
		//tempScore = highScore;
		dataManager = DataManager.Instace;
		dataManager.OnHttpRequest += OnHttpRequest;
	}

	public void OnHttpRequest (int request_id, WWW www)
	{
		if(request_id == HUDControllerID)
		{
			if(www.text.Trim() == string.Empty)	// success
			{
				Debug.Log("Update successed");
				ClientInfoData.score = distance;
			}else{
				Debug.Log("Update Failed");
				try{
					//SetClientScoreData(www);
					// TODO pre
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
		SetDistance ();
		if (distance > highScore && !emphasize) {
			EmphasizeSocre();
			emphasize = true;
		}
		if (moveScore) {
			UpdateHighScore();
		}
	}
	void NormalScore()
	{
		distanceText.color = Color.white;
	}
	void EmphasizeSocre()
	{
		distanceText.color = Color.red;
		Invoke ("NormalScore", 1f);
	}
	void UpdateHighScore()
	{
		tempScore += scoreMoveSpeed;
		if (tempScore < distance)
			highScoreText.text = "High Score : " + tempScore + " AU";
		else {
			source.PlayOneShot(tada);
			highScoreText.text = "High Score : " + distance + " AU";
			ClientInfoData.score = distance;
			moveScore = false;
		}
	}
	
	void SetDistance()
	{
		if (playerController.isLive) {
			distance = (int)player.transform.position.z;
			distanceText.text = "Distance : " + distance + " AU";
		}
	}

	public void LoadScoreData()
	{
		highScore = ClientInfoData.score;
		tempScore = highScore;
	}
	public void SaveData()
	{
		if(FB.IsLoggedIn)
		{
			if(distance > ClientInfoData.score){
				dataManager.UpdateUserScore(HUDControllerID, FB.UserId,distance, ClientInfoData.fb_full_name);
				// TODO save clientInfoData
				Invoke("ScoreMove",2f);
			}
		}else{
			//highScore = PlayerPrefs.GetInt ("HighScore", 0);
			if (distance > ClientInfoData.score) {
				PlayerPrefs.SetInt ("HighScore", distance);
				Invoke("ScoreMove",2f);
			}
		}
	}
	public void ScoreMove()
	{
		scoreMoveSpeed = (int)((distance - highScore) * 0.1f);
		moveScore = true;
	}



}
