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
<<<<<<< HEAD
    void OnTriggerEnter(Collider Other)
    {
        if (Other.GetComponent<Collider>().tag == "enemy")
        
        {
            script = Other.gameObject.GetComponent<enemyPathfinding>();
=======
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "enemy")
        
        {
            script = other.gameObject.GetComponent<enemyPathfinding>();
>>>>>>> origin/Toni_Sound&Vision
            script.setAlertArea(this.gameObject); 
        }
               
    }
}
