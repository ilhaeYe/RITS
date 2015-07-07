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

	private Dictionary<string,string> profile = null;

	void Start ()
	{
	
	}

	void Awake ()
	{
		enabled = false;
		FB.Init (SetInit, OnHideUnity);
		if (!FB.IsLoggedIn) {
			Debug.Log("Didn't login");
			// create login window
			//FB.Login ("email,publish_actions", LoginCallback);
		}
	}

	private void SetInit ()
	{
		Debug.Log ("FB Init done.");
		//enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn) {                                                                                        
			Debug.Log ("Already logged in");                                                    
			DealWithFBMenus(true);
		}else{
			DealWithFBMenus(false);
		}
	}
	
	private void OnHideUnity (bool isGameShown)
	{                                                                                            
		Debug.Log ("OnHideUnity");                                                              
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
			Debug.Log ("FB login worked!");
			DealWithFBMenus(true);
		}else{
			Debug.Log ("FB login fail!");
			DealWithFBMenus(false);
		}
	}

	void DealWithFBMenus(bool isLoggedIn)
	{
		if(isLoggedIn)
		{
			// get profile picture code
			FB.API(Util.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, DealWithProfilePicture);
			// get username code
			FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithUserName);

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
			FB.API("/me?fields=id,first_name", Facebook.HttpMethod.GET, DealWithUserName);
			return;
		}
		profile = Util.DeserializeJSONProfile(result.Text);

		Text UserMsg = UIFBUserName.GetComponent<Text>();
		UserMsg.text = "Hello, " + profile["first_name"];

	}

	public void ShareWithFriends()
	{
		FB.Feed(
			linkCaption: "I'm playing this awesome game",
			picture: "http://iu.wefondyou.com/king-include/uploads/381a4d73-faed-4625-a665-7dd4581b3566-508807781.jpg",
			linkName: "Check out this game",
			link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")
			);
	}
	public void InviteFriends()
	{
		FB.AppRequest(
			message: "This game is awesome, join me now!",
			title: "Invite your friends to join you"
			);
	}

}
