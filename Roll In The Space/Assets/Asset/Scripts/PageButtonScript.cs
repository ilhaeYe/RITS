using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageButtonScript : MonoBehaviour {

	private Text txt;
	private int _goToPageIndex;
	public int goToPageIndex{
		get{ return _goToPageIndex; }
		set{ _goToPageIndex = value; }
	}
	public void SetText(string str)
	{
		txt.text = str;
	}
	public void SetColor(Color color)
	{
		txt.color = color;
	}
	public void SetEnable(bool enabled)
	{
		if(enabled)
		{
			Button button = GetComponent<Button>();
			button.enabled = enabled;
			Image img = GetComponent<Image>();
			Color tempColor = img.color;
			tempColor.a = 1f;
			img.color = tempColor;
			tempColor = txt.color;
			tempColor.a = 1f;
			txt.color = tempColor; 
		}else{
			Button button = GetComponent<Button>();
			button.enabled = enabled;
			Image img = GetComponent<Image>();
			Color tempColor = img.color;
			tempColor.a = 0.3f;
			img.color = tempColor;
			tempColor = txt.color;
			tempColor.a = 0.3f;
			txt.color = tempColor; 
		}
	}
	// Use this for initialization
	void Awake () {
		txt = transform.Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
