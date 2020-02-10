using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrientationController : MonoBehaviour
{
	public SpriteRenderer sprite;

	private Vector3 _baseScale;

	private Rigidbody2D _rb;
	private MultiplayerPlayer _multiplayerPlayer;

	// Start is called before the first frame update
    void Start()
    {
	    _baseScale = sprite.transform.localScale;

	    _rb = GetComponent<Rigidbody2D>();
	    _multiplayerPlayer = GetComponent<MultiplayerPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
	    // Debug.Log($"Facing: {_multiplayerPlayer.Data.facing}");
	    sprite.transform.localScale = Vector3.Scale(_baseScale, new Vector3(_multiplayerPlayer.Data.facing, 1, 1));
    }
}
