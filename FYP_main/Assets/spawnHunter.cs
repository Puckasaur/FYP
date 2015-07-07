using UnityEngine;
using System.Collections;

public class spawnHunter : MonoBehaviour {
    public GameObject huntingDog;
    GameObject newDog;
    public int spawnedHunters;
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
            if (spawnedHunters <= 2)
            {
                newDog = (GameObject)Instantiate(huntingDog, this.transform.position, Quaternion.identity);
                spawnedHunters++;
            }
        }
}
}
