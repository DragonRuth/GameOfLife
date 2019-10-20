using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GenerationsItem : ListItemScript {
	public Toggle isUsed;

	public void Setup(int iItemNumber, string iName, string iDis, string iType, ScrolViewScript iscroll,SceneManager iSceneManager, bool iUsed) 
	{
		base.Setup (iItemNumber, iName, iDis, iType, iscroll,iSceneManager);
		isUsed.isOn = iUsed;
	}
	public override void HandleClick()
	{
		base.HandleClick ();
		sceneManager.setCurrentGenerationNumber (ItemNumber);
	}
}
