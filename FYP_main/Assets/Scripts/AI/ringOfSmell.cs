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
    public Vector3 scalingRate = new Vector3(1.0f, 0.0f, 1.0f);
    GameObject player;
    public ParticleSystem particle;
    RaycastHit hit;
	AudioSource sniff;
	public bool setToOff;
    public bool playerSeen = false;
    public bool disguised = false;
    public bool smellingPlayer = false;
    bool visualCueActive = false;
    public float detectionTimer;
    public float defaultDetectionRange;
    public float alarmBonus;
    float maxDifference = 0.2f;
    public bool smellDetected = false;
    public float sniffDistance;
    public float visualDistance;
    public float detectionDistance;
    public float somethingElseDistance;
    public Renderer rend;
    public Color maxColor;
    public Color color;
    public Color minColor;
    public float colorAlpha;
    Vector3 exitrange;
    float maximumDistance;
    
    void Start()
    {
        maxColor.a = colorAlpha;
        color.a = colorAlpha;
        minColor.a = colorAlpha;
        rend = GetComponent<Renderer>();
        radius = startRadius;
        sniff = GetComponent<AudioSource>();
        chaseTransScript = GameObject.Find ("BGM").GetComponent<chaseTransition>();
        detectionTimer = defaultDetectionRange;
        particle = GetComponent<ParticleSystem>();

    }
    void Update()
    {
        if (this.transform.localScale.x < radius - maxDifference)
        {
            this.transform.localScale += scalingRate;
        }

        else if (transform.localScale.x > radius + maxDifference)
        {
            transform.localScale -= scalingRate;
        }

        if (radius == maxRadius && color != Color.cyan) // Fat radius
        {
            rend.material.SetColor("_Color", maxColor);
            particle.startColor = new Color(maxColor.r,maxColor.g,maxColor.b);

        }

        else if (radius < maxRadius && radius > minRadius) // Any radius which 
        {
            rend.material.SetColor("_Color", color);
            particle.startColor = new Color(color.r, color.g, color.b);
        }

        else if (radius == minRadius)
        {
            rend.material.SetColor("_Color", minColor);
            particle.startColor = new Color(minColor.r, minColor.g, minColor.b);
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

        if (other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog")
        {
            Physics.Linecast(transform.parent.position, other.transform.position, out hit);
            Debug.DrawLine(transform.parent.position, other.transform.position, Color.cyan);
            if (hit.collider == other)
            {
                smellingPlayer = true;
                if (hit.distance <= sniffDistance)
                {
                }

                if (hit.distance <= detectionDistance)
                {
                    chaseTransScript.chaseTrans();

                    if (script != null)
                    {
                        smellDetected = true;
                    }
                    if (scriptFatDog != null)
                    {
                        smellDetected = true;
                    }
                    if (huntingDogScript != null)
                    {
                        huntingDogScript.stateManager(2);
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
        radius = 0;
        disguised = true;

    }
    public void isNotDisguised(string script)
    {
        radius = startRadius;
        disguised = false;
    }
    public void increaseSmell(float value)
    {
        if (radius < maxRadius) radius += value;
        if (radius > maxRadius-1 || radius > maxRadius) radius = maxRadius;
        
    }
    public void decreaseSmell(float value)
    {
        if (radius > minRadius) radius -= value;
        if (radius < minRadius+1 || radius < minRadius) radius = minRadius;
    }
}
