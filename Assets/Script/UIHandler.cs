using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Image healthFill;

    public void UpdateHealth()
    {
        healthFill.fillAmount = (player.health / player.maxHealth);
    }
    
}
