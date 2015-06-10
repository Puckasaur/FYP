using UnityEngine;
using System.Collections;


public class SoundSphere : MonoBehaviour 
{
    Vector3 scalingRate = new Vector3(1.0f, 1.0f, 1.0f);
    public float maxDiameter;
    Enemy hearSound;
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
        // if an enemy enters the sound sphere, this code sends a message to the enemy
        Debug.Log("Stuff");
        if (other.gameObject.tag == "looker")
        {
            hearSound = GameObject.FindObjectOfType(typeof(Enemy)) as Enemy;
            hearSound.SendMessage("hearSound");
        }
    }
    void setMaxDiameter(float value)
    {
        maxDiameter = value;
    }
    
    
}
