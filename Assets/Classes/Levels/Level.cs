using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
[System.Serializable]
public class Level {


	public int[] initiation;

	public byte resoureces;

	public NumericalGoal[] goals;

	public string discription;
	public string Name;
	public string Tip;

	public bool isplayerAllowetoChGens;
	public bool isTimeSet;
	public int time;

	public string[] serializedGoals;


	public Level(string iName, string Dis, string iTip, int[] init, string[] g, byte res, int itime, bool genAllowence, bool TimeRestriction)
	{
		Name = iName;
		discription = Dis;
		Tip = iTip;
		initiation = init;

		serializedGoals = g;
		deserializeGoals (g);
		resoureces = res;
		time = itime;
		isplayerAllowetoChGens = genAllowence;
		isTimeSet = TimeRestriction;

	}

	public bool isLevelNotCompleted(int[] n){
		bool flag = false;
		for (int i = 0; i < goals.Length; i++) {
			if (!goals [i].CheckGoal (n [i]))
				flag = true;
		}
		return flag; //возвращает true если левел еще не завершен
	}

	public NumericalGoal[] getLevelGoasl() {
		return goals;
	}

	public byte getResouces() {
		return resoureces;
	}
	public bool isLevelTimeRestricted() {
		return isTimeSet;
	}
	public int getTime(){
		return time;
	}

	public int[] getInitiation(){
		return initiation;
	}

	public string getTip() {
		return Tip;
	}

	public void deserializeGoals(string[] s){
		goals = new NumericalGoal[2];
		goals[0] = JsonUtility.FromJson<NumericalGoal>(s[0]);
		goals[1] = JsonUtility.FromJson<NumericalGoal>(s[1]);
		
	}


}
