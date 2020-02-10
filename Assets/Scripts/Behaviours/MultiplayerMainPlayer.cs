using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MultiplayerMainPlayer : MonoBehaviour
{
	private MultiplayerPlayer _multiplayerPlayer;
	private PlayerMovement _playerMovement;

	void Start()
	{
		_multiplayerPlayer = GetComponent<MultiplayerPlayer>();
		_playerMovement = GetComponent<PlayerMovement>();
	}

	void Update()
	{
		_multiplayerPlayer.UpdateTransform(transform.position, transform.rotation, transform.localScale);
		_multiplayerPlayer.UpdateFacing(_playerMovement.Facing);
	}

	public Dictionary<string, string> GetDataAsDict()
	{
		return _multiplayerPlayer.GetDataAsDict();
	}

	public string GetDataAsJSON()
	{
		var t = transform;
		
		SocketConnection.PlayerData playerData = new SocketConnection.PlayerData(
			_multiplayerPlayer.Data.name,
			_multiplayerPlayer.Data.color,
			_multiplayerPlayer.Data.socketId,
			t.position,
			t.localScale,
			t.rotation,
			_playerMovement.Facing
		);

		return JsonUtility.ToJson(playerData);
	}
}
