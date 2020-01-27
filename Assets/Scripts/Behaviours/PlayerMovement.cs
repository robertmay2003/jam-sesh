using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float jumpStrength;
	public float moveSpeed;
	public float acceleration;

	public bool canMove;

	public int Facing => _facing;

	private Rigidbody2D _rb;

	private float _xVelocity;
	private int _facing = 1;

	// Start is called before the first frame update
    void Start()
    {
	    _rb = GetComponent<Rigidbody2D>();

	    canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
	    if (!canMove) { return; }

	    // Jump
	    if (Input.GetAxis("Jump") > 0 && IsGrounded())
	    {
		    _rb.AddForce(Vector2.up * jumpStrength);
	    }

	    // Move
	    float input = Mathf.Round(Input.GetAxis("Horizontal")) * moveSpeed;
	    _xVelocity += (input - _xVelocity) * Time.deltaTime / acceleration;
	    _xVelocity = Mathf.Clamp(_xVelocity, -moveSpeed, moveSpeed);

	    if (Mathf.Abs(input) > 0.1f) _facing = (int) Mathf.Sign(input);

	    Vector2 velocity = _rb.velocity;
	    velocity.x = _xVelocity;
	    _rb.velocity = velocity;

	    // Stand up
	    if (Input.GetKeyDown("f") && IsGrounded(1f))
	    {
			transform.rotation = Quaternion.identity;
	    }
    }

    private bool IsGrounded(float margin = 0.05f)
    {
	    LayerMask mask =~ LayerMask.GetMask("Player");
	    return Physics2D.Raycast(transform.position, Vector3.down,margin, mask);
    }
}
