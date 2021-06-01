using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPoolingManager : MonoBehaviour
{
	public static ObjectPoolingManager instance;
	public int size = 20;
	public GameObject barrier;
	public GameObject tallBarrier;
	public GameObject tree;
	public GameObject coin;
	private List<GameObject> list = new List<GameObject>();

	public float[] positionX = { -2, 0, 2 };

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

		for (int i = 0; i < size; i++)
		{
			GameObject obj = Instantiate(barrier);
			obj.SetActive(false);
			list.Add(obj);
		}
		for (int i = 0; i < size; i++)
		{
			GameObject obj = Instantiate(tallBarrier);
			obj.SetActive(false);
			list.Add(obj);
		}
		for (int i = 0; i < size; i++)
		{
			GameObject obj = Instantiate(tree);
			obj.SetActive(false);
			list.Add(obj);
		}
		for (int i = 0; i < size; i++)
		{
			GameObject obj = Instantiate(coin);
			obj.SetActive(false);
			list.Add(obj);
		}
		list = Randomize<GameObject>(list);
	}

	public float zPosition = 100;
	private void Update()
	{
		List<GameObject> obj = Randomize<GameObject>(GetBlocks());
		
		if(obj.Count == 3)
		{
			for(int i=0;i < UnityEngine.Random.Range(1,3);i++)
			{
				float xPosition = positionX[i];
				GameObject block = obj[i];
				block.gameObject.transform.position = new Vector3(xPosition, block.transform.position.y, zPosition);
				block.SetActive(true);
			}
			zPosition += 50;
		}
	}

	public static List<T> Randomize<T>(List<T> list)
	{
		List<T> randomizedList = new List<T>();
		System.Random rnd = new System.Random();
		while (list.Count > 0)
		{
			int index = rnd.Next(0, list.Count); //pick a random item from the master list
			randomizedList.Add(list[index]); //place it at the end of the randomized list
			list.RemoveAt(index);
		}
		return randomizedList;
	}

	List<GameObject> GetBlocks()
	{
		List<GameObject> getObj = new List<GameObject>();
		int i = 0;
		foreach (GameObject obj in list)
		{
			if (!obj.activeInHierarchy && i < 3)
			{
				getObj.Add(obj);
				i++;
			}
		}
		return getObj;
	}
}
	
