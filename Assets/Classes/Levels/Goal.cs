using UnityEngine;
using System.Collections;
[System.Serializable]
public class Goal  {
	public string Name;
	public bool  isSet;
	public bool achieved;
	public bool failed;

	public string getName()
	{
		return Name;
	}

	public void setName(string n) {
		Name =  n;
	}

	public virtual bool CheckGoal(int n){
		return achieved;
	}

	public void setGoal(){
		isSet = true;
	}

	public bool getSet(){
		return isSet;
	}
}
[System.Serializable]
public class NumericalGoal : Goal {
	public int value;

	public override bool CheckGoal(int n){
		if (!isSet)
			return true;
		else if (n >= value)
				achieved = true;
			return achieved;
	}
	public void setValue(int n) {
		value = n;
	}

	public int getValue() {
		return value;
	}
	public NumericalGoal(string n, int v, bool set) {
		Name = n;
		value = v;
		isSet = set;
	}

}

