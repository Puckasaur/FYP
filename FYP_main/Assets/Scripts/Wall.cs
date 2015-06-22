using UnityEngine;
using System.Collections;

public class wall : MonoBehaviour
{


    public Color transparenColor;
    private Color m_InitialColor;

    void Start()
    {
        m_InitialColor = GetComponent<Renderer>().material.color;
    }

    public void SetTransparent()
    {

        GetComponent<Renderer>().material.color = transparenColor;
    }

    public void SetToNormal()
    {

        GetComponent<Renderer>().material.color = m_InitialColor;
    }
}
