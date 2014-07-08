using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PS.Data;
using PS.Events;
using PS.Requests;

/// <summary>
/// Handles main game functionality.
/// </summary>
public class GameController : MonoBehaviour
{
	// used to find the game controller in the sea of gameobjects
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
		// find the server so that we can interact with it
		_server = Utility.FindComponent<Server>(Server.NAME);
		_server.ConnectionLostEvent += OnConnectionLost;
		_server.ExtensionEvent += OnResponse;

		_chat = Utility.FindComponent<ChatController>(ChatController.NAME);
	
		_cam = Camera.main.GetComponent<CamFollow>();

		LoadMap();

		// pull player image from prefs and randomly place them on the top of the map
		int type = PlayerPrefs.GetInt(Constants.PLAYER_TYPE, 0);;
		int x = Random.Range(5, 20);
		int y = Random.Range(0, -10);

		_player = CreateCharacter(type, new Vector2(x, y));

		_cam.Target = _player.gameObject;

		_otherPlayers = new Dictionary<string, Player>();

		// let the server (and other players) this player has just joined the game
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

			// check if the player can move in the direction requested.  if not make them face that direction
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

			// let the server know this player is moving to a new map cell
			if (send)
			{
				PsObject psobj = new PsObject();
				psobj.SetIntArray(ServerConstants.PLAYER_POSITION, Utility.Vector2ToList(_player.Target));
				
				_server.SendRequest(new ExtensionRequest(PlayerCommand.GetCommand(PlayerCommand.PlayerEnum.Move), psobj));
			}
		}

		// shoot a fireball
		if (Input.GetKey(KeyCode.Space) && _player.CanShoot)
		{
			_player.Shoot();

			// let the server know this player shot something
			PsObject psobj = new PsObject();
			psobj.SetIntArray(ServerConstants.PLAYER_POSITION, Utility.Vector2ToList(_player.Target));
			psobj.SetIntArray(ServerConstants.PLAYER_HEADING, Utility.Vector2ToList(_player.GetDirectionVector()));
			
			_server.SendRequest(new ExtensionRequest(PlayerCommand.GetCommand(PlayerCommand.PlayerEnum.Shoot), psobj));
		}
	}

	void OnDestroy()
	{
		// remove the listeners so the controller can get garbage collected away
		_server.ConnectionLostEvent -= OnConnectionLost;
		_server.ExtensionEvent -= OnResponse;
	}

	private void OnConnectionLost(Dictionary<string, object> message)
	{
		// clear all players from screen
		GameObject.DestroyImmediate(_player.gameObject);
		foreach (string key in _otherPlayers.Keys)
			GameObject.DestroyImmediate(_otherPlayers[key].gameObject);

		_running = false;
	}

	// got some kind of message from the server
	private void OnResponse(Dictionary<string, object> message)
	{
		string command = (string)message["command"];
		string subCommand = (string)message["subcommand"];
		PsObject data = (PsObject)message["data"];
	
		// if the message is a player command
		if (command == PlayerCommand.BASE_COMMAND)
		{
			// get the subcommand 
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

	// a group of players to add.  generally when a player first logs in and gets all the currently connected players
	private void AddPlayers(PsObject psobj)
	{
		List<PsObject> players = psobj.GetPsObjectArray(ServerConstants.PLAYER_OBJ);

		for (int i = 0; i < players.Count; ++i)
			AddPlayer(players[i], false);
	}

	// add a single player.  generally when somebody new joins after the game has started
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

		// update chat a player entered
		if (updateStatus)
			_chat.UserAction(name, ChatAction.Enter);
	}

	// another player moved
	private void PlayerMove(PsObject psobj)
	{
		string name = psobj.GetString(ServerConstants.PLAYER_NAME);

		Player player = FindPlayer(name);
		if (player == null)
            return;

		List<int> position = psobj.GetIntArray(ServerConstants.PLAYER_POSITION);
		player.MoveTo(Utility.ListToVector2(position));
	}

	// another player shot
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

	// player left the game
	private void PlayerLeave(PsObject psobj)
	{
		string name = psobj.GetString(ServerConstants.PLAYER_NAME);

		Player player = FindPlayer(name);
		if (player == null)
			return;

		// update chat a player left
		_chat.UserAction(name, ChatAction.Leave);

		GameObject.Destroy(player.gameObject);
	}

	private Player FindPlayer(string name)
	{
		Player player = null;
		_otherPlayers.TryGetValue(name, out player);

		return player;
	}

	// load a map and create all the tiles.  creating a gameobject for each tile which is not very efficient
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

	// create you
	private Player CreateCharacter(int index, Vector2 position)
	{
		string path = PrefabUtility.GetCharactersPath("char" + index.ToString());
		
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.layer = (int)Mathf.Log(PlayerLayer.value, 2);

		Player player = go.GetComponent<Player>();
		player.Face(PlayerDirection.Down);

		return player;
	}

	// create another player
	private Player CreateCharacter(int index, Vector2 position, LayerMask layer)
	{
		string path = PrefabUtility.GetCharactersPath("char" + index.ToString());
		GameObject go = (GameObject)Object.Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.layer = (int)Mathf.Log(layer.value, 2);

		return go.GetComponent<Player>();
    }
}
