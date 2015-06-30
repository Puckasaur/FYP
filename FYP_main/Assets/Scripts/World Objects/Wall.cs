using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	
	public Color transparenColor;
	
	private Renderer[] m_Renderers;
	
	private Material[] transparentMaterial;
	private Material[] m_InitialMaterial;
	
	void Start () 
	{
		//Get all the renderers
		m_Renderers = GetComponentsInChildren<Renderer> ();
		//Update length of the array to match with the number of renderer
		m_InitialMaterial = new Material[m_Renderers.Length];
		transparentMaterial = new Material[m_Renderers.Length];
		
		
		for (int i = 0; i < m_Renderers.Length; i++){
			//store the initial material
			m_InitialMaterial[i] = m_Renderers[i].material;
			
			//create a transparent material based on the initial material
			transparentMaterial[i] = new Material(m_InitialMaterial[i]);
			transparentMaterial[i].SetFloat("_Mode", 2);
			transparentMaterial[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			transparentMaterial[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			transparentMaterial[i].SetInt("_ZWrite", 0);
			transparentMaterial[i].DisableKeyword("_ALPHATEST_ON");
			transparentMaterial[i].EnableKeyword("_ALPHABLEND_ON");
			transparentMaterial[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
			transparentMaterial[i].renderQueue = 3000;
			transparentMaterial[i].SetColor ("_Color", transparenColor);
		}
	}
	
	public void SetTransparent()
	{
		for (int i = 0; i < m_Renderers.Length; i++){
			m_Renderers[i].material = transparentMaterial[i];
		}
	}
	
	public void SetToNormal()
	{
		for (int i = 0; i <m_Renderers.Length; i++){
			m_Renderers[i].material = m_InitialMaterial[i];
		}
	}
}
//=======
//﻿using UnityEngine;
//using System.Collections;
//
//public class Wall : MonoBehaviour
//{
//	private Renderer[] children;
//	
//	public Color transparenColor;
//	private Color m_InitialColor;
//	
//	void Start()
//	{
//		m_InitialColor = GetComponent<Renderer>().material.color;
//		
//		//m_InitialColor = GameObject.FindWithTag ("PARENT").GetComponentInChildren<Renderer>().material.color;
//		children = GetComponentsInChildren<Renderer>();
//	}
//	
//	public void SetTransparent()
//	{
//		
//		GetComponent<Renderer>().material.color = transparenColor;
//		
//		//GameObject.FindWithTag ("PARENT").GetComponentInChildren<Renderer>().material.color = transparenColor;
//		
//		foreach (Renderer i in children) 
//		{
//			i.material.color = transparenColor;
//		}
//		
//		
//	}
//	
//	public void SetToNormal()
//	{
//		
//		GetComponent<Renderer>().material.color = m_InitialColor;
//		
//		//GameObject.FindWithTag ("PARENT").GetComponentInChildren	<Renderer>().material.color = m_InitialColor;
//		
//		foreach (Renderer i in children) 
//		{
//			i.material.color = m_InitialColor;
//		}
//		
//	}
//}
//>>>>>>> origin/AiTestGround
