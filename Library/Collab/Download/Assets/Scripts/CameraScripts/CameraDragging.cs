using UnityEngine;
using System.Collections;

public class CameraDragging : MonoBehaviour {
	
	// Use this for initialization
	private float minXPosition = -651f ; // left X border
	private float maxXPosition = 1781f ; //  right X border
	private float minYPosition = 886f; // bottom Y border
	private float maxYPosition = -201f; //  top Y border
	private bool allowCameraDrag = false;
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && allowCameraDrag) {
			// Get movement of the finger since last frame
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;


			float _xMove = transform.position.x - touchDeltaPosition.x;
			float _yMove = transform.position.y - touchDeltaPosition.y; 
			_xMove = Mathf.Clamp(_xMove, minXPosition, maxXPosition); 
			_yMove = Mathf.Clamp(_yMove, maxYPosition, minYPosition); 
			// Move object across XY plane
			transform.position = new Vector3(_xMove, _yMove, transform.position.z);
			//if ((transform.position.x < maxXPosition) && (transform.position.x > minXPosition) && (transform.position.y < maxYPosition) && (transform.position.y > minYPosition)) {
			//	transform.Translate (-touchDeltaPosition.x, -touchDeltaPosition.y, 0);
			//} 
			//if (transform.position.x > maxXPosition)
			//	transform.position = new Vector3 (maxXPosition - 20, transform.position.y, transform.position.z);
			//if (transform.position.x < minXPosition)
		//		transform.position = new Vector3 (minXPosition + 20, transform.position.y, transform.position.z);
		//	if (transform.position.y > maxYPosition)
		//		transform.position = new Vector3 (transform.position.x, maxYPosition - 20, transform.position.z);
		//	if (transform.position.y < minYPosition)
		//		transform.position = new Vector3 (transform.position.x, minYPosition + 20, transform.position.z);
			//menu.Translate(-touchDeltaPosition.x , -touchDeltaPosition.y , 0);
			//transform.position = new Vector3( Mathf.Clamp(transform.position.x, minXPosition, maxXPosition),transform.position.y,transform.position.z);
			//transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minYPosition, maxYPosition), transform.position.z);
		}

		//transform.position = new Vector3( Mathf.Clamp(transform.position.x, minXPosition, maxXPosition),transform.position.y,transform.position.z);
		// transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minYPosition, maxYPosition), transform.position.z);
	}

	public void setDragAllowence() {
		allowCameraDrag = !allowCameraDrag;
	}

	public void cameraZoom(int i){
		if (i < 0) {
			minXPosition += 213;
			maxXPosition -= 213;
			minYPosition -= 94;
			maxYPosition += 94;

		} else {
			minXPosition -= 213;
			maxXPosition += 213;
			minYPosition += 94;
			maxYPosition -= 94;

		}
	}

	public void cameraFix() {
		float xPos = transform.position.x;
		float yPos = transform.position.y;
		xPos = Mathf.Clamp(xPos, minXPosition, maxXPosition); 
		yPos = Mathf.Clamp(yPos, maxYPosition, minYPosition); 	
		transform.position = new Vector3(xPos, yPos, transform.position.z);
	}

}
