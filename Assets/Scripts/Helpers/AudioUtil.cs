using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
	public static class Audio
	{
		public static AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, float volume = 1f, float pitch = 1f){
			GameObject tempGameObject = new GameObject("TempAudio"); // create the temp object
			tempGameObject.transform.position = pos; // set its position
			AudioSource aSource = tempGameObject.AddComponent<AudioSource>(); // add an audio source
			aSource.clip = clip; // define the clip
			// set other aSource properties here, if desired
			aSource.volume = volume;
			aSource.Play(); // start the sound
			Object.Destroy(tempGameObject, clip.length / pitch); // destroy object after clip duration
			return aSource; // return the AudioSource reference
		}
	}
}
