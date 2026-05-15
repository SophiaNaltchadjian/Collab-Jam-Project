using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public int creeperGenerations;
    public GameObject GameOverPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void LeaveGame()
    {
        Application.Quit();
    }
    public void ShowGameOver()
    {
        GameOverPanel.SetActive(true);
    }
}
