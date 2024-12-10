using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Method to load the game scene
    public void StartGame()
    {
        // Replace "Main Scene v7" with the exact name of your game scene
        SceneManager.LoadScene("Main Scene v7");
    }

    // Optional: Method to quit the game
    public void QuitGame()
    {
        // Works only in a built application, not in the Unity editor
        Application.Quit();
        Debug.Log("Game Quit!"); // Just for debugging in the Unity Editor
    }
}
