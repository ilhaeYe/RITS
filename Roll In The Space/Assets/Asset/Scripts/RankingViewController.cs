using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class RankingViewController : MonoBehaviour {

	private const int rankingViewID = 100;
	private GameObject pageButtonsContens;
	private GameObject contents;
	//private GameObject parent;
	public GameObject contentFramePrefab;
	public GameObject pageButtonPrefab;
	public GameObject scrollbarContainer;
	private Scrollbar scrollbar;
	//public GameObject DB;
	private DataManager dataManager;
	private enum QueryState { All, Friends };
	private QueryState queryState;
	
	// data struct
	public struct UserData
	{
		public string rank, score;
		public string fb_id;
		//public DateTime dt;
	}
	public List<UserData> dataList = new List<UserData>();	
	public List<GameObject> framesList = new List<GameObject>();
	public List<GameObject> pageButtonsList = new List<GameObject>();

	public int limitPageSize = 30;
	public int limitBlockSize = 10;

	public int allCount;
	public int currentPageIndex;	// start to 0
	public int currentBlockIndex;	// start to 0
	public int lastPageIndex;
	public int lastBlockIndex;

	// Use this for initialization
	void Start () {
		//parent = transform.parent.gameObject;
		//contents = transform.Find("Contents").gameObject;
		pageButtonsContens = transform.Find("PageButtonsContents").gameObject;
		contents = transform.Find("Ranking View").Find("Contents").gameObject;
		if(scrollbarContainer != null)
			scrollbar = scrollbarContainer.GetComponent<Scrollbar>();
		dataManager = DataManager.Instace;
		dataManager.OnHttpRequest += OnHttpRequest;
		queryState = QueryState.All;
	}

	public void OnHttpRequest (int request_id, WWW www)
	{
		if(request_id == rankingViewID)
		{
			ClearUserData();
			UpdateRequestDataList(www);
			UpdatePagingSystem();
			UpdateRankingView();
		}
	}
	private void ClearUserData()
	{
		if (dataList.Count > 0)
			dataList.Clear ();
	}
	private void UpdatePagingSystem()
	{
		// TODO
		ClearButtons();
		lastPageIndex = Mathf.CeilToInt((float)allCount / limitPageSize);
		lastBlockIndex = lastPageIndex / limitBlockSize;
		currentBlockIndex = currentPageIndex / limitBlockSize;

		GameObject prevBlock = AddPageButton("<<", currentBlockIndex - 1);
		if(currentBlockIndex == 0)
			prevBlock.GetComponent<PageButtonScript>().SetEnable(false);
		int fromIndex = currentBlockIndex * limitBlockSize;
		Debug.Log(fromIndex);
		int currentEndPageIndex = fromIndex + limitBlockSize;
		int toIndex = (currentEndPageIndex > lastPageIndex)? lastPageIndex : currentEndPageIndex; 
		for(int i=fromIndex; i<toIndex; i++)
		{
			GameObject btn = AddPageButton(""+i, i);
			if(i == currentPageIndex)
				btn.GetComponent<PageButtonScript>().SetColor(Color.blue);

		}
		GameObject nextBlock = AddPageButton(">>", currentBlockIndex + 1);
		if(currentBlockIndex == lastBlockIndex)
			nextBlock.GetComponent<PageButtonScript>().SetEnable(false);

		
		//if(currentBlockIndex != lastBlockIndex){

		//}
	}

	private GameObject AddPageButton(string str, int goToPageIndex)
	{
		GameObject clone;
		clone = Instantiate(pageButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		clone.transform.SetParent(pageButtonsContens.transform);
		PageButtonScript con = clone.GetComponent<PageButtonScript>();
		con.SetText(str);
		con.goToPageIndex = goToPageIndex;
		pageButtonsList.Add(clone);
		return clone;
	}

	private void UpdateRequestDataList(WWW www)
	{
		JsonData jData = JsonMapper.ToObject (www.text);
		allCount = int.Parse(jData["allCount"].ToString());
		currentPageIndex = int.Parse(jData["currentPageIndex"].ToString());
		for (int i=0; i<jData["rankData"].Count; i++) {
			UserData obj = new UserData ();
			obj.rank = (string)jData ["rankData"] [i] ["rank"];
			obj.fb_id = (string)jData ["rankData"] [i] ["fb_id"];
			obj.score = (string)jData ["rankData"] [i] ["score"];
			dataList.Add (obj);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void UpdateRankingView()
	{
		ClearFrames();
		SetUserDataInFrames();
	}
	public void GetMyPageInAllRanking()
	{
		dataManager.GetUserAreaRankData(rankingViewID, FB.UserId, limitPageSize);
		//UpdateRankingView();
	}
	public void GetMyPageInFriendsRanking()
	{

	}
	public void GetTargetIndexInAllRanking(int _pageIndex = 0)
	{
		dataManager.GetUserAllRankData(rankingViewID, _pageIndex, limitPageSize);
	}
	public void GetTargetIndexInFriendsRanking()
	{

	}


	private void SetUserDataInFrames()
	{
		int dataCnt = dataList.Count;
		float scrollbarValue = 1;
		Vector3 size = contentFramePrefab.GetComponent<RectTransform>().sizeDelta;
		if(dataCnt > 0){
			for(int i=0; i<dataCnt; i++)
			{
//				Debug.Log(i);
				GameObject clone;
				//Vector3 p = new Vector3(center.x, center.y - size.y * i, center.z);
				clone = Instantiate (contentFramePrefab, Vector3.zero, Quaternion.identity) as GameObject;
				clone.transform.SetParent(contents.transform);
				clone.transform.localPosition = new Vector3(0, -size.y * i, 0);
				Text rankTxt = clone.transform.FindChild("rank").GetComponent<Text>();
				Text scoreTxt = clone.transform.FindChild("score").GetComponent<Text>();
				Text idTxt = clone.transform.FindChild("id").GetComponent<Text>();
				Image image = clone.transform.FindChild("image").GetComponent<Image>();

				rankTxt.text = dataList[i].rank;
				scoreTxt.text = dataList[i].score;
				idTxt.text = dataList[i].fb_id;
				StartCoroutine(SetContentFBImage(idTxt.text, image));

				if(FB.UserId.CompareTo(idTxt.text) == 0)
				{
					rankTxt.color = Color.red;
					scoreTxt.color = Color.red;
					idTxt.color = Color.red;
					scrollbarValue = 1 - i / (float)dataCnt;
					//Debug.Log(i + "scrollbarValue : " + scrollbarValue);
				}

				framesList.Add(clone);
			}
		}else{
			Debug.Log("Data list is empty.");
		}
		scrollbar.value = scrollbarValue;

	}
	
	private void ClearFrames()
	{
		int cnt = framesList.Count;
		if(cnt > 0)
		{
			for(int i=cnt-1; i>= 0; i--)
			{
				Destroy(framesList[i].gameObject);
			}
			framesList.Clear();
		}
	}

	private void ClearButtons()
	{
		int cnt = pageButtonsList.Count;
		if (cnt > 0)
		{
			for(int i=cnt-1; i>=0; i--)
			{
				Destroy(pageButtonsList[i].gameObject);
			}
			pageButtonsList.Clear();
		}
	}

	private IEnumerator SetContentFBImage(string userId, Image image)
	{
		WWW www = new WWW("https://graph.facebook.com/" + userId + "/picture?g&width=50&height=50"); //+ "?access_token=" + FB.AccessToken);
		yield return www;

		if(www.error == null){
			if(image != null)
				image.sprite = Sprite.Create(www.texture, new Rect(0, 0, 50, 50), new Vector2(0,0));
		}
		else{
			Debug.Log("Image Load Error! ( User ID : " + userId + " )");
		}
	}
	public void test()
	{
		Dictionary<string,string> dic = new Dictionary<string, string>();
		dic.Add("?action=", "updateScore");
		dic.Add("&fb_id=", "123456789");
		dic.Add("&score=", ""+12345);

		foreach(var pair in dic)
		{
			Debug.Log(pair.Key + "|" + pair.Value);
		}

	}
}
