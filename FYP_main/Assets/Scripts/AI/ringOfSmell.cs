using UnityEngine;
using System.Collections;

public class ringOfSmell : MonoBehaviour {
    enemyPathfinding script;
    //guardDog guard;
    float radius;
    public float startRadius;
    bool playerSeen = false;
    bool visualCueActive = false;
    Vector3 scalingRate = new Vector3(1.0f, 0.0f, 1.0f);
    public float detectionTimer = 60.0f;
    public float alarmBonus;
    GameObject player;
    RaycastHit hit;
    
    public float sniffDistance;
    public float visualDistance;
    public float detectionDistance;
	AudioSource sniff;
    
    void Start()
    {
        if (transform.parent.tag == "enemy")
        {
        script = this.transform.parent.GetComponent<enemyPathfinding>();
			sniff = GetComponent<AudioSource>();

    }
        //else if (transform.parent.tag == "guard")
        //{
        //    guard = transform.parent.GetComponent<guardDog>();
        //}
        //sniffDistance = radius;
        //visualDistance = radius - (radius / 4);
        //detectionDistance = radius / 4;
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
            if (script.States == enumStates.alert || script.States == enumStates.idleSuspicious || script.States == enumStates.chase)
            {
                radius = startRadius + alarmBonus;

            }
            else
            {
                radius = startRadius;

            }
        if (playerSeen)
        {

            Physics.Linecast(transform.parent.position, player.transform.position, out hit);
           // print(hit.collider);
            if (hit.collider == player.GetComponent<Collider>())
			{             
				if (script.States != enumStates.alert)
				{ transform.parent.LookAt(player.transform); }
				if(script.States == enumStates.alert || script.States == enumStates.idleSuspicious)
				{
					playerSeen = false;
				}

            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            player = other.gameObject;

                playerSeen = true;
                //transform.parent.LookAt(other.transform);
            }
        }

    void OnTriggerStay(Collider other)
    {
        
        //-----------------------------------------------------------------------//
        //if player crosses the cone, informs the parent(Enemy) of visible player//
        //-----------------------------------------------------------------------//
       // print(other.gameObject.tag);
        if (other.gameObject.tag == "player")
        {
            print("lol");
            detectionTimer--;

            if (detectionTimer <= 0)
            {
                detectionTimer = 60;
                
            }
            Physics.Raycast(transform.parent.position, player.transform.position, out hit);
            if(hit.distance <= sniffDistance)
            {

				if(!sniff.isPlaying)
				{
					sniff.Play();
				}
            }
            if(hit.distance <= visualDistance)
            {
                if (!gameObject.GetComponent<ParticleSystem>())
                {
                    gameObject.AddComponent<ParticleSystem>();
                    visualCueActive = true;
                }
            }
            if(hit.distance <= detectionDistance)
            {
                script.stateManager(2);
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "player")
        {

            //if (script.States != enumStates.chase && script.States != enumStates.alert)
            //{
            detectionTimer = 60.0f;

            if (visualCueActive)
            {
                Destroy(GetComponent<ParticleSystem>());
            }
            if (sniff.isPlaying)
            {
                sniff.Stop();
            }
            if (script.States != enumStates.chase)
            {
                script.areaCounter = 0;
                script.alertTimer = script.defaultAlertTimer;
                script.stateManager(3);
            }
            
        }
    }
}
