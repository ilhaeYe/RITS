using UnityEngine;
using UnityEngine.UI;
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

//	public GameObject DB;
	private DataManager dataManager;
	void Awake()
	{
		source = GetComponent<AudioSource> ();
		playerController = player.GetComponent<PlayerController>();
	}

	void Start () {
		SetDistance ();
		LoadData ();
		tempScore = highScore;
//		playerController = player.GetComponent<PlayerController>();
//		dataManager = DB.GetComponent<DataManager>();
	}

	// Update is called once per frame
	void Update () {
		SetDistance ();
		if (distance >= highScore && !emphasize) {
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
			highScoreText.text = "High Score : " + tempScore;
		else {
			source.PlayOneShot(tada);
			highScoreText.text = "High Score : " + distance;
			moveScore = false;
		}
	}
	
	void SetDistance()
	{
		if (playerController.isLive) {
			distance = (int)player.transform.position.z;
			distanceText.text = "Distance : " + distance;
		}
	}

	public void LoadData()
	{
		highScore = PlayerPrefs.GetInt ("HighScore", 0);
		highScoreText.text = "High Score : " + (int)highScore;
	}
	public void SaveData()
	{
		highScore = PlayerPrefs.GetInt ("HighScore", 0);
		if (distance > highScore) {
			PlayerPrefs.SetInt ("HighScore", distance);
			// TODO :: DB
			Invoke("ScoreMove",2f);
			StartCoroutine("DBUpdate");

		}
	}
	public void ScoreMove()
	{
		scoreMoveSpeed = (int)((distance - highScore) * 0.1f);
		moveScore = true;
	}
	IEnumerator DBUpdate()
	{
		if(FB.IsLoggedIn)
		{
			//string url = "http://ilhaeye.tk/DBController.php";
			// for debug
			string url = "http://192.168.219.161/DBController.php";
			string action = "updateScore";
			string score = "" + distance;
			url = url + "?action=" + action + "&fb_id=" + FB.UserId + "&score=" + score;
			WWW www = new WWW(url);
			yield return www;
			if(www.error == null)
			{
				Debug.Log("WWW OK! : " + www.text);
			}else{
				Debug.Log("WWW Error : " + www.error);

			}

			//bool isInDB = dataManager.SearchDB(FB.UserId);
			//if(isInDB) dataManager.UpdateDB(FB.UserId,distance);
			//else dataManager.InsertDB(FB.UserId,distance);
		}
		else{
			// guestRank
		}
		//Debug.Log( "FB userID : " + FB.UserId);
		//dataManager.
	}

}
