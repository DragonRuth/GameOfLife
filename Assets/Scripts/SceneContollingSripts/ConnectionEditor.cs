using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Classes.GameClasses.PropertiesSpace;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PropertiesSpace;
using Classes.GameClasses.FuncManegerSpace;
using System.Collections.Generic;
public class ConnectionEditor : MonoBehaviour {
	private List<InputField[]> FriendEnemy;
	private List<float> CoefficientsEnem;
	private List<float> CoefficientsFriends;
	private List<Property> affectedProperties;
	public Transform Content;
	public ScrolViewScript propertyes;
	public GameObject ConnectionPrefab;
	public List<Property> availablepropertyes;
	private MainManager mainManager;
	private Property propToEdit;
	private List<Property> propsOfFunction;
	private bool isCol;
	// Use this for initialization
	void Start () {
		FriendEnemy = new List <InputField[]> ();
		CoefficientsEnem = new List<float> ();
		CoefficientsFriends = new List<float> ();
		affectedProperties = new List <Property> ();
	}


	public void setMainManager(MainManager mainM) 
	{
		mainManager = mainM;
	}

	public void OpenPropertyToEditConnections(List <Property> propList, Property prop) 
	{
		propertyes.AddPropertyesOfGeneration(propList);
		availablepropertyes = propList;
		propToEdit = prop;
		FunctionStatAndDynam func = (FunctionStatAndDynam)propToEdit.getFunction();
		if (func != null) {
			for (int i = 0; i < propList.Count; i++)
				for (int j = 0; j < func.getSufferingProperties ().Length; j++)
					if (propList[i] == func.getSufferingProperties ()[j]) 
					{
						propList.RemoveAt (i);
						i--;
						break;
					}
			affectedProperties = new List<Property> (func.getSufferingProperties ());
			CoefficientsEnem = new List<float> (func.getCoefficientsEnemy ());
			CoefficientsFriends = new List<float> (func.getCoefficientsFriend ());
		} else 
		{
			affectedProperties = new List<Property> ();
			CoefficientsEnem = new List<float> ();
			CoefficientsFriends = new List<float> ();
		}
		RefreshPropertyes ();
		RefreshConnections ();
			
	}

	public void RefreshPropertyes() 
	{
		propertyes.RemoveItems ();
		propertyes.AddPropertyesOfGeneration (availablepropertyes);
	}

	public void AddConnectionItem() 
	{
		SaveValues ();
		int n = propertyes.getSelected ();
		if (n != -1 && affectedProperties.Count < 3) {
			Property propToAdd = availablepropertyes [n];
			availablepropertyes.RemoveAt (n);
			affectedProperties.Add (propToAdd);
			CoefficientsEnem.Add (0);
			CoefficientsFriends.Add(0);
		}
		RefreshConnections ();
		RefreshPropertyes ();
		
	}
	public void removeConnectionItem() 
	{
		SaveValues ();
		if (FriendEnemy.Count > 0) 
		{
			CoefficientsFriends.RemoveAt (CoefficientsFriends.Count - 1);
			CoefficientsEnem.RemoveAt (CoefficientsEnem.Count - 1);
		}
		availablepropertyes.Add(affectedProperties[affectedProperties.Count -1]);
		affectedProperties.RemoveAt (affectedProperties.Count - 1);
		RefreshConnections ();
		RefreshPropertyes ();
	}

	public void RemoveConnections() 
	{
		int Childs = Content.childCount;
		for (int i = Childs - 1; i > -1; i--)
		{
			GameObject.Destroy(Content.GetChild(i).gameObject);

		}
		FriendEnemy.Clear ();
	}

	public void Additems(List<Property> propList, List<float> enemC, List<float> friendC)
	{
		for (int i = 0; i < propList.Count; i++) {
			Transform newItem = GameObject.Instantiate (ConnectionPrefab).transform;
			newItem.SetParent (Content);
			newItem.GetChild (0).GetChild (0).GetComponent<Text> ().text = propList [i].getNameAndDescription () [0];
			InputField newFr = newItem.GetChild (1).GetComponent<InputField> ();
			InputField newEn = newItem.GetChild (2).GetComponent<InputField>();
			newEn.text = enemC[i].ToString();
			newFr.text = friendC[i].ToString();
			InputField[] input = new InputField[2] {newFr,newEn};
			FriendEnemy.Add(input);
		}

	}

	public void RefreshConnections() 
	{
		RemoveConnections ();
		Additems (affectedProperties, CoefficientsEnem, CoefficientsFriends);
		
	}

	public void SaveValues() 
	{
		for (int i = 0; i < FriendEnemy.Count; i++)
		{
			CoefficientsFriends [i]  = float.Parse(FriendEnemy [i] [0].text);
			CoefficientsEnem [i] = float.Parse(FriendEnemy [i] [1].text);
		}
	}

	public void saveConnections() 
	{
		SaveValues ();
		int[] props = new int[affectedProperties.Count];
        for (int i = 0; i < affectedProperties.Count; i++)
        {
            props[i] = affectedProperties[i].getNumber();
            Debug.Log(affectedProperties[i].getNumber());
        }
        //Debug.Log("Prop num" + propToEdit.getNumber());
        mainManager.resetFunction (propToEdit.getNumber(), props, CoefficientsFriends.ToArray(), CoefficientsEnem.ToArray());

	}





}
