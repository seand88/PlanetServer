using UnityEngine;
using System.Collections.Generic;

public class Utility
{
	public static bool HasComponent(string name)
	{
		return GameObject.Find(name) != null;
	}	

	public static T FindComponent<T>(string name) where T : MonoBehaviour
	{
		return (T)GameObject.Find(name).GetComponent<T>();
	}

	public static Vector2 ListToVector2(List<int> list)
	{
		return new Vector2(list[0], list[1]);
	}

	public static List<int> Vector2ToList(Vector2 vector)
	{
		return new List<int>() { (int)vector.x, (int)vector.y };
	}
}
