using UnityEngine;
using System.Collections;


public class soundSphere : MonoBehaviour 
{
    enemyPathfinding script;

    Vector3 scalingRate = new Vector3(1.0f, 1.0f, 1.0f);
    public float maxDiameter;
	// Use this for initialization
	void Start () 
    {


	}
	
	// Update is called once per frame
	void Update () 
    {
        this.transform.localScale += scalingRate;
        if (this.transform.localScale.x >= maxDiameter)
            Destroy(this.gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
        //----------------------------------------------------------------------------//
        // if an enemy enters the sound sphere, this code sends a message to the enemy//
        //----------------------------------------------------------------------------//
        if (other.gameObject.tag == "enemy")
        {

			script = other.GetComponent<enemyPathfinding>();
            if (this.transform.parent != other.transform && script.States != enumStates.chase)
            {
                script.escapeTimer = 0;
                script.stateManager(6);
				script.soundSource = transform.parent.gameObject;
            }
        }
    }
    public void setMaxDiameter(float value)
    {
        maxDiameter = value;
    }
    
    
}
