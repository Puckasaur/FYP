using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    public float speed = 1.0f;
    public float turnSpeed = 2.0f;
	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () 
    {
     
        
        
	}

    public void hearSound()
    {
        Debug.Log("WhoGoesThere!");
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

    }
}