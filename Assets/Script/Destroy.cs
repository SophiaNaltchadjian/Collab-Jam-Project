using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] GameController gameController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyGameObject()
    {
        gameController.StartGame();
        Destroy(gameObject);
    }
    public void GoToMenu()
    {
        gameController.GoToMenu();
    }
}
