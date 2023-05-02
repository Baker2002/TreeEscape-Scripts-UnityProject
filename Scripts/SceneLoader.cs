using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public bool TutorialPlayed = false;
    public Button _playButton;
    

    private void FixedUpdate()
    {
        if (TutorialPlayed)
        {
            _playButton.interactable = true;
        }
        else
        {
            _playButton.interactable = false;
            
        }
    }
    
    public void LoadScene(string sampleScene)
    {
        SceneManager.LoadScene(sampleScene);
    }
    public void LoadTutorial(string sampleScene)
    {
        TutorialPlayed = true;
        SceneManager.LoadScene(sampleScene);
    }
    
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
