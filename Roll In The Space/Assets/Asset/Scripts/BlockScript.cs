using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour
{
	public AudioClip blockSound;
	public float step;
	public Color32 color;
	
	private SystemController sys;
	private AudioSource source;
	protected Vector3 moveDes;

	void Start()
	{
		Renderer rend = GetComponent<Renderer>();
		if(rend != null) rend.material.color = color;
	}

	void FixedUpdate()
	{
		float moveSpeed = step * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, moveDes, moveSpeed);
	}

	public virtual void Awake ()
	{
		moveDes = transform.position;
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
	public virtual void DoButtonLogic()
	{

	}
	public void DestroyBlock()
	{
		// TODO

	}

}
