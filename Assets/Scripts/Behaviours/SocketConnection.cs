using System.Collections;
using System.Collections.Generic;
using SocketIO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SocketIOComponent))]
public class SocketConnection : MonoBehaviour
{
	public MultiplayerPlayersManager playerList;
	public TMP_InputField inputField;
	public SoundManager soundManager;

	public MultiplayerMainPlayer player;

	private SocketIOComponent _socket;
	private PlayerDeathDelegate _deathDelegate;

	public string id => _socket.sid;

	void Start()
	{
		_socket = GetComponent<SocketIOComponent>();
		_deathDelegate = GameObject.FindGameObjectWithTag("DeathBarrier").GetComponent<PlayerDeathDelegate>();
	}

	void Update()
	{
		SendPlayerData();
	}

	void SendPlayerData()
	{
		if (player != null)
		{
			_socket.Emit("update", player.GetDataAsJSON());

			/** DEBUG
			 *
			Dictionary<string, string> playerData = player.GetDataAsDict();
			foreach (KeyValuePair<string, string> kvp in playerData)
			{
				Debug.Log($"Key = {kvp.Key}, Value = {kvp.Value}");
			}
			 */
		}
	}

	void SpawnPlayer(SocketIOEvent e)
	{
		// Spawn the new player in the world
		string name = Util.String.StripQuotes(e.data["name"].ToString());
		Color color;
		ColorUtility.TryParseHtmlString(Util.String.StripQuotes(e.data["color"].ToString()), out color);

		string socketId = Util.String.StripQuotes(e.data["socket"]["id"].ToString());

		playerList.SpawnPlayer(name, color, socketId);
	}

	void DestroyPlayer(SocketIOEvent e)
	{
		// Destroy the player
		playerList.DestroyPlayer(Util.String.StripQuotes(e.data["socket"]["id"].ToString()));
	}

	void UpdatePlayers(SocketIOEvent e)
	{
		List<MultiplayerPlayersManager.PlayerData> datas = new List<MultiplayerPlayersManager.PlayerData>();

		foreach (string key in e.data["players"].keys)
		{
			JSONObject player = e.data["players"][key];

			string name = Util.String.StripQuotes(player["name"].ToString());
			Color color;
			ColorUtility.TryParseHtmlString(Util.String.StripQuotes(player["color"].ToString()), out color);
			string socketId = Util.String.StripQuotes(player["socket"]["id"].ToString());

			int facing = (int) ParseFloat(player["facing"]);
			// Debug.Log($"String: {player["facing"]}, int: {facing}");

			Vector3 position = ParseVector3(player["position"]);

			Vector3 scale = ParseVector3(player["scale"]);

			Quaternion rotation = ParseQuaternion(player["rotation"]);

			MultiplayerPlayersManager.PlayerData data = new MultiplayerPlayersManager.PlayerData(name, color, socketId)
			{
				position = position, rotation = rotation, scale = scale, facing = facing
			};

			// Debug.Log($"PlayerData facing: {data.facing}");

			datas.Add(data);
		}

		playerList.UpdatePlayers(datas);
	}

	void ProcessEvent(SocketIOEvent e)
	{
		string eventName = Util.String.StripQuotes(e.data["event"].ToString());
		Debug.Log(eventName);

		JSONObject data = e.data["data"];

		switch (eventName)
		{
			case "sound":
				int index = (int) ParseFloat(data["sound"]);
				Vector3 soundPos = ParseVector3(data["position"]);

				soundManager.PlaySound(index, soundPos);
				break;
			case "death":
				Vector3 deathPos = ParseVector3(data["position"]);

				_deathDelegate.OnDeath(deathPos);
				break;
			default:
				break;
		}
	}

	private Vector3 ParseVector3(JSONObject json)
	{
		Vector3 v = new Vector3(
			ParseFloat(json["x"]),
			ParseFloat(json["y"]),
			ParseFloat(json["z"])
		);
		return v;
	}

	private Quaternion ParseQuaternion(JSONObject json)
	{
		return new Quaternion(
			ParseFloat(json["x"]),
			ParseFloat(json["y"]),
			ParseFloat(json["z"]),
			ParseFloat(json["w"])
		);
	}

	private float ParseFloat(JSONObject json)
	{
		float f;

		string s = json.ToString();
		f = float.Parse(Util.String.StripQuotes(json.ToString()));
		return f;
	}

	public void Join()
	{
		Debug.Log("Joining");

		// Join the game
		string name = inputField.text;
		string color = "#" + ColorUtility.ToHtmlStringRGB(Color.HSVToRGB(Random.value, 1, 1));
		Dictionary<string, string> data = new Dictionary<string, string>()
		{
			{"name", name},
			{"color", color}
		};

		// Set up socket callbacks
		_socket.On("join", SpawnPlayer);
		_socket.On("event", ProcessEvent);
		_socket.On("leave", DestroyPlayer);

		_socket.On("update", UpdatePlayers);

		_socket.Emit("join", JSONObject.Create(data));
	}

	public void SendSound(int index, Vector3 position)
	{
		Dictionary<string, string> positionData = new Dictionary<string, string>()
		{
			{"x", position.x.ToString()},
			{"y", position.y.ToString()},
			{"z", position.z.ToString()}
		};

		Dictionary<string, string> eventData = new Dictionary<string, string>()
		{
			{"player", _socket.sid},
			{"sound", index.ToString()},
		};

		Dictionary<string, string> data = new Dictionary<string, string>()
		{
			{"event", "sound"}
		};

		JSONObject positionJSON = JSONObject.Create(positionData);

		JSONObject eventDataJSON = JSONObject.Create(eventData);
		eventDataJSON["position"] = positionJSON;

		JSONObject dataJSON = JSONObject.Create(data);
		dataJSON["data"] = eventDataJSON;

		_socket.Emit("event", dataJSON);
	}

	public void SendDeath(Vector3 position)
	{
		Dictionary<string, string> positionData = new Dictionary<string, string>()
		{
			{"x", position.x.ToString()},
			{"y", position.y.ToString()},
			{"z", position.z.ToString()}
		};

		Dictionary<string, string> data = new Dictionary<string, string>()
		{
			{"event", "death"}
		};

		JSONObject positionJSON = JSONObject.Create(positionData);

		JSONObject eventDataJSON = new JSONObject {["position"] = positionJSON};

		JSONObject dataJSON = JSONObject.Create(data);
		dataJSON["data"] = eventDataJSON;

		_socket.Emit("event", dataJSON);
	}
}
