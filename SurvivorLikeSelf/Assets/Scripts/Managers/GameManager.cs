using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static EventManager gameManager;
    public static EventManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindFirstObjectByType<EventManager>();

                if (!gameManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {

                    //  Sets this to not be destroyed when reloading scene
                    DontDestroyOnLoad(gameManager);
                }
            }
            return gameManager;
        }
    }

    void OnEnable()
    {
        EventManager.StartListening("gameover", Restart);
    }

    void OnDisable()
    {
        EventManager.StopListening("gameover", Restart);
    }

    public void Restart(Dictionary<string, object> message)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
