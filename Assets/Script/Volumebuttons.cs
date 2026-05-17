using UnityEngine;

public class Volumebuttons : MonoBehaviour
{

    [SerializeField] private bool volumeup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {

        if (volumeup)
        {
            if (AudioMaster.AM.volume < 10)
            {
                AudioMaster.AM.volume += 1;
            }
        }
        else
        {
            if (AudioMaster.AM.volume > 0)
            {
                AudioMaster.AM.volume -= 1;
            }
        }
        AudioMaster.AM.Sound(2);
    }
}
