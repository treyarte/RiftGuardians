using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI; //Make note of this because if it says UIElements then the image and slider fields do not work

namespace Ui
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fill;
        [SerializeField] private Gradient gradient;
        [SerializeField] private PlayerHealth _player;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Image _healthIcon;
        [SerializeField] private Sprite _hpSprite;
        [SerializeField] private Sprite _armorSprite;
        [SerializeField] private Sprite _deathSprite;

        private void OnEnable()
        {
            //Adding events
            CrossSplineDmg.DealDamageOnDeath += SetHealth;
            PlayerHealth.DamagePlayer += SetHealth;
            PlayerHealth.OnPlayerDeath += SetDeathIcon;
        }

        private void OnDisable()
        {
            //Disabling events
            CrossSplineDmg.DealDamageOnDeath -= SetHealth;
            PlayerHealth.DamagePlayer -= SetHealth;
            PlayerHealth.OnPlayerDeath -= SetDeathIcon;
        }

        // Start is called before the first frame update
        void Start()
        {
            float maxHealth = _player.GetMaxHealth();
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
            _healthText.text = $"{maxHealth}/{maxHealth}";
            fill.color = gradient.Evaluate(1f);
        }

        /// <summary>
        /// Updates the health bar progress and text
        /// </summary>
        /// <param name="health"></param>
        private void SetHealth(float health)
        {
            float currentHealth = _player.GetCurrentHealth();
            float maxHealth = _player.GetMaxHealth();
            slider.value = currentHealth;
            _healthText.text = $"{currentHealth}/{maxHealth}";
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    
        /// <summary>
        /// To be called when the player dies to set the icon to a death icon
        /// </summary>
        /// <param name="_"></param>
        private void SetDeathIcon(Player _)
        {
            _healthIcon.sprite = _deathSprite;
        } 
    }
}
