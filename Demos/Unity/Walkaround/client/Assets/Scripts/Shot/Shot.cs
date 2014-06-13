using UnityEngine;
using System.Collections;

public class Shot : MonoBehaviour
{
	public float Speed;
	public float Lifetime;

	public Vector2 Heading { get; private set; }
	public LayerMask Layer { get; private set; }

	public static Shot Create(Player player, Vector2 heading)
	{
		string path = PrefabUtility.GetShotsPath("Fireball");
	
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), player.Position, Quaternion.identity);
		go.name = "Shot (Clone)";
		Shot shot = go.GetComponent<Shot>();
		shot.Setup(heading, player.gameObject.layer);

		return shot;
	}

	public static Shot Create(Player player, Vector2 position, Vector2 heading)
	{
		string path = PrefabUtility.GetShotsPath("Fireball");
		
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.name = "Shot (Clone)";
		Shot shot = go.GetComponent<Shot>();
		shot.Setup(heading, player.gameObject.layer);
		
		return shot;
	}

	void Update()
	{
		transform.Translate(Heading * Time.deltaTime * Speed);
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.gameObject.layer == Layer)
			return;

		Object.Destroy(gameObject);
	}

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