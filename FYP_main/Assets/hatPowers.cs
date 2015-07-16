using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hatPowers : MonoBehaviour
{

    bool disguisedAsDog = false;
    bool smellHidden = false;
    public bool hatDisguiseAsDog;
    public bool hatHideSmell;
    public List<GameObject> enemies = new List<GameObject>();
    GameObject[] patrolEnemy;
    GameObject[] hunterEnemy;
    float disguiseTimer;
    float disguiseCooldown = 0;
    public float defaultDisguiseTimer;
    public float defaultDisguiseCooldown;
    float hiddenSmellTimer;
    float hiddenSmellCooldown = 0;
    public float defaultHiddenSmellTimer;
    public float defaultHiddenSmellCooldown;

    ringOfSmell ring;

    // Use this for initialization
	void Start () 
    {
        disguiseTimer = defaultDisguiseTimer;
        hiddenSmellTimer = defaultHiddenSmellTimer;
        ring = GetComponentInChildren<ringOfSmell>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (hatDisguiseAsDog)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (disguiseCooldown <= 0)
                {
                    if (!disguisedAsDog)
                    {
                        disguisedAsDog = true;
                        disGuiseAsDog();
                    }
                }
            }
        }
        if (hatHideSmell)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (hiddenSmellCooldown <= 0)
                {
                    if (!smellHidden)
                    {
                        smellHidden = true;
                        ring.isDisguised("tempMove");
                    }
                }
            }
        }
        if(disguisedAsDog)
        {
            disguiseTimer-=Time.deltaTime;
            if(disguiseTimer <= 0)
            {
                disguiseTimer = defaultDisguiseTimer;
                disguisedAsDog = false;
                disGuiseAsDog();
                disguiseCooldown = defaultDisguiseCooldown;
            }
        }
        if(smellHidden)
        {
            hiddenSmellTimer-=Time.deltaTime;
            if(hiddenSmellTimer<= 0)
            {            
                hiddenSmellTimer = defaultHiddenSmellTimer;
                smellHidden = false;
                ring.isNotDisguised("hatPower");
                hiddenSmellCooldown = defaultHiddenSmellCooldown;
            }
        }
        if(disguiseCooldown > 0)
        {
            disguiseCooldown -= Time.deltaTime;
        }
        if(hiddenSmellCooldown >0)
        {
            hiddenSmellCooldown -= Time.deltaTime;
        }
	}
    void disGuiseAsDog()
    {
        enemies.Clear();
        patrolEnemy = GameObject.FindGameObjectsWithTag("enemy");
        hunterEnemy = GameObject.FindGameObjectsWithTag("huntingDog");
        foreach (GameObject enemy in patrolEnemy)
        {
            enemies.Add(enemy);
        }
        foreach (GameObject enemy in hunterEnemy)
        {
            enemies.Add(enemy);
        }
        if (disguisedAsDog)
        {
            foreach (GameObject enemy in enemies)
            {
                coneOfVision cone = enemy.GetComponentInChildren<coneOfVision>();
                cone.isDisguised();
            }
        }
        else if (!disguisedAsDog)
        {
            foreach (GameObject enemy in enemies)
            {
                coneOfVision cone = enemy.GetComponentInChildren<coneOfVision>();
                cone.isNotDisguised();
            }
        }
    }
}
