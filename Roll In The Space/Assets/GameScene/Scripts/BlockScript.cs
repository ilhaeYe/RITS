using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour
{
	public AudioClip blockSound;

	private SystemController sys;
	private AudioSource source;
	void Awake ()
	{
		sys = GameObject.FindGameObjectWithTag ("SystemController").GetComponent<SystemController> ();
		source = GetComponent<AudioSource> ();
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			source.PlayOneShot(blockSound);
			sys.GameOver();
			Renderer r = gameObject.GetComponent<Renderer> ();
			r.material.color = Color.red;
		}
	}

}
