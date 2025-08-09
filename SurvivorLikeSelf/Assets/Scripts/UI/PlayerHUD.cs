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
    private ItemList _fullItemList = null;
    [SerializeField]
    private List<ItemChoice> _itemChoices = new List<ItemChoice>();
    

    private int _maxHealth = 0;

    void OnEnable()
    {
        EventManager.OnStartWave += SwitchToGameplay;
        EventManager.OnEndWave += SwitchToItems;

        EventManager.OnPlayerHealthInitialization += IntializeHealth;
        EventManager.OnPlayerHealthChange += SetHealth;
        EventManager.OnTimerChange += SetTimer;
    }

    void OnDisable()
    {
        EventManager.OnStartWave -= SwitchToGameplay;
        EventManager.OnEndWave -= SwitchToItems;

        EventManager.OnPlayerHealthInitialization -= IntializeHealth;
        EventManager.OnPlayerHealthChange -= SetHealth;
        EventManager.OnTimerChange -= SetTimer;
    }

    public void SwitchToGameplay()
    {
        _timerText.color = Color.white;
        _itemSelectionElements.SetActive(false);
        _gameplayElements.SetActive(true);
    }

    private void SwitchToItems()
    {
        List<Item> newItems = _fullItemList.RollNextItems(1);

        for (int i = 0; i < newItems.Count; i++)
        {
            _itemChoices[i].SetChoice(newItems[i]);
        }

        _gameplayElements.SetActive(false);
        _itemSelectionElements.SetActive(true);
    }

    private void IntializeHealth(float newMaxHP)
    {
        _maxHealth = Mathf.CeilToInt(newMaxHP);
        string newHealthMax = _maxHealth.ToString();
        _healthText.text = newHealthMax + " / " + newHealthMax;
        _filledBarImage.fillAmount = 1f;
    }

    private void SetHealth(float newHP)
    {
        int currentHealth = Mathf.CeilToInt(newHP);
        _healthText.text = currentHealth.ToString() + " / " + _maxHealth.ToString();
        _filledBarImage.fillAmount = (float)currentHealth / _maxHealth;
    }

    private void SetTimer(int newValue)
    {
        _timerText.text = newValue.ToString();
        if (newValue <= 5) _timerText.color = Color.red;
    }
}
