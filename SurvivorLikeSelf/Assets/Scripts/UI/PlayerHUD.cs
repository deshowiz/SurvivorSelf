using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField]
    private CanvasGroup _gameplayElements = null;
    [SerializeField]
    private CanvasGroup _itemSelectionElements = null;
    [Header("Gameplay References")]
    [SerializeField]
    private Image _filledBarImage = null;
    [SerializeField]
    private TextMeshProUGUI _healthText = null;
    [SerializeField]
    private TextMeshProUGUI _timerText = null;
    [SerializeField]
    private TextMeshProUGUI _goldText = null;
    [Header("Item Choice References")]
    [SerializeField]
    private ItemList _fullItemList = null;
    [SerializeField]
    private List<ItemChoice> _itemChoices = new List<ItemChoice>();
    [SerializeField]
    private int numChoices = 3;
    [Header("Stat References")]
    private StatDisplayPanel _statDisplayPanel = null;
    private int _maxHealth = 0;
    private int _currentGold = 0;

    void OnEnable()
    {
        EventManager.OnStartWave += SwitchToGameplay;
        EventManager.OnEndWave += SwitchToItems;

        EventManager.OnPlayerHealthInitialization += IntializeHealth;
        EventManager.OnPlayerHealthChange += SetHealth;
        EventManager.OnTimerChange += SetTimer;
        EventManager.OnGoldChange += SetGold;
    }

    void OnDisable()
    {
        EventManager.OnStartWave -= SwitchToGameplay;
        EventManager.OnEndWave -= SwitchToItems;

        EventManager.OnPlayerHealthInitialization -= IntializeHealth;
        EventManager.OnPlayerHealthChange -= SetHealth;
        EventManager.OnTimerChange -= SetTimer;
        EventManager.OnGoldChange -= SetGold;
    }

    public void SwitchToGameplay()
    {
        _timerText.color = Color.white;
        _itemSelectionElements.alpha = 0;
        _itemSelectionElements.blocksRaycasts = false;
        _itemSelectionElements.interactable = false;
        _gameplayElements.alpha = 1;
        _gameplayElements.blocksRaycasts = true;
        _gameplayElements.interactable = true;
    }

    private void SwitchToItems()
    {
        List<Item> newItems = _fullItemList.RollNextItems(numChoices);

        for (int i = 0; i < newItems.Count; i++)
        {
            _itemChoices[i].SetChoice(newItems[i], _currentGold);
        }

        _gameplayElements.alpha = 0;
        _gameplayElements.blocksRaycasts = false;
        _gameplayElements.interactable = false;
        _itemSelectionElements.alpha = 1;
        _itemSelectionElements.blocksRaycasts = true;
        _itemSelectionElements.interactable = true;
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

    private void SetGold(int newGoldValue)
    {
        _currentGold = newGoldValue;
        _goldText.text = _currentGold.ToString();
    }
}
