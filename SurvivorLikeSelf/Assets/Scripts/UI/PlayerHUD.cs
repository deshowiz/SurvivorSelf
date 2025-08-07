using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField]
    private GameObject _gameplayElements = null;
    [SerializeField]
    private GameObject _itemSelectionElements = null;
    [Header("Gameplay References")]
    [SerializeField]
    private Image _filledBarImage = null;
    [SerializeField]
    private TextMeshProUGUI _healthText = null;
    [SerializeField]
    private TextMeshProUGUI _timerText = null;
    [Header("Item Choice References")]
    [SerializeField]
    private List<ItemChoice> _itemChoices = new List<ItemChoice>();

    private int _maxHealth = 0;

    void OnEnable()
    {
        EventManager.StartListening("StartWave", SwitchToGameplay);
        EventManager.StartListening("WaveEnd", SwitchToItems);

        EventManager.StartListening("PlayerHealthInitialized", IntializeHealth);
        EventManager.StartListening("PlayerSetHealth", SetHealth);
        EventManager.StartListening("SetSecondDisplay", SetTimer);
    }

    void OnDisable()
    {
        EventManager.StopListening("StartWave", SwitchToGameplay);
        EventManager.StopListening("WaveEnd", SwitchToItems);

        EventManager.StopListening("PlayerHealthInitialized", IntializeHealth);
        EventManager.StopListening("PlayerSetHealth", SetHealth);
        EventManager.StopListening("SetSecondDisplay", SetTimer);
    }

    public void SwitchToGameplay(Dictionary<string, object> message)
    {
        _timerText.color = Color.white;
        _itemSelectionElements.SetActive(false);
        _gameplayElements.SetActive(true);
    }

    private void SwitchToItems(Dictionary<string, object> message)
    {
        List<Item> newItems = (List<Item>)message["rolledItems"];

        for (int i = 0; i < newItems.Count; i++)
        {
            _itemChoices[i].SetChoice(newItems[i]);
        }

        _gameplayElements.SetActive(false);
        _itemSelectionElements.SetActive(true);
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
