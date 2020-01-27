using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathDelegate : MonoBehaviour
{
	public SocketConnection socket;
	public ParticleSystem particle;
	public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendDeath(Vector3 position)
    {
		socket.SendDeath(position);
    }

    public void OnDeath(Vector3 position)
    {
	    Vector3 particlePos = position;
	    particlePos.y += 1.5f;

	    ParticleSystem ps = Instantiate(particle, particlePos, particle.transform.rotation);
	    Destroy(ps.gameObject, ps.main.duration);

	    AudioSource.PlayClipAtPoint(deathSound, position);
    }
}
