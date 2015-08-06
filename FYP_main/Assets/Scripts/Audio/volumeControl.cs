using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class volumeControl : MonoBehaviour {

	float volControl;

	Slider volSlider;

	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
		volControl = volSlider.value;

	}
}
