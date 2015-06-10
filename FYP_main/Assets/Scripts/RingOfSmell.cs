using UnityEngine;
using System.Collections;

public class RingOfSmell : MonoBehaviour {

    float scale = 5.0f;
	// Use this for initialization
	void Start () 
    {
        this.transform.localScale += new Vector3 (scale,0,scale);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        //if player crosses the cone, informs the parent(Enemy) of visible player
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.transform.parent.SendMessage("playerSpotted", SendMessageOptions.DontRequireReceiver);
        }
    }
    void setScale()
    {
        this.transform.localScale += new Vector3(scale, 0, scale);

    }
}
