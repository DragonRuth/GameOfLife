using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PointSpace;
public class MainSceneManager : MonoBehaviour {


	private int zoom = 2;
	public Transform CellPrefab;
	public Transform Parent;
	public  bool allowTouch = true;
	protected MainManager mainManager;
	// Use this for initialization
	void Start () {
		int[] positions = new int[0];
		mainManager = new MainManager(24, 50, 1, positions, 0);

		int[] n = { 1, 1, 0, 0 };
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
		float[][] coefFr = new float[1][];
		coefFr[0] = new float[1];
		coefFr[0][0] = 1;
		/*coefFr[1] = new float[1];
        coefFr[1][0] = 1;
        coefFr[2] = new float[1];
        coefFr[2][0] = 1;*/
		float[][] coefEn = new float[1][];
		coefEn[0] = new float[1];
		coefEn[0][0] = -1;
		/*coefEn[1] = new float[1];
        coefEn[1][0] = -1;
        coefEn[2] = new float[1];
        coefEn[2][0] = -1;*/
		int[] funcT = { 0};
		bool[] immort = { false, false };
		string[][] col = new string[1][];
		col[0] = new string[2];
		col[0][0] = "1";
		col[0][1] = "2";
		float[] livePD = { 0 };
		float[] setP = { 1 };

		/*Position[] pos = new Position[4];
        pos[0] = new Position(9, 10);
        pos[1] = new Position(10, 9);
        pos[2] = new Position(11, 10);
        pos[3] = new Position(10, 11);
        int[] generations = { 0, 0, 0, 0, 0 };
        int[] teams = { 1, 1, 1, 1, 1 };
        mainManager.addPoints(pos, teams, generations);*/

		mainManager.initRules(n, i, nam, desc, liveint, deadint, power, liveP, livePD, setP, col, 1, propNumP, immort, 1, propNum, coefFr, coefEn, funcT);
		mainManager.createPlayers(1, 5, 0, 1);
		mainManager.setPlayersParametres(2, 1, 1, 0);

		PaintGrid();
	}

	void PaintGrid()
	{
		for (int y = 0; y < 24; y++)
		{
			for (int x = 0; x < 50; x++)
			{
				float nx = (float)(-5.4 + x * 0.22);
				float ny = (float)(2.9 - y * 0.25);
				Position posit = new Position(y, x);
				Transform Cell = (Transform)Instantiate(CellPrefab, new Vector3(x, y, 0), Quaternion.identity);

				Cell.GetComponent<CellTouchSript>().setPointsManager(mainManager.getPointsManager());
				Cell.GetComponent<CellTouchSript>().setGens(mainManager.getGenerations());
				Cell.GetComponent<CellTouchSript>().setPosition(posit);

				Cell.SetParent(Parent);
				Vector3 pos = new Vector3(nx, ny, 0);

				Cell.transform.localPosition = pos;

			}
		}
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
}
