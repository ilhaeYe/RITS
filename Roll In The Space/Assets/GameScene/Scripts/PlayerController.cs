using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float jumpSpeed;
	public float radians;
	public float rotateSpeed;
	public float blockingSpeed;
	public float minusSpeed;
	public float touchSpeed;
	private bool canJump = true;
	public bool isLive = true;
	public bool speedRun = false;
	public Texture normalTexture;
	public Texture itemUpTexture;
	SphereCollider sphereCollider;
	Rigidbody rb;
	Animator anim;
	SystemController sys;
	Renderer render;

	void Start ()
	{
		radians = 0f;
		sphereCollider = GetComponent<SphereCollider> ();
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		sys = GameObject.Find ("SystemController").GetComponent<SystemController> ();
		render = GetComponent<Renderer> ();
	}
	
	void Update ()
	{

	}

	void FixedUpdate ()
	{
		if (isLive && sys.isPause) {
			if (Input.GetKey ("space")) {
				sys.StartGame ();
			}
		} else if (isLive && !sys.isPause) {
			if (speedRun) {
				speed -= Time.deltaTime * minusSpeed;
				if (speed <= 15) {
					speedRun = false;
					speed = 15;
					sphereCollider.isTrigger = false;
				} else if (speed <= 30 && speed >= 29) {
					render.material.mainTexture = normalTexture;
					anim.SetTrigger ("Blink");
				}
			}
			float moveSpeed = speed * Time.deltaTime;
			Vector3 moveDes = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 1);
			transform.position = Vector3.MoveTowards (transform.position, moveDes, moveSpeed);
			transform.Rotate (new Vector3 (45, 0, 0) * Time.deltaTime);

			if (Input.GetKey ("space") && canJump) {
				canJump = false;
			}
//			if (Input.GetKey ("d")) {
//				float movement = rotateSpeed * Time.deltaTime;
//				radians += movement;
//				transform.position = new Vector3 (Mathf.Sin (radians * Mathf.Deg2Rad), Mathf.Cos (radians * Mathf.Deg2Rad), transform.position.z);
//			}
//			if (Input.GetKey ("a")) {
//				float movement = rotateSpeed * Time.deltaTime;
//				radians -= movement;
//				transform.position = new Vector3 (Mathf.Sin (radians * Mathf.Deg2Rad), Mathf.Cos (radians * Mathf.Deg2Rad), transform.position.z);
//			}

			float movement = rotateSpeed * Time.deltaTime * Input.GetAxisRaw ("Horizontal");
			radians += movement;
			transform.position = new Vector3 (Mathf.Sin (radians * Mathf.Deg2Rad), Mathf.Cos (radians * Mathf.Deg2Rad), transform.position.z);
		} else if (!isLive) {
			Invoke ("StopMove", 1);
		}
	}

	void StopMove ()
	{
		rb = GetComponent<Rigidbody> ();
		rb.velocity = Vector3.zero;
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.CompareTag ("Ground")) {
			canJump = true;
		}
	}

	public void SpeedRun ()
	{
		speed = 100;
		speedRun = true;
		sphereCollider.isTrigger = true;
		render.material.mainTexture = itemUpTexture;
	}
}
