using UnityEngine;
using System.Collections;
//-----------------------------------------------------//
// A destroyable object, creates a sound when destroyed//
//-----------------------------------------------------//
public class breakableObject: MonoBehaviour 
{
    GameObject newSphere;
    GameObject brokenObject;
    float maxScale = 0.0f;
    public GameObject Sphere;
    public GameObject brokenSphere;
    public GameObject brokenCube;
    public TemporaryMovement playerMovement;
    GameObject bone;
    public bool makeSound = false;
    public float timer = 60.0f;
    public float expireTimer = 10;
    public float boneRadius;
    public float ballRadius;
    public float cubeRadius;
    soundSphere sphereScript;
	void Start () 
    {
        //---------------------------------------------------//
        // set the volume of the sound sphere for this object//
        //---------------------------------------------------//
        if (this.gameObject.tag == "bottle")
            maxScale = 30.0f;
        else if (this.gameObject.tag == "glass")
            maxScale = 40.0f;
        else if(this.gameObject.tag == "jar")
        {
            maxScale = 50.0f;
        }
        playerMovement = GameObject.FindGameObjectWithTag("player").GetComponent<TemporaryMovement>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //---------------------------------------------//
        // expands the sound sphere until maximum range//
        //---------------------------------------------//

        if (this.gameObject.tag == "bone")
        {
            if(timer <=0)
            {
            
            makeSound = false;
            timer += 60;
                newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
                newSphere.transform.parent = transform;
                sphereScript = newSphere.GetComponent<soundSphere>();
                sphereScript.setMaxDiameter(boneRadius);
                expireTimer--;
            
            }
            timer--;

        }

		if (this.gameObject.tag == "bagOfAir")
		{
			if(timer <=0)
			{
				
				makeSound = false;
				timer += 60;
				newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
				newSphere.transform.parent = transform;
				sphereScript = newSphere.GetComponent<soundSphere>();
				sphereScript.setMaxDiameter(boneRadius);
				expireTimer--;
				
			}
			timer--;
			
		}

	}
    void LateUpdate()
    {
        if (expireTimer <= 0)
        {
            destroySelf();
        }
    }
    void OnCollisionEnter(Collision Other)
    {
        //----------------------------------------------------------//
        // When object falls to the ground it creates a sound sphere//
        //----------------------------------------------------------//
        //if(makeSound)
        if(Other.gameObject.tag == "player")
        {
            makeSound = true;
        }
        if (this.transform.position.y <= 0.5f && makeSound == true)
            {
                //makeSound = false;
                {
                    objectBreaking();
                }
            }
    }

    public void ObjectFalling()
    {    
        makeSound = true;
    }
    public void destroySelf()
    {
        if(gameObject.tag == "bone")
        {
            playerMovement.bonesPlaced--;
        }
        Destroy(gameObject);
    }
    public void objectBreaking()
    {
        if (this.gameObject.tag == "bottle")
        {
            brokenObject = (GameObject)Instantiate(brokenSphere, this.transform.position, Quaternion.identity);
            newSphere = (GameObject)Instantiate(Sphere, this.transform.position, Quaternion.identity);
            newSphere.transform.parent = brokenObject.transform;
            sphereScript = newSphere.GetComponent<soundSphere>();
            sphereScript.setMaxDiameter(maxScale);
            destroySelf();
        }

        if (this.gameObject.tag == "glass")
        {
            brokenObject = (GameObject)Instantiate(brokenCube, this.transform.localPosition, Quaternion.identity);
            newSphere = (GameObject)Instantiate(Sphere, this.transform.position, Quaternion.identity);
            newSphere.transform.parent = brokenObject.transform;
            sphereScript = newSphere.GetComponent<soundSphere>();
            sphereScript.setMaxDiameter(maxScale);
            destroySelf();
        }

        if (this.gameObject.tag == "jar")
        {
            brokenObject = (GameObject)Instantiate(brokenSphere, this.transform.position, Quaternion.identity);
            newSphere = (GameObject)Instantiate(Sphere, this.transform.position, Quaternion.identity);
            newSphere.transform.parent = brokenObject.transform;
            sphereScript = newSphere.GetComponent<soundSphere>();
            sphereScript.setMaxDiameter(maxScale);
            destroySelf();
        }
    }
}