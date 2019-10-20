using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using Classes.Game.PointsManagerSpace;
using Classes.GameClasses.PointSpace;
//using Classes.Game.GenerationsPropertiesTableSpace;
//using Classes.Game.PlayersManagerSpace;

public class CellPVC : CellTouchSript {
	void OnMouseDown() {

		if (!IsPointerOverUIObject()  && GameManager.instance.isTouchAllowed())
		{
			if (!stateAlive) 
			{
				spriteRenderer.sprite = sprites [1];
				stateAlive = mainManager.bringToLifePointPV (1, 0, pos);
			}
		}


	}
	void Update()
	{
		if (renderer.isVisible)
		{
			if (mainManager.checkLive (pos) == 1)
			{
				spriteRenderer.sprite = sprites [1];
				stateAlive = true;
			} else if (mainManager.checkLive (pos) == 2)
			{
				spriteRenderer.sprite = sprites [2];
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
}
