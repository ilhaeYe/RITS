using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour {
	public GameObject player;
	public Text distanceText;
	public Text highScoreText;
	public AudioClip tada;
	
	public PlayerController playerController;
	public int highScore;
	public int distance;
	private AudioSource source;
	private bool emphasize = false;
	private bool moveScore = false;
	private int scoreMoveSpeed = 10;
	private int tempScore;

	void Awake()
	{
		source = GetComponent<AudioSource> ();
	}

	void Start () {
		SetDistance ();
		LoadData ();
		tempScore = highScore;

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
		}
	}
	public void ScoreMove()
	{
		scoreMoveSpeed = (int)((distance - highScore) * 0.1f);
		moveScore = true;
	}
}
