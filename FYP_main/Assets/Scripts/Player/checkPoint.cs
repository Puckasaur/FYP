/* 
 * To put inside player's gameObject.
 * respawnPosition = variable that needs to be added in the enemy script.
 * An empty gameObject set at the position of the check point has to be added to the scene.
 * A trigger has to be added to the scene to turn the check point on with a box collider (trigger set to on) and tag == "checkPoint".
*/

using UnityEngine;
using System.Collections;

public class checkPoint: MonoBehaviour
{
	chaseTransition chaseTransScript;
    public GameObject checkPointPosition; // Position of the check point
    public GameObject checkPointPosition_2; // Position of the check point
    public Transform startPosition;
    public bool checkPointActivated = false; // if the check point has been reached or not
    public bool checkPointActivated_2 = false;

    private string currentLevel; // the current level
    public  GameObject[] allEnemies; // needed to reset enemies' positions
    private GameObject[] allHunters; // Hunters need to be destroyed on player death
    private GameObject[] allFatDogs;
    public GameObject[] allKeys;
    public GameObject[] allDoors;
    public GameObject[] allDestructibles;
    public GameObject[] allBones;
    public GameObject[] allSpheres;

    public enemyPathfinding script;
    public huntingDog hunterScript;
    public fatDogAi fatDogScript;
    public TemporaryMovement playerScript;
    public GameObject player;
    public breakableObject bo;

	public bool sendBack;

    void Start()
    {	
		sendBack = false;
        currentLevel = Application.loadedLevelName; // get current level name
		chaseTransScript = GameObject.Find ("BGM").GetComponent<chaseTransition>();//get chase music transition script
    }

    void OnTriggerEnter(Collider other) // turns the check point on
    {
        if (other.gameObject.name == "checkPoint_Trigger")
        {
            this.checkPointActivated = true;
        }
        if (other.gameObject.name == "checkPoint_Trigger_2")
        {
            this.checkPointActivated_2 = true;
        }
    }

    void OnCollisionEnter(Collision other) // On collision with an enemy
    {
        if ((other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog") && checkPointActivated == false && checkPointActivated_2 == false ) // if check point has not been reached
        {
            if (other.gameObject.tag == "enemy")
            {
                other.gameObject.GetComponent<enemyPathfinding>().agent.velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }

            if (other.gameObject.tag == "fatDog")
            {
                other.gameObject.GetComponent<fatDogAi>().agent.velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }

            this.transform.position = startPosition.transform.position;
            resetLevel();
            //Application.LoadLevel(currentLevel);
        }

        else if ((other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog") && (checkPointActivated == true || checkPointActivated_2 == true)) // if check point has been reached
        {
            if (other.gameObject.tag == "enemy")
            {
                other.gameObject.GetComponent<enemyPathfinding>().agent.velocity = Vector3.zero;
            }
            if (checkPointActivated_2)
            {
                this.transform.position = checkPointPosition_2.transform.position;
            }
            if (checkPointActivated)
            {
                this.transform.position = checkPointPosition.transform.position;
            } 
            resetLevel();
        }
    }

    public static bool isNull(System.Object aObj)
    {
        return aObj == null || aObj.Equals(null);
    }

    void resetLevel()
    {
        sendBack = true;
        playerScript = player.GetComponent<TemporaryMovement>();
        playerScript.resetKeys();
        allEnemies = GameObject.FindGameObjectsWithTag("enemy");
        allHunters = GameObject.FindGameObjectsWithTag("huntingDog");
        allKeys = GameObject.FindGameObjectsWithTag("key");
        allDoors = GameObject.FindGameObjectsWithTag("door");
        allDestructibles = GameObject.FindGameObjectsWithTag("destructible");
        allBones = GameObject.FindGameObjectsWithTag("bone");
        allFatDogs = GameObject.FindGameObjectsWithTag("fatDog");
        allSpheres = GameObject.FindGameObjectsWithTag("soundSphere");
		chaseTransScript.resetChaseTrans(); //resets BGM.

        foreach(GameObject hunter in allHunters)
        {
            hunterScript = (huntingDog)hunter.GetComponent<huntingDog>();
            hunterScript.selfDestruct();
            //Destroy(hunter);
        }
        foreach(GameObject fatDog in allFatDogs)
        {
            fatDogScript = (fatDogAi)fatDog.GetComponent<fatDogAi>();
            fatDogScript.agent.Stop();
            fatDogScript.agent.velocity = Vector3.zero;
            fatDogScript.GetComponent<Rigidbody>().velocity = Vector3.zero;
            fatDogScript.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (fatDogScript.respawnPosition != null)
            {
                fatDog.transform.position = fatDogScript.respawnPosition;
            }
            fatDogScript.stateManager(4);
        }
        foreach (GameObject enemy in allEnemies)
        {
            script = (enemyPathfinding)enemy.GetComponent<enemyPathfinding>();
            script.agent.Stop();
            script.agent.velocity = Vector3.zero;
            script.GetComponent<Rigidbody>().velocity = Vector3.zero;
            script.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (script.respawnPosition != null)
            {
                enemy.transform.position = script.respawnPosition;
            }

            script.currentTarget = script.firstTarget;
            script.targetCounter = 0;

            script.agent.speed = script.patrolSpeed;
            script.stateManager(1);
            script.agent.SetDestination(script.currentTarget.position);
            script.newTargetTimer = script.defaultNewTargetTimer;
        }

        foreach(GameObject key in allKeys)
        {
            instantiateKey Key = key.GetComponent<instantiateKey>();
            Key.checkpoint();
        }

        foreach(GameObject bone in allBones)
        {
            //bo = bone.gameObject.GetComponent<breakableObject>();
            player.GetComponent<TemporaryMovement>().bonesPlaced--;
            Destroy(bone);
        }

        foreach (GameObject destructible in allDestructibles)
        {
            instantiateDestructible DES = destructible.GetComponent<instantiateDestructible>();
            DES.checkpoint();
        }

        foreach (GameObject door in allDoors)
        {
            DoorTrigger dt = door.GetComponent<DoorTrigger>();
            dt.checkpoint();
        }       
        foreach (GameObject sphere in allSpheres)
        {
            Destroy(sphere);
        }
    }
}