using UnityEngine;
using System.Collections;

public class instantiateDestructible : MonoBehaviour {
    public GameObject destructible;
    GameObject newDestructible;
	// Use this for initialization
	void Start () 
    {
        newDestructible = (GameObject)Instantiate(destructible, transform.localPosition, Quaternion.identity);
        newDestructible.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void checkpoint()
    {
        Destroy(newDestructible);
            newDestructible = (GameObject)Instantiate(destructible, transform.localPosition, Quaternion.identity);
            newDestructible.transform.parent = transform;
        
    }
}
