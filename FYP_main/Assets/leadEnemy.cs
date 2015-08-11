using UnityEngine;
using System.Collections;

public class leadEnemy : MonoBehaviour {

    public GameObject enemy;
    public enemyPathfinding enemyScript;
    public enemyPathfinding thisEnemyScript;
    int firstTargetCounter;
    int secondTargetCounter;
    
	// Use this for initialization
	void Start () 
    {
        enemyScript = enemy.GetComponent<enemyPathfinding>();
        thisEnemyScript = gameObject.GetComponent<enemyPathfinding>();
        enemyScript.isPaired = true;
        thisEnemyScript.isPaired = true;

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (enemyScript.isPatrolling && thisEnemyScript.isPatrolling)
        {
            if (enemyScript.isOnWaypoint)
            {
                if (thisEnemyScript.isOnWaypoint)
                {
                    thisEnemyScript.currentTarget = thisEnemyScript.targets[firstTargetCounter];
                    enemyScript.currentTarget = enemyScript.targets[secondTargetCounter];
                    print(thisEnemyScript.currentTarget + "  <<  thisEnemyScript.currentTarget" + enemyScript.currentTarget + "  << enemyScript.currentTarget");
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
                else
                {
                    enemyScript.stateManager(0);
                }
            }
        }
        else if( enemyScript.isPatrolling && !thisEnemyScript.isPatrolling)
        {
            if (enemyScript.isOnWaypoint)
            {
                print("something should happen");
                enemyScript.currentTarget = enemyScript.targets[secondTargetCounter];
                enemyScript.isOnWaypoint = false;
                firstTargetCounter++;
                secondTargetCounter++;
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
        else if(thisEnemyScript.isPatrolling && !enemyScript.isPatrolling)
        {
            if (thisEnemyScript.isOnWaypoint)
            {
                thisEnemyScript.currentTarget = thisEnemyScript.targets[firstTargetCounter];
                thisEnemyScript.isOnWaypoint = false;
                firstTargetCounter++;
                secondTargetCounter++;
                thisEnemyScript.stateManager(0);
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
        else
        {

        }
	}
}
