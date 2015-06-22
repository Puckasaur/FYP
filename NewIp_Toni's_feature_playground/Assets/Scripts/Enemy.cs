using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public float speed = 1.0f;
    public float turnSpeed = 2.0f;
    bool lookForSound = false;
	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(lookForSound)
        {
            GameObject brokenObject = GameObject.FindGameObjectWithTag("Broken Object");//FindObjectOfType<GameObject>();
            if (brokenObject.tag == "Broken Object")
            { 
                Vector3 dir = (brokenObject.transform.localPosition) - (this.transform.localPosition);
                transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
                this.GetComponent<Rigidbody>().AddForce(dir * speed);
            }
            if ((brokenObject.transform.localPosition.x - this.transform.localPosition.x <= 2) && (brokenObject.transform.localPosition.x - this.transform.localPosition.x >= -2) && (brokenObject.transform.localPosition.z - this.transform.localPosition.z <= 2) && (brokenObject.transform.localPosition.z - this.transform.localPosition.z >= -2))
                patrol();
        }
        
        
	}

    public void hearSound()
    {
        Debug.Log("WhoGoesThere!");
        lookForSound = true;
        
        //change state to search
    }
    public void playerSpotted()
    {
        Vector3 Player_direction = (FindObjectOfType<CharacterController>().transform.localPosition)-(this.transform.localPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player_direction), turnSpeed * Time.deltaTime);
        Debug.Log("Found You!");
        this.GetComponent<Rigidbody>().AddForce(Player_direction * speed);
        //change state to chase
    }
    public void patrol()
    {
        //change state to patrol
        lookForSound = false;
    }
}