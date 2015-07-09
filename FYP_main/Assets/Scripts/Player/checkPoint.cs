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
    public GameObject checkPointPosition; // Position of the check point
    public bool checkPointActivated = false; // if the check point has been reached or not

    private string currentLevel; // the current level
    private GameObject[] allEnemies; // needed to reset enemies' positions
    private GameObject[] allHunters; // Hunters need to be destroyed on player death

    enemyPathfinding script;
    void Start()
    {
        currentLevel = Application.loadedLevelName; // get current level name
    }

    void OnTriggerEnter(Collider other) // turns the check point on
    {
        if (other.gameObject.tag == "checkPoint") this.checkPointActivated = true;
    }

    void OnCollisionEnter(Collision other) // On collision with an enemy
    {
        if ((other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog") && checkPointActivated == false) // if check point has not been reached
        {
            Application.LoadLevel(currentLevel);
        }

        else if ( (other.gameObject.tag == "enemy" || other.gameObject.tag == "huntingDog")&&checkPointActivated == true ) // if check point has been reached
        {            
            this.transform.position = checkPointPosition.transform.position;

            allEnemies = GameObject.FindGameObjectsWithTag("enemy");
            allHunters = GameObject.FindGameObjectsWithTag("huntingDog");
            foreach(GameObject hunter in allHunters)
            {
                hunter.GetComponent<huntingDog>().statesHunter = enumStatesHunter.idleSuspicious;
                Destroy(hunter);
                print("EXTERMINATE");
            }
            foreach (GameObject enemy in allEnemies)
            {
                script = enemy.GetComponent<enemyPathfinding>();
                //Vector3 respawnPos = script.respawnPosition;
                enemy.transform.position = script.respawnPosition;
                script.currentTarget = script.lastTarget;
                script.agent.speed = script.patrolSpeed;
                script.stateManager(0);
            }

        }
    }
}