using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PointSpace;
using UnityEngine.UI;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.GameClasses.PropertiesSpace;
using System.IO;

public class LevelConstructor : LevelsSceneController {
	public InputField[] GoalsValue;
	public Toggle[] setGoal;

	public Toggle isPlayerAllowertoChoseGenerations;
	public InputField Seconds;
	private int[] AlivepPoints;
	public InputField Inputresources;
	private NumericalGoal[] goalsToset;


	public InputField Tip;

	public InputField Name;
	public InputField Dis;
	private Position pos;

	private bool isTimeRestricted;

	public Image alarmMenu;
	

	public Level level;
	public  override void StartUp()
	{
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);
		_fileName = Path.Combine (Application.persistentDataPath, "levels.txt");


		rules = new Rules ();

		setRules ();
		int[] positions = new int[0];

		GoalsValue [0].text = "100";
		GoalsValue [1].text = "100";
		Seconds.text = "100";
		Inputresources.text = "100";
		mainManager = new MainManager (13, 20, 1, positions, 0);
		mainManager.loadRules (rules);

		mainManager.createPlayers (1, 150, 0, 0);
		mainManager.setPlayersParametres (1, 1, 5, 1, 5);
		Generations = mainManager.getGenerations();

		base.PaintGrid();


	}





	public void setTime() {
		if (!isTimeRestricted) {
			alarmMenu.GetComponent<popUpMenu> ().Open ();
			isTimeRestricted = true;
		} else {
			alarmMenu.GetComponent<popUpMenu> ().Close ();
			isTimeRestricted = false;
		}
	}

	public void SaveLevel() {
		AlivepPoints = mainManager.savePointsAndGetArray ();
		string[] g = saveGoalsasString ();
		print (g);
		level = new Level(Name.text,Dis.text,Tip.text,AlivepPoints, g, byte.Parse(Inputresources.text),int.Parse(Seconds.text),isPlayerAllowertoChoseGenerations.isOn, isTimeRestricted);
		string l = JsonUtility.ToJson (level);
		print (l);
		if (!File.Exists (_fileName)) {
			File.Create (_fileName).Dispose ();
			StreamWriter stream = new StreamWriter (_fileName);
			stream.WriteLine (l);
			stream.Close();
		} else 
		{
			StreamWriter stream = new StreamWriter (_fileName,true,System.Text.Encoding.Default);
			stream.WriteLine (l);
			stream.Close ();
		}
	}

	public string[] saveGoalsasString() {
		
		NumericalGoal g1 = new NumericalGoal ("score", int.Parse (GoalsValue [0].text),setGoal[0].isOn);
		NumericalGoal g2 = new NumericalGoal ("alive", int.Parse (GoalsValue [1].text), setGoal [1].isOn);
		goalsToset = new NumericalGoal[]{ g1, g2 };
		string[] s = new string[2];
		s [0] = JsonUtility.ToJson (g1);
		print (s[0]);
		s [1] = JsonUtility.ToJson (g2);
		return s;
				
	}

	public override void StepLevel() {
	}
		

}
