using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	private MapData _map;

	private CamFollow _cam;
	private Player _player;

	void Start()
	{
		_cam = Camera.main.GetComponent<CamFollow>();

		LoadMap();
		CreateCharacter(3, new Vector2(10, -10));
	}
	
	void Update()
	{
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (_map.CanMove(_player.Position + Player.DIR_DOWN))
				_player.MoveTo(PlayerDirection.Down);
			else
				_player.Face(PlayerDirection.Down);
        }
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			if (_map.CanMove(_player.Position + Player.DIR_LEFT))
				_player.MoveTo(PlayerDirection.Left);
			else
				_player.Face(PlayerDirection.Left);
        }
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			if (_map.CanMove(_player.Position + Player.DIR_RIGHT))
				_player.MoveTo(PlayerDirection.Right);
			else
				_player.Face(PlayerDirection.Right);
        }
		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (_map.CanMove(_player.Position + Player.DIR_UP))
				_player.MoveTo(PlayerDirection.Up);
			else
				_player.Face(PlayerDirection.Up);
		}
	}

	private void LoadMap()
	{
		_map = new MapData();
		_map.LoadMap("map1");

		for (int x = 0; x < _map.Width; ++x)
		{
			for (int y = 0; y < _map.Height; ++y)
			{
				string path = "Prefabs/Tiles/" + _map.GetTileResource(x, y);
				
				GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), new Vector3(x, -y, 0), Quaternion.identity);
			}
		}

		_cam.Map = _map;
	}

	private void CreateCharacter(int index, Vector2 position)
	{
		string path = "Prefabs/Characters/char" + index.ToString();
		
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		_player = go.GetComponent<Player>();

		_cam.Target = go;
	}
}
