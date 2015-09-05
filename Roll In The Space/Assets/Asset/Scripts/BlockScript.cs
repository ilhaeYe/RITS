using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour
{
	public GameObject brickParticle;
	public AudioClip blockSound;
	public AudioClip destroySound;
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
		}else if(collision.gameObject.CompareTag ("PlayerFire")){
			Destroy(collision.gameObject);
			DestroyBlock();
			Destroy(gameObject);
		}
	}
	public virtual void DoButtonLogic()
	{

	}

	public void DestroyBlock()
	{
		source.PlayOneShot(destroySound);
		Collider col = GetComponent<Collider>();
		col.enabled = false;

		GameObject obj = (GameObject)Instantiate (brickParticle, transform.position, Quaternion.identity);
		Renderer r = obj.GetComponent<Renderer>();
		r.material.color = color;
	}

}
