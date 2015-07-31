using UnityEngine;
using System.Collections;

public class instantiateKey : MonoBehaviour 
{
    public GameObject key;
    public int keyNumber;
    GameObject newKey;
    Key keyscript;

	// Use this for initialization
    void Awake()
    {
        if (transform.GetComponentInChildren<Key>() == null)
        {
            newKey = (GameObject)Instantiate(key, transform.localPosition, transform.localRotation);
            newKey.transform.parent = transform;
            keyscript = newKey.GetComponent<Key>();
            keyscript.keyNumber = keyNumber;
        }
    }

	public void checkpoint()
    {
        Destroy(newKey);
        newKey = (GameObject)Instantiate(key, transform.localPosition, transform.localRotation);
        newKey.transform.parent = transform;
        keyscript = newKey.GetComponent<Key>();
        keyscript.keyNumber = keyNumber;

    }
	// Update is called once per frame
	void Update () 
    {
	
	}
}
