using UnityEngine;
using System.Collections;

/// <summary>
/// A shot from a player.
/// </summary>
public class Shot : MonoBehaviour
{
	public float Speed;
	public float Lifetime;

	public Vector2 Heading { get; private set; }
	public LayerMask Layer { get; private set; }

	/// <summary>
	/// Create a shot.  Generally for the player.
	/// </summary>
	/// <param name="heading">Heading of the shot.</param>
	/// <param name="heading">Heading of the shot.</param>
	public static Shot Create(Player player, Vector2 heading)
	{
		string path = PrefabUtility.GetShotsPath("Fireball");
	
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), player.Position, Quaternion.identity);
		go.name = "Shot (Clone)";
		Shot shot = go.GetComponent<Shot>();
		shot.Setup(heading, player.gameObject.layer);

		return shot;
	}

	/// <summary>
	/// Create a shot.  Generally for another player on the server.
	/// </summary>
	/// <param name="heading">Heading of the shot.</param>
	/// <param name="position">Position of the player when fired.</param>
	/// <param name="heading">Heading of the shot.</param>
	public static Shot Create(Player player, Vector2 position, Vector2 heading)
	{
		string path = PrefabUtility.GetShotsPath("Fireball");
		
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.name = "Shot (Clone)";
		Shot shot = go.GetComponent<Shot>();
		shot.Setup(heading, player.gameObject.layer);
		
		return shot;
	}

	// just going to move in a straight line
	void Update()
	{
		transform.Translate(Heading * Time.deltaTime * Speed);
	}

	// if the shot hits somebody other then the caster make it go away
	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.layer == Layer)
			return;

		Object.Destroy(gameObject);
	}

	// kill the shot after a bit
	private IEnumerator Timer()
	{
		yield return new WaitForSeconds(Lifetime);
		
		Object.Destroy(gameObject);
	}

	private void Setup(Vector2 heading, LayerMask layer)
	{
		Heading = heading;
		Layer = layer;

		StartCoroutine(Timer());
	}
}