using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	
	public string loadLevel;
	
	public GameObject Background;
	public GameObject Text;
	public GameObject ProgressBar;
	
	private int loadProgress = 0;

	private moviePlayer mp;
	
	// Use this for initialization
	void Start () {
		Background.SetActive(false);
		ProgressBar.SetActive(false);
		Text.SetActive(false);

		mp = GameObject.Find ("Plane").GetComponent<moviePlayer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mp.movieDone)
		{
			StartCoroutine(DisplayLoadingScreen());
			mp.movieDone = false;
		}
	}
	
	IEnumerator DisplayLoadingScreen()
	{
		Background.SetActive(true);
		ProgressBar.SetActive(true);
		Text.SetActive(true);
		
		ProgressBar.transform.localScale = new Vector3(loadProgress, 
		                                               ProgressBar.transform.localScale.y, 
		                                               ProgressBar.transform.localScale.z);
		
		Text.GetComponent<GUIText> ().text = "Loading Progress" + loadProgress + "%";
		
		AsyncOperation ao = Application.LoadLevelAsync(""); 
		
		while (ao.isDone != true)
		{
			loadProgress = (int) (ao.progress * 100);
			Text.GetComponent<GUIText> ().text = "Loading Progress" + loadProgress + "%";
			ProgressBar.transform.localScale = new Vector3(ao.progress, 
			                                               ProgressBar.transform.localScale.y, 
			                                               ProgressBar.transform.localScale.z);
			
			yield return null;
		}
		
	}
}
