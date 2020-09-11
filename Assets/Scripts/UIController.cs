using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject resumeButton;
    public GameObject nextLevelButton;
    public GameObject levelClearTxt;

    private Scene currActiveScene;

    void Start()
    {
    	currActiveScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Escape))
    	{
    		PauseGame();
    	}
    }

    public void PauseGame()
    {
    	Time.timeScale = 0;
    	pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
    	Time.timeScale = 1;
    	pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
    	Time.timeScale = 1;
    	SceneManager.LoadScene(currActiveScene.name);
    }

    public void EndGame()
    {
    	pausePanel.SetActive(true);
    	resumeButton.SetActive(false);
    	levelClearTxt.SetActive(true);

        if(currActiveScene.name == "Level1")
        {
            nextLevelButton.SetActive(true);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
    }
}
