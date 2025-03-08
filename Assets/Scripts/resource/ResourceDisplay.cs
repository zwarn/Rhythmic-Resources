using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace resource
{
    public class ResourceDisplay : MonoBehaviour
    {
        public Image icon;
        public TMP_Text amountText;

        public void Initialize(Resource resource)
        {
            icon.sprite = resource.sprite;
            amountText.text = "0";
        }

        public void SetValue(int amount)
        {
            amountText.text = amount.ToString();
        }
    }
}