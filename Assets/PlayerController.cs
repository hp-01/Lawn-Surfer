using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public Rigidbody body;
	public float gravity = -18f;
	public float height = 3f;
	private Vector3 target = Vector3.zero;
	public GameObject timerText;
	public int health = 100;
	public float speed = 11f;
	private bool isActive = true;
	private bool jump = false;
	private bool isGrounded = false;
	public float distance = 2f;
	public GameObject lightEffect;
	public GameObject hardEffect;
	public GameObject explosion;
	private bool alive = false;
	private float time = 0f;

	//Swipe Detection
	// If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
	public const float MAX_SWIPE_TIME = 0.5f;

	// Factor of the screen width that we consider a swipe
	// 0.17 works well for portrait mode 16:9 phone
	public const float MIN_SWIPE_DISTANCE = 0.17f;

	public static bool swipedRight = false;
	public static bool swipedLeft = false;
	public static bool swipedUp = false;
	public static bool swipedDown = false;


	public bool debugWithArrowKeys = true;

	Vector2 startPos;
	float startTime;

	private void Awake()
	{
		body = GetComponent<Rigidbody>();
		Physics.gravity = Vector3.up * gravity;
	}

	private void Update()
	{
		time += Time.deltaTime;
		timerText.GetComponent<Text>().text = "" + (int)time; 
		if (!alive && time > 3f)
		{
			alive = true;
			timerText.SetActive(false);
		}
		health = Mathf.Clamp(health, 0, 100);
		HealthCheck();

		swipedRight = false;
		swipedLeft = false;
		swipedUp = false;
		swipedDown = false;

		if (Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
			{
				startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
				startTime = Time.time;
			}
			if (t.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
					return;

				Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);

				Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

				if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
					return;

				if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
				{ // Horizontal swipe
					if (swipe.x > 0)
					{
						swipedRight = true;
					}
					else
					{
						swipedLeft = true;
					}
				}
				else
				{ // Vertical swipe
					if (swipe.y > 0)
					{
						swipedUp = true;
					}
					else
					{
						swipedDown = true;
					}
				}
			}
		}

		if (debugWithArrowKeys)
		{
			swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow);
			swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow);
			swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow);
			swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);
		}

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		if (swipedLeft)
		{
			height = 1.5f;
			distance = 2f;
			target = new Vector3(target.x - distance, 0, z + 5);
			jump = true;
		}
		else if(swipedRight)
		{
			height = 1.5f;
			distance = 2f;
			target = new Vector3(target.x + distance, 0, z + 5);
			jump = true;
		}
		else if(swipedUp)
		{
			height = 2.8f;
			distance = 10f;
			target = new Vector3(x, 0, transform.position.z + distance);
			jump = true;
		}
		if (isGrounded)
		{
			if (x > -1.8 && x < 1.8)
			{
				transform.position = new Vector3(0, y, z);
			}
			else if (transform.position.x < -1.8)
			{
				transform.position = new Vector3(-2, y, z);
			}
			else if (x > 1.8)
			{
				transform.position = new Vector3(2, y, z);
			}
		}
	}

	private void HealthCheck()
	{
		if(health > 36 &&health <= 65)
		{
			lightEffect.SetActive(true);
		}

		if (health > 1 && health <= 35)
		{
			lightEffect.SetActive(false);
			hardEffect.SetActive(true);
		}
		if(health == 0)
		{
			lightEffect.SetActive(false);
			hardEffect.SetActive(false);
			explosion.SetActive(true);
			alive = false;
		}
	}

	private void FixedUpdate()
	{
		if (!alive) return;
		Vector3 velocity = Vector3.forward * speed;
		if(jump && isGrounded)
		{
			body.velocity = CalculateLaunchVelocity();
			jump = false;
			isGrounded = false;
		}
		if(isGrounded)
		{
			body.velocity = velocity;
		}
	}
	
	private Vector3 CalculateLaunchVelocity()
	{
		float displacementY = target.y - transform.position.y;
		Vector3 displacementXZ = new Vector3(target.x -transform.position.x, 0,
			target.z - transform.position.z);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
		Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity));
		return velocityXZ + velocityY * -Mathf.Sign(gravity);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
		}
		if(collision.gameObject.CompareTag("LeftWall"))
		{
			distance = 2f;
			target = new Vector3(-distance, 0, transform.position.z);
			body.velocity = CalculateLaunchVelocity();
			health -= 10;
		}
		else if (collision.gameObject.CompareTag("RightWall"))
		{
			distance = 2f;
			target = new Vector3(distance, 0, transform.position.z);
			body.velocity = CalculateLaunchVelocity();
			health -= 10;
		}
		if (collision.gameObject.CompareTag("Tree"))
		{
			health = 0;
		}
		else if(collision.gameObject.CompareTag("Barrier"))
		{
			health -= 35;
		}
		else if (collision.gameObject.CompareTag("TallBarrier"))
		{
			health -= 65;
		}
	}
}
/*if (Input.GetKeyDown(KeyCode.LeftArrow))
{
	height = 1.5f;
	distance = 2f;
	target = new Vector3(target.x - distance, 0, z + 5);
	jump = true;
}
else if (Input.GetKeyDown(KeyCode.RightArrow))
{
	height = 1.5f;
	distance = 2f;
	target = new Vector3(target.x + distance, 0, z + 5);
	jump = true;

}
else if (Input.GetKeyDown(KeyCode.UpArrow))
{
	height = 2.8f;
	distance = 10f;
	target = new Vector3(x, 0, transform.position.z + distance);
	jump = true;
}*/
