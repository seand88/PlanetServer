using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PS.Data;
using PS.Events;
using PS.Requests;

public class GameController : MonoBehaviour
{
	public LayerMask PlayerLayer;
	public LayerMask OthersLayer;

	private Server _server;

	private MapData _map;

	private CamFollow _cam;
	private Player _player;

	void Start()
	{
		_server = Utility.GetServer();
		_server.ExtensionEvent += OnResponse;

		_cam = Camera.main.GetComponent<CamFollow>();

		LoadMap();

		int type = Random.Range(0, 3);
		int x = Random.Range(5, 20);
		int y = -10;

		_player = CreateCharacter(type, new Vector2(x, y));

		_cam.Target = _player.gameObject;

		PsArray position = new PsArray();
		position.AddInt(x);
		position.AddInt(y);

		PsObject psobj = new PsObject();
		psobj.SetInt(ServerConstants.PLAYER_TYPE, type);
		psobj.SetPsArray(ServerConstants.PLAYER_POSITION, position);

		_server.SendRequest(new ExtensionRequest("player.start", psobj));
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
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			if (_map.CanMove(_player.Position + Player.DIR_UP))
				_player.MoveTo(PlayerDirection.Up);
			else
				_player.Face(PlayerDirection.Up);
		}

		if (Input.GetKey(KeyCode.Space))
			_player.Shoot();
	}

	void OnDestroy()
	{
		_server.ExtensionEvent -= OnResponse;
	}

	private void OnResponse(Dictionary<string, object> message)
	{
		string subCommand = (string)message["subcommand"];
		PsObject data = (PsObject)message["data"];
	}

	private void LoadMap()
	{
		_map = new MapData();
		_map.LoadMap("map1");

		for (int x = 0; x < _map.Width; ++x)
		{
			for (int y = 0; y < _map.Height; ++y)
			{
				string path = PrefabUtility.GetTilePath(_map.GetTileResource(x, y));
				
				Object.Instantiate(Resources.Load<GameObject>(path), new Vector3(x, -y, 0), Quaternion.identity);
			}
		}

		_cam.Map = _map;
	}

	private Player CreateCharacter(int index, Vector2 position)
	{
		string path = PrefabUtility.GetCharactersPath("char" + index.ToString());
		
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.layer = (int)Mathf.Log(PlayerLayer.value, 2);

		return go.GetComponent<Player>();
	}

	private Player CreateCharacter(int index, Vector2 position, LayerMask layer)
	{
		string path = PrefabUtility.GetCharactersPath("char" + index.ToString());
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.layer = (int)Mathf.Log(layer.value, 2);

		return go.GetComponent<Player>();
    }
}
