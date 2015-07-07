using UnityEngine;
using System.Collections;

public class spawnHunter : MonoBehaviour {
    public GameObject huntingDog;
    GameObject newDog;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
{
        if (other.gameObject.tag == "soundSphere")
        {
            newDog = (GameObject)Instantiate(huntingDog, this.transform.position, Quaternion.identity);
            
        }
}
}
