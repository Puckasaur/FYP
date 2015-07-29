using UnityEngine;
using System.Collections;

public class DisplayInstruction : MonoBehaviour
{
    private SpriteRenderer instruction;

	void Start () 
    {
        instruction = GetComponentInChildren<SpriteRenderer>();
       if (instruction.enabled == true) instruction.enabled = false;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player") instruction.enabled = true;
    }

    void OnTriggerExit()
    {
        instruction.enabled = false;
    }
}