using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Player myPlayer;
    public Image progressfill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        progressfill.fillAmount = ((float)myPlayer.ItensCount/3) + 0.05f;
    }
}
