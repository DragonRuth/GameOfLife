using UnityEngine;
using System.Collections;

public class CollectionItem :ListItemScript  {
	private PropertyEditor propEditor;
	// Use this for initialization
	public void Setup(int iItemNumber, string iName, string iType, ScrolViewScript iscroll,PropertyEditor propEd) {
		ItemNumber = iItemNumber;
		Name.text = iName;
		Type = "Collection";
		scrollview = iscroll;
		propEditor = propEd;
	}
	public override void HandleClick()
	{
		base.HandleClick ();
		propEditor.setCurrentCollItem (ItemNumber);
	}
}
