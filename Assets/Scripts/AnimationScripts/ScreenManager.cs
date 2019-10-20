using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Classes.Game.MainManagerSpace;
using System.IO;
using System;
public class ScreenManager : MonoBehaviour {

	//Screen to open automatically at the start of the Scene
	public Animator initiallyOpen;
	public Animator initiallyOpenLevel;

	public NetworkManager _network;

	public List<string>[] LevelsNames;
	public ScrolViewScript LevelScroll;

	public ScrolViewScript MyRules;
	public ScrolViewScript MyLevels;

	//Currently Open Screen
	private Animator m_Open;
	private Animator m_OpenLevel;

	//Hash of the parameter we use to control the transitions.
	private int m_OpenParameterId;
	private int m_OpenFLevelParametrId;
	private int m_OpenBLevelParametrId;

	//The GameObject Selected before we opened the current Screen.
	//Used when closing a Screen, so we can go back to the button that opened it.
	private GameObject m_PreviouslySelected;

	//Animator State and Transition names we need to check against.
	const string k_OpenTransitionName = "Open";
	const string k_OpenLevelFTransitionName = "ForwardOpened";
	const string k_OpenLevelBTransitionName = "BackOpened";
	const string k_ClosedStateName = "Closed";
	public Toggle musicToggle;


	protected string  _LvlfileName;
	protected string _RfileName;

	private List<Level> Levels;
	private List<Rules> Rules;
	private int currentThingNumber;

	private int currentlyDownloadingThingId;
	private string currentlyDownloadingType;

	public void OnEnable()
	{

		_LvlfileName = Path.Combine (Application.persistentDataPath, "levels.txt");
		_RfileName = Path.Combine (Application.persistentDataPath, "rules.txt");
		//We cache the Hash to the "Open" Parameter, so we can feed to Animator.SetBool.
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);
		m_OpenFLevelParametrId = Animator.StringToHash(k_OpenLevelFTransitionName);
		m_OpenBLevelParametrId = Animator.StringToHash(k_OpenLevelBTransitionName);

		//If set, open the initial Screen now.
		if (initiallyOpen == null)
			return;
		OpenPanel(initiallyOpen);
		m_OpenLevel = initiallyOpenLevel;
		m_OpenLevel.SetBool (m_OpenFLevelParametrId, true);
		m_OpenLevel.SetBool (m_OpenParameterId, true);
		OpenLevelForward (initiallyOpenLevel);

		Levels = new List<Level> ();
		Rules = new List<Rules> ();

		LevelsNames = new List<string>[3];
		LevelsNames[0] = new List<string> ();
		LevelsNames[1] = new List<string> ();
		LevelsNames [2] = new List<string> ();
		LevelsNames [0].Add ("Level1");
		LevelsNames [0].Add ("Level2");
		LevelsNames [0].Add ("Level3");
		LevelsNames [1].Add ("Level1");
		LevelsNames [1].Add ("Level2");
		LevelsNames [1].Add ("Level3");
		LevelsNames [1].Add ("level4");





	}

	//Closes the currently open panel and opens the provided one.
	//It also takes care of handling the navigation, setting the new Selected element.
	public void OpenPanel (Animator anim)
	{
		if (m_Open == anim)
			return;

		//Activate the new Screen hierarchy so we can animate it.
		anim.gameObject.SetActive(true);

		anim.transform.SetAsLastSibling();

		CloseCurrent();

		//m_PreviouslySelected = newPreviouslySelected;

		//Set the new Screen as then open one.
		m_Open = anim;
		//Start the open animation
		m_Open.SetBool(m_OpenParameterId, true);


	}
	public void OpenLevelForward(Animator anim)
	{
		if (m_OpenLevel == anim)
			return;
		anim.gameObject.SetActive (true);
	//	anim.transform.SetAsLastSibling ();

		m_OpenLevel.SetBool (m_OpenParameterId, false);
		m_OpenLevel.SetBool (m_OpenBLevelParametrId, true);
		m_OpenLevel.SetBool (m_OpenFLevelParametrId, false);
		StartCoroutine(DisableLevelPanelDeleyed(m_OpenLevel));
		//m_OpenLevel.gameObject.SetActive (false);
		m_OpenLevel = anim;

		m_OpenLevel.SetBool (m_OpenFLevelParametrId, true);
		m_OpenLevel.SetBool (m_OpenParameterId, true);
	}

	public void OpenLevelBackwards(Animator anim)
	{
		if (m_OpenLevel == anim)
			return;
		

		m_OpenLevel.SetBool (m_OpenParameterId, false);
		m_OpenLevel.SetBool (m_OpenFLevelParametrId, false);

		StartCoroutine(DisableLevelPanelDeleyed(m_OpenLevel));
		//m_OpenLevel.gameObject.SetActive (false);
		m_OpenLevel = anim;

	
		anim.gameObject.SetActive (true);
		//anim.transform.SetAsLastSibling ();
		m_OpenLevel.SetBool (m_OpenFLevelParametrId, true);
		m_OpenLevel.SetBool (m_OpenParameterId, true);
		m_OpenLevel.SetBool (m_OpenBLevelParametrId, false);

	}


	public void CloseCurrent()
	{
		if (m_Open == null)
			return;

		//Start the close animation.
		m_Open.SetBool(m_OpenParameterId, false);


		StartCoroutine(DisablePanelDeleyed(m_Open));
		//No screen open.
		m_Open = null;
	}

	//Coroutine that will detect when the Closing animation is finished and it will deactivate the
	//hierarchy.
	IEnumerator DisablePanelDeleyed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition(0))
				closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

			wantToClose = !anim.GetBool(m_OpenParameterId);

			yield return new WaitForEndOfFrame();
		}

		if (wantToClose)
			anim.gameObject.SetActive(false);
	}
	IEnumerator DisableLevelPanelDeleyed(Animator anim)
	{
		bool closedStateReached = false;
		bool wantToClose = true;
		while (!closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition (0))
				if (anim.GetCurrentAnimatorStateInfo (0).IsName ("ForwardLevel") || anim.GetCurrentAnimatorStateInfo (0).IsName ("BackLevel"))
					closedStateReached = true;

			wantToClose = !anim.GetBool(m_OpenParameterId);

			yield return new WaitForEndOfFrame();
		}

		if (wantToClose)
			anim.gameObject.SetActive(false);
	}


	public void  animBtn(Animator anim){
		anim.Play("BtnRotate");
	}

    public void changeLevel(int levelNum)
    {
        GameManager.instance.changeSceen(levelNum);
    }

	public void changeMusic() {
		if (musicToggle.isOn) {
			AudioManager.instance.StartMusic (true);
		} else AudioManager.instance.StartMusic (false);


	}

	void Start(){
		checkMusicToggle ();
	}
	private void checkMusicToggle(){
		musicToggle.isOn = AudioManager.getMusic ();	
	}

	public void setGameMode (bool t)
	{
		GameManager.instance.setFieldType (t);
	}

	public void setLevelBundle (int n) 
	{
		GameManager.instance.setCurrentLevelBundle (n);
	}

	public void setLevelNumber ( int n)
	{
		GameManager.instance.setCurrenLevelNumber (n);
	}

	public void OpenLevels() 
	{
		if (!File.Exists (_LvlfileName)) {
			Debug.Log ("No file");
			return;
		} else {
			print (_LvlfileName);
			StreamReader stream = new StreamReader (_LvlfileName); 
			string l = "";
			LevelsNames [2].Clear ();
			Levels.Clear ();
			while ((l = stream.ReadLine ()) != null) {
				Level lvl = JsonUtility.FromJson<Level> (l);
				Levels.Add (lvl);
				LevelsNames [2].Add (lvl.Name);
			}
			RefreshLevels ();
			//RefreshMyLevels ();
			stream.Close ();
		}
		
	}

	public void OpenRules() 
	{
		if (!File.Exists (_RfileName)) 
		{
			Debug.Log ("No file");
			return;
		} else 
		{
			print (_RfileName);
			StreamReader stream = new StreamReader(_RfileName); 
			string r = "";

			Rules.Clear ();
			while ((r = stream.ReadLine ()) != null) {
				Rules.Add (JsonUtility.FromJson<Rules> (r));
			}
			RefreshMyRules ();
			stream.Close();

		}

	}


	public void RefreshMyRules()
	{
		MyRules.RemoveItems ();
		MyRules.addMyRules (Rules);
	}
	public void RefreshMyLevels(){
		MyLevels.RemoveItems ();
		MyLevels.addMyLevels (Levels);
	}

	public void RefreshLevels()
	{
		LevelScroll.RemoveItems ();
		LevelScroll.AddLevelItems (LevelsNames[GameManager.instance.getCurrentLevelBunbleNumber()]);
	}

	public void setThingNumber(int n){
		currentThingNumber = n;
	}

	public void deleteRules() 
	{
		if (currentThingNumber != -1) {
			string[] readText = System.IO.File.ReadAllLines (_RfileName);
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (_RfileName, false)) {
				for (int i = 0; i < readText.Length; i++) {
					if (i != currentThingNumber)// if(i!=1)
						file.WriteLine (readText [i]); 
				}
				file.Close ();
			}	
		}
		OpenRules ();
		setThingNumber (-1);
	}

	public void deleteLevels() 
	{
		if (currentThingNumber != -1) {
			string[] readText = System.IO.File.ReadAllLines (_LvlfileName);
			using (System.IO.StreamWriter file = new System.IO.StreamWriter (_LvlfileName, false)) {
				for (int i = 0; i < readText.Length; i++) {
					if (i != currentThingNumber)// if(i!=1)
						file.WriteLine (readText [i]); 
				}
				file.Close ();
			}	
		}
		OpenLevels ();
		RefreshMyLevels ();
		setThingNumber (-1);
	}

	public void StartDownload() {
		StartCoroutine (_network.getSingleData (OnJsonSingleDataLoaded));
		
	}

	public void StartUpload() {
		
	}

	public void OnJsonSingleDataLoaded(string data) {
		Debug.Log (data);
	}

	public void OnItemwasLoaded(string data){
		
	}
	public void OnDataWasUploaded(string data){
		
	}
}