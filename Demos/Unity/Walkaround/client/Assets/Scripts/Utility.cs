using UnityEngine;
using System.Collections;

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
}
