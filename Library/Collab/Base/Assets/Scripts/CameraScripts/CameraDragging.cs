using UnityEngine;
using System.Collections;

public class CameraDragging : MonoBehaviour {
	public Transform menu;
    // Use this for initialization
    public float minXPosition  = -2431; // left X border
    public float maxXPosition  = 3571; //  right X border
    public float minYPosition = -1393; // top Y border
    public float maxYPosition = 1990; //  top Y border
	private bool allowCameraDrag = false;
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && allowCameraDrag) {
			// Get movement of the finger since last frame
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            // Move object across XY plane
            float _xMove = transform.position.x - touchDeltaPosition.x;
            float _yMove = transform.position.y - touchDeltaPosition.y;
            //_xMove = Mathf.Clamp(_xMove, minXPosition, maxXPosition);
            //_yMove = Mathf.Clamp(_yMove, minYPosition, maxYPosition);
          //  transform.Translate(-touchDeltaPosition.x , -touchDeltaPosition.y , 0);
            menu.Translate(-touchDeltaPosition.x , -touchDeltaPosition.y , 0);
            //transform.position = new Vector3( Mathf.Clamp(transform.position.x, minXPosition, maxXPosition),transform.position.y,transform.position.z);
            //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minYPosition, maxYPosition), transform.position.z);
            transform.position = new Vector3(_xMove, _yMove, transform.position.z);

        } 

		//transform.position = new Vector3( Mathf.Clamp(transform.position.x, minXPosition, maxXPosition),transform.position.y,transform.position.z);
       // transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minYPosition, maxYPosition), transform.position.z);
    }

	public void setDragAllowence() {
		allowCameraDrag = !allowCameraDrag;
	}

}
