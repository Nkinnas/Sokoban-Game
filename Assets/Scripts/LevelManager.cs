using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject[] boxes;  
    public GameObject[] destinations; 
    public GameObject completionPanel;  

    private bool levelCompleted = false;

    void Start()
    {
        completionPanel.SetActive(false); 
    }

    void Update()
    {
        if (!levelCompleted && AreAllBoxesOnCorrectDestinations())
        {
            LevelComplete();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }   

        if (Input.GetKeyDown(KeyCode.N)){
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.P)){
            LoadPreviousLevel();
        }
        if (Input.GetKeyDown(KeyCode.M)){
            LoadMainMenu();
        }
        
    }

    bool AreAllBoxesOnCorrectDestinations()
    {
        foreach (GameObject box in boxes)
        {
            bool boxOnCorrectDestination = false;
            string boxTag = box.tag;
            foreach (GameObject destination in destinations)
            {
                if (boxTag == destination.tag)
                {      
                    if (Vector3.Distance(box.transform.position, destination.transform.position) < 0.5f)
                    {
                        boxOnCorrectDestination = true;
                        break; 
                    }
                }
            }
            if (!boxOnCorrectDestination)
            {
                return false;  
            }
        }
        return true; 
    }
    void LevelComplete()
    {
        levelCompleted = true;
        completionPanel.SetActive(true);  
        Debug.Log("Level Complete!");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
    }
    public void LoadPreviousLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);  
    }
    public void LoadMainMenu(){
        SceneManager.LoadScene(0);
    }
   
}
