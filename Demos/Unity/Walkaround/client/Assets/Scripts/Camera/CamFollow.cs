using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour
{
	public GameObject Target;
	public Vector3 Offset;

	public MapData Map { get; set; }

	void LateUpdate()
	{
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
