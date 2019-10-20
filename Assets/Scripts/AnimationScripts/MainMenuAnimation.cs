using UnityEngine;
using System.Collections;

public class MainMenuAnimation : MonoBehaviour {

	public Texture[] textures;
	public Renderer rend;
	public float changeInterval = 0.5F;
	void Start () {
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (textures.Length == 0)
			return;

		int index = Mathf.FloorToInt(Time.time / changeInterval);
		index = index % textures.Length;
		rend.material.mainTexture = textures[index];
	}
}
