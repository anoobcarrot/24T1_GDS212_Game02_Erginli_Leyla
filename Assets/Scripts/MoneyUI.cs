using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    private int totalMoney = 0;

    public void UpdateMoney(int moneyEarned)
    {
        totalMoney += moneyEarned;
        moneyText.text = "$" + totalMoney.ToString();
    }
}

