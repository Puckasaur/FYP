using UnityEngine;
using System.Collections;

public class alertArea : MonoBehaviour {
    enemyPathfinding script;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "enemy")
        
        {
            script = other.gameObject.GetComponent<enemyPathfinding>();

            script.setAlertArea(this.gameObject); 
        }
               
    }
}
