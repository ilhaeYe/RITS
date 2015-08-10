using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class SystemController : MonoBehaviour {
	public List<GameObject> grounds;
	public List<GameObject> prefabs;
	public GameObject bgPrefabs;
	public List<GameObject> backGrounds;

	private bool keyAvailable = false;
	private GameObject player;
	private PlayerController playerController;
	private Animator anim_gameOver;
	private Text spacebar;
	private HUDController HUD;

	// pause
	public bool isStartingPause = true;
	public bool pauseState = false;
	public bool pauseEnabled = false;
	public GameObject resumeButton;
	public GameObject restartButton;
	public GameObject mainButton;
	public GameObject BGObject;
	private BGPlayerScript bgController;

	void Awake()
	{
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerController> ();
		spacebar = GameObject.Find ("PressSpaceBarText").GetComponent<Text> ();
		HUD = GameObject.Find ("HUDCanvas").GetComponent<HUDController> ();
		anim_gameOver = GameObject.Find ("HUDCanvas").GetComponent<Animator> ();
		bgController = BGObject.GetComponent<BGPlayerScript>();
	}

	void Start()
	{
		InitGame ();
	}

	public void InitGame()
	{
		Time.timeScale = 1;
		ShowPauseMenu(false);
		HUD.LoadScoreData ();
		keyAvailable = false;
		player.transform.position = new Vector3(0, 1, 1);
		playerController.isLive = true;
		backGrounds [0].transform.position = new Vector3(0, 0, 0);

		// init ground gameobject
		Vector3 gVec = new Vector3 (0, 0, 50);
		//CreateGrounds (gVec);
		GameObject obj = (GameObject)Instantiate (prefabs [0], gVec, Quaternion.identity);
		grounds.Add (obj);
		isStartingPause = true;
		ShowSpaceBar ();

		// init keyInfo
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsWebPlayer
		    || Application.platform == RuntimePlatform.OSXWebPlayer) {
			Renderer leftTouch = GameObject.Find ("LeftTouch").GetComponent<Renderer> ();
			leftTouch.material.color = new Vector4 (0, 0, 0, 0);
			Renderer rightTouch = GameObject.Find ("RightTouch").GetComponent<Renderer> ();
			rightTouch.material.color = new Vector4 (0, 0, 0, 0);
		} else if (Application.platform == RuntimePlatform.Android) {
			Renderer a = GameObject.Find ("A").GetComponent<Renderer> ();
			a.material.color = new Vector4 (0, 0, 0, 0);
			Renderer d = GameObject.Find ("D").GetComponent<Renderer> ();
			d.material.color = new Vector4 (0, 0, 0, 0);
		}
	}

	public void GameOver()
	{
		bgController.PauseBG();
		playerController.isLive = false;
		isStartingPause = true;
		pauseEnabled = false;

		Rigidbody rb = player.gameObject.GetComponent<Rigidbody> ();
		Vector3 vec = new Vector3 (Mathf.Sin (playerController.radians * Mathf.Deg2Rad), Mathf.Cos (playerController.radians * Mathf.Deg2Rad), -1f);
		rb.AddForce (vec * playerController.blockingSpeed);

		// game over animation
		anim_gameOver.SetTrigger ("GameOver");

		HUD.SaveData ();
		Invoke ("RestartSetting", 2.5f);
	}

	void RestartSetting()
	{
		keyAvailable = true;
		ShowSpaceBar ();
		ShowPauseMenu(true);
		SetEnableResumeButton(false);
	}

	public void CreateGrounds(Vector3 gVec)
	{
		int r = Random.Range(0, prefabs.Count);	// return value = min ~ max -1
		GameObject obj = (GameObject)Instantiate (prefabs [r], gVec, Quaternion.identity);
		grounds.Add (obj);
	}

	public void DeleteGrounds()
	{
		Destroy (grounds [0]);
		grounds.RemoveAt (0);
	}

	public void CreateBackGround(Vector3 gVec)
	{
		GameObject Obj = (GameObject)Instantiate (bgPrefabs, gVec, Quaternion.identity);
		backGrounds.Add (Obj);
		Destroy (backGrounds [0]);
		backGrounds.RemoveAt (0);
	}

	void Update()
	{
		if (keyAvailable) {
			if(Input.GetKey(KeyCode.Space))
			{
				//InitGame();
				Application.LoadLevel("GameScene");
			}
		}
	}
	public void ShowSpaceBar()
	{
		float r = spacebar.color.r;
		float g = spacebar.color.g;
		float b = spacebar.color.b;

		spacebar.color = new Vector4 (r, g, b, 1f);
	}
	public void HideSpaceBar()
	{
		float r = spacebar.color.r;
		float g = spacebar.color.g;
		float b = spacebar.color.b;
		
		spacebar.color = new Vector4 (r, g, b, 0f);
	}
	public void StartGame()
	{
		isStartingPause = false;
		pauseEnabled = true;
		HideSpaceBar();
		HideKeyInfo ();
		bgController.PlayBG();
	}

	public void HideKeyInfo()
	{
		Destroy( GameObject.Find ("KeyInfo"),1f);
	}

	public void PauseButtonClicked()
	{
		if(playerController.isLive)
		{
			if(pauseState == false)
			{
				Pause();
			}else{
				Resume();
			}
		}
	}
	public void Pause()
	{
		Debug.Log("pause!!");
		Time.timeScale = 0;
		pauseState = true;
		ShowPauseMenu(true);
		bgController.PauseBG();
	}
	public void Resume()
	{
		Debug.Log("resume!!");
		Time.timeScale = 1;
		pauseState = false;
		ShowPauseMenu(false);
		bgController.ResumeBG();
	}
	public void ShowPauseMenu(bool b)
	{
		resumeButton.SetActive(b);
		restartButton.SetActive(b);
		mainButton.SetActive(b);
	}
	public void SetEnableResumeButton(bool b)
	{
		resumeButton.GetComponent<Button>().interactable = b;
	}
	public void GoToMainMenuScene()
	{
		Application.LoadLevel("MainScene");
	}
	public void ReStart()
	{
		Application.LoadLevel("GameScene");
	}
}
