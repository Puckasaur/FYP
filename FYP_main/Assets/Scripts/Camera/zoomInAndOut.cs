using UnityEngine;
using System.Collections;

public class zoomInAndOut : MonoBehaviour {

	public int zoom = 20;
	public int normal = 60;
	public float smooth = 5;

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Mouse ScrollWheel") > 0.001) {
			//Debug.Log ("asd");
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView,zoom,Time.deltaTime*smooth);
		}

		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
		//	Debug.Log ("dsa");
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView,normal,Time.deltaTime*smooth);
			
		}
	}
}
