using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PropertyesItem : PropertyOfGenerationItem {
	public Toggle isUsed;

	public void Setup(int iItemNumber, string iName, string iDis, string iType, ScrolViewScript iscroll,SceneManager iSceneManager, byte iPropType, bool iIsUsed) 
	{
		base.Setup (iItemNumber, iName, iDis, iType, iscroll, iSceneManager, iPropType);
		isUsed.isOn = iIsUsed;
	}

	public override void HandleClick()
	{
		base.HandleClick ();
		sceneManager.setCurrentPropertyNumber (ItemNumber);
		sceneManager.setCurrentPropertyType (PropType);
	}
}
