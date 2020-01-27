using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathController : MonoBehaviour
{

	private PlayerMovement _movement;
	private Rigidbody2D _rb;

	// Start is called before the first frame update
    void Start()
    {
	    _movement = GetComponent<PlayerMovement>();
	    _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.gameObject.CompareTag("DeathBarrier"))
	    {
		    // Communicate death to others
		    other.gameObject.GetComponent<PlayerDeathDelegate>().SendDeath(transform.position);


			FreezePlayer();

			Invoke(nameof(RespawnPlayer), 2f);
	    }
    }

    private void FreezePlayer()
    {
	    _movement.canMove = false;

	    _rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void RespawnPlayer()
    {
	    _movement.canMove = true;

	    _rb.velocity = Vector2.zero;
	    _rb.angularVelocity = 0f;
	    _rb.constraints = RigidbodyConstraints2D.None;

	    transform.position = Vector3.zero;
	    transform.rotation = Quaternion.identity;
	    transform.localScale = transform.localScale * 0.95f;
    }
}
