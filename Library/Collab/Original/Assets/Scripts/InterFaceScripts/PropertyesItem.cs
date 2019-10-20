using UnityEngine;
using System.Collections;

public class PropertyesItem : ListItemScript {
	public byte PropType;

	public void Setup(int iItemNumber, string iName, string iDis, string iType, ScrolViewScript iscroll,SceneManager iSceneManager, byte iPropType) 
	{
		base.Setup (iItemNumber, iName, iDis, iType, iscroll, iSceneManager);
		PropType = iPropType;
		switch (iPropType)
		{
		case 0:
			ButtonComponent.image.color = new Color (0.3f, 0785f, 0.582f);
			break;
		case 1:
			ButtonComponent.image.color = new Color (0.75f, 0.3f, 0.582f);
			break;
		case 2:
			ButtonComponent.image.color = new Color (0.582f, 0.49f, 0.3f);
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
