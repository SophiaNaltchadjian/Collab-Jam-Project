using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Image healthFill;

    public void UpdateHealth()
    {
        healthFill.fillAmount = (float)player.health / (float)player.maxHealth;
    }
    
}
