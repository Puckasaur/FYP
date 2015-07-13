﻿using UnityEngine;
using System.Collections;

public class decreaseSmell : MonoBehaviour
{

    public float value;
    GameObject player;
    ringOfSmell script;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
        script = player.GetComponentInChildren<ringOfSmell>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player" || other.gameObject.tag == "smell")
        {
            script.decreaseSmell(value);
        }
    }
}