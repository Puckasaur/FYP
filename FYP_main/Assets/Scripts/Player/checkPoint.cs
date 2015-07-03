/* 
 * To put inside player's gameObject.
 * respawnPosition = variable that needs to be added in the enemy script.
 * An empty gameObject set at the position of the check point has to be added to the scene.
 * A trigger has to be added to the scene to turn the check point on with a box collider (trigger set to on) and tag == "checkPoint".
*/

using UnityEngine;
using System.Collections;

public class CheckPointSystem : MonoBehaviour
{
    public GameObject checkPointPosition; // Position of the check point
    public bool checkPoint = false; // if the check point has been reached or not

    private string currentLevel; // the current level
    private GameObject[] allEnemies; // needed to reset enemies' positions

    void Start()
    {
        currentLevel = Application.loadedLevelName; // get current level name
    }

    void OnTriggerEnter(Collider other) // turns the check point on
    {
        if (other.gameObject.tag == "checkPoint") this.checkPoint = true;
    }

    void OnCollisionEnter(Collision other) // On collision with an enemy
    {
        if (other.gameObject.tag == "enemy" && checkPoint == false) // if check point has not been reached
        {
            Application.LoadLevel(currentLevel);
        }

        else if (other.gameObject.tag == "enemy" && checkPoint == true) // if check point has been reached
        {
            allEnemies = GameObject.FindGameObjectsWithTag("enemy");

            foreach (GameObject enemy in allEnemies)
            {
                Vector3 respawnPos = enemy.GetComponent<enemyPathfinding>().respawnPosition;
                enemy.transform.position = respawnPos;

                enemy.GetComponent<enemyPathfinding>().States = enumStates.patrol;
            }

            this.transform.position = checkPointPosition.transform.position;
        }
    }
}