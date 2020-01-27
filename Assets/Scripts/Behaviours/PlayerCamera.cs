using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	public GameObject player;
	public float smoothTime = 0.3f;

	private Vector3 velocity = Vector3.zero;

	// Update is called once per frame
    void Update()
    {
	    if (player != null)
	    {
		    Vector3 targetPosition = transform.position;
		    targetPosition.x = player.transform.position.x;

		    // Smoothly move the camera towards that target position
		    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
	    }
    }
}
