using UnityEngine;
using System.Collections;

public class smellIncrease : MonoBehaviour {

    public float value;
    GameObject player;
    ringOfSmell script;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("player");
        script = player.GetComponentInChildren<ringOfSmell>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "player" || other.gameObject.tag == "smell")
        {
            script.increaseSmell(value);
        }
    }

    void OnTriggerExit()
    {
        script.radius = Mathf.Floor(script.radius);
    }
}
