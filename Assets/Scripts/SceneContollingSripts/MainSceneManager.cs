using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PointSpace;
using UnityEngine.UI;
public abstract class  MainSceneManager : MonoBehaviour {

	protected Color[] genColors = new Color[10]{new Color(1.0f,1.0f,1.0f),new Color(0.3f,0.0f,0.1f),new Color(0.1f,0.0f,0.3f),new Color(0.0f,0.8f,0.0f),new Color(0.2f,0.0f,0.5f),new Color(0.7f,0.0f,0.2f),new Color(0.4f,0.0f,0.7f),new Color(0.2f,0.7f,0.3f),new Color(0.9f,0.2f,0.9f),new Color(0.6f,0.5f,0.0f),};
	private int zoom = 2;
	public Transform CellPrefab;
	public Transform HexCellPrefab;
	public Transform Parent;
	public  bool allowTouch = true;
	public Button CameraButton;
	public Sprite[] CameraButtonSprites;
	private bool CameraBtnFlag = true;
	protected MainManager mainManager;
	// Use this for initialization
	void Start () {
		StartUp ();
		//int[] positions = new int[0];
		//mainManager = new MainManager(24, 50, 1, positions, 0);

		/*int[] n = { 1, 1, 0, 0 };
		bool[] i = { false, false, false };
		string[] nam = { "-", "-", "-" };
		string[] desc = { "-", "-", "-" };
		float[][] liveint = new float[1][];
		liveint[0] = new float[2];
		liveint[0][0] = 3;
		liveint[0][1] = 3;
		float[][] deadint = new float[1][];
		deadint[0] = new float[4];
		deadint[0][0] = -100;
		deadint[0][1] = 1;
		deadint[0][2] = 4;
		deadint[0][3] = 100;
		float[] power = { 1 };
		float[] liveP = { 0 };
		int[][] propNumP = new int[1][];
		propNumP[0] = new int[1];
		propNumP[0][0] = 0;
		//propNumP[0][1] = 1;
		// propNumP[0][2] = 2;
		//propNumP[1] = new int[0];
		int[][] propNum = new int[1][];
		propNum[0] = new int[1];
		propNum[0][0] = 0;
		/*propNum[1] = new int[1];
        propNum[1][0] = 1;
        propNum[2] = new int[1];
        propNum[2][0] = 2;*/
		//float[][] coefFr = new float[1][];
		//coefFr[0] = new float[1];
		//coefFr[0][0] = 1;
		/*coefFr[1] = new float[1];
        coefFr[1][0] = 1;
        coefFr[2] = new float[1];
        coefFr[2][0] = 1;*/
		//float[][] coefEn = new float[1][];
		//coefEn[0] = new float[1];
		//coefEn[0][0] = -1;
		/*coefEn[1] = new float[1];
        coefEn[1][0] = -1;
        coefEn[2] = new float[1];
        coefEn[2][0] = -1;*/
		//int[] funcT = { 0};
		//bool[] immort = { false, false };
		//string[][] col = new string[1][];
		//col[0] = new string[2];
		//col[0][0] = "1";
		//col[0][1] = "2";
		//float[] livePD = { 0 };
		//float[] setP = { 1 };

        /*Position[] pos = new Position[4];
        pos[0] = new Position(9, 10);
        pos[1] = new Position(10, 9);
        pos[2] = new Position(11, 10);
        pos[3] = new Position(10, 11);
        int[] generations = { 0, 0, 0, 0, 0 };
        int[] teams = { 1, 1, 1, 1, 1 };
        mainManager.addPoints(pos, teams, generations);*/
        //string[] genNam = { "The first generation" };
        //string[] genDsec = { "To test the output" };
        //mainManager.initRules(n, i, nam, desc, liveint, deadint, power, liveP, livePD, setP, col, 1, genNam, genDsec, propNumP, immort, 1, propNum, coefFr, coefEn, funcT);
        //mainManager.createPlayers(1, 5, 0, 0);
		//mainManager.setPlayersParametres(2, 1, 1, 1);
  
	}




	// Update is called once per frame
	void Update () {

	}

	public virtual void StepForward()
	{
		mainManager.step();

	}

	public void setAllowTouch() {
		GameManager.instance.setAllowTouch ();
	}


	public void zoomIn(Camera camera) {
		if (zoom > 1) {
			camera.GetComponent<CameraDragging> ().cameraZoom (1);
			camera.orthographicSize = camera.orthographicSize - 100;
			camera.GetComponent<CameraDragging> ().cameraFix ();
			zoom--;
		}
	}

	public void zoomOut(Camera camera) {
		if (zoom < 6) {
			camera.GetComponent<CameraDragging> ().cameraZoom (-1);
			camera.orthographicSize = camera.orthographicSize + 100;
			camera.GetComponent<CameraDragging> ().cameraFix ();
			zoom++;
		}
	}


	public void GoHome()
	{
		GameManager.instance.changeSceen(0);
	}
	public  int  getScore() {
		return mainManager.getPointsManager ().getAlivePoints ().Count;
	}




	public void ChangeCamBtnSprite(){
		if (CameraBtnFlag)
			CameraButton.image.overrideSprite = CameraButtonSprites [1];
		else CameraButton.image.overrideSprite = CameraButtonSprites [0];
		CameraBtnFlag = !CameraBtnFlag;
	}

    public abstract void StartUp();

	public void ReloadLevel(int levelNum)
	{
		GameManager.instance.changeSceen(levelNum);
	}

	public void killAllPoints()
	{
		mainManager.killAllPoints();

	}
}

