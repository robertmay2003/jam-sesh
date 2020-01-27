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

	public JSONObject GetDataAsJSON()
	{
		Dictionary<string, string> dict = GetDataAsDict();
		JSONObject json = JSONObject.Create(dict);

		Vector3 position = transform.position;
		Dictionary<string, string> positionDict = new Dictionary<string, string>()
		{
			{"x", position.x.ToString()},
			{"y", position.y.ToString()},
			{"z", position.z.ToString()}
		};

		Vector3 scale = transform.localScale;
		Dictionary<string, string> scaleDict = new Dictionary<string, string>()
		{
			{"x", scale.x.ToString()},
			{"y", scale.y.ToString()},
			{"z", scale.z.ToString()}
		};

		Quaternion rotation = transform.rotation;
		Dictionary<string, string> rotationDict = new Dictionary<string, string>()
		{
			{"x", rotation.x.ToString()},
			{"y", rotation.y.ToString()},
			{"z", rotation.z.ToString()},
			{"w", rotation.w.ToString()}
		};

		json["position"] = JSONObject.Create(positionDict);
		json["scale"] = JSONObject.Create(scaleDict);
		json["rotation"] = JSONObject.Create(rotationDict);

		json["facing"] = JSONObject.CreateStringObject(_playerMovement.Facing.ToString());

		return json;
	}
}
