using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PointSpace;
using UnityEngine.UI;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.GameClasses.PropertiesSpace;
using System.IO;

public class LevelsSceneController : MainSceneManager {

	public LevelBundle Levels;
	public Text GameResult;
	protected Color[] genLevelColors = new Color[5]{new Color(1.0f,1.0f,1.0f),new Color(0.0f,0.3f,0.48f),new Color(0.0f,0.0f,0.0f),new Color(0.0f,0.6f,0.03f),new Color(0.475f,0.001f,0.01f)};
	protected List<Generation> Generations; // все поколения, что есть.
	protected int currentlyActiveGenerationNum = 0;

	public Text LevelName;
	public Text LevelDis;

	private Level currentLevel;
	private List<Level> CreatedLevels;
	public Rules rules;

	private NumericalGoal[] goals;

	public Text tipField;
	public Text score_goal;
	public Text score_player;
	public Text alive_goal;
	public Text alive_player;
	public Button genButton;
	public Image ScoreBtn;
	public Image AliveBtn;
	public Button NextLevl;
	public Image TimerBtn;
	public Text resources;
	public Text timertext;
	public byte res;
	private bool Time;
	private int Timer;
	private IEnumerator coroutine;

	protected int[] points;
	public Animator VictoryScreen;
	public Animator IntroductionScreen;
	protected const string k_OpenTransitionName = "Open";
	protected const string k_ClosedStateName = "Closed";

	protected int m_OpenParameterId;
	protected string  _fileName;


	public override void StartUp(){
		CreatedLevels = new List<Level>();
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);
		_fileName = Path.Combine (Application.persistentDataPath, "levels.txt");
		CreateLevels();

		rules = new Rules ();
	
		setRules ();
		int[] positions = new int[0];

		Introduction ();
		mainManager = new MainManager (13, 20, 1, positions, 0);

		mainManager.initLevel (rules, points);
		mainManager.createPlayers (1, res, 0, 0);
		mainManager.setPlayersParametres (1, 0, 0, 1, 0);
		Generations = mainManager.getGenerations();

		PaintGrid();


	}

	public IEnumerator CloseIntrodaction() {
		yield return new WaitForSeconds (2);
		IntroductionScreen.SetBool(m_OpenParameterId, false);
	}
	protected void PaintGrid()
	{
		for (int y = 0; y < 13; y++)
		{
			for (int x = 0; x < 20; x++)
			{
				float nx = (float)(-2.1 + x * 0.22);
				float ny = (float)(1.5 - y * 0.25);
				Position posit = new Position(y, x);
				Transform Cell = (Transform)Instantiate(CellPrefab, new Vector3(x, y, 0), Quaternion.identity);

				//Cell.GetComponent<CellSanbox>().setPointsManager(mainManager.getPointsManager());
				//Cell.GetComponent<CellSanbox>().setGens(mainManager.getGenerations());
				Cell.GetComponent<CellLevel>().setPosition(posit);
				Cell.GetComponent<CellLevel> ().setMainManager (mainManager);
				//Cell.GetComponent<CellSanbox>().setPlayersManager(mainManager.getPlayersManager());
				Cell.GetComponent<CellLevel> ().setSxeneManager (this);


				Cell.SetParent(Parent);
				Vector3 pos = new Vector3(nx, ny, 0);

				Cell.transform.localPosition = pos;

			}
		}
	}

	public Color getGenColor(int n) {
		return genLevelColors [n];
	}

	public int getCurrentlyActiveGenerationNum()
	{
		return currentlyActiveGenerationNum;
	}

	public void setCurrentlyActiveGenerationNum(int n)
	{
		currentlyActiveGenerationNum = n;
	}

	public override void StepForward() {


		base.StepForward ();
		score_player.text = mainManager.getScore (1).ToString ();
		alive_player.text = getScore ().ToString();
		StepLevel ();

	}

	public virtual void StepLevel() {

		int[] values = new int[2]{ mainManager.getScore (1), getScore () };
		if (!currentLevel.isLevelNotCompleted (values)) {
			Victory (true);
		}
	}

	public void Victory(bool V) {
		if (V) {
			GameResult.text = "Level Complete!";
			VictoryScreen.gameObject.SetActive (true);
			VictoryScreen.SetBool (m_OpenParameterId, true);
		} else {
			GameResult.text = "Level Failed!";

			VictoryScreen.gameObject.SetActive (true);
			VictoryScreen.SetBool (m_OpenParameterId, true);
			NextLevl.interactable = false;
		}
	}



	public virtual  void Introduction() {
		
		currentLevel = Levels.Levels [GameManager.instance.getCurrentLevelNumber()];
		points = currentLevel.getInitiation ();

		LevelName.text = currentLevel.Name;
		LevelDis.text = currentLevel.discription ;
		IntroductionScreen.SetBool(m_OpenParameterId, true);

		StartCoroutine("CloseIntrodaction");
		StartLevel ();

	}

	public void StartLevel(){
		goals = new NumericalGoal[2];
		tipField.text = currentLevel.getTip ();
		goals = currentLevel.getLevelGoasl ();
		print (goals [0].getSet ());
		if (goals [0].getSet ()) {
			coroutine = OpenBtn (ScoreBtn);
			StartCoroutine(coroutine);
			score_goal.text = goals [0].getValue ().ToString();
		}
		if (goals [1].getSet ()) {
			coroutine = OpenBtn (AliveBtn);
			StartCoroutine(coroutine);
			alive_goal.text = goals [1].getValue ().ToString();
		}
		res = currentLevel.getResouces ();
		resources.text = res.ToString();
		if (!currentLevel.isplayerAllowetoChGens) {
			genButton.interactable = false;
		}
		if (currentLevel.isLevelTimeRestricted()) {
			Time = currentLevel.isLevelTimeRestricted();
			Timer = currentLevel.getTime ();
			coroutine = OpenBtn (TimerBtn);
			StartCoroutine(coroutine);

			timertext.text = Timer.ToString ();
			InvokeRepeating("LevelTimer",0.0f,1.0f);	
		}

	

	}

	public IEnumerator OpenBtn(Image g) {
		yield return new WaitForFixedUpdate();
		g.GetComponent<popUpMenu>().Open ();
	}

	void LevelTimer() {
		
		Timer -= 1;
		timertext.text = Timer.ToString ();
		int[] values = new int[2]{ mainManager.getScore (1), getScore () };
		if (Timer < 0 && currentLevel.isLevelNotCompleted(values)) {
			Victory (false);
			CancelInvoke ();
		}

		
	}

	public void setRules() {

		rules.NameOfTheRules = "level constructor";
		rules.DisOfTheRules ="levels" ;
		rules.numberOfProperties =  new int[]{6,4,2,0};
		rules.importance = new bool[]{false,false,false,false,false,false};
		rules.names = new string[]{"Classic","Quick Classic","Impact plus","Impact minus","for buffing1","for buffing2"} ;
		rules.descriptions = new string[]{"","","","","",""} ;
		rules.aliveInterLinery = new float[]{4.0f,2.0f,3.0f,3.0f,2.0f,2.0f,2.0f,0.0f,0.0f};
		rules.deadInterLinery =  new float[]{4.0f,4.0f,-10.0f,1.0f,4.0f,100.0f,4.0f,-9.0f,0.0f,3.0f,9.0f,0.0f,0.0f};
		rules.onePointPowrStat =  new float[]{1.0f,1.0f,1.0f,1.0f};
		rules.livePStat = new float[] {0.0f,0.0f,0.0f,0.0f} ;
		rules.livePDyn = new float[]{0.0f,0.0f} ;
		rules.setP =  new float[]{0.0f,0.0f} ;
		rules.collectionsNamLinery =new string[]{"0"}  ;

		rules.numberOfGens = 5;
		rules.genNames = new string[]{"Alive points classic","Alive points quick","walls","Impact plus","Impact minus"};
		rules.genDesc = new string[]{"","","","",""} ;
		rules.propersNumbersLinery = new int[]{5,3,0,4,5,3,4,5,1,0,1,2,1,3};
		rules.immort = new bool[]{false,false,true,true,true};

		rules.numberOfFuncs = 6 ;
		rules.propNumbersLinery = new int[]{6,1,0,1,1,2,4,5,2,4,5,0,0};
		rules.coefFrLinery = new float[]{6.0f,1.0f,1.0f,1.0f,1.0f,2.0f,1.0f,1.0f,2.0f,-1.0f,-1.0f,0.0f,0.0f};
		rules.coefEnLinery = new float[]{6.0f,1.0f,-1.0f,1.0f,-1.0f,2.0f,1.0f,1.0f,2.0f,-1.0f,-1.0f,0.0f,0.0f};
		rules.funcToProp = new int[]{0,1,2,3,4,5};
	}


	public void CreateLevels() {
		int n = GameManager.instance.getCurrentLevelBunbleNumber ();

		switch (n){
		case 0:
			{
				string[] s = new string[] {
					"{\"Name\":\"score\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":25}",
					"{\"Name\":\"alive\",\"isSet\":false,\"achieved\":false,\"failed\":false,\"value\":100}"
				};
				//Levels = new LevelBundle[2];
				int[] init = new int[]{ 1, 0, 5, 13, 1, 0, 5, 12, 1, 0, 5, 11, 1, 0, 4, 11, 1, 0, 3, 12 };
				Level lvl1 = new Level ("Level1", "Intro", "Hello! This is the game of life! Click button with big plus", init, s, 0, 20, false, false);
				s = new string[] {
					"{\"Name\":\"score\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":25}",
					"{\"Name\":\"alive\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":3}"
				};
				init = new int[]{ };
				Level lvl2 = new Level ("Level2", "Intro", "Some Patterns Can Produce Cycles. Use it to your advance!", init, s, 3, 20, false, false);
				s = new string[] {
					"{\"Name\":\"score\",\"isSet\":false,\"achieved\":false,\"failed\":false,\"value\":25}",
					"{\"Name\":\"alive\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":45}"
				};
				Level lvl3 = new Level ("Level3", "Intro", "There pattern that can produce a lot of cells. Try Find One", init, s, 5, 20, false, false);
				Level[] introlevels = { lvl1,lvl2,lvl3 };
				LevelBundle Intro = new LevelBundle ("Intro", 3, introlevels);
				Intro.setCurrent (GameManager.instance.getCurrentLevelNumber ());
				Levels = Intro;
				break;
			}
		case 1:
			{
				string[] s = new string[] {
					"{\"Name\":\"score\",\"isSet\":false,\"achieved\":false,\"failed\":false,\"value\":25}",
					"{\"Name\":\"alive\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":15}"
				};

				int[] init = new int[]{ 1, 3, 7, 9 };
				Level lvl1 = new Level ("Level1", "Power Points", "This points support ypurs!", init, s, 1, 20, false, false);
				s = new string[] {
					"{\"Name\":\"score\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":60}",
					"{\"Name\":\"alive\",\"isSet\":false,\"achieved\":false,\"failed\":false,\"value\":3}"
				};
				init = new int[]{1,0,6,5,1,0,5,6,1,0,6,6,1,0,7,6,1,0,6,7,1,0,5,13,1,0,6,13,1,0,6,12,1,0,7,13,1,0,6,14,1,4,4,11,1,4,4,15,1,4,8,11,1,4,8,15};
				Level lvl2 = new Level ("Level2", "Bad Cells", "Red cells prevent your cells from developing", init, s, 0, 20, false, false);
				s = new string[] {
					"{\"Name\":\"score\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":3000}",
					"{\"Name\":\"alive\",\"isSet\":false,\"achieved\":false,\"failed\":false,\"value\":100}"
				};
				init = new int[]{ };
				Level lvl3 = new Level ("Level3", "Fast Generation", "You can now chose type of the cell!", init, s, 3, 20,true, false);
				s = new string[] {
					"{\"Name\":\"score\",\"isSet\":true,\"achieved\":false,\"failed\":false,\"value\":2000}",
					"{\"Name\":\"alive\",\"isSet\":false,\"achieved\":false,\"failed\":false,\"value\":100}"
				};
				init = new int[]{ };
				Level lvl4 = new Level ("Level4", "Time is going Fast!", "Act Fast!", init, s, 5, 20,true, true);
				Level[] basiclevels = { lvl1,lvl2,lvl3,lvl4 };
				LevelBundle Basic = new LevelBundle ("Basic", 4, basiclevels);
				Basic.setCurrent (GameManager.instance.getCurrentLevelNumber ());
				Levels = Basic;
				break;
			}
		case 2:
			{
				OpenLevels ();
				print ("this is custon lvl Bundle");
				LevelBundle Created = new LevelBundle ("cretaed", CreatedLevels.Count, CreatedLevels.ToArray ());
				Created.setCurrent (GameManager.instance.getCurrentLevelNumber ());
				Levels = Created;
				break;
			}
	
		}
	}

	void Update(){
		resources.text = mainManager.getResourcrs (1).ToString();
	}

	public void OpenLevels() 
	{
		if (!File.Exists (_fileName)) {
			Debug.Log ("No file");
			return;
		} else {
			print (_fileName);
			StreamReader stream = new StreamReader (_fileName); 
			string l = "";

			while ((l = stream.ReadLine ()) != null) {
				Level lvl = JsonUtility.FromJson<Level> (l);
				CreatedLevels.Add (lvl);

			}

			stream.Close ();
		}

	}

	public void Next() {
		int n = Levels.getnextLevel();
		if (n != -1) {
			GameManager.instance.setCurrenLevelNumber (n);
			ReloadLevel (3);
		} else
			GoHome ();
	}


}
