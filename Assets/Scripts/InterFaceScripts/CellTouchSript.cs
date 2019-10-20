using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using Classes.Game.PointsManagerSpace;
using Classes.GameClasses.PointSpace;
//using Classes.Game.GenerationsPropertiesTableSpace;
//using Classes.Game.PlayersManagerSpace;
using Classes.Game.MainManagerSpace;

public class CellTouchSript : MonoBehaviour {
	public Sprite[] sprites;
	protected SpriteRenderer spriteRenderer;
	protected bool stateAlive;
    //protected PointsManager pointManager;
    //protected List<Generation> gens;
    protected Position pos;
	protected MainManager mainManager;
    //protected PlayersManager players;
    protected Renderer renderer;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		stateAlive = false;
        renderer = GetComponent<Renderer>();
	}


    protected bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List <RaycastResult> results = new List <RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

  

    /*public void setPointsManager(PointsManager pnt)
    {
        pointManager = pnt;
    }

    public void setGens(List<Generation> g)
    {
        gens = g;
    }*/

    public void setPosition(Position p)
    {
        pos = p;
    }

	public void setMainManager(MainManager mm) 
	{
		mainManager = mm;
	}

    /*public void setPlayersManager(PlayersManager pm)
    {
        players = pm;
    }*/

}
