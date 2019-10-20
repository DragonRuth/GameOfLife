using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ActiveGenerationItem : MonoBehaviour {
	public Button ButtonComponent;
	protected int ItemNumber;
    private bool isChosen;
    public Button RemoveBtn;
    public Button AddBtn;
	public Text textForDis;
	public string NameofGen;
	public string DisofGen;
	public SceneManager sceneManager;
	void Start()
	{
		ButtonComponent.onClick.AddListener (HandleClick);
	}

	public void Setup (int iItemNumber, string iName, string iDis){
		ItemNumber = iItemNumber;
		NameofGen = iName;
		DisofGen = iDis;
        
	
	}
	public  void HandleClick()
	{
        if ( !isChosen)
        {
            RemoveBtn.transform.SetAsLastSibling();
            RemoveBtn.interactable = true;
            AddBtn.interactable = false;
            isChosen = true;
        } else
        {
            AddBtn.transform.SetAsLastSibling();
            AddBtn.interactable = true;
            RemoveBtn.interactable = false;
            isChosen = false;
        }
        print("My Generation number");
        print(ItemNumber);
		textForDis.text = DisofGen;
		sceneManager.setCurrentGenerationNumber (ItemNumber);
		sceneManager.setCurrentlyActiveGenerationNum (ItemNumber);
	}
}
