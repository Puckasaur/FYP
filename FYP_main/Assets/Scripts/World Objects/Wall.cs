using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
	private Renderer[] children;
	
	public Color transparenColor;
	private Color m_InitialColor;
	
	void Start()
	{
		m_InitialColor = GetComponent<Renderer>().material.color;
		
		//m_InitialColor = GameObject.FindWithTag ("PARENT").GetComponentInChildren<Renderer>().material.color;
		children = GetComponentsInChildren<Renderer>();
	}
	
	public void SetTransparent()
	{
		
		GetComponent<Renderer>().material.color = transparenColor;
		
		//GameObject.FindWithTag ("PARENT").GetComponentInChildren<Renderer>().material.color = transparenColor;
		
		foreach (Renderer i in children) 
		{
			i.material.color = transparenColor;
		}
		
		
	}
	
	public void SetToNormal()
	{
		
		GetComponent<Renderer>().material.color = m_InitialColor;
		
		//GameObject.FindWithTag ("PARENT").GetComponentInChildren	<Renderer>().material.color = m_InitialColor;
		
		foreach (Renderer i in children) 
		{
			i.material.color = m_InitialColor;
		}
		
	}
}
