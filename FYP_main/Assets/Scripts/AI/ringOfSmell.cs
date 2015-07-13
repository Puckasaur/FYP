//using UnityEngine;
//using System.Collections;

//public class ringOfSmell : MonoBehaviour {
//    enemyPathfinding script;
//    fatDogAi scriptFatDog;
//    huntingDog huntingDogScript;
//    chaseTransition chaseTransScript;
//    public float radius;
//    public float startRadius;
//    public bool playerSeen = false;
//    bool visualCueActive = false;
//    Vector3 scalingRate = new Vector3(1.0f, 0.0f, 1.0f);
//    public float detectionTimer = 60.0f;
//    public float alarmBonus;
//    GameObject player;
//    RaycastHit hit;
    
//    public float sniffDistance;
//    public float visualDistance;
//    public float detectionDistance;
//    public float somethingElseDistance;
//    AudioSource sniff;

//    public bool setToOff;
    
//    void Start()
//    {
//        chaseTransScript = GameObject.Find ("BGM").GetComponent<chaseTransition>();
//        radius = startRadius;
//        sniff = GetComponent<AudioSource>();
//    }

//    void Update()
//    {
//        if (this.transform.localScale.x < radius)
//        {
//            this.transform.localScale += scalingRate;
//        }

//        else if (transform.localScale.x > radius)
//        {
//            transform.localScale -= scalingRate;
//        }

//    }
    
//    void OnTriggerEnter(Collider other)
//    {

//        if (other.gameObject.tag == "enemy")
//        {
//            script = other.GetComponent<enemyPathfinding>();

//        }
//        else if(other.gameObject.tag == "huntingDog")
//        {
//            huntingDogScript = other.GetComponent<huntingDog>();
//=======
﻿using UnityEngine;
using System.Collections;

public class ringOfSmell : MonoBehaviour {
    enemyPathfinding script;
    fatDogAi scriptFatDog;
    huntingDog huntingDogScript;
    Vector3 scalingRate = new Vector3(1.0f, 0.0f, 1.0f);
    GameObject player;
    RaycastHit hit;
	AudioSource sniff;

	public bool setToOff;
    public bool playerSeen = false;
    bool visualCueActive = false;
    public float startRadius;
    public float detectionTimer = 60.0f;
    public float alarmBonus;
    GameObject player;
    RaycastHit hit;
   public bool smellDetected = false;
    public float sniffDistance;
    public float visualDistance;
    public float detectionDistance;
    public float somethingElseDistance;
	AudioSource sniff;
    public float radius;

    void Start()
    {
        radius = startRadius;
        sniff = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (this.transform.localScale.x < radius)
        {
            this.transform.localScale += scalingRate;

        }
        else if(other.gameObject.tag == "fatDog")
        {
            scriptFatDog = other.GetComponent<fatDogAi>();
        }
    }

    void OnTriggerStay(Collider other)
    {
        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
		if (setToOff == false) {
			if (other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog") {
				detectionTimer--;

				if (detectionTimer <= 0) {
					detectionTimer = 60;
				}
				Physics.Raycast (transform.parent.position, other.transform.position, out hit);
				if (hit.distance <= sniffDistance) {
					chaseTransScript.chaseTrans();
				}

//<<<<<<< Updated upstream
//                if (hit.distance <= detectionDistance) {
//                    if (script != null) {
//                        script.stateManager (2);
//                    } else if (scriptFatDog != null) {
//                        playerSeen = true;
//                        scriptFatDog.stateManager (2);
//                    } else if (huntingDogScript != null) {
//                        huntingDogScript.stateManager (2);
//                    }
//                }
//            }
//        }
//    }
//    void OnTriggerExit(Collider other)
//    {
//        if(other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog")
//        {

//            detectionTimer = 60.0f;
//            chaseTransScript.outChaseTrans();
//            if (visualCueActive)
//            {
//                Destroy(GetComponent<ParticleSystem>());
//            }
//            if (script != null)
//            {
//                if (script.States != enumStates.chase)
//                {
//                    script.areaCounter = 0;
//                    script.alertTimer = script.defaultAlertTimer;
//                    script.stateManager(3);
//                }
//            }
//            else if (scriptFatDog != null)
//            {
//                if (scriptFatDog.States != enumStatesFatDog.chase)
//                {                    

//                    scriptFatDog.stateManager(3);
//                }
//            }

            
            
//        }
//    }
//=======
   // }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            script = other.GetComponent<enemyPathfinding>();
            smellDetected = true;
        }
        else if(other.gameObject.tag == "huntingDog")
        {
            huntingDogScript = other.GetComponent<huntingDog>();
            smellDetected = true;
        }
        else if(other.gameObject.tag == "fatDog")
        {
            scriptFatDog = other.GetComponent<fatDogAi>();
            smellDetected = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
       
        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog")
        {           
            detectionTimer--;

            if (detectionTimer <= 0)
            {
                detectionTimer = 60;
            }
            Physics.Raycast(transform.parent.position, other.transform.position, out hit);
            if (hit.distance <= sniffDistance)
            {
                if (!sniff.isPlaying)
                {
                    sniff.Play();
                }
            }

            if (hit.distance <= detectionDistance)
            {
                if (script != null)
                {
                    //smellDetected = true;
                   // scriptFatDog.RotateDogWhileSmelling();
                }
                if (scriptFatDog != null)
                {
                    smellDetected = true;
                    //scriptFatDog.RotateDogWhileSmelling();
                }
                if (huntingDogScript != null)
                {
                   // smellDetected = true;
                  //  scriptFatDog.RotateDogWhileSmelling();
                }
            }
            if (hit.distance <= somethingElseDistance)
            {
                if (script != null)
                {
                    //script.stateManager(2);
                }
                if (scriptFatDog != null)
                {
                    playerSeen = true;
                    scriptFatDog.stateManager(2);
                }
                if (huntingDogScript != null)
                {
                   // huntingDogScript.stateManager(2);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog")
        {
            detectionTimer = 60.0f;
            if (visualCueActive)
            {
                Destroy(GetComponent<ParticleSystem>());
            }
            if (sniff.isPlaying)
            {
                sniff.Stop();
            }
            if (script != null)
            {
                if (script.States != enumStates.chase)
                {
                    script.areaCounter = 0;
                    script.alertTimer = script.defaultAlertTimer;
                    script.stateManager(3);
                }
            }
            else if (scriptFatDog != null)
            {
                if (scriptFatDog.States != enumStatesFatDog.chase)
                {                    
                    scriptFatDog.stateManager(3);
                }
            }
        }
        smellDetected = false;
    }
//>>>>>>> Stashed changes
}
