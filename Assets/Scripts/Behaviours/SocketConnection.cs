using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SocketIOUnity))]
public class SocketConnection : MonoBehaviour
{
	#region Inspector Variables

	public MultiplayerPlayersManager playerList;
	public TMP_InputField inputField;
	public SoundManager soundManager;

	#endregion

	#region Public Properties

	public MultiplayerMainPlayer Player
	{
		get => _player;
		set => _player = value;
	}

	#endregion

	#region Private Variables

	private MultiplayerMainPlayer _player;
	private SocketIOUnity _socket;
	private PlayerDeathDelegate _deathDelegate;

	#endregion

	#region Event Data Types

	public class CustomEventData
	{
		public string eventName;
		public string eventData;

		public CustomEventData(string name, string data)
		{
			eventName = name;
			eventData = data;
		}
	}

	public class SoundEventData
	{
		public float sound;
		public Vector3 position;

		public SoundEventData(float soundName, Vector3 pos)
		{
			sound = soundName;
			position = pos;
		}
	}

	public class DeathEventData
	{
		public Vector3 position;

		public DeathEventData(Vector3 pos)
		{
			position = pos;
		}
	}

	public class JoinEventData
	{
		public string name;
		public string color;
		public string socketId;

		public JoinEventData(string playerName, string playerColor, string id)
		{
			name = playerName;
			color = playerColor;
			socketId = id;
		}
	}

	public class LeaveEventData
	{
		public string socketId;

		public LeaveEventData(string id)
		{
			socketId = id;
		}
	}

	[System.Serializable]
	public class PlayerData
	{
		public string name;
		public string color;
		public string socketId;

		public float facing;

		public Vector3 position;
		public Vector3 scale;

		public Quaternion rotation;

		public PlayerData
		(
			string playerName,
			string playerColor,
			string id,

			Vector3 playerPosition,
			Vector3 playerScale,

			Quaternion playerRotation,

			float facingDirection = 0f
		)
		{
			name = playerName;
			color = playerColor;
			socketId = id;

			position = playerPosition;
			scale = playerScale;

			rotation = playerRotation;

			facing = facingDirection;
		}

		public PlayerData(string playerName, string playerColor, string id)
		{
			name = playerName;
			color = playerColor;
			socketId = id;

			position = Vector3.zero;
			scale = Vector3.one;

			rotation = Quaternion.identity;

			facing = 0f;
		}

		public Color GetColor()
		{
			ColorUtility.TryParseHtmlString(color, out Color returnVal);
			return returnVal;
		}
	}

	[System.Serializable]
	public class PlayerListEntry
	{
		public string id;
		public PlayerData playerData;
	}

	public class UpdateEventData
	{
		public List<PlayerListEntry> players = new List<PlayerListEntry>();
	}

	#endregion

	public string id => _socket.id;

	void Start()
	{
		_socket = GetComponent<SocketIOUnity>();
		_deathDelegate = GameObject.FindGameObjectWithTag("DeathBarrier").GetComponent<PlayerDeathDelegate>();
	}

	void Update()
	{
		SendPlayerData();
	}

	void SendPlayerData()
	{
		if (Player != null)
		{
			_socket.Emit("update", Player.GetDataAsJSON());
		}
	}

	void SpawnPlayer(string dataString)
	{
		// Debug.Log("Spawn Mark 1");
		JoinEventData data = JsonUtility.FromJson<JoinEventData>(dataString);
		// Debug.Log("Spawn Mark 2");
		// Debug.Log($"Spawn Data: {data}");

		// Spawn the new player in the world
		string name = data.name;
		string color = data.color;
		string socketId = data.socketId;

		playerList.SpawnPlayer(new PlayerData(name, color, socketId));
	}

	void DestroyPlayer(string dataString)
	{
		LeaveEventData data = JsonUtility.FromJson<LeaveEventData>(dataString);

		// Destroy the player
		playerList.DestroyPlayer(data.socketId);
	}

	void UpdatePlayers(string dataString)
	{
		// Debug.Log($"Update: {dataString}");
		UpdateEventData data = JsonUtility.FromJson<UpdateEventData>(dataString);
		// Debug.Log($"Players: {data.players.Count}");

		playerList.UpdatePlayers(data.players.Select(e => e.playerData).ToList());
	}

	void ProcessEvent(string dataString)
	{
		CustomEventData data = JsonUtility.FromJson<CustomEventData>(dataString);
		// Debug.Log($"Event: {data.eventName}, Data: {data.eventData}\n ({dataString})");

		switch (data.eventName)
		{
			case "sound":
				SoundEventData soundData = JsonUtility.FromJson<SoundEventData>(data.eventData);
				int index = (int) soundData.sound;
				Vector3 soundPos = soundData.position;

				soundManager.PlaySound(index, soundPos);
				break;
			case "death":
				DeathEventData deathData = JsonUtility.FromJson<DeathEventData>(data.eventData);
				Vector3 deathPos = deathData.position;

				_deathDelegate.OnDeath(deathPos);
				break;
			default:
				break;
		}
	}

	public void Join()
	{
		// Debug.Log("Joining");

		// Join the game
		string name = inputField.text;
		string color = "#" + ColorUtility.ToHtmlStringRGB(Color.HSVToRGB(Random.value, 1, 1));
		JoinEventData data = new JoinEventData(name, color, _socket.id);

		// Debug.Log("Join Mark 1");

		// Set up socket callbacks
		_socket.On("join", SpawnPlayer);
		_socket.On("event", ProcessEvent);
		_socket.On("leave", DestroyPlayer);

		_socket.On("update", UpdatePlayers);

		_socket.Emit("join", JsonUtility.ToJson(data));
	}

	public void SendSound(int index, Vector3 position)
	{
		SoundEventData soundData = new SoundEventData(index, position);
		CustomEventData data = new CustomEventData("sound", JsonUtility.ToJson(soundData));

		string dataJSON = JsonUtility.ToJson(data);

		// Tell server to play sound
		_socket.Emit("serverEvent", dataJSON);

		// Tell clients to play particle
		_socket.Emit("event", dataJSON);
	}

	public void SendDeath(Vector3 position)
	{
		DeathEventData deathData = new DeathEventData(position);
		CustomEventData data = new CustomEventData("death", JsonUtility.ToJson(deathData));

		string dataJSON = JsonUtility.ToJson(data);

		_socket.Emit("event", dataJSON);
	}
}
