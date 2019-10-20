using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PointSpace;
using UnityEngine.UI;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.GameClasses.PropertiesSpace;

public class ComputerSceneManager : MainSceneManager {
    public Text score_player;
    public Text score_enemy;
	public Text resources;
	public Image setupScreen;
	public Animator VictoryScreen;
	public Text GameResult;

	public string PlayerVictory ="You WON!";
	public string ComputerVictory ="You Lose";
	public string Tie = "A tie";
	private int scoreToWin;
	private int enemy;
	private int player;

	const string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";

	private int m_OpenParameterId;


	public override void StepForward() {
		if (player >= scoreToWin || enemy >= scoreToWin)
		{ 
			if (player >= scoreToWin && enemy >= scoreToWin) {
				if (player > enemy)
					Victory (PlayerVictory);
				else if (enemy > player)
					Victory (ComputerVictory);
				else
					Victory(Tie);
			} else if (enemy >= scoreToWin)
				Victory (ComputerVictory);
			else if (player >= scoreToWin)
				Victory (PlayerVictory);
		}
		base.StepForward ();
		enemy = mainManager.getPlayersManager ().getScore (2);
		player = mainManager.getPlayersManager ().getScore (1);
        score_player.text = player.ToString();
		score_enemy.text = enemy.ToString();


    }
	

	public override void StartUp(){
		int[] positions = new int[0];
		mainManager = new MainManager(24, 50, 1, positions, 0);

		int[] n = { 3, 1, 1, 1 };
		bool[] i = { true, false, false };
		string[] nam = { "Classic", "Dynam", "Collection" };
		string[] desc = { "1", "-", "-" };
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
		int[][] propNum = new int[3][];
		propNum[0] = new int[1];
		propNum[0][0] = 0;
		propNum[1] = new int[0];
		// propNum[1][0] = 1;
		propNum[2] = new int[1];
		propNum[2][0] = 2;
		float[][] coefFr = new float[3][];
		coefFr[0] = new float[1];
		coefFr[0][0] = 1;
		coefFr[1] = new float[1];
		coefFr[1][0] = 1;
		coefFr[2] = new float[1];
		coefFr[2][0] = 1;
		float[][] coefEn = new float[3][];
		coefEn[0] = new float[1];
		coefEn[0][0] = -1;
		coefEn[1] = new float[1];
		coefEn[1][0] = -1;
		coefEn[2] = new float[1];
		coefEn[2][0] = -1;
		int[] funcT = { 0, 1, 2 };
		bool[] immort = { false, false };
		string[][] col = new string[1][];
		col[0] = new string[2];
		col[0][0] = "1";
		col[0][1] = "2";
		float[] livePD = { 0 };
		float[] setP = { 1 };
		string[] genNam = { "The first generation" };
		string[] genDsec = { "To test the output" };
		mainManager.initRules(n, i, nam, desc, liveint, deadint, power, liveP, livePD, setP, col, 1, genNam, genDsec, propNumP, immort, 3, propNum, coefFr, coefEn, funcT);
		mainManager.createPlayers (1, 5, 0, 1);
		mainManager.setPlayersParametres (1, 1, 2, 1, 5);

		setupScreen.GetComponent<popUpMenu> ().Open ();
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);

		PaintGrid();
	}

	public void setToWinScore(int score){
		scoreToWin = score;
	}
	public void Victory(string result) {
		
		GameResult.text = result;
		VictoryScreen.gameObject.SetActive(true);
		VictoryScreen.SetBool(m_OpenParameterId, true);
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

				//Cell.GetComponent<CellSanbox>().setPointsManager(mainManager.getPointsManager());
				//Cell.GetComponent<CellSanbox>().setGens(mainManager.getGenerations());
				Cell.GetComponent<CellPVC>().setPosition(posit);
				Cell.GetComponent<CellPVC> ().setMainManager (mainManager);
				//Cell.GetComponent<CellSanbox>().setPlayersManager(mainManager.getPlayersManager());



				Cell.SetParent(Parent);
				Vector3 pos = new Vector3(nx, ny, 0);

				Cell.transform.localPosition = pos;

			}
		}
	}
	void Update(){
	resources.text = mainManager.getResourcrs (1).ToString();
	}

}