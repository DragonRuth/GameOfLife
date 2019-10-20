using UnityEngine;
using System.Collections;
using Classes.Game.MainManagerSpace;
using Classes.GameClasses.PointSpace;
using UnityEngine.UI;
using System.Collections.Generic;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.GameClasses.PropertiesSpace;
using System.IO;

public class SceneManager : MainSceneManager {
    public PropertyEditor propertyEditor;
	public ConnectionEditor connectionEditor;
	public Image propEditPanel;
	public Toggle isImmortal;

    public Text score;

    public Button[] genButtons;
    public Button addGenBtn;
	public Sprite[] GenButtonSprites;

    private List<int> NumberOfActiveGenerations;

	private bool steps=false;

	public int currentTouchGeneration = 0; // определяет, какое поколение надо ставить

	//выбранные в данный момент в меню поколения
	public int currentlySelectedRules = 0;
	public int currentlySelectedGeneration = 0;
	public int currentlyselectedProperty = 0;
	public byte currentlyChosenPropertyType = 0;
	private int currentlyActiveGenerationNum = 0;

	private List<Generation> Generations; // все поколения, что есть.
	private List<Property> Properties; //все свойства.
                                      //public List<RulesItem> Rules; // все правила

    public List<bool> addedGenerations;
    public List<List<bool>> addedProperties;
	public List<List<bool>> addedPropertiesInFunctions;

	//ссылки на скорлл_вью
	public ScrollRect RulesSc;
	public ScrollRect GenSc;
	public ScrollRect PropSc;
	public ScrollRect GenPropSc;

	// ссылки на сам класс каждого скролл_вью
	private ScrolViewScript RulesScroll;
	private ScrolViewScript GenScroll;
	private ScrolViewScript PropScroll; 
	private ScrolViewScript GenPropScroll; 

	//Компоненты редактора property
	public Image[] Menus;

	public InputField Name;
	public InputField Dis;
	private string currentlySavingItem;
	private int menuToReturn;

	public List<Rules> rules;

	private string _fileName;
	//Здесь первичная инициализация. Обрати внимание, что общую функциональность следует вынести в класс предок.
	public override void StartUp() 
	{
		if (GameManager.instance.getFieldType ()) 
		{
			int[] positions = new int[0];
			mainManager = new MainManager (24, 50, 1, positions, 0);
		} else 
		{
			int[] positions = {2, 8};
			mainManager = new MainManager (24, 50, 1, positions, 0);
		}
			
		rules = new List<Rules> ();
        int[] n = { 3, 1, 1, 1 };
        bool[] i = { true, false, false };
        string[] nam = { "Classic", "Dynam", "Collection" };
        string[] desc = { "1", "-", "-" };
        float[][] liveint = new float[1][];
        liveint[0] = new float[2];
        liveint[0][0] = 3;
        liveint[0][1] = 3;
        float[][] deadint = new float[1][];
        deadint[0] = new float[4];
        deadint[0][0] = -100;
        deadint[0][1] = 1;
        deadint[0][2] = 4;
        deadint[0][3] = 100;
        float[] power = { 1 };
        float[] liveP = { 0 };
        int[][] propNumP = new int[1][];
        propNumP[0] = new int[1];
        propNumP[0][0] = 0;
        //propNumP[0][1] = 1;
        // propNumP[0][2] = 2;
        //propNumP[1] = new int[0];
        int[][] propNum = new int[3][];
        propNum[0] = new int[1];
        propNum[0][0] = 0;
        propNum[1] = new int[0];
       // propNum[1][0] = 1;
        propNum[2] = new int[1];
        propNum[2][0] = 2;
        float[][] coefFr = new float[3][];
        coefFr[0] = new float[1];
        coefFr[0][0] = 1;
        coefFr[1] = new float[1];
        coefFr[1][0] = 1;
        coefFr[2] = new float[1];
        coefFr[2][0] = 1;
        float[][] coefEn = new float[3][];
        coefEn[0] = new float[1];
        coefEn[0][0] = -1;
        coefEn[1] = new float[1];
        coefEn[1][0] = -1;
        coefEn[2] = new float[1];
        coefEn[2][0] = -1;
        int[] funcT = { 0, 1, 2 };
        bool[] immort = { false, false };
        string[][] col = new string[1][];
        col[0] = new string[2];
        col[0][0] = "1";
        col[0][1] = "2";
        float[] livePD = { 0 };
        float[] setP = { 1 };
        string[] genNam = { "The first generation" };
        string[] genDsec = { "To test the output" };
        mainManager.initRules(n, i, nam, desc, liveint, deadint, power, liveP, livePD, setP, col, 1, genNam, genDsec, propNumP, immort, 3, propNum, coefFr, coefEn, funcT);
        //mainManager.createPlayers(1, 5, 0, 0);
        //Rules rules = mainManager.saveRules();
        //mainManager.loadRules(rules);

        RulesScroll = RulesSc.GetComponent<ScrolViewScript>();
		GenScroll = GenSc.GetComponent<ScrolViewScript>();
		PropScroll = PropSc.GetComponent<ScrolViewScript>();
	    GenPropScroll = GenPropSc.GetComponent<ScrolViewScript>();
		propertyEditor.setMainManager (mainManager);
		connectionEditor.setMainManager (mainManager);

        //обновим списки
        //RefreshRules ();
        //	RefreshGenerations ();
        //	RefreshPropertyes ();
        //	RefreshPropertyesOfGeneration ();
       
		LoadRules ();

		addedGenerations[0] = true;
		NumberOfActiveGenerations.Add(0);
		genButtons[0].GetComponent<ActiveGenerationItem>().Setup(0, mainManager.getGenerations()[0].getNameAndDescription()[0],
			mainManager.getGenerations()[0].getNameAndDescription()[1]);
		
		if (GameManager.instance.getFieldType ())
			PaintGrid ();
		else
			PaintHexGrid ();
		_fileName = Path.Combine (Application.persistentDataPath, "rules.txt");
    }

	//не обновляется таблиица добавленных покалений
	public void LoadRules() 
	{
		Generations = mainManager.getGenerations();
		Properties = mainManager.getGPT().getPropertiesList();
		addedGenerations = new List<bool>();
		addedProperties = new List<List<bool>>();
		NumberOfActiveGenerations = new List<int>();
		for (int it = 0; it < Generations.Count; it++)
		{
			print (it);
			addedGenerations.Add(false);
			List<bool> propsOfGeneration = new List<bool>();
			int jt = 0;
			if (Generations[it].getProperties().Count > 0)
			{
				int kt = 0;
				for (; jt < Properties.Count; jt++)
					if (Generations[it].getProperties()[kt] == Properties[jt])
					{
						propsOfGeneration.Add(true);
						kt++;
						if (kt == Generations[it].getProperties().Count)
							break;
					}
					else
						propsOfGeneration.Add(false);
			}
			for (; jt < Properties.Count; jt++)
				propsOfGeneration.Add(false);
			addedProperties.Add(propsOfGeneration);
		}


	}

	//Шаги
	public override void StepForward() {
		base.StepForward ();
		score.text = getScore ().ToString();
	}

	public void StartSteprs() {
		steps = true;
		InvokeRepeating ("StepForward", 0.2f, 0.5f);
	}



	public void StopSteps(){
		steps = false;
		CancelInvoke ();
	}

	//Используется для добавки поколения " В игру"
    public void AddGeneration()
    {
		if (currentlySelectedGeneration != -1) {
			
			if (NumberOfActiveGenerations.Count < 10 && !addedGenerations [currentlySelectedGeneration]) {
				print (currentlySelectedGeneration);
				NumberOfActiveGenerations.Add (currentlySelectedGeneration);
				genButtons [NumberOfActiveGenerations.Count - 1].interactable = true;
				genButtons [NumberOfActiveGenerations.Count - 1].image.overrideSprite = GenButtonSprites [0];
				genButtons [NumberOfActiveGenerations.Count - 1].image.color = genColors [NumberOfActiveGenerations.Count - 1];
				genButtons [NumberOfActiveGenerations.Count - 1].GetComponent<ActiveGenerationItem> ().Setup (currentlySelectedGeneration, mainManager.getGenerations () [currentlySelectedGeneration].getNameAndDescription () [0], 
					mainManager.getGenerations () [currentlySelectedGeneration].getNameAndDescription () [1]);
				addedGenerations [currentlySelectedGeneration] = true;
			}
		}
    }
    public int getNumberOfActiveGenerations() {
		return NumberOfActiveGenerations.Count;
    }

    /*public void setNumberOfActiveGenerations(int n)
    {
		NumberOfActiveGenerations = n;
    }*/

	//сеттеры текущих выбранных значений
	public void setCurrentRulesNumber(int N)
	{
		currentlySelectedRules = N;
	}
	public void setCurrentGenerationNumber(int N)
	{
		currentlySelectedGeneration = N;
	}
	public void setCurrentPropertyNumber(int N) 
	{
		currentlyselectedProperty = N;
	}
	public void setCurrentPropertyType(byte Type)
	{
		currentlyChosenPropertyType = Type;
		print (currentlyChosenPropertyType);
	}

	//обновление списков
	public void RefreshRules()
	{
		RulesScroll.RemoveItems ();
		RulesScroll.AddRules (rules);
	}

	public void RefreshGenerations()
	{
		print("Refreshed generations!");
		GenScroll.RemoveItems ();
        Debug.Log(Generations.Count);
		GenScroll.AddGenerations (Generations, addedGenerations);
	}

	public void RefreshPropertyes()
	{
		PropScroll.RemoveItems ();
		PropScroll.AddPropertys (Properties, addedProperties[currentlySelectedGeneration]);
	}

	public void RefreshPropertyesOfGeneration()
	{
		GenPropScroll.RemoveItems ();
		GenPropScroll.AddPropertyesOfGeneration (Generations [currentlySelectedGeneration].getProperties());
    }

	public void SetCurrentlySavingItem(string Type){
		currentlySavingItem = Type;
	}
	public void Add( )
	{
		switch (currentlySavingItem) 
		{
		    case "Generation":
            {
             	addedGenerations.Add(false);
             	List<bool> propsOfGeneration = new List<bool>();
             	for (int i = 0; i < Properties.Count; i++)
             		propsOfGeneration.Add(false);
             	addedProperties.Add(propsOfGeneration);
             	mainManager.createGenerationWithoutProp(Name.text, Dis.text);
                   // Generations = mainManager.getGenerations();
              	break;
			}
		    case "Property":
			{
			propertyEditor.SaveProperty ();
				for (int i = 0; i < addedProperties.Count; i++)
					addedProperties [i].Add (false);
			break;
			}
		case "Rules":
			{
				saveRules ();
				break;
			}

		}

	}

    public void editPropertyofDeneration()
    {
		if (currentlyselectedProperty != -1) {
			propEditPanel.GetComponent<popUpMenu> ().Open ();
			StartCoroutine ("openPropertytoEditCoroutine");
		}
    }

	public void editGeneration() 
	{
		if (currentlySelectedGeneration != -1) 
		{
			Menus [2].GetComponent<popUpMenu> ().Open ();
			Menus[1].GetComponent<popUpMenu> ().Close ();
			RefreshPropertyesOfGeneration ();
			isImmortal.isOn = Generations [currentlySelectedGeneration].getImmortale ();
		}
	}

	public IEnumerator  openPropertytoEditCoroutine()
	{
		yield return new WaitForFixedUpdate();
		propertyEditor.OpenPropertyToEdit(Generations[currentlySelectedGeneration].getProperties()[currentlyselectedProperty], currentlyChosenPropertyType);
	}

	public void editProperty()
	{
		if (currentlyselectedProperty != -1) {
			propEditPanel.GetComponent<popUpMenu> ().Open ();
			StartCoroutine ("openSynglePropertytoEditCoroutine");
		}
	}
	public IEnumerator  openSynglePropertytoEditCoroutine()
	{
		yield return new WaitForFixedUpdate();
		propertyEditor.OpenPropertyToEdit(Properties[currentlyselectedProperty], currentlyChosenPropertyType);
	}

	public void openProperetytoSetConnections(popUpMenu toOpen) {
		print (currentlyChosenPropertyType);
		if (currentlySelectedGeneration != -1 && currentlyselectedProperty != -1)
			if (currentlyChosenPropertyType != 2) {
				toOpen.Open ();
				Menus [0].GetComponent<popUpMenu> ().Close ();
				List<Property> editPropertyies = new List<Property>();
				for (int i = 0; i < Properties.Count; i++)
					if (Properties [i].getType () < 2)
						editPropertyies.Add (Properties [i]);
				connectionEditor.OpenPropertyToEditConnections (editPropertyies, Properties [currentlyselectedProperty]);
			}
	}


    public void addPropertyToGeneration()
    {
		if (currentlySelectedGeneration != -1 && currentlyselectedProperty != -1)
	        if (!addedProperties[currentlySelectedGeneration][currentlyselectedProperty])
	        {
	            mainManager.addPropertyToGeneration(currentlySelectedGeneration, currentlyselectedProperty);
	            addedProperties[currentlySelectedGeneration][currentlyselectedProperty] = true;
	            mainManager.correctGeneration(currentlySelectedGeneration);
	        }
    }

    public void removeQuickAcGeneration()
    {
		if (currentlySelectedGeneration != -1) 
		{
			addedGenerations [currentlySelectedGeneration] = false;
			int btnNum = NumberOfActiveGenerations.IndexOf (currentlySelectedGeneration);
			ShiftAcGenBtn (btnNum);
			NumberOfActiveGenerations.Remove (currentlySelectedGeneration);
			addGenBtn.transform.SetAsLastSibling ();
			addGenBtn.interactable = true;
		}
    }

    public void ShiftAcGenBtn(int RemovedBtn)
    {
		Color color = genColors[RemovedBtn];
        for (int i = RemovedBtn; i < NumberOfActiveGenerations.Count - 1; i++)
        {
            
            genColors[i] = genColors[i + 1];

            genButtons[i].image.color = genColors[i];
          
           
            if (!((i + 1) == NumberOfActiveGenerations.Count))
            {
                genButtons[i].GetComponent<ActiveGenerationItem>().Setup(NumberOfActiveGenerations[i + 1], mainManager.getGenerations()[NumberOfActiveGenerations[RemovedBtn + 1]].getNameAndDescription()[0],
                    mainManager.getGenerations()[NumberOfActiveGenerations[i + 1]].getNameAndDescription()[1]);
            }
        }
        genColors[NumberOfActiveGenerations.Count -1] = color;
        genButtons[NumberOfActiveGenerations.Count -1].image.overrideSprite = GenButtonSprites[1];
        genButtons[NumberOfActiveGenerations.Count -1].interactable = false;
    }

    public void deletePropertyFromGeneration()
    {
		if (currentlySelectedGeneration != -1 && currentlyselectedProperty != -1)
	        if (Generations[currentlySelectedGeneration].getProperties().Count > 0)
	        {
	           // Generations = mainManager.getGenerations();
	           // Properties = mainManager.getGPT().getPropertiesList();
	            //int propertyNumber = Properties.IndexOf(Generations[currentlySelectedGeneration].getProperties()[currentlyselectedProperty]);
	            int propertyNumber = mainManager.getGPT().getPropertiesList().IndexOf(Generations[currentlySelectedGeneration].getProperties()[currentlyselectedProperty]);
	            Debug.Log(mainManager.getGPT().getPropertiesList().Count);
	            addedProperties[currentlySelectedGeneration][propertyNumber] = false;
	            Generations[currentlySelectedGeneration].removeProperty(currentlyselectedProperty);
	            mainManager.correctGeneration(currentlySelectedGeneration);
				currentlyselectedProperty = 0;
	        }
    }

    public void deleteGeneration()
    {
		if (currentlySelectedGeneration != -1) {
			if (addedGenerations [currentlySelectedGeneration])
				removeQuickAcGeneration ();
			mainManager.deleteGeneration (currentlySelectedGeneration);
			addedProperties.RemoveAt (currentlySelectedGeneration);
		}
    }

    public void deleteProperty()
    {
		if (currentlySelectedGeneration != -1 && currentlyselectedProperty != -1) 
		{
			mainManager.deleteProperty (currentlyselectedProperty);
			for (int i = 0; i < Generations.Count; i++)
				addedProperties [i].RemoveAt (currentlyselectedProperty);
		}
    }

    public void killAllPoints()
    {
        mainManager.killAllPoints();
    }

	public void setMenuToReurn(int n)
	{
		menuToReturn = n;
	}

	public void Return()
	{
		Menus [menuToReturn].GetComponent<popUpMenu> ().Open ();
		RefreshGenerations ();
		if (currentlySelectedGeneration != -1 ) {
			RefreshPropertyes ();
			RefreshPropertyesOfGeneration ();	
		}
		if (menuToReturn == 2)
			menuToReturn = 1;
	}



	public int getCurrentlyActiveGenerationNum()
	{
		return currentlyActiveGenerationNum;
	}

	public void setCurrentlyActiveGenerationNum(int n)
	{
		currentlyActiveGenerationNum = n;
	}

	void PaintGrid()
	{
		for (int y = 0; y < 24; y++)
		{
			for (int x = 0; x < 50; x++)
			{
				float nx = (float)(-5.4 + x * 0.22);
				float ny = (float)(2.9 - y * 0.25);
				Position posit = new Position(y, x);
				Transform Cell = (Transform)Instantiate(CellPrefab, new Vector3(x, y, 0), Quaternion.identity);

				//Cell.GetComponent<CellSanbox>().setPointsManager(mainManager.getPointsManager());
				//Cell.GetComponent<CellSanbox>().setGens(mainManager.getGenerations());
				Cell.GetComponent<CellSanbox>().setPosition(posit);
				Cell.GetComponent<CellSanbox> ().setMainManager (mainManager);
				//Cell.GetComponent<CellSanbox>().setPlayersManager(mainManager.getPlayersManager());
				Cell.GetComponent<CellSanbox> ().setSxeneManager (this);


				Cell.SetParent(Parent);
				Vector3 pos = new Vector3(nx, ny, 0);

				Cell.transform.localPosition = pos;

			}
		}
	}
	void PaintHexGrid()
	{
		float n = 0.11f;
		for (int y = 0; y < 24; y++) {
			if (y % 2 == 0)
				n = 0;
			else
				n = 0.11f;
			for (int x = 0; x < 50; x++) {
				float nx = (float)(-5.4 + x * 0.22 + n);
				float ny = (float)(2.9 - y * 0.22);
				Position posit = new Position (y, x);
				Transform Cell = (Transform)Instantiate (HexCellPrefab, new Vector3 (x, y, 0), Quaternion.identity);

				//Cell.GetComponent<CellSanbox>().setPointsManager(mainManager.getPointsManager());
				//Cell.GetComponent<CellSanbox>().setGens(mainManager.getGenerations());
				Cell.GetComponent<CellSanbox> ().setPosition (posit);
				Cell.GetComponent<CellSanbox> ().setMainManager (mainManager);
				//Cell.GetComponent<CellSanbox>().setPlayersManager(mainManager.getPlayersManager());
				Cell.GetComponent<CellSanbox> ().setSxeneManager (this);


				Cell.SetParent (Parent);
				Vector3 pos = new Vector3 (nx, ny, 0);

				Cell.transform.localPosition = pos;
			}
		}
	}

	public Color getGenColor(int n) {
		return genColors [n];
	}

	public void setGenImmoratality() 
	{
		Generations [currentlySelectedGeneration].setImmortale(isImmortal.isOn);
	}

	public void saveRules()
	{
		Rules rl = new Rules ();
		rl.saveRules (mainManager);
		rl.setNameAndDis (Name.text, Dis.text);
		string r = JsonUtility.ToJson (rl);


		if (!File.Exists (_fileName)) {
			File.Create (_fileName);
			StreamWriter stream = new StreamWriter (_fileName);
			stream.WriteLine (r);
			stream.Close();
		} else 
		{
			StreamWriter stream = new StreamWriter (_fileName,true,System.Text.Encoding.Default);
			stream.WriteLine (r);
			stream.Close ();
		}
	}

	public void OpenRules() 
	{
		if (!File.Exists (_fileName)) 
		{
			Debug.Log ("No file");
			return;
		} else 
		{
			print (_fileName);
			StreamReader stream = new StreamReader(_fileName); 
			string r = "";
			int i = 0;
			rules.Clear ();
			while ((r = stream.ReadLine ()) != null) {
				rules.Add (JsonUtility.FromJson<Rules> (r));
			}
			RefreshRules ();
				//mainManager.loadRules (rules);

			//LoadRules ();
		/*	while ((r = stream.ReadLine ()) != null) 
			{
				rules = JsonUtility.FromJson<Dictionary<string,object>> (r);
				print((string)rules ["Name"]);
				print((string)rules["dis"]);
				MainManager newMain = (MainManager)rules ["mainManager"];
				mainManager = newMain;
				LoadRules ();
			}
			*/

		}

	}

	public void ApplyRules() 
	{
		RemoveAllActiveGenerations ();
		if (currentlySelectedRules != -1) {
			mainManager.loadRules (rules[currentlySelectedRules]);
			LoadRules ();
		}

	}
	public void RemoveAllActiveGenerations() 
	{
		addGenBtn.transform.SetAsLastSibling ();
		addGenBtn.interactable = true;
		for ( int i = 0 ; i < NumberOfActiveGenerations.Count; i++) {
			genButtons[i].image.overrideSprite = GenButtonSprites[1];
			genButtons[i].interactable = false;

		}

	}

}
