using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PS.Data;
using PS.Events;
using PS.Requests;

public class GameController : MonoBehaviour
{
	public static string NAME = "GameController";

	public LayerMask PlayerLayer;
	public LayerMask OthersLayer;

	private Server _server;
	private ChatController _chat;
	private MapData _map;

	private CamFollow _cam;
	private Player _player;
	private Dictionary<string, Player> _otherPlayers;

	private bool _running;

	void Start()
	{
		_server = Utility.FindComponent<Server>(Server.NAME);
		_server.ConnectionLostEvent += OnConnectionLost;
		_server.ExtensionEvent += OnResponse;

		_chat = Utility.FindComponent<ChatController>(ChatController.NAME);
	
		_cam = Camera.main.GetComponent<CamFollow>();

		LoadMap();

		int type = PlayerPrefs.GetInt(Constants.PLAYER_TYPE, 0);;
		int x = Random.Range(5, 20);
		int y = Random.Range(0, -10);

		_player = CreateCharacter(type, new Vector2(x, y));

		_cam.Target = _player.gameObject;

		_otherPlayers = new Dictionary<string, Player>();

		PsObject psobj = new PsObject();
		psobj.SetInt(ServerConstants.PLAYER_TYPE, type);
		psobj.SetIntArray(ServerConstants.PLAYER_POSITION, new List<int>() { x, y });

		_server.SendRequest(new ExtensionRequest(PlayerCommand.GetCommand(PlayerCommand.PlayerEnum.Start), psobj));

		_running = true;
	}
	
	void Update()
	{
		if (_running && _player.CanMove)
		{
			bool send = false;

			if (Input.GetKey(KeyCode.DownArrow))
			{
				if (_map.CanMove(_player.Position + Player.DIR_DOWN))
				{
					_player.MoveTo(PlayerDirection.Down);
					send = true;
				}
				else
					_player.Face(PlayerDirection.Down);
	        }
			else if (Input.GetKey(KeyCode.LeftArrow))
			{
				if (_map.CanMove(_player.Position + Player.DIR_LEFT))
				{
					_player.MoveTo(PlayerDirection.Left);
					send = true;
				}
				else
					_player.Face(PlayerDirection.Left);
	        }
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				if (_map.CanMove(_player.Position + Player.DIR_RIGHT))
				{
					_player.MoveTo(PlayerDirection.Right);
					send = true;
				}
				else
					_player.Face(PlayerDirection.Right);
	        }
			else if (Input.GetKey(KeyCode.UpArrow))
			{
				if (_map.CanMove(_player.Position + Player.DIR_UP))
				{
					_player.MoveTo(PlayerDirection.Up);
					send = true;
				}
				else
					_player.Face(PlayerDirection.Up);
			}

			if (send)
			{
				PsObject psobj = new PsObject();
				psobj.SetIntArray(ServerConstants.PLAYER_POSITION, Utility.Vector2ToList(_player.Target));
				
				_server.SendRequest(new ExtensionRequest(PlayerCommand.GetCommand(PlayerCommand.PlayerEnum.Move), psobj));
			}
		}

		if (Input.GetKey(KeyCode.Space) && _player.CanShoot)
		{
			_player.Shoot();

			PsObject psobj = new PsObject();
			psobj.SetIntArray(ServerConstants.PLAYER_POSITION, Utility.Vector2ToList(_player.Target));
			psobj.SetIntArray(ServerConstants.PLAYER_HEADING, Utility.Vector2ToList(_player.GetDirectionVector()));
			
			_server.SendRequest(new ExtensionRequest(PlayerCommand.GetCommand(PlayerCommand.PlayerEnum.Shoot), psobj));
		}
	}

	void OnDestroy()
	{
		_server.ConnectionLostEvent -= OnConnectionLost;
		_server.ExtensionEvent -= OnResponse;
	}

	private void OnConnectionLost(Dictionary<string, object> message)
	{
		GameObject.DestroyImmediate(_player.gameObject);
		foreach (string key in _otherPlayers.Keys)
			GameObject.DestroyImmediate(_otherPlayers[key].gameObject);

		_running = false;
	}

	private void OnResponse(Dictionary<string, object> message)
	{
		string command = (string)message["command"];
		string subCommand = (string)message["subcommand"];
		PsObject data = (PsObject)message["data"];
	
		if (command == PlayerCommand.BASE_COMMAND)
		{
			PlayerCommand.PlayerEnum action = (PlayerCommand.PlayerEnum)System.Enum.Parse(typeof(PlayerCommand.PlayerEnum), subCommand, true);

			switch (action)
			{
				case PlayerCommand.PlayerEnum.InfoPlayer:
					AddPlayers(data);
					break;

				case PlayerCommand.PlayerEnum.InfoGroup:
					AddPlayer(data, true);
					break;

				case PlayerCommand.PlayerEnum.Move:
					PlayerMove(data);
					break;

				case PlayerCommand.PlayerEnum.Shoot:
					PlayerShoot(data);
					break;

				case PlayerCommand.PlayerEnum.Leave:
					PlayerLeave(data);
					break;
			}
		}
	}

	private void AddPlayers(PsObject psobj)
	{
		List<PsObject> players = psobj.GetPsObjectArray(ServerConstants.PLAYER_OBJ);

		for (int i = 0; i < players.Count; ++i)
			AddPlayer(players[i], false);
	}

	private void AddPlayer(PsObject psobj, bool updateStatus)
	{
		string name = psobj.GetString(ServerConstants.PLAYER_NAME);
		if (_otherPlayers.ContainsKey(name))
			return;

		int type = psobj.GetInt(ServerConstants.PLAYER_TYPE);
		List<int> position = psobj.GetIntArray(ServerConstants.PLAYER_POSITION);

		Player player = CreateCharacter(type, Utility.ListToVector2(position), OthersLayer);
		player.Face(PlayerDirection.Down);
		_otherPlayers.Add(name, player);

		if (updateStatus)
			_chat.UserAction(name, ChatAction.Enter);
	}

	private void PlayerMove(PsObject psobj)
	{
		string name = psobj.GetString(ServerConstants.PLAYER_NAME);

		Player player = FindPlayer(name);
		if (player == null)
            return;

		List<int> position = psobj.GetIntArray(ServerConstants.PLAYER_POSITION);
		player.MoveTo(Utility.ListToVector2(position));
	}

	private void PlayerShoot(PsObject psobj)
	{
		string name = psobj.GetString(ServerConstants.PLAYER_NAME);
		
		Player player = FindPlayer(name);
		if (player == null)
            return;

		List<int> position = psobj.GetIntArray(ServerConstants.PLAYER_POSITION);
		List<int> heading = psobj.GetIntArray(ServerConstants.PLAYER_HEADING);
		Shot.Create(player, Utility.ListToVector2(position), Utility.ListToVector2(heading));
	}

	private void PlayerLeave(PsObject psobj)
	{
		string name = psobj.GetString(ServerConstants.PLAYER_NAME);

		Player player = FindPlayer(name);
		if (player == null)
			return;

		_chat.UserAction(name, ChatAction.Leave);

		GameObject.Destroy(player.gameObject);
	}

	private Player FindPlayer(string name)
	{
		Player player = null;
		_otherPlayers.TryGetValue(name, out player);

		return player;
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

		Player player = go.GetComponent<Player>();
		player.Face(PlayerDirection.Down);

		return player;
	}

	private Player CreateCharacter(int index, Vector2 position, LayerMask layer)
	{
		string path = PrefabUtility.GetCharactersPath("char" + index.ToString());
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.layer = (int)Mathf.Log(layer.value, 2);

		return go.GetComponent<Player>();
    }
}
