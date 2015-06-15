using UnityEngine;
using System.Collections;

// A destroyable object, creates a sound when destroyed
public class BreakableObject: MonoBehaviour 
{
    GameObject newSphere;
    GameObject brokenObject;
    float maxScale = 0.0f;
    public GameObject Sphere;
    public GameObject brokenSphere;
    public GameObject brokenCube;
    bool makeSound = false;
    public float timer = 60.0f;
    public float expireTimer = 10;
	// Use this for initialization
	void Start () 
    {
        // set the volume of the sound sphere for this object
        if (this.gameObject.tag == "ball")
            maxScale = 100.0f;
        else if (this.gameObject.tag == "cube")
            maxScale = 50.0f;
        else if (this.gameObject.tag == "Bone")
            maxScale = 25.0f;
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        // expands the sound sphere until maximum range

        if (this.gameObject.tag == "Bone")
        {
            if(timer <=0)
            {
            Debug.Log("Catch a bone");
            newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
            makeSound = false;
            timer += 60;
            if (newSphere)
            {
                newSphere.SendMessage("setMaxDiameter", maxScale,SendMessageOptions.DontRequireReceiver);
                expireTimer--;
            }
            }
            timer--;

        }
        if (expireTimer <= 0)
            Destroy(gameObject);
	    
	}
    void OnCollisionEnter(Collision other)
    {

        // When object falls to the ground it creates a sound sphere
        Debug.Log("Collision");
        if(makeSound)
        {
            if (this.transform.localPosition.y <= 1.0f)
            {
                newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
                makeSound = false;
                if (newSphere)
                {
                    newSphere.SendMessage("setMaxDiameter", maxScale, SendMessageOptions.DontRequireReceiver);
                    if (this.gameObject.tag == "ball")
                    {
                        brokenObject = (GameObject)Instantiate(brokenSphere, this.transform.localPosition, Quaternion.identity);
                        Destroy(this.gameObject);
                    }

                    if (this.gameObject.tag == "cube")
                    {
                        brokenObject = (GameObject)Instantiate(brokenCube, this.transform.localPosition, Quaternion.identity);
                        Destroy(this.gameObject);
                    }
                }
            }
            
            
        }

    }
    void ObjectFalling()
    {
        
        makeSound = true;
    }
}
