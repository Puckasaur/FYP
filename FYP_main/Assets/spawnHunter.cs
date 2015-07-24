using UnityEngine;
using System.Collections;

public class spawnHunter : MonoBehaviour {
    public GameObject huntingDog;
    GameObject newDog;
    public int spawnedHunters;
    public bool spawnhunter;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(spawnhunter)
        {
            if (spawnedHunters <= 2)
            {
                newDog = (GameObject)Instantiate(huntingDog, this.transform.position, Quaternion.identity);
                newDog.transform.parent = transform;
                spawnedHunters++;
            }
        }
	}
    void OnTriggerEnter(Collider other)
{
        if (other.gameObject.tag == "soundSphere")
        {
            if (other.transform.parent.tag != "bone")
            {
                if (spawnedHunters <= 2)
                {
                    newDog = (GameObject)Instantiate(huntingDog, this.transform.position, Quaternion.identity);
                    newDog.transform.parent = transform;
                    spawnedHunters++;
                }
            }
        }
}
}
