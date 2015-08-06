using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class moviePlayer : MonoBehaviour 
{
    public MovieTexture[] movies;
    public Material[] materials;
    public MovieTexture movie;
    public string filename;
    public Texture texture;
    //public MovieTexture mp;
    public Material material;
	// Use this for initialization
	void Awake () 
    {
        //mp = gameObject.GetComponent<MovieTexture>();
        //texture = 
        material = gameObject.GetComponent<Renderer>().sharedMaterial;
        movie = (MovieTexture)GetComponent<Renderer>().sharedMaterial.mainTexture;

        filename = (PlayerPrefs.GetString("Movie"));
        movies = Resources.LoadAll<MovieTexture>("Movie");
        materials = Resources.LoadAll<Material>("Movie/Materials");
        for (int j = 0; j < materials.Length;j++ )
        {
            if(materials[j].name == filename)
            {
                print("found one");
                material = materials[j] as Material;
                gameObject.GetComponent<Renderer>().sharedMaterial = material;
            }
        }
            for (int i = 0; i < movies.Length; i++)
            {
                if (movies[i].name == filename)
                {

                    movie = movies[i];
                    //material = movies[i];
                    //mp = movies[i];
                    //movie = mp;
                }
            }
        //movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
        if(movie != null)
        {
            movie.Play();
            //mp.Play();
            if(movie.isPlaying == true)
            print(" All Ok!");
        }
	}
	
	// Update is called once per frame
	void Update () 
    {   
        if(!movie.isPlaying)
        {
            print(" no longer playing");
            Application.LoadLevel(PlayerPrefs.GetInt("Scene"));
        }
	}
}
