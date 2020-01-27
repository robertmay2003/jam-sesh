using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerJamController : MonoBehaviour
{

	public int soundCount = 5;

	private int[] _sounds;
	private SoundManager _manager;

	// Start is called before the first frame update
    void Start()
    {
	    _manager = GameObject.FindObjectOfType<SoundManager>();
	    _sounds = _manager.GetRandomSounds(soundCount).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
	    for (int i = 0; i < soundCount; i++)
	    {
		    if (Input.GetKeyDown((i + 1).ToString()))
		    {
			    _manager.PlaySound(_sounds[i], transform.position);
		    }
	    }
    }
}
