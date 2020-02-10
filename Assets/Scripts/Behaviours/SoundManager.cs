using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : MonoBehaviour
{
	public ParticleSystem particle;

	private class SoundRequestData
	{
		public int index;
		public Vector3 position;
	}

	public void PlaySound(int index, Vector3 position)
	{
		/*
		SoundRequestData requestData = new SoundRequestData()
		{
			index = index, position = position
		};
		
		StartCoroutine(FetchSound(requestData, PlayClip));
		*/
		
		PlaySoundParticle(position);
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
    
    private IEnumerator FetchSound(SoundRequestData data, System.Action<SoundRequestData, AudioClip> onComplete)
    {
	    string url = $"{GetBaseURL()}/instrument?i={data.index}";
	    // Debug.Log($"Getting sound from url {url}");
	    
	    using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV)) {
		    yield return uwr.SendWebRequest();
		    if (uwr.isNetworkError || uwr.isHttpError) {
			    Debug.LogError(uwr.error);
			    yield break;
		    }

		    AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
		    onComplete(data, clip);
	    }
    }

    private void PlayClip(SoundRequestData data, AudioClip clip)
    {
	    Debug.Log($"Clip length: {clip.length}");
	    AudioSource.PlayClipAtPoint(clip, data.position);

	    PlaySoundParticle(data.position);
    }

    private string GetBaseURL()
    {
	    string fullURL = Application.absoluteURL;
	    string[] splitURL = fullURL.Split('/');

	    string protocol = splitURL[0];
	    string host = splitURL[2];

	    return $"{protocol}//{host}";
    }

    private void PlaySoundParticle(Vector3 position)
    {
	    Vector3 particlePos = position;
	    particlePos.y += 1.5f;

	    ParticleSystem ps = Instantiate(particle, particlePos, Quaternion.identity);
	    Destroy(ps.gameObject, ps.main.duration);
    }
}
