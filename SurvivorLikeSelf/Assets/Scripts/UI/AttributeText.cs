using TMPro;
using UnityEngine;

public class AttributeText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _attributeNameText = null;
    [SerializeField]
    private TextMeshProUGUI _attributeValueText = null;

    public void PopulateText(int value)
    {
        if (value == 0)
        {
            _attributeNameText.color = Color.white;
            _attributeValueText.color = Color.white;
        }
        else if (value > 0)
        {
            _attributeNameText.color = Color.green;
            _attributeValueText.color = Color.green;
        }
        else
        {
            _attributeNameText.color = Color.red;
            _attributeValueText.color = Color.red;
        }

        _attributeValueText.text = value.ToString();
    }
}
