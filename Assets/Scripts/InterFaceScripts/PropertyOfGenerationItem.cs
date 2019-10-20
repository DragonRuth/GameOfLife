using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PropertyOfGenerationItem : ListItemScript {
	public byte PropType;
	public Text propTypetext;
	public void Setup(int iItemNumber, string iName, string iDis, string iType, ScrolViewScript iscroll,SceneManager iSceneManager, byte iPropType) 
	{
		base.Setup (iItemNumber, iName, iDis, iType, iscroll, iSceneManager);
		PropType = iPropType;
		switch (iPropType)
		{
		case 0:
			propTypetext.text = "Statical";
			break;
		case 1:
			propTypetext.text = "Dynamical";
			break;
		case 2:
			propTypetext.text = "Collection";
			break;
		}
	}

	public override void HandleClick()
	{
		base.HandleClick ();
		sceneManager.setCurrentPropertyNumber (ItemNumber);
		sceneManager.setCurrentPropertyType (PropType);
	}


}
