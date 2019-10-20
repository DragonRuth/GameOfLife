using UnityEngine;
using System.Collections;
using System;
public class NetworkManager : MonoBehaviour {
	private const string downaloadDataJson = "https://safe-sierra-17438.herokuapp.com/download";
	private const string downloadOne ="https://safe-sierra-17438.herokuapp.com/getOne";
	private const string uploadData = "https://safe-sierra-17438.herokuapp.com/upload";


	private bool IsResponseValid(WWW www){
		if (www.error != null)
		{
			Debug.Log ("basd connection");
			return false;
		}
		else if (string.IsNullOrEmpty(www.text) )
		{
			Debug.Log ("bad data");
			return false;
		} else {
			return true;
		}
			
	}

	private IEnumerator CallAPI(string url, Hashtable args, Action<string> callback) {

		WWW www;
		if (args == null) {
			www = new WWW (url);
		} else {
			WWWForm form = new WWWForm ();
			foreach (DictionaryEntry arg in args) {
				form.AddField (arg.Key.ToString (), arg.Value.ToString ());
			}
			www = new WWW (url, form);
		}
			
		yield return www;

		if (!IsResponseValid (www))
			yield break;
		callback (www.text);


	}

	public IEnumerator getSingleData(Action<string> callback) {
		return CallAPI (downloadOne,null, callback);
	}

	public IEnumerator DownloadData(Action<string> callback) {
		return CallAPI (downaloadDataJson, null, callback);
	}

	public IEnumerator LoadData(Action<string> callback) {
		return CallAPI (uploadData, null, callback);
	}

	public void StartDownload() {
		print("Hello!");
	}

}
