using UnityEngine;
using System.Collections;

/// <summary>
/// Makes the attached camera follow the target.
/// </summary>
public class CamFollow : MonoBehaviour
{
	/// <summary>
	/// The target to follow.
	/// </summary>
	public GameObject Target;
	/// <summary>
	/// How far the camera is from the target. 
	/// </summary>
	public Vector3 Offset;

	/// <summary>
	/// Current map. 
	/// </summary>
	/// <value>The map.</value>
	public MapData Map { get; set; }

	void LateUpdate()
	{
		// keep the camera centered on the player while keeping the camera confined to the edges of the map
		if (Target)
		{
			int half_width = Constants.SCREEN_WIDTH / Constants.TILE_WIDTH / 2;
			int half_height = Constants.SCREEN_HEIGHT / Constants.TILE_HEIGHT / 2;
	
			float x = Mathf.Max(Target.transform.position.x, half_width);		
			float y = Mathf.Min(Target.transform.position.y, -half_height);
		
			x = Mathf.Min(x, Map.Width - half_width);
			y = Mathf.Max(y, -(Map.Height - half_height));    
	
			Vector3 pos = new Vector3(x, y, 0);
	
			transform.position = pos + Offset;
		}
	}
}
