using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ringOfSmell : MonoBehaviour {

    chaseTransition chaseTransScript;
    enemyPathfinding script;
    fatDogAi scriptFatDog;
    huntingDog huntingDogScript;
    public float radius;
    public float startRadius;
    public float maxRadius;
    public float minRadius;

    Vector3 scalingRate = new Vector3(1.0f, 0.0f, 1.0f);
    GameObject player;
    RaycastHit hit;
	AudioSource sniff;

    //public List<GameObject> enemies = new List<GameObject>();

	public bool setToOff;
    public bool playerSeen = false;
    public bool disguised = false;
    public bool smellingPlayer = false;
    bool visualCueActive = false;
    public float detectionTimer;
    public float defaultDetectionRange;
    public float alarmBonus;

    public bool smellDetected = false;

    public float sniffDistance;
    public float visualDistance;
    public float detectionDistance;
    public float somethingElseDistance;
	//AudioSource sniff;

    //Vector3 to make sure the enemy does not smell the player when the player exits the ring of smell
    Vector3 exitrange;
    float maximumDistance;
    
    void Start()
    {
        radius = startRadius;
        sniff = GetComponent<AudioSource>();
        chaseTransScript = GameObject.Find ("BGM").GetComponent<chaseTransition>();
        detectionTimer = defaultDetectionRange;
       // disGuiseAsDog();

    }

    void Update()
    {
        if (this.transform.localScale.x < radius)
        {
            this.transform.localScale += scalingRate;
        }

        else if (transform.localScale.x > radius)
        {
            transform.localScale -= scalingRate;
        }
    }

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
        else if(other.gameObject.tag == "looker")
        {
            if (other.transform.parent.tag == "enemy")
            {
                script = other.transform.parent.GetComponent<enemyPathfinding>();
                smellDetected = true;
            }
            else if (other.transform.parent.tag == "huntingDog")
            {
                huntingDogScript = other.transform.parent.GetComponent<huntingDog>();
                smellDetected = true;
            }
            else if (other.transform.parent.tag == "fatDog")
            {
                scriptFatDog = other.transform.parent.GetComponent<fatDogAi>();
                smellDetected = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {

        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//  
        if(other.gameObject.tag == "looker")
        {
            ////print("hitting enemy ring");
            if(script != null)
            {
                script.stateManager(2);
            }
            if(huntingDogScript != null)
            {
                huntingDogScript.stateManager(2);
            }
            if(scriptFatDog != null)
            {
                scriptFatDog.stateManager(2);
            }
        }
        else if (other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog")
        {
            //detectionTimer--;

            //if (detectionTimer <= 0)
            //{
            //    detectionTimer = 60;
            //}
            Physics.Linecast(transform.parent.position, other.transform.position, out hit);
            Debug.DrawLine(transform.parent.position, other.transform.position, Color.cyan);
            if (hit.collider == other)
            {
                smellingPlayer = true;
                if (hit.distance <= sniffDistance)
                {

                    //                if (!sniff.isPlaying)
                    //                {
                    //                    sniff.Play();
                    //                }
                }

                if (hit.distance <= detectionDistance)
                {
                    chaseTransScript.chaseTrans();

                    if (script != null)
                    {
                        // script.stateManager(2);
                        smellDetected = true;
                        // scriptFatDog.RotateDogWhileSmelling();
                    }
                    if (scriptFatDog != null)
                    {
                        smellDetected = true;
                        //scriptFatDog.RotateDogWhileSmelling();
                    }
                    if (huntingDogScript != null)
                    {
                        huntingDogScript.stateManager(2);
                        // smellDetected = true;
                        //  scriptFatDog.RotateDogWhileSmelling();
                    }
                }
                if (hit.distance <= somethingElseDistance)
                {
                    if (script != null)
                    {
                        script.stateManager(2);
                    }
                    if (scriptFatDog != null)
                    {
                        playerSeen = true;
                        scriptFatDog.stateManager(2);
                    }
                    if (huntingDogScript != null)
                    {
                        huntingDogScript.stateManager(2);
                    }
                }
            }
            else
            {                 
                exitrange = other.transform.position - transform.parent.position;

                if(exitrange.x > maximumDistance || exitrange.y > maximumDistance  || exitrange.z > maximumDistance )
                {
                playerSeen = false;
                }
                // transform.parent.position, other.transform.position
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog")
        {
            if (smellingPlayer)
            {
                smellingPlayer = false;
                detectionTimer = 60.0f;
                chaseTransScript.outChaseTrans();

                if (visualCueActive)
                {
                    Destroy(GetComponent<ParticleSystem>());
                }
                //            if (sniff.isPlaying)
                //            {
                //                sniff.Stop();
                //            }
                if (script != null)
                {
                    if (script.States != enumStates.chase)
                    {
                        if (script.turnTowardsSmellTimer <= 0)
                        {
                            script.tempSmellPosition = script.player.transform.position;
                            script.continueRotation = true;
                            script.stateManager(8);
                        }
                        else
                        {
                            return;
                        }
                    smellDetected = false;
                    script.turnTowardsSmellTimer = script.defaultTurnTowardsSmellTimer;
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
        }
        
        
    }
    public void isDisguised(string script)
    {
        ////print(script);
        ////print("1");
        radius = 0;
        disguised = true;

    }
    public void isNotDisguised(string script)
    {
        ////print(script);
        ////print("2");
        radius = startRadius;
        disguised = false;
    }
    public void increaseSmell(float value)
    {
        ////print("3");
        //radius = startRadius;
        if (radius < maxRadius) radius += value;
        if (radius > maxRadius-1 || radius > maxRadius) radius = maxRadius;
        
    }
    public void decreaseSmell(float value)
    {
        ////print("4");
        //radius = startRadius;
        if (radius > minRadius) radius -= value;
        if (radius < minRadius+1 || radius < minRadius) radius = minRadius;
        //radius = Mathf.Floor(radius);
    }
}
