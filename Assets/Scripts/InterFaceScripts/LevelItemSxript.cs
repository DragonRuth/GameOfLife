using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LevelItemSxript : MonoBehaviour {
	public Button ButtonComponent;
	protected int ItemNumber;
	public string Type;
	public Text Number;
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
		Number.text = ItemNumber.ToString ();
		Name.text = iName;
		Type = iType;
		scrollview = iscroll;
		sceneManager = isceMan;
	
	}
	public  void HandleClick()
	{
		sceneManager.setLevelNumber (ItemNumber);
		sceneManager.changeLevel (3);
	}


}
