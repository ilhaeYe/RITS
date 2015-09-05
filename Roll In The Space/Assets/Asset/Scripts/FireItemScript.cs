using UnityEngine;
using System.Collections;

public class FireItemScript : MonoBehaviour {
	public AudioClip audioClip;
	
	private AudioSource source;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			source.PlayOneShot(audioClip);
			PlayerController player = other.GetComponent<PlayerController>();
			player.FireUp();
			Destroy(gameObject,1f);
		}
	}
}
