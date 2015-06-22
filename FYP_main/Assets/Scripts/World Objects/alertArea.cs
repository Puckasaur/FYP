using UnityEngine;
using System.Collections;

public class alertArea : MonoBehaviour {
    enemyPathfinding script;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnTriggerEnter(Collider Other)
    {
        if (Other.GetComponent<Collider>().tag == "enemy")
        
        {
            script = Other.gameObject.GetComponent<enemyPathfinding>();
            script.setAlertArea(this.gameObject); 
        }
               
    }
}
