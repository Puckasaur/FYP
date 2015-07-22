using UnityEngine;
using System.Collections;

public class leadEnemy : MonoBehaviour {

    public GameObject enemy;
    public enemyPathfinding enemyScript;
    public enemyPathfinding thisEnemyScript;
    int firstTargetCounter;
    int secondTargetCounter;
	// Use this for initialization
	void Start () {
        enemyScript = enemy.GetComponent<enemyPathfinding>();
        thisEnemyScript = gameObject.GetComponent<enemyPathfinding>();
	}
	
	// Update is called once per frame
	void Update () {
	if(enemyScript.isOnWaypoint)
    {
        if (thisEnemyScript.isOnWaypoint)
        {
            thisEnemyScript.currentTarget = thisEnemyScript.targets[firstTargetCounter];
            enemyScript.currentTarget = enemyScript.targets[secondTargetCounter];
            enemyScript.isOnWaypoint = false;
            firstTargetCounter++;
            secondTargetCounter++;
            thisEnemyScript.stateManager(0);
            enemyScript.stateManager(0);
            if (firstTargetCounter >= enemyScript.targets.Count)
            {
                firstTargetCounter = 0;
            }
            if (secondTargetCounter >= thisEnemyScript.targets.Count)
            {
                secondTargetCounter = 0;
            }
        }
    }
	}
}
