using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using JetBrains.Annotations;
using UnityEngine;

public class MultiplayerPlayersManager : MonoBehaviour
{
	private List<MultiplayerPlayer> _players = new List<MultiplayerPlayer>();

	private PlayerSpawner _playerSpawner;
	private SocketConnection _socketConnection;
	private PlayerDeathDelegate _deathDelegate;

	// Start is called before the first frame update
    void Start()
    {
	    _playerSpawner = GameObject.FindObjectOfType<PlayerSpawner>();
	    _socketConnection = GetComponent<SocketConnection>();
	    _deathDelegate = GameObject.FindGameObjectWithTag("DeathBarrier").GetComponent<PlayerDeathDelegate>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPlayer(SocketConnection.PlayerData newPlayer)
    {
	    // Debug.Log($"Socket id: {socketId}; My id: {_socketConnection.id}");

	    if (newPlayer.socketId == _socketConnection.id)
		{
			GameObject playerGameObject = _playerSpawner.SpawnMainPlayer(newPlayer);

			MultiplayerPlayer player = playerGameObject.GetComponent<MultiplayerPlayer>();
			_players.Add(player);

			Camera camera = Camera.main;
			camera.GetComponent<PlayerCamera>().player = playerGameObject;

			_socketConnection.Player = player.GetComponent<MultiplayerMainPlayer>();
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
	    int index = _players
		    .Select((p, i) => new {Player=p, Index=i})
		    .Where((p) => p.Player.Data.socketId == socketId).ToList()[0].Index;

	    MultiplayerPlayer player = _players[index];
	    Debug.Log($"Player {player.name} ({socketId}) disconnected");
	    
	    _deathDelegate.OnDeath(player.transform.position);
	    Destroy(player.gameObject);
	    
	    _players.RemoveAt(index);
    }

    [CanBeNull]
    public MultiplayerPlayer GetPlayerById(string id)
    {
	    foreach (MultiplayerPlayer player in _players)
	    {
		    if (player.Data.socketId == id) return player;
	    }

	    return null;
    }

    public void UpdatePlayers(List<SocketConnection.PlayerData> playerDatas)
    {
	    foreach (SocketConnection.PlayerData data in playerDatas)
	    {
		    if (data.socketId != _socketConnection.id)
		    {
			    MultiplayerPlayer player = GetPlayerById(data.socketId);

			    if (player != null)
			    {
				    player.Data = data;
			    }
			    else
			    {
				    SpawnPlayer(data);
			    }
		    }
	    }
    }
}
