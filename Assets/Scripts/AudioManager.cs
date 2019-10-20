using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	// Use this for initialization
	public  AudioSource Music;
	public static bool music;

	void Awake() {
		//if we don't have an [_instance] set yet

		if (!instance) {
			instance = this;
			DontDestroyOnLoad (this);
			DontDestroyOnLoad (Music);
			setMusic (true);
			StartMusic (music);
		
		}
		//otherwise, if we do, kill this thing
		else {
			Destroy (this.gameObject);
			Destroy (Music);
		}

	}
	public void StartMusic(bool m) {
		if (m) {
			Music.Play ();
			setMusic (true);
		}
		else {
			Music.Stop();
			setMusic (false);
		}
	}


	public static void setMusic(bool m){
		music = m;
	}
	public static bool getMusic(){
		return music;
		}
}
