using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private GameObject _mainPlayerPrefab;

	public GameObject SpawnPlayer(SocketConnection.PlayerData playerData)
	{
		GameObject player = Instantiate(
			_playerPrefab,
			transform.position,
			Quaternion.identity
		);

		player.GetComponent<PlayerColorController>().color = playerData.GetColor();
		player.GetComponent<PlayerNameController>().Name = playerData.name;
		player.GetComponent<MultiplayerPlayer>().Data = playerData;

		return player;
	}

	public GameObject SpawnMainPlayer(SocketConnection.PlayerData playerData)
	{
		GameObject player = Instantiate(
			_mainPlayerPrefab,
			transform.position,
			Quaternion.identity
		);

		player.GetComponent<PlayerColorController>().color = playerData.GetColor();
		player.GetComponent<MultiplayerPlayer>().Data = playerData;

		return player;
	}

}
