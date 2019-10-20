using UnityEngine;
using System.Collections;

public class LevelBundle : MonoBehaviour {

	public Level[] Levels;
	public string Name;
	public int current;
	public int size;

	public int getnextLevel()
	{
		current++;
		if (current == size) {
			return -1;
		} else
			return current;
	}

	public LevelBundle( string iName, int isize, Level[] iLevels) 
	{
		Name = iName;
		size = isize;
		Levels = iLevels;
	}

	public void setCurrent( int n)
	{
		current = n;
	}

	public int getCurrent() 
	{
		return current;
	}
}
