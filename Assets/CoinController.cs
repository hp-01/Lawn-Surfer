using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
	public float speed = 45f;
	public int score = 100;

	// Update is called once per frame
	void Update()
    {
		transform.Rotate(new Vector3(0, speed, 0) * Time.deltaTime);
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			GameManager.instance.score += score;
			gameObject.SetActive(false);
		}
	}
}
