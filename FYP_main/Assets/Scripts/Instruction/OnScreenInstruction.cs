using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnScreenInstruction : MonoBehaviour {

	public GameObject textBox;
	public GameObject instruction1;

	public bool checkInputTrue = false;

	void Start()
	{
		textBox.SetActive(false);
		instruction1.SetActive(false);
	}

	void OnTriggerEnter(Collider instruction)
	{
		if (instruction.gameObject.tag == "player")
		{
			instruction1.SetActive(true);
			textBox.SetActive(true);
		}
	}

	void OnTriggerExit(Collider instruction)
	{
		if (instruction.gameObject.tag == "player")
		{
			instruction1.SetActive(false);
			textBox.SetActive(false);
		}
	}
}
