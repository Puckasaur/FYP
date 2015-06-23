using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
	GameObject[] parentChild;

    public Color transparenColor;
    private Color m_InitialColor;
	//public GameObject test;

    void Start()
    {
        m_InitialColor = GetComponent<Renderer>().material.color;
        m_InitialColor = GetComponentInChildren<Renderer>().material.color;

		parentChild = Resources.LoadAll<GameObject> ("PARENT");
    }

    public void SetTransparent()
    {

        GetComponent<Renderer>().material.color = transparenColor;
		GetComponentInChildren<Renderer>().material.color = transparenColor;

		Renderer selectChild = GetComponentInChildren<Renderer> ();

		selectChild.material.color = transparenColor;
    }

    public void SetToNormal()
    {

        GetComponent<Renderer>().material.color = m_InitialColor;
		GetComponentInChildren<Renderer>().material.color = m_InitialColor;

		if (GetComponentInChildren<Renderer> ()) 
		{
			Debug.Log ("Normal");
		}
    }
}
