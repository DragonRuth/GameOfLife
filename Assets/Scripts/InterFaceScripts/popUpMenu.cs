using UnityEngine;
using System.Collections;

public class popUpMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	public void Close() {
		gameObject.SetActive (false);
	}
	public void Open() {
		gameObject.SetActive (true);
	}

}
