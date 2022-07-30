using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private int startingCoins = 3;
    [SerializeField] public int currentCoins;

    private void Start() {
        currentCoins = startingCoins;
    }

    private void Update() {
        coinText.text = currentCoins.ToString();
    }

    public void SellItem(int amount) {
        currentCoins += amount;
    }

    public void BuyPack(int amount) {
        currentCoins -= amount;
    }

    
}