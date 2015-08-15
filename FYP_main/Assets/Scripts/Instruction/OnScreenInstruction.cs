using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnScreenInstruction : MonoBehaviour {

	public GameObject chatBox;
	public GameObject instruction1;

	//public GameObject instructionForPlayer;
	
	void Start()
	{	
		chatBox.SetActive(false);
		instruction1.SetActive(false);
		//instructionForPlayer.SetActive(false);

	}

	void OnTriggerEnter(Collider instruction)
	{
		if (instruction.gameObject.tag == "player")
		{
			instruction1.SetActive(true);
			chatBox.SetActive(true);
			//instructionForPlayer.SetActive(true);
				

			StartCoroutine(FadeIn());
		}
	}

	void OnTriggerExit(Collider instruction)
	{
		if (instruction.gameObject.tag == "player")
		{
			StartCoroutine(FadeOut());

			//instruction1.SetActive(false);
			//chatBox.SetActive(false);
		}
	}

	IEnumerator FadeOut()
	{	
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.8f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
		
		yield return new WaitForSeconds(0.03f);
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.6f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
		
		yield return new WaitForSeconds(0.03f);
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.4f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
		
		yield return new WaitForSeconds(0.03f);
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.2f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
		
		yield return new WaitForSeconds(0.03f);
		
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.0f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.0f);
		
		instruction1.SetActive(false);
		//instructionForPlayer.SetActive(false);
		
	}
	
	IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(0.03f);
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.2f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
		
		yield return new WaitForSeconds(0.03f);
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.4f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
		
		yield return new WaitForSeconds(0.03f);
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.6f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.6f);
		
		yield return new WaitForSeconds(0.03f);
		
		instruction1.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.8f);
		chatBox.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
		
		yield return new WaitForSeconds(0.03f);
	}

	IEnumerator delay()
	{
		yield return new WaitForSeconds(1);
	}
}
