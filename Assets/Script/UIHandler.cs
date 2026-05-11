using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Slider healthFill;

    public void UpdateHealth()
    {
        healthFill.value = player.health;
    }
    
}
