using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private Slider healthFill;
    [SerializeField] private Slider altFireFill;
    [SerializeField] private Image dashFill;
    [SerializeField] private Image[] powerupIcons;
    [SerializeField] private Image altFire;
    [SerializeField] private Sprite[] altFireIcons;

    public void UpdateHealth()
    {
        healthFill.value = player.health;
    }

    public void UpdateAltFire()
    {
        altFireFill.value = player.altFireDelayValue;
    }

    public void UpdateDash()
    {
        dashFill.fillAmount = player.dashCooldownValue;
    }

    public void PowerupIconToggle(int icon, bool visible)
    {
        powerupIcons[icon].gameObject.SetActive(visible);
    }

    public void AltFireIconToggle(int icon)
    {
        if (!altFire.gameObject.activeSelf) altFire.gameObject.SetActive(true);
        altFire.sprite = altFireIcons[icon];
    }

}
