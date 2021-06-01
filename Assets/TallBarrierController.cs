using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallBarrierController : MonoBehaviour
{
	private float xRotation = 0;
	bool goDown = false;
	public float goDownSpeed = 10;

	private void OnEnable()
	{
		transform.localRotation = Quaternion.Euler(0, 0, 0);
		goDown = false;
		xRotation = 0;
	}


	private void Update()
	{
		if (xRotation > 90f)
		{
			gameObject.SetActive(false);
		}
		if (goDown)
		{
			xRotation += Time.deltaTime * goDownSpeed;
			transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			goDown = true;
		}
	}
}
