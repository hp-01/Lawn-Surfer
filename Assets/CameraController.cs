using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform player;
	public Vector3 distance;
    // Start is called before the first frame update
    void Start()
    {
		distance = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + distance.z);
    }
}
