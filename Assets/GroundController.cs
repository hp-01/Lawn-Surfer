using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
	public Transform player;

	public float xPosition;
	private void OnEnable()
	{
		xPosition = transform.position.z + 260;
	}
    // Update is called once per frame
    void Update()
    {
        if(player.position.z > xPosition)
		{
			gameObject.SetActive(false);
		}
    }
}
