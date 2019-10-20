using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RulesItemScript : ListItemScript {
	public Toggle isUsed;

	public void Setup(int iItemNumber, string iName, string iDis, string iType, ScrolViewScript iscroll,SceneManager iSceneManager) 
	{
		base.Setup (iItemNumber, iName, iDis, iType, iscroll,iSceneManager);

	}
	public override void HandleClick()
	{
		base.HandleClick ();
		sceneManager.setCurrentRulesNumber (ItemNumber);
	}
}
