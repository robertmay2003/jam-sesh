using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public ParticleSystem particle;

	public void PlaySound(int index, Vector3 position)
    {
	    string filename = (index < 26) ? $"Drum_{index}" : $"Inst_{index - 26}";
	    Debug.Log(filename);
	    AudioClip sound = Resources.Load<AudioClip>($"Instruments/{filename}");
	    Debug.Log(sound);
	    AudioSource.PlayClipAtPoint(sound, position);

	    Vector3 particlePos = position;
	    particlePos.y += 1.5f;

	    ParticleSystem ps = Instantiate(particle, particlePos, Quaternion.identity);
	    Destroy(ps.gameObject, ps.main.duration);
    }

    public List<int> GetRandomSounds(int count)
    {
	    List<int> sounds = new List<int>();

	    while (sounds.Count < count)
	    {
		    int index = Random.Range(1, 176);
		    if (!sounds.Contains(index)) sounds.Add(index);
	    }

	    return sounds;
    }

}
