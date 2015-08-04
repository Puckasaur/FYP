using UnityEngine;
using System.Collections;

public class startMovie : MonoBehaviour {

    public string movieFolder;
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

            moviePlayer mp = GameObject.FindGameObjectWithTag("moviePlayer").GetComponent<moviePlayer>();
            mp.endOfLevel(movieFolder);
        }
    }
}
