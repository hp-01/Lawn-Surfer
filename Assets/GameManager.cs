using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
	public PlayerController playerController;
	public Transform player;
	public int score = 0;
	public static GameManager instance;
	public GameObject groundPrefab;
	List<GameObject> grounds;
	private int positionZ = 0;
	public Text scoreText;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this);
		}
	}

	private void Start()
	{
		grounds = new List<GameObject>();
		for(int i=0;i<4;i++)
		{
			GameObject obj = Instantiate(groundPrefab);
			obj.SetActive(false);
			grounds.Add(obj);
		}
	}

	int startPosition = 250;
	private void Update()
	{
		if(player.position.z > positionZ)
		{
			for(int i=0;i<2;i++)
			{
				GameObject ground = GetGround();
				ground.gameObject.transform.position = new Vector3(0, -1, startPosition);
				ground.SetActive(true);
				startPosition += 500;
			}
			positionZ += 250;
		}
		scoreText.text = "" + score;
	}

	GameObject GetGround()
	{
		foreach(GameObject ground in grounds)
		{
			if(!ground.activeInHierarchy)
			{
				return ground;
			}
		}
		GameObject obj = Instantiate(groundPrefab);
		obj.SetActive(false);
		grounds.Add(obj);
		return obj;
	}
}
