using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ListItemScript : MonoBehaviour {
	public Button ButtonComponent;
	protected int ItemNumber;
	public Text Name;
	public Text Dis;
	public string Type;
	protected SceneManager sceneManager;
	protected ScrolViewScript scrollview;
	//private ScrollViewScript scrollview;
	void Start()
	{
		ButtonComponent.onClick.AddListener (HandleClick);
	}

	public void Setup (int iItemNumber, string iName, string iDis, string iType, ScrolViewScript iscroll, SceneManager iSceneManager) 
	{
		ItemNumber = iItemNumber;
		Name.text = iName;
		Dis.text = iDis;
		Type = iType;
		scrollview = iscroll;
		sceneManager = iSceneManager;
	}
	public virtual void HandleClick()
	{
		scrollview.setSelected (ItemNumber);
	}


}
