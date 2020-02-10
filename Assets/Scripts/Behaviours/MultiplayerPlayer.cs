using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using UnityEngine;

public class MultiplayerPlayer : MonoBehaviour
{
	private SocketConnection.PlayerData _data;

	public SocketConnection.PlayerData Data
	{
		get => _data;
		set
		{
			_data = value;
			UpdateTransform();
		}
	}

	// Update the player based on the data (others)
	private void UpdateTransform()
    {
	    transform.position = _data.position;
	    transform.rotation = _data.rotation;
	    transform.localScale = _data.scale;
    }

    public Dictionary<string, string> GetDataAsDict()
    {
	    Dictionary<string, string> stringDict = new Dictionary<string, string>();
	    foreach (FieldInfo key in _data.GetType().GetFields())
	    {
		    stringDict[key.Name] = key.GetValue(_data).ToString();
	    }

	    return stringDict;
    }

    // Update the data based on player position (main)
    public void UpdateTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
	    _data.position = position;
	    _data.rotation = rotation;
	    _data.scale = scale;
    }

    // Update the direction the player is facing (main)
    public void UpdateFacing(int facing)
    {
	    _data.facing = facing;
    }
}
