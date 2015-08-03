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
    public bool checkPointActivated = false; // if the check point has been reached or not

    private string currentLevel; // the current level
    public  GameObject[] allEnemies; // needed to reset enemies' positions
    private GameObject[] allHunters; // Hunters need to be destroyed on player death
    public GameObject[] allKeys;
    public GameObject[] allDoors;
    public GameObject[] allDestructibles;
    public GameObject[] allBones;

    public enemyPathfinding script;
    public huntingDog hunterScript;
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
        if (other.gameObject.tag == "checkPoint") this.checkPointActivated = true;
    }

    void OnCollisionEnter(Collision other) // On collision with an enemy
    {

        if ((other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog") && checkPointActivated == false) // if check point has not been reached
        {
			chaseTransScript.resetChaseTrans();//resets BGM.
            Application.LoadLevel(currentLevel);
        }

        else if ((other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog" || other.gameObject.tag == "fatDog") && checkPointActivated == true) // if check point has been reached
        {     
            if(other.gameObject.tag == "enemy")
            {
                other.gameObject.GetComponent<enemyPathfinding>().agent.velocity = Vector3.zero;
            }
			sendBack = true;

            this.transform.position = checkPointPosition.transform.position;
            playerScript = player.GetComponent<TemporaryMovement>();
            playerScript.resetKeys();
            allEnemies = GameObject.FindGameObjectsWithTag("enemy");
            allHunters = GameObject.FindGameObjectsWithTag("huntingDog");
            allKeys = GameObject.FindGameObjectsWithTag("key");
            allDoors = GameObject.FindGameObjectsWithTag("door");
            allDestructibles = GameObject.FindGameObjectsWithTag("destructible");
            allBones = GameObject.FindGameObjectsWithTag("bone");
			chaseTransScript.resetChaseTrans();//resets BGM.

            foreach(GameObject hunter in allHunters)
            {
                hunterScript = (huntingDog)hunter.GetComponent<huntingDog>();
                hunterScript.selfDestruct();
                //Destroy(hunter);
            }
            foreach (GameObject enemy in allEnemies)
            {

                script = (enemyPathfinding)enemy.GetComponent<enemyPathfinding>();
                script.agent.Stop();
                script.agent.velocity = Vector3.zero;
                script.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
        }
    }
    public static bool isNull(System.Object aObj)
    {
        return aObj == null || aObj.Equals(null);
    }
}