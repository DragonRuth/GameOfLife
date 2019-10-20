using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {



    public static GameManager instance;
	private  bool allowTouch = true;
	public Texture2D fadeText;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;
	private int currentLevelBundle ;
	private int currentLevel;
	private bool fieldMode; // true - square ; false - hex;

    private int LoadingSceneNumber = 0;

	void Awake() {
        //if we don't have an [_instance] set yet
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
			print("this code runs only once");
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
		allowTouch = true;
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

	public void setAllowTouch(){
		allowTouch = !allowTouch;
	}

	public  bool isTouchAllowed() {
		return allowTouch;
	}

	public void setFieldType ( bool t) 
	{
		fieldMode = t;
	}
	public bool getFieldType ()
	{
		return fieldMode;
	}

	public void setCurrentLevelBundle(int n) 
	{
		currentLevelBundle = n;
	}

	public int getCurrentLevelBunbleNumber() 
	{
		return currentLevelBundle;
	}

	public void setCurrenLevelNumber(int n)
	{
		currentLevel = n;
	}

	public int getCurrentLevelNumber() 
	{
		return currentLevel;
	}
}
