using UnityEngine;
using System.Collections;

public class startMovie : MonoBehaviour {

    public string movieFolder;
    public int nextScene;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            PlayerPrefs.SetInt("Scene", nextScene);
            PlayerPrefs.SetString("Movie", movieFolder);
            PlayerPrefs.Save();
            Application.LoadLevel(1);
        }
    }
}
