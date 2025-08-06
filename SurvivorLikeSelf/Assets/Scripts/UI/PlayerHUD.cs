using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Image _filledBarImage = null;
    [SerializeField]
    private TextMeshProUGUI _healthText = null;
    [SerializeField]
    private TextMeshProUGUI _timerText = null;

    private int _maxHealth = 0;

    void OnEnable()
    {
        EventManager.StartListening("PlayerHealthInitialized", IntializeHealth);
        EventManager.StartListening("PlayerSetHealth", SetHealth);
        EventManager.StartListening("SetSecondDisplay", SetTimer);
    }

    void OnDisable()
    {
        EventManager.StopListening("PlayerHealthInitialized", IntializeHealth);
        EventManager.StopListening("PlayerSetHealth", SetHealth);
        EventManager.StopListening("SetSecondDisplay", SetTimer);
    }

    private void IntializeHealth(Dictionary<string, object> message)
    {
        _maxHealth = Mathf.CeilToInt((float)message["maxHealth"]);
        string newHealthMax = _maxHealth.ToString();
        _healthText.text = newHealthMax + " / " + newHealthMax;
        _filledBarImage.fillAmount = 1f;
    }

    private void SetHealth(Dictionary<string, object> message)
    {
        int currentHealth = Mathf.CeilToInt((float)message["currentHealth"]);
        _healthText.text = currentHealth.ToString() + " / " + _maxHealth.ToString();
        _filledBarImage.fillAmount = (float)currentHealth / _maxHealth;
    }

    private void SetTimer(Dictionary<string, object> message)
    {
        int newValue = (int)message["secondValue"];
        _timerText.text = newValue.ToString();
        if (newValue <= 5) _timerText.color = Color.red;
    }
}
