using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    public Player myPlayer;
    public Image progressfill;
    private float progfillamount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progfillamount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        progfillamount = Mathf.Lerp(progfillamount, ((float)myPlayer.ItensCount / 3) + 0.05f, 1.8f * Time.fixedDeltaTime);
        progressfill.fillAmount = progfillamount;
    }
}
