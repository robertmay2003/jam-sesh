using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using JetBrains.Annotations;
using SocketIO;
using UnityEngine;

public class MultiplayerPlayersManager : MonoBehaviour
{
	[System.Serializable]
	public class PlayerData
	{
		public string id;

		public string name;
		public Color color;

		public int facing;

		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;

		public PlayerData(string playerName, Color playerColor, string socketId)
		{
			name = playerName;
			id = socketId;
			color = playerColor;

			position = Vector3.zero;
			rotation = Quaternion.identity;
			scale = Vector3.one;
		}
	}

	private List<MultiplayerPlayer> _players = new List<MultiplayerPlayer>();

	private PlayerSpawner _playerSpawner;
	private SocketConnection _socketConnection;

    // Start is called before the first frame update
    void Start()
    {
	    _playerSpawner = GameObject.FindObjectOfType<PlayerSpawner>();
	    _socketConnection = GetComponent<SocketConnection>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPlayer(string name, Color color, string socketId)
    {
	    PlayerData newPlayer = new PlayerData(name, color, socketId);

	    Debug.Log($"Socket id: {socketId}; My id: {_socketConnection.id}");

	    if (socketId == _socketConnection.id)
		{
			GameObject playerGameObject = _playerSpawner.SpawnMainPlayer(newPlayer);

			MultiplayerPlayer player = playerGameObject.GetComponent<MultiplayerPlayer>();
			_players.Add(player);

			Camera camera = Camera.main;
			camera.GetComponent<PlayerCamera>().player = playerGameObject;

			_socketConnection.player = player.GetComponent<MultiplayerMainPlayer>();
		}
	    else
	    {
		    GameObject playerGameObject = _playerSpawner.SpawnPlayer(newPlayer);

		    MultiplayerPlayer player = playerGameObject.GetComponent<MultiplayerPlayer>();
		    _players.Add(player);
	    }
    }

    public void DestroyPlayer(string socketId)
    {
	    foreach (MultiplayerPlayer player in _players)
	    {
		    if (player.Data.id == socketId)
		    {
			    Destroy(player.gameObject);
			    _players.Remove(player);
		    }
	    }
    }

    [CanBeNull]
    public MultiplayerPlayer GetPlayerById(string id)
    {
	    foreach (MultiplayerPlayer player in _players)
	    {
		    if (player.Data.id == id) return player;
	    }

	    return null;
    }

    public void UpdatePlayers(List<PlayerData> playerDatas)
    {
	    foreach (PlayerData data in playerDatas)
	    {
		    if (data.id != _socketConnection.id)
		    {
			    MultiplayerPlayer player = GetPlayerById(data.id);

			    if (player != null)
			    {
				    player.Data = data;
			    }
			    else
			    {
				    SpawnPlayer(data.name, data.color, data.id);
			    }
		    }
	    }
    }
}
