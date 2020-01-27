using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerJamController : MonoBehaviour
{
	public int soundCount = 5;

	private int[] _sounds;

	private SoundManager _manager;
	private SocketConnection _socket;

	// Start is called before the first frame update
	void Start()
	{
		_manager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
		_socket = GameObject.FindWithTag("SocketConnection").GetComponent<SocketConnection>();
		_sounds = _manager.GetRandomSounds(soundCount).ToArray();
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < soundCount; i++)
		{
			if (Input.GetKeyDown((i + 1).ToString()))
			{
				PlaySound(_sounds[i]);
			}
		}
	}

	void PlaySound(int index)
	{
		_socket.SendSound(index, transform.position);
	}
}
