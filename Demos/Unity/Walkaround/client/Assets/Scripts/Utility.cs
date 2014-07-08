using UnityEngine;
using System.Collections.Generic;

public class Utility
{
	/// <summary>
	/// Determines if has component the specified name.
	/// </summary>
	/// <returns>true if has component the specified name; otherwise, false.</returns>
	/// <param name="name">Name.</param>
	public static bool HasComponent(string name)
	{
		return GameObject.Find(name) != null;
	}	

	/// <summary>
	/// Finds the component.
	/// </summary>
	/// <returns>The component.</returns>
	/// <param name="name">Name of the component.</param>
	/// <typeparam name="T">MonoBehavior derived class type.</typeparam>
	public static T FindComponent<T>(string name) where T : MonoBehaviour
	{
		return (T)GameObject.Find(name).GetComponent<T>();
	}

	/// <summary>
	/// Convert a List to a Vector2.
	/// </summary>
	/// <returns>Vector2 from list.</returns>
	/// <param name="list">List to convert.</param>
	public static Vector2 ListToVector2(List<int> list)
	{
		return new Vector2(list[0], list[1]);
	}


	/// <summary>
	/// Convert a Vector2 to list.
	/// </summary>
	/// <returns>List from Vector2.</returns>
	/// <param name="vector">Vector2 to convert.</param>
	public static List<int> Vector2ToList(Vector2 vector)
	{
		return new List<int>() { (int)vector.x, (int)vector.y };
	}
}
