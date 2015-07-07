//<<<<<<< HEAD
﻿using UnityEngine;
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
    GameObject bone;
    bool makeSound = false;
    public float timer = 60.0f;
    public float expireTimer = 10;
    public float boneRadius;
    public float ballRadius;
    public float cubeRadius;
    soundSphere sphereScript;
	// Use this for initialization
	void Start () 
    {
        //---------------------------------------------------//
        // set the volume of the sound sphere for this object//
        //---------------------------------------------------//
        if (this.gameObject.tag == "ball")
            maxScale = 30.0f;
        else if (this.gameObject.tag == "cube")
            maxScale = 40.0f;
        else if (this.gameObject.tag == "bone")
            maxScale = 25.0f;
        
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
                //brokenObject = (GameObject)Instantiate(bone, this.transform.position, Quaternion.identity);
                newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
                newSphere.transform.parent = transform;
                sphereScript = newSphere.GetComponent<soundSphere>();
                sphereScript.setMaxDiameter(maxScale);
                expireTimer--;
            
            }
            timer--;

        }
        if (expireTimer <= 0)
        {
            Destroy(gameObject);
        }
	}
    void OnCollisionEnter(Collision Other)
    {
        //----------------------------------------------------------//
        // When object falls to the ground it creates a sound sphere//
        //----------------------------------------------------------//
        //if(makeSound)
        {
            if (this.transform.localPosition.y <= 0.5f)
            {
               // newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
                makeSound = false;
                //if (newSphere)
                //{
                    //sphereScript = newSphere.GetComponent<soundSphere>();
                    //sphereScript.setMaxDiameter(maxScale);
                    if (this.gameObject.tag == "ball")
                    {
                        brokenObject = (GameObject)Instantiate(brokenSphere, this.transform.position, Quaternion.identity);
                        newSphere = (GameObject)Instantiate(Sphere, this.transform.position, Quaternion.identity);
                        newSphere.transform.parent = brokenObject.transform;
                        sphereScript = newSphere.GetComponent<soundSphere>();
                        sphereScript.setMaxDiameter(maxScale);
                        Destroy(this.gameObject);
                    }

                    if (this.gameObject.tag == "cube")
                    {
                        brokenObject = (GameObject)Instantiate(brokenCube, this.transform.localPosition, Quaternion.identity);
                        newSphere = (GameObject)Instantiate(Sphere, this.transform.position, Quaternion.identity);
                        newSphere.transform.parent = brokenObject.transform;
                        sphereScript = newSphere.GetComponent<soundSphere>();
                        sphereScript.setMaxDiameter(maxScale);
                        Destroy(this.gameObject);
                    }
                //}
            }
            
            
        }

    }
    void ObjectFalling()
    {
        
        makeSound = true;
    }
}
//=======
//﻿using UnityEngine;
//using System.Collections;
////-----------------------------------------------------//
//// A destroyable object, creates a sound when destroyed//
////-----------------------------------------------------//
//public class breakableObject: MonoBehaviour 
//{
//    GameObject newSphere;
//    GameObject brokenObject;
//    float maxScale = 0.0f;
//    public GameObject Sphere;
//    public GameObject brokenSphere;
//    public GameObject brokenCube;
//    bool makeSound = false;
//    public float timer = 60.0f;
//    public float expireTimer = 10;
//    soundSphere sphereScript;
//    // Use this for initialization
//    void Start () 
//    {
//        //---------------------------------------------------//
//        // set the volume of the sound sphere for this object//
//        //---------------------------------------------------//
//        if (this.gameObject.tag == "ball")
//            maxScale = 100.0f;
//        else if (this.gameObject.tag == "cube")
//            maxScale = 50.0f;
//        else if (this.gameObject.tag == "bone")
//            maxScale = 25.0f;
        
//    }
	
//    // Update is called once per frame
//    void Update () 
//    {
//        //---------------------------------------------//
//        // expands the sound sphere until maximum range//
//        //---------------------------------------------//

//        if (this.gameObject.tag == "bone")
//        {
//            if(timer <=0)
//            {
//            newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
//            makeSound = false;
//            timer += 60;
//            if (newSphere)
//            {
//                sphereScript = newSphere.GetComponent<soundSphere>();
//                sphereScript.setMaxDiameter(maxScale);
//                expireTimer--;
//            }
//            }
//            timer--;

//        }
//        if (expireTimer <= 0)
//        {
//            Destroy(gameObject);
//        }
//    }
//    void OnCollisionEnter(Collision Other)
//    {
//        //----------------------------------------------------------//
//        // When object falls to the ground it creates a sound sphere//
//        //----------------------------------------------------------//
//        if(makeSound)
//        {
//            if (this.transform.localPosition.y <= 1.0f)
//            {
//                newSphere = (GameObject)Instantiate(Sphere, this.transform.localPosition, Quaternion.identity);
//                makeSound = false;
//                if (newSphere)
//                {
//                    sphereScript = newSphere.GetComponent<soundSphere>();
//                    sphereScript.setMaxDiameter(maxScale);
//                    if (this.gameObject.tag == "ball")
//                    {
//                        brokenObject = (GameObject)Instantiate(brokenSphere, this.transform.localPosition, Quaternion.identity);
//                        Destroy(this.gameObject);
//                    }

//                    if (this.gameObject.tag == "cube")
//                    {
//                        //brokenObject = (GameObject)Instantiate(brokenCube, this.transform.localPosition, Quaternion.identity);
//                        Destroy(this.gameObject);
//                    }
//                }
//            }
            
            
//        }

//    }
//    void ObjectFalling()
//    {
        
//        makeSound = true;
//    }
//}
//>>>>>>> origin/Toni-prototype1
