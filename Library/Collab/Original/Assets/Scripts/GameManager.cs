using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

	public Texture2D fadeText;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

    private int LoadingSceneNumber = 0;

	void Awake() {
        //if we don't have an [_instance] set yet
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);
    }

    void OnGUI() {
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeText);
	}

	public float BeginFade(int dir) {
		fadeDir = dir;
		return (fadeSpeed);
	}

	void OnLevelWasLoaded() {
		BeginFade (-1);
	}

	public IEnumerator fadingCoroutine() {
		float fadeTime = BeginFade (1);
		yield return new WaitForSeconds(fadeTime);
		UnityEngine.SceneManagement.SceneManager.LoadScene (LoadingSceneNumber);
	}


	public void changeSceen(int levelNum) {
        LoadingSceneNumber = levelNum;
		StartCoroutine ("fadingCoroutine");
	}


}
