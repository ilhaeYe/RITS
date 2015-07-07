using UnityEngine;
using System.Collections;

public class TouchListener : MonoBehaviour
{
	delegate void listener (ArrayList touches);

	event listener touchBegin, touchMove, touchEnd, touchStationary;

	PlayerController playerController;
	// Use this for initialization
	void Start ()
	{
		touchBegin += (touches) => {
//			Debug.Log ("begin");
			ClickMethod (touches);
		};
		touchMove += (touches) => {
//			Debug.Log ("Move");
			ClickMethod (touches);
		};
		touchEnd += (touches) => {
//			touches.Clear();
			Debug.Log ("End");
//			ClickMethod (touches);
		};
		touchStationary += (touches) => {
			ClickMethod (touches);
		};
		playerController = GetComponent<PlayerController> ();
	}

	void ClickMethod (ArrayList touches)
	{
		float half = Screen.width * 0.5f;
		int cnt = touches.Count;
		for (int i = 0; i < cnt; i++) {
			Touch t = (Touch)touches [i];
			if (t.position.x <= half) {
				float movement = playerController.touchSpeed * Time.deltaTime;
				playerController.radians -= movement;
				transform.position = new Vector3 (Mathf.Sin (playerController.radians * Mathf.Deg2Rad), Mathf.Cos (playerController.radians * Mathf.Deg2Rad), transform.position.z);
			} else {
				float movement = playerController.touchSpeed * Time.deltaTime;
				playerController.radians += movement;
				transform.position = new Vector3 (Mathf.Sin (playerController.radians * Mathf.Deg2Rad), Mathf.Cos (playerController.radians * Mathf.Deg2Rad), transform.position.z);
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		int cnt = Input.touchCount;
		if (cnt == 0)
			return;

		// event check flag
		bool begin, move, end, stationary;
		begin = move = end = stationary = false;

		// parameter
		ArrayList result = new ArrayList ();

		for (int i=0; i<cnt; i++) {
			Touch touch = Input.GetTouch (i);
			result.Add (touch);
//			if(touch.phase==TouchPhase.Began&&touchBegin!=null) begin = true;
//			else if(touch.phase==TouchPhase.Moved&&touchMove!=null) move = true;
//			else if(touch.phase==TouchPhase.Ended&&touchEnd!=null) end = true;
//			else if(touch.phase==TouchPhase.Stationary&&touchStationary!=null) stationary = true;
//
//			if(begin) touchBegin(result);
//			else if(end) touchEnd(result);
//			else if(move) touchMove(result);
//			else if(stationary) touchStationary(result);
			if (touch.phase == TouchPhase.Stationary && touchStationary != null)
				stationary = true;
			//			else if(touch.phase==TouchPhase.Ended&&touchEnd!=null) end = true;
			if (stationary)
				touchStationary (result);
			if (end) touchEnd(result);
		}

	}
}
