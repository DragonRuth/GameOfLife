using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.GameClasses.PropertiesSpace;
using Classes.Game.MainManagerSpace;


public class ScrolViewScript : MonoBehaviour {
	public GameObject Prefab; // the prefab for this particular scrolllist;


	public Transform Content;
	public List<GameObject> Items;
	public SceneManager sceneMan;
	public PropertyEditor propEd;
	public ScreenManager scrMan;
	private int currenttlySelected = -1;

	public void AddGenerations(List<Generation> List, List<bool> addedGenerations)
	{
		for (int i = 0; i < List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);
			newButton.GetComponent<GenerationsItem> ().Setup (i, List[i].getNameAndDescription()[0], 
                List[i].getNameAndDescription()[1], "Generation", this, sceneMan, addedGenerations[i]);
			Items.Add (newButton);
		}
	}


	public void RemoveItems()
	{
		
        int Childs = Content.childCount;
        for (int i = Childs - 1; i > -1; i--)
		{
			GameObject.Destroy(Content.GetChild(i).gameObject);
			
		}
        Items.Clear();
	}

	public void AddRules(List<Rules> List)
	{
		for (int i = 0; i < List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);
			newButton.GetComponent<RulesItemScript> ().Setup (i, List [i].NameOfTheRules, List [i].DisOfTheRules, "Rules", this, sceneMan);
			Items.Add (newButton);
		}
	}

	public void AddPropertys(List<Property> List, List<bool> addedProperties)
	{
		for (int i = 0; i < List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);
            
            
			newButton.GetComponent<PropertyesItem> ().Setup (i, List [i].getNameAndDescription()[0], List [i].getNameAndDescription()[1], "Property", this, sceneMan, List[i].getType(), addedProperties[i]);
			Items.Add (newButton);
		}
	}

	public void AddPropertyesOfGeneration(List<Property> List)
	{
		for (int i = 0; i < List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);


			newButton.GetComponent<PropertyOfGenerationItem> ().Setup (i, List [i].getNameAndDescription()[0], List [i].getNameAndDescription()[1], "Property", this, sceneMan, List[i].getType());
			Items.Add (newButton);
		}
	}

	public void AddCollectionItems(List<string> List)
	{
		for (int i = 0; i < List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);


			newButton.GetComponent<CollectionItem> ().Setup (i,List[i],"CollectionItem", this, propEd);
			Items.Add (newButton);
		}
	}

	public void AddLevelItems(List<string> List) {
		for (int i = 0; i <List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);

			newButton.GetComponent<LevelItemSxript> ().Setup (i,"Level",List[i],this,scrMan);
			Items.Add (newButton);
		}
	}
		

	public void setSelected(int N)
	{
		for (int i = 0; i < Items.Count; i++) {
			Items [i].GetComponent<Button> ().image.color = new Color (1.0f, 1.0f, 1.0f);
		}
		currenttlySelected = N;
		Items [N].GetComponent<Button> ().image.color = new Color (0.5f, 0.5f, 0.5f);
	}
		
	public int getSelected()
	{
		return currenttlySelected;
	}

	public void addMyLevels(List<Level> List){
		for (int i = 0; i <List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);

			newButton.GetComponent<MyThingItem> ().Setup (i,"Level",List[i].Name,this,scrMan);
			Items.Add (newButton);
		}
	}
	public void addMyRules(List<Rules> List){
		for (int i = 0; i <List.Count; i++) {
			GameObject newButton = (GameObject)GameObject.Instantiate (Prefab);
			newButton.transform.SetParent (Content);

			newButton.GetComponent<MyThingItem> ().Setup (i,"Rules",List[i].NameOfTheRules,this,scrMan);
			Items.Add (newButton);
		}
	}


}
