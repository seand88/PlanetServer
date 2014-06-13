using UnityEngine;
using System.Collections.Generic;

public class Utility
{
	public static bool HasServer()
	{
		return GameObject.Find(Server.NAME) != null;
	}	
	
	public static Server GetServer() 
	{ 
		return (Server)GameObject.Find(Server.NAME).GetComponent<Server>(); 
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
