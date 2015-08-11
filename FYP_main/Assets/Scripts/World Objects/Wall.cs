using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	
	public Color transparenColor;
	
	private Renderer[] m_Renderers;

	private Renderer[] m_RenderersChildren;
	private Renderer[] m_RenderersParent;
	
	private Material[] transparentMaterial;

	private Material[] transparentMaterialChildren;
	private Material[] transparentMaterialParent;

	private Material[] m_InitialMaterial;

	private Material[] m_InitialMaterialChildren;
	private Material[] m_InitialMaterialParent;
	
//	void Start () 
//	{
//		//Get all the renderers
//		m_Renderers = GetComponentsInChildren<Renderer> ();
//		//Update length of the array to match with the number of renderer
//		m_InitialMaterial = new Material[m_Renderers.Length];
//		transparentMaterial = new Material[m_Renderers.Length];
//		
//		
//		for (int i = 0; i < m_Renderers.Length; i++){
//			//store the initial material
//			m_InitialMaterial[i] = m_Renderers[i].material;
//			
//			//create a transparent material based on the initial material
//			transparentMaterial[i] = new Material(m_InitialMaterial[i]);
//			transparentMaterial[i].SetFloat("_Mode", 2);
//			transparentMaterial[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//			transparentMaterial[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//			transparentMaterial[i].SetInt("_ZWrite", 0);
//			transparentMaterial[i].DisableKeyword("_ALPHATEST_ON");
//			transparentMaterial[i].EnableKeyword("_ALPHABLEND_ON");
//			transparentMaterial[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
//			transparentMaterial[i].renderQueue = 3000;
//			transparentMaterial[i].SetColor ("_Color", transparenColor);
//		}
//	}
//	
//	public void SetTransparent()
//	{
//		for (int i = 0; i < m_Renderers.Length; i++){
//			m_Renderers[i].material = transparentMaterial[i];
//		}
//	}
//	
//	public void SetToNormal()
//	{
//		for (int i = 0; i <m_Renderers.Length; i++){
//			m_Renderers[i].material = m_InitialMaterial[i];
//		}
//	}

	void Start () 
	{
		//Get all the renderers

        if (transform.parent.tag == "Folder")
        {
            m_RenderersChildren = transform.GetComponentsInChildren<Renderer>();
        }

        else if(transform.parent.tag == "subFolder")
        {
            m_RenderersChildren = transform.parent.GetComponentsInChildren<Renderer>();
        }
        //else if(transform.GetChild(1) != null)
        //{
        //    m_RenderersChildren = transform.GetComponentsInChildren<Renderer>();
        //}

        
        //Get all the renderers
        m_Renderers = GetComponents<Renderer>();

        //Update length of the array to match with the number of renderer
        m_InitialMaterial = new Material[m_Renderers.Length];
        transparentMaterial = new Material[m_Renderers.Length];


        for (int j = 0; j < m_Renderers.Length; j++)
        {
            //store the initial material
            m_InitialMaterial[j] = m_Renderers[j].material;

            //create a transparent material based on the initial material
            transparentMaterial[j] = new Material(m_InitialMaterial[j]);
            transparentMaterial[j].SetFloat("_Mode", 2);
            transparentMaterial[j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparentMaterial[j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparentMaterial[j].SetInt("_ZWrite", 0);
            transparentMaterial[j].DisableKeyword("_ALPHATEST_ON");
            transparentMaterial[j].EnableKeyword("_ALPHABLEND_ON");
            transparentMaterial[j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparentMaterial[j].renderQueue = 3000;
            transparentMaterial[j].SetColor("_Color", transparenColor);
        }

		//Update length of the array to match with the number of renderer
        if (m_RenderersChildren != null)
        {
            m_InitialMaterialChildren = new Material[m_RenderersChildren.Length];
            //m_InitialMaterialParent = new Material[m_RenderersParent.Length];

            transparentMaterialChildren = new Material[m_RenderersChildren.Length];
            //transparentMaterialParent = new Material[m_RenderersParent.Length];


            for (int i = 0; i < m_RenderersChildren.Length; i++)
            {
                //store the initial material
                m_InitialMaterialChildren[i] = m_RenderersChildren[i].material;

                //create a transparent material based on the initial material
                transparentMaterialChildren[i] = new Material(m_InitialMaterialChildren[i]);
                transparentMaterialChildren[i].SetFloat("_Mode", 2);
                transparentMaterialChildren[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                transparentMaterialChildren[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                transparentMaterialChildren[i].SetInt("_ZWrite", 0);
                transparentMaterialChildren[i].DisableKeyword("_ALPHATEST_ON");
                transparentMaterialChildren[i].EnableKeyword("_ALPHABLEND_ON");
                transparentMaterialChildren[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                transparentMaterialChildren[i].renderQueue = 3000;
                transparentMaterialChildren[i].SetColor("_Color", transparenColor);
            }
        }
		/*for (int j = 0; j < m_RenderersParent.Length; j++){
			//store the initial material
			m_InitialMaterialParent[j] = m_RenderersParent[j].material;

			transparentMaterialParent[j] = new Material(m_InitialMaterialParent[j]);
			transparentMaterialParent[j].SetFloat("_Mode", 2);
			transparentMaterialParent[j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			transparentMaterialParent[j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			transparentMaterialParent[j].SetInt("_ZWrite", 0);
			transparentMaterialParent[j].DisableKeyword("_ALPHATEST_ON");
			transparentMaterialParent[j].EnableKeyword("_ALPHABLEND_ON");
			transparentMaterialParent[j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
			transparentMaterialParent[j].renderQueue = 3000;
			transparentMaterialParent[j].SetColor ("_Color", transparenColor);
		}*/
	}
	
	public void SetTransparent()
	{
        if (m_RenderersChildren != null)
            for (int i = 0; i < m_RenderersChildren.Length; i++)
            {
                m_RenderersChildren[i].material = transparentMaterialChildren[i];
            }
        if(m_Renderers != null)
        {
            for(int j = 0; j<m_Renderers.Length; j++)
            {
                m_Renderers[j].material = transparentMaterial[j];
            }
        }

        //if (m_RenderersParent != null)
        //    for (int j = 0; j < m_RenderersParent.Length; j++)
        //    {
        //        m_RenderersParent[j].material = transparentMaterialParent[j];
        //    }
	}
	
	public void SetToNormal()
	{
		if (m_RenderersChildren != null)
		for (int i = 0; i <m_RenderersChildren.Length; i++){
			m_RenderersChildren[i].material = m_InitialMaterialChildren[i];
		}
        if (m_Renderers != null)
            for (int j = 0; j < m_Renderers.Length; j++)
            {
                m_Renderers[j].material = m_InitialMaterial[j];
            }

        //if (m_RenderersParent != null)
        //for (int j = 0; j <m_RenderersParent.Length; j++){
        //    m_RenderersParent[j].material = m_InitialMaterialParent[j];
        //}
		//StopCoroutine (timerDelay());
	}
}