using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneManagementScript : MonoBehaviour {
    
    public void Awake()
    {
        
    }
    public void NextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene <= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
            Debug.Log("Scene Loaded Successfully");

        }
        Debug.Log("Scene Load Unsuccessful");

    }
}
