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
    bool loadingLevel = false;
    public Material material;
    AsyncOperation async;
    AudioSource movieSource;
	public bool movieDone = false;

	// Use this for initialization
	void Awake () 
    {
        material = gameObject.GetComponent<Renderer>().sharedMaterial;
        movie = (MovieTexture)GetComponent<Renderer>().sharedMaterial.mainTexture;
        movieSource = gameObject.GetComponent<AudioSource>();
        filename = (PlayerPrefs.GetString("Movie"));
        movies = Resources.LoadAll<MovieTexture>("Movie");
        materials = Resources.LoadAll<Material>("Movie/Materials");
        for (int j = 0; j < materials.Length;j++ )
        {
            if(materials[j].name == filename)
            {
                material = materials[j] as Material;
                gameObject.GetComponent<Renderer>().sharedMaterial = material;
            }
        }
            for (int i = 0; i < movies.Length; i++)
            {
                if (movies[i].name == filename)
                {

                    movie = movies[i];

                }
            }
            if (movie != null)
            {
                movieSource.clip = movie.audioClip;
                movie.Play();
                movieSource.Play();
            }

		//Background.SetActive(false);
		//ProgressBar.SetActive(false);
		//Text.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {   
        if(movie.isPlaying == true && loadingLevel != true)
        {
            async = Application.LoadLevelAsync(PlayerPrefs.GetInt("Scene"));
            async.allowSceneActivation = false;
            loadingLevel = true;
        }
        if(!movie.isPlaying)
        {
            async.allowSceneActivation = true;
			//StartCoroutine(DisplayLoadingScreen());
        }
	}

//	IEnumerator DisplayLoadingScreen()
//	{
//		Background.SetActive(true);
//		ProgressBar.SetActive(true);
//		Text.SetActive(true);
//		
//		ProgressBar.transform.localScale = new Vector3(loadProgress, 
//		                                               ProgressBar.transform.localScale.y, 
//		                                               ProgressBar.transform.localScale.z);
//		
//		Text.GetComponent<GUIText> ().text = "Loading Progress " + loadProgress + "%";
////		
////		AsyncOperation ao = Application.LoadLevelAsync(""); 
//
//		//async.allowSceneActivation = true;
////		
//		while (!async.isDone)
//		{
//			loadProgress = (int) (async.progress * 100);
//			Text.GetComponent<GUIText> ().text = "Loading Progress " + loadProgress + "%";
//			ProgressBar.transform.localScale = new Vector3(async.progress, 
//			                                               ProgressBar.transform.localScale.y, 
//			                                               ProgressBar.transform.localScale.z);
//			
//			yield return null;
//		}
//		
//	}
}
