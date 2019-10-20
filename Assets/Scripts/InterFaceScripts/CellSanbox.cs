using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Classes.Game.PointsManagerSpace;
using Classes.GameClasses.PointSpace;
using Classes.Game.GenerationsPropertiesTableSpace;
using Classes.Game.PlayersManagerSpace;

public class CellSanbox : CellTouchSript {
	private SceneManager sceneManager;
	void OnMouseDown() {

		if (!IsPointerOverUIObject()  && GameManager.instance.isTouchAllowed() &&  sceneManager.getCurrentlyActiveGenerationNum () != -1)
		{
			if (!stateAlive)
			{
				spriteRenderer.sprite = sprites[1];
				mainManager.bringToLifePoint(1, sceneManager.getCurrentlyActiveGenerationNum (), pos);
				spriteRenderer.color = sceneManager.getGenColor (sceneManager.getCurrentlyActiveGenerationNum ());
				stateAlive = true;
			} else
            {
                spriteRenderer.sprite = sprites[0];
                mainManager.killPoint(pos);
                stateAlive = false;
            }
		}


	}
	void Update()
	{
		if (renderer.isVisible)
		{
			if (mainManager.checkLive (pos) > 0) 
			{
				///вот тут надо проверить, какого поколения клетка и рисовать ее соответсвенно ее поколению
				spriteRenderer.sprite = sprites[1];
				spriteRenderer.color = sceneManager.getGenColor (mainManager.getGeneratioPointsNumber (pos));
				stateAlive = true;
			}
			else
			{
				spriteRenderer.color = new Color (1.0f, 1.0f, 1.0f);
				spriteRenderer.sprite = sprites[0];
				stateAlive = false;
			}
		}
	}
	
	public void setSxeneManager(SceneManager scenMan) 
	{
		sceneManager = scenMan;
	}
}
