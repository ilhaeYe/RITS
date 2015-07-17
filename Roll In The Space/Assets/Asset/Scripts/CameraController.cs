using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject player;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
		
		//Quaternion rotation = Quaternion.identity;
		
		//rotation.eulerAngles = new Vector3 (20, 0, -90);
		//transform.rotation = rotation;

	}
	
	// Update is called once per frame
	void Update () {
		PlayerController playerController = player.GetComponent<PlayerController> ();
		float radians = playerController.radians;
		transform.position = player.transform.position + offset;

		Vector3 desMove = new Vector3 (Mathf.Sin (radians * Mathf.Deg2Rad) * 2, Mathf.Cos (radians * Mathf.Deg2Rad) * 2, transform.position.z);

		transform.position = desMove;

		Quaternion rotation = Quaternion.identity;

		rotation.eulerAngles = new Vector3 (Mathf.Cos (radians * Mathf.Deg2Rad) * 20, Mathf.Sin (radians * Mathf.Deg2Rad) * -20, -radians);
		transform.rotation = rotation;
	}
}
