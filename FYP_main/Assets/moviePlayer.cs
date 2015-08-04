using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class moviePlayer : MonoBehaviour 
{
    Renderer movieRenderer;
    MovieTexture movie;
    string folderName;
    public bool makeTexture = true;
    List<Texture> pictures = new List<Texture>();
    bool loop = false;
    int counter = 0;
    bool film = true;
    bool isPlaying = true;
    float pictureRateInSeconds = 1;
    public string startFolder;
    public string value;
    public float nextPicture = 0;
    public Camera mainCamera;
    public Camera movieCamera;
	// Use this for initialization
	void Awake () 
    {        
        if(film == true)
        {
            pictureRateInSeconds = 0.0166666666f;
        }
        Texture[] textures = Resources.LoadAll<Texture>(startFolder);
        for(int i = 0; i< textures.Length; i++)
        {
            pictures.Add(textures[i]);
        }
        movieRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(isPlaying)
        {
            movieCamera.enabled = true;
            mainCamera.enabled = false;
        if(Time.time >= nextPicture)
        {
            if(makeTexture)
            {
                movieRenderer.material.mainTexture = pictures[counter];
            }           
            nextPicture = Time.time + pictureRateInSeconds;
            counter += 1;
        }
        if(counter >= pictures.Count)
        {
            if(loop)
            {
                counter = 0;
            }
            else
            {
                isPlaying = false;

            }
        }
        }
        else
        {
            movieCamera.enabled = false;
            mainCamera.enabled = true;
        }
        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    print("Return pressed");
        //    if(movie.isPlaying)
        //    {
        //        movie.Pause();
        //        print("pausing movie");
        //    }
        //    else
        //    {
        //        movie.Play();
        //        print("playing movie");
        //    }
        //}
	}
    public void endOfLevel(string value)
    {
            Texture[] textures = Resources.LoadAll<Texture>(value);
            for (int i = 0; i < textures.Length; i++)
            {
                pictures.Add(textures[i]);
            }
            isPlaying = true;
    }
}
