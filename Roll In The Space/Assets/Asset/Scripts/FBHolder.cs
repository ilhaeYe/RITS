using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FBHolder : MonoBehaviour
{
	public GameObject UIFBLoggedIn;
	public GameObject UIFBNotLoggedIn;
	public GameObject UIFBAvatar;
	public GameObject UIFBUserName;
	public GameObject UIFBHighScore;
	public GameObject UIFBAchievement;

	public GameObject RankingView;
	private RankingViewController rankingViewController;

	//public GameObject DB;
	//private DataManager dataManager;
	//private ClientInfoData cid;

	private Dictionary<string,string> profile = null;
	private List<object> fList = null;
	//private DebugingScript ds;
	void Awake ()
	{
		//dataManager = DB.GetComponent<DataManager>();
		//cid = ClientInfoData.Instance;
		//ds = GameObject.Find("DebugObject").GetComponent<DebugingScript>();
		if(RankingView != null) rankingViewController = RankingView.GetComponent<RankingViewController>();
		DealWithFBMenus(false);

		enabled = false;
		FB.Init (SetInit, OnHideUnity);
		if (!FB.IsLoggedIn) {
			Debug.Log("Didn't login");
			//ds.debugTxt += "Didn't login\n";

			DoThingsAfterNotLoggedIn();
		}else{
			//ds.debugTxt = "Already logged in\n";

			DoThingsAfterLoggedIn();
		}
	}

	private void SetInit ()
	{
		Debug.Log ("FB Init done.");
		//ds.debugTxt += "FB Init done.\n";
		//enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn) {
			Debug.Log ("Already logged in");
			//ds.debugTxt += "Already logged in\n";
			DoThingsAfterLoggedIn();
		}else{
			DoThingsAfterNotLoggedIn();
		}
	}
	
	private void OnHideUnity (bool isGameShown)
	{                                                                                            
		Debug.Log ("OnHideUnity");                     
		//ds.debugTxt += "OnHideUnity";

		if (!isGameShown) {                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		} else {                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1; 
		}                                                                                        
	}

	public void FBlogin()
	{
		FB.Login("email", LoginCallback);
	}

	void LoginCallback(FBResult result)
	{
		if (FB.IsLoggedIn) {
			DoThingsAfterLoggedIn();
		}else{
			DoThingsAfterNotLoggedIn();
		}
	}
	void DoThingsAfterLoggedIn()
	{
		Debug.Log ("FB login worked!");
		//ds.debugTxt += "FB login worked!\n";
		
		DealWithFBMenus(true);
		GetMyRankingAreaData();
		InitRankingViewUI(true);
	}
	void DoThingsAfterNotLoggedIn()
	{
		Debug.Log ("FB login fail!");
		//ds.debugTxt += "FB login fail!\n";
		
		DealWithFBMenus(false);
		NotLoggedInGetScoreData();
		InitRankingViewUI(false);
	}
	void InitRankingViewUI(bool isLoggedIn)
	{
		if(rankingViewController != null)
		{
			rankingViewController.InitUI(isLoggedIn);
		}
	}

	void DealWithFBMenus(bool isLoggedIn)
	{
		if(isLoggedIn)
		{
			// get profile picture code
			FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePicture);
			// get username code
			FB.API("/me?fields=id,first_name,last_name", Facebook.HttpMethod.GET, DealWithUserName);

		}else{

		}
		UIFBLoggedIn.SetActive(isLoggedIn);
		UIFBNotLoggedIn.SetActive(!isLoggedIn);
	}
 
	void DealWithProfilePicture(FBResult result)
	{
		if(result.Error != null)
		{
			Debug.Log("problem with getting profile picture");
			//ds.debugTxt += "problem with getting profile picture\n";

			FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePicture);
			return;
		}
		Image UserAvatar = UIFBAvatar.GetComponent<Image>();
		UserAvatar.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2(0,0));
	}
	void DealWithUserName(FBResult result)
	{
		if(result.Error != null)
		{
			Debug.Log("problem with getting profile picture");
			//ds.debugTxt += "problem with getting profile picture\n";
			FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithUserName);
			return;
		}
		profile = Util.DeserializeJSONProfile(result.Text);
		Debug.Log(result.Text);
		//ds.debugTxt += result.Text;

		Text UserMsg = UIFBUserName.GetComponent<Text>();
		string first_name = profile["first_name"];
		string last_name = profile["last_name"];
		string full_name = first_name + "_" + last_name;
		UserMsg.text = full_name;
		// save client name
		ClientInfoData.SetClinetName(FB.UserId, first_name, last_name);
	}

	public void ShareWithFriends()
	{
		//ds.debugTxt += "ShareButtonClicked!\n";
		FB.Feed(
			linkCaption: "I'm playing this awesome game",
			picture: "http://ilhaeye.com/RITS/icon.png",
			linkName: "Check out this game",
			linkDescription: "Runner Game",
			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest") 
			//link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
			);
	}
	public void InviteFriends()
	{
		//ds.debugTxt += "InviteButtonClicked!\n";
		FB.AppRequest(
			message: "This game is awesome, join me now!",
			title: "Invite your friends to join you"
			);
	}
	public void GetFriendsList()
	{
//		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)",
		FB.API("/me/friends",
		       Facebook.HttpMethod.GET,
		       GetfriendsListCallback);
	}

	public void GetfriendsListCallback(FBResult result)
	{
		Debug.Log(result.Text);
		//friendsProfile = Util.DeserializeJSONFriends(result.Text);
		fList = Util.DeserializeJSONFriends(result.Text);
		Debug.Log(fList.Count);

		string debugTxt = result.Text + "\n cnt : " + fList.Count;
		//ds.debugTxt += debugTxt;

	}

	public void GetMyRankingAreaData()
	{
		if(FB.IsLoggedIn)
		{
			if(RankingView != null){
				// in mainmenu scene
				rankingViewController.GetMyPageInAllRanking();
			}else{
				Debug.Log("RankingView is null");
				//ds.debugTxt += "RankingView is null\n";

				// in game scene
				DataManager dm = DataManager.Instace;
				dm.GetUserRankData(HighScoreScript.highScoreScriptID, FB.UserId);
			}
		}
	}
	public void NotLoggedInGetScoreData()
	{
		ClientInfoData.score = PlayerPrefs.GetInt ("HighScore", 0);

	}

	public void StartGame()
	{
		Application.LoadLevel("GameScene");
	}
	
}
