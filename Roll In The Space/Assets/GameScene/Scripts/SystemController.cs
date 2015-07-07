using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class SystemController : MonoBehaviour {
	public List<GameObject> grounds;
	public List<GameObject> prefabs;
	public GameObject bgPrefabs;
	public List<GameObject> backGrounds;
	public bool isPause = true;
	public AudioClip baseSE;

	private bool keyAvailable = false;
	private GameObject player;
	private PlayerController playerController;
	private Animator anim_gameOver;
	private Text spacebar;
	private AudioSource bgSound;
	private HUDController HUD;
	void Awake()
	{
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerController> ();
		spacebar = GameObject.Find ("PressSpaceBarText").GetComponent<Text> ();
		bgSound = GetComponent<AudioSource> ();
		HUD = GameObject.Find ("HUDCanvas").GetComponent<HUDController> ();
		anim_gameOver = GameObject.Find ("HUDCanvas").GetComponent<Animator> ();
	}

	void Start()
	{
		InitGame ();
	}

	public void InitGame()
	{
		HUD.LoadData ();
		keyAvailable = false;
		player.transform.position = new Vector3(0, 1, 1);
		playerController.isLive = true;
		backGrounds [0].transform.position = new Vector3(0, 0, 0);

//		// clear ground gameobjects
//		int size = grounds.Count;
//		if (size != 0) {
//			for (int i=size; i>=0; i--) {
//				Destroy (grounds [i]);
//				grounds.RemoveAt (i);
//			}
//		}

		// init ground gameobject
		Vector3 gVec = new Vector3 (0, 0, 50);
		//CreateGrounds (gVec);
		GameObject obj = (GameObject)Instantiate (prefabs [0], gVec, Quaternion.identity);
		grounds.Add (obj);
		isPause = true;
		ShowSpaceBar ();

		// init keyInfo
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
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
		bgSound.Stop ();
		playerController.isLive = false;
		isPause = true;

		Rigidbody rb = player.gameObject.GetComponent<Rigidbody> ();
		Vector3 vec = new Vector3 (Mathf.Sin (playerController.radians * Mathf.Deg2Rad), Mathf.Cos (playerController.radians * Mathf.Deg2Rad), -1f);
		rb.AddForce (vec * playerController.blockingSpeed);

		// game over animation
//		GameObject obj = GameObject.Find ("HUDCanvas");
//		Animator anim = obj.GetComponent<Animator> ();
		anim_gameOver.SetTrigger ("GameOver");

		HUD.SaveData ();
		Invoke ("Restart", 2);
	}

	void Restart()
	{
		keyAvailable = true;
		ShowSpaceBar ();
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
	public void PlaySounds()
	{
		bgSound.Play ();
	}
	public void StartGame()
	{
		isPause = false;
		HideSpaceBar();
		PlaySounds ();
		HideKeyInfo ();
	}

	public void HideKeyInfo()
	{
		Destroy( GameObject.Find ("KeyInfo"),1f);
	}


}
