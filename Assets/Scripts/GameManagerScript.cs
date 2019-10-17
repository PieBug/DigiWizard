using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    bool GameEnded = false;
    float RespawnTimer = 1f;

    public void EndGame() 
    {
        if(GameEnded == false)
        {
            GameEnded = true;
            Debug.Log("Game Ended");
            Invoke("Restart", RespawnTimer);
        }
       
    }

    void Restart()
    {
        SceneManager.LoadScene(1);
    }

}
