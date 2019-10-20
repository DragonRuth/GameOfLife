using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Classes.GameClasses.PropertiesSpace;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PropertiesSpace;
using System.Collections.Generic;


//Ну ты понял

public class PropertyEditor : MonoBehaviour {
	
	public Image StatMenu;
    public Image DinMenu;
    public Image ColMenu;


	public Button[] ControlBtns;
	//StatPropItems
	public InputField statZeroPoint;
	public InputField statPower;
	public List<InputField[]> stDeadIntervals;
	public List<InputField[]> stAliveIntervals;

	public GameObject IntevalPrefab;
	public Transform AliveIntContent;
	public Transform deadIntContentl;
	//DinPropsItems

	public InputField dinZeroPoint;
	public InputField dinSet;

	//ColPropsItems

	public InputField colNameField;
	public ScrollRect Collect;
	public ScrolViewScript CollectScroll;

	//Dis and Name
	public InputField NameInput;
	public InputField DisInput;
	public Image SaveMenu;
	public Toggle isMustHave;

    private List<float[]> deadIntervals;
    private List<float[]> liveIntervals;
    private List<string> collection;
    private float onePointPower;
    private float zeroPoint;
    private float setPoint;
	private int currentColItem = 0;
	private byte currentlySavingPropertyType;
	private Property property = null;
	private MainManager mainManager;

	void Start()
	{
		collection = new List<string>();
		deadIntervals = new List<float[]>();
		liveIntervals = new List<float[]>();
		stDeadIntervals = new List<InputField[]> ();
		stAliveIntervals = new List<InputField[]>();

	}

	public void setMainManager(MainManager mainM) 
	{
		mainManager = mainM;
	}

	public void OpenPropertyToEdit(Property prop, byte currentlyChosenPropertyType) 
    {
		NewProperty ();
		currentlySavingPropertyType = currentlyChosenPropertyType;
		property = prop;
		isMustHave.isOn = prop.getMustHave ();
		switch (currentlyChosenPropertyType)
		{
		case 0:
			print ("stst");
			StaticalProperty stat = (StaticalProperty)prop;
			StatMenu.GetComponent<popUpMenu> ().Open ();
			AddIntervals (stat.getLiveInterval (), liveIntervals);
			AddIntervals (stat.getDeadInterval (), deadIntervals);
			onePointPower = stat.getPower ();
			zeroPoint = stat.getLivePoint ();
			RefreshStat ();
			ControlBtns [0].interactable = true;
			ControlBtns [1].interactable = false;
			ControlBtns [2].interactable = false;
		
			break;
		case 1:
			DynamicalProperty din = (DynamicalProperty)prop;
			DinMenu.GetComponent<popUpMenu> ().Open ();
			setPoint = din.getSetPoint ();
			zeroPoint = din.getLivePoint ();
			RefreshDin ();
			ControlBtns [1].interactable = true;
			ControlBtns [0].interactable = false;
			ControlBtns [2].interactable = false;
			break;
		case 2:
			CollectionalProperty col = (CollectionalProperty)prop;
			ColMenu.GetComponent<popUpMenu> ().Open ();
			collection.AddRange(col.getCollection ());
			RefreshCollections ();
			ControlBtns [2].interactable = true;
			ControlBtns [0].interactable = false;
			ControlBtns [1].interactable = false;
			break;
		}
    }



	public void NewProperty()
	{
		onePointPower = 0 ;
		zeroPoint = 0;
		setPoint = 0;
		liveIntervals.Clear ();
		deadIntervals.Clear ();
		collection.Clear ();
		print (liveIntervals.Count);
		currentColItem = 0;
		statPower.text = onePointPower.ToString ();
		statZeroPoint.text = zeroPoint.ToString ();
		dinZeroPoint.text = zeroPoint.ToString ();
		dinSet.text = setPoint.ToString ();
		ControlBtns [0].interactable = true;
		ControlBtns [1].interactable = true;
		ControlBtns [2].interactable = true;
		RefreshDin ();
		RefreshCollections ();
		RefreshStat ();
		property = null;

	}
		

    public void addCollectionItem()
    {
		collection.Add (colNameField.text);
		RefreshCollections ();
    }

	public void removeCollectionItem()
	{
		if (currentColItem < collection.Count) {
			collection.RemoveAt (currentColItem);
			RefreshCollections ();
		}
	}

	public void setCurrentCollItem(int n)
	{
		currentColItem = n;
	}

	public void RefreshCollections()
	{
		CollectScroll.RemoveItems ();
		CollectScroll.AddCollectionItems(collection);
	}

	public void RemoveIntervalsItems(Transform Content, List<InputField[]> InputList)
	{
		int Childs = Content.childCount;
		for (int i = Childs - 1; i > -1; i--)
		{
			GameObject.Destroy(Content.GetChild(i).gameObject);

		}
		InputList.Clear();

	}

	public void AddIntervals(float[][] Intervals, List<float[]> valueInterval)
	{
		
		int IntervalsAmount = Intervals.GetLength (0);
		if (IntervalsAmount <= 3) {
			for (int i = 0; i < IntervalsAmount; i++) {
				valueInterval.Add (Intervals [i]);
			}
		}

	}

	public void RemoveInterlvalItem(string type)
	{
		SaveValues ();
		switch (type) {
		case "dead":
			if (deadIntervals.Count > 0) {
				
				deadIntervals.RemoveAt (deadIntervals.Count - 1);
			}
			break;
		case "live":
			if (liveIntervals.Count > 0) {
	
				liveIntervals.RemoveAt (liveIntervals.Count - 1);
			}
			break;
		}
		RefreshInterval (type);

	}
	public void AddInterval(string type)
	{
		SaveValues ();
		switch (type) {
		case "dead":
			if (deadIntervals.Count < 3) {
				float[] f = new float[2] { 0, 0 };
				deadIntervals.Add (f);
			}
			break;
		case "live":
			if (liveIntervals.Count < 3) {
				
				float[] f = new float[2] { 0, 0 };
				liveIntervals.Add (f);
			}
			break;
		}

		RefreshInterval (type);
	}

	public void AddIntervalsItems(Transform Content, List<InputField[]> stInput, List<float[]> valueInterval)
	{

		for (int i = 0; i < valueInterval.Count; i++) {
			Transform newInt = GameObject.Instantiate (IntevalPrefab).transform;
			newInt.SetParent (Content);
			InputField newFrom = newInt.GetChild (0).GetComponent<InputField> ();
			InputField newTo = newInt.GetChild (1).GetComponent<InputField>();
			newFrom.text = valueInterval[i][0].ToString();
			newTo.text = valueInterval[i][1].ToString();
			InputField[] input = new InputField[2] {newFrom,newTo};
			stInput.Add(input);
		}

	}

	public void RefreshInterval(string Type){
		
		switch (Type) {
		case "dead":
			RemoveIntervalsItems (deadIntContentl, stDeadIntervals);
			AddIntervalsItems (deadIntContentl, stDeadIntervals, deadIntervals);
			break;
		case "live":
			RemoveIntervalsItems (AliveIntContent, stAliveIntervals);
			AddIntervalsItems(AliveIntContent, stAliveIntervals, liveIntervals);
			break;
		}
		
	}

	public void RefreshDin(){
		dinZeroPoint.text = zeroPoint.ToString ();
		dinSet.text = setPoint.ToString ();
	}
	public void RefreshStat(){
		statPower.text = onePointPower.ToString ();
		statZeroPoint.text = zeroPoint.ToString ();

		RefreshInterval ("dead");
		RefreshInterval("live");
	}

	public void setCurentPropType(int type){
		currentlySavingPropertyType = (byte)type;
	}

	public void SaveValues(){
		for (int i = 0; i < stAliveIntervals.Count; i++) {
			liveIntervals [i] [0] = float.Parse(stAliveIntervals [i] [0].text);
			liveIntervals [i] [1] = float.Parse(stAliveIntervals [i] [1].text);
		}
		for (int i = 0; i < stDeadIntervals.Count; i++) {
			deadIntervals [i] [0] = float.Parse(stDeadIntervals [i] [0].text);
			deadIntervals [i] [1] = float.Parse(stDeadIntervals [i] [1].text);
		}

		
	}

	public void SaveProperty()
	{
		if (property == null) {
			switch (currentlySavingPropertyType) {
			case 0:
				SaveValues ();
				zeroPoint = float.Parse (statZeroPoint.text);
				onePointPower = float.Parse (statPower.text);
				mainManager.createStaticalProperty (NameInput.text, DisInput.text, liveIntervals.ToArray (), deadIntervals.ToArray (), onePointPower, zeroPoint, isMustHave.isOn);
				int[] props = new int[0];
				mainManager.createFunction (props, null, null);
				mainManager.addLastFunctionToLastProperty (); 
				break;
			case 1:
				zeroPoint = float.Parse (dinZeroPoint.text);
				setPoint = float.Parse (dinSet.text);
				mainManager.createDynamicalProperty (NameInput.text, DisInput.text, zeroPoint, setPoint, isMustHave.isOn);
				props = new int[0];
				mainManager.createFunction (props, null, null);
				mainManager.addLastFunctionToLastProperty (); 
				break;
			case 2:
				mainManager.createCollectionalProperty (NameInput.text, DisInput.text, collection.ToArray (), isMustHave.isOn);
				int[] prop = { mainManager.getGPT ().getNumberOfProperties () - 1 };
				float[] coefFr = { 1 };
				float[] coefEn = { -1 };
				mainManager.createFunction (prop, coefFr, coefEn);
				mainManager.addLastFunctionToLastProperty ();
				break;
			}
		} else
		{
			switch (currentlySavingPropertyType) {
			case 0:
				SaveValues ();
				zeroPoint = float.Parse (statZeroPoint.text);
				onePointPower = float.Parse (statPower.text);

				mainManager.resetStaticalProperty (NameInput.text, DisInput.text, property.getNumber(), liveIntervals.ToArray (), deadIntervals.ToArray (), onePointPower, zeroPoint,isMustHave.isOn);
				break;
			case 1:
				zeroPoint = float.Parse (dinZeroPoint.text);
				setPoint = float.Parse (dinSet.text);

				mainManager.resetDynamicalProperty (NameInput.text, DisInput.text, property.getNumber(), zeroPoint, setPoint,isMustHave.isOn);
				break;
			case 2:
				
				mainManager.resetCollectionalProperty (NameInput.text, DisInput.text, property.getNumber(), collection.ToArray (), isMustHave.isOn);
				break;
			}
		}
	}

	public void setNameAndDis()
	{
		if (property != null) 
		{
			NameInput.text = property.getNameAndDescription () [0];
			DisInput.text = property.getNameAndDescription () [1];
		}
		
	}
}
