using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BGPlayerScript : MonoBehaviour {

	public AudioSource au;
	public AudioClip[] musicList;

	public Text n;
	public Text p;
	public Text t;

	public Image img;

	void Start () {
		au.clip = musicList[Random.Range(0,musicList.Length)];
		n.text = au.clip.name;
		p.text = "0%";
		t.text = ChangeSecToMinStr(au.time) + " / " + ChangeSecToMinStr(au.clip.length);
	}
	string ChangeSecToMinStr(float sec)
	{
		return string.Format("{0:#0} : {1:00}", (int)(sec/60), (int)(sec%60));
	}
	
	void Update () {
		t.text = ChangeSecToMinStr(au.time) + " / " + ChangeSecToMinStr(au.clip.length);
		float per = au.time / au.clip.length;
		img.fillAmount = per;
		p.text = (int)(per * 100) + "%";
	}

	public void PlayBG()
	{
		au.Play();
	}
	public void PauseBG()
	{
		au.Pause();
	}
	public void ResumeBG()
	{
		au.Play();
	}
}
