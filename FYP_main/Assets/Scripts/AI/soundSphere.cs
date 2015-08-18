using UnityEngine;
using System.Collections;


public class soundSphere : MonoBehaviour 
{
    enemyPathfinding script;
    Vector3 scalingRate = new Vector3(1.0f, 0.125f, 1.0f);
    public float maxDiameter;
    RaycastHit hit;
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
        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "fatDog")
        {
            script = other.GetComponent<enemyPathfinding>();
            if (script != null)
            {
                if (this.transform.parent != other.transform)
                    if (script.States != enumStates.chase && script.States != enumStates.distracted && script.States != enumStates.eatBone)
                    {
                        if (script.soundSource)
                        {
                            if (script.soundSource.tag != "enemy" || script.soundSource.tag != "fatDog")
                            {
                                if (transform.parent.gameObject.tag == "enemy" || transform.parent.gameObject.tag == "fatDog")
                                {
                                    script.stateManager(6);
                                    script.soundSource = transform.parent.gameObject;
                                }
                            }

                        }
                        else if (script.soundSource == null)
                        {
                            Physics.Linecast(transform.parent.position,other.transform.position,out hit);
                            if(transform.parent.gameObject.tag == "enemy" || transform.parent.gameObject.tag == "fatDog" || hit.collider.tag == "enemy" || hit.collider.tag == "fatDog")
                            {
                            script.stateManager(6);
                            script.soundSource = transform.parent.gameObject;
                            }
                        }
                        if (transform.parent.gameObject != null)
                        {
                            
                            script._soundSource = transform.parent.gameObject.transform.position;
                           
                        } 
                    }
            }
            fatDogAi fatDogScript = other.GetComponent<fatDogAi>();
            if (fatDogScript != null)
            {
                if (this.transform.parent != other.transform)
                    if (fatDogScript.States != enumStatesFatDog.chase && fatDogScript.States != enumStatesFatDog.distracted && fatDogScript.States != enumStatesFatDog.eatBone)
                    {
                        
                        if (fatDogScript.soundSource)
                        {
                            if (fatDogScript.soundSource.tag != "enemy" || fatDogScript.soundSource.tag != "fatDog")
                            {
                                if (transform.parent.gameObject.tag == "enemy" || transform.parent.gameObject.tag == "fatDog")
                                {
                                    fatDogScript.stateManager(6);
                                    fatDogScript.soundSource = transform.parent.gameObject;
                                }
                            }

                        }
                        else if (fatDogScript.soundSource == null)
                        {
                            Physics.Linecast(transform.parent.position, other.transform.position, out hit);
                            if (transform.parent.gameObject.tag == "enemy" || transform.parent.gameObject.tag == "fatDog" || hit.collider.tag == "enemy" || hit.collider.tag == "fatDog")
                            {
                                fatDogScript.stateManager(6);
                                fatDogScript.soundSource = transform.parent.gameObject;
                            }
                        }
                        //fatDogScript.stateManager(6);
                        //fatDogScript.soundSource = transform.parent.gameObject;
                    }
            }
        }
    }
    public void setMaxDiameter(float value)
    {
        maxDiameter = value;
    }
    
    
}
