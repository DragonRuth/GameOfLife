using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MyThingItem : MonoBehaviour {
	public Button ButtonComponent;
	protected int ItemNumber;
	public string Type;

	public Text Name;
	public ScreenManager sceneManager;
	protected ScrolViewScript scrollview;
	//private ScrollViewScript scrollview;
	void Start()
	{
		ButtonComponent.onClick.AddListener (HandleClick);
	}

	public void Setup (int iItemNumber, string iType, string iName, ScrolViewScript iscroll, ScreenManager isceMan) 
	{
		ItemNumber = iItemNumber;

		Name.text = iName;
		Type = iType;
		scrollview = iscroll;
		sceneManager = isceMan;

	}
	public virtual void HandleClick()
	{
		scrollview.setSelected (ItemNumber);
		sceneManager.setThingNumber (ItemNumber);

	}


}