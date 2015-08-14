using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BGPlayerScript : MonoBehaviour {

	public AudioSource au;
	public AudioClip[] musicList;

	public Text n;
	public Text p;
	public Text t;

	public Image img;

	private bool isStart = false;
	private int ci = 0;
	private int musicSize = 0;

	void InitMusicList()
	{
		int n = musicList.Length;
		musicSize = n;
		while( n > 1 )
		{
			n--;
			int k = Random.Range(0, n);
			AudioClip temp = musicList[k];
			musicList[k] = musicList[n];
			musicList[n] = temp;
		}
	}
	void Start () {
		InitMusicList();
		au.clip = musicList[ci];
		UpdateBGInfo();
	}
	string ChangeSecToMinStr(float sec)
	{
		return string.Format("{0:#0} : {1:00}", (int)(sec/60), (int)(sec%60));
	}

	public void DebugTime()
	{
		SetBGTime(au.clip.length - 5);
	}

	void SetBGTime (float t) 
	{
		au.time = t;
	}

	void Update () {
		if(!au.isPlaying && isStart)
		{
			SetNextBG();
			PlayBG();
		}
		UpdateBGInfo();
	}

	void UpdateBGInfo()
	{
		n.text = au.clip.name;
		t.text = ChangeSecToMinStr(au.time) + " / " + ChangeSecToMinStr(au.clip.length);
		float per = au.time / au.clip.length;
		img.fillAmount = per;
		p.text = (int)(per * 100) + "%";
	}

	void SetNextBG()
	{
		ci++;
		if(ci >= musicSize)
			ci = 0;
		au.clip = musicList[ci];
		au.time = 0;
	}

	public void PlayBG()
	{
		isStart = true;
		au.Play();
	}
	public void PauseBG()
	{
		isStart = false;
		au.Pause();
	}
	public void ResumeBG()
	{
		PlayBG();
	}

}
