using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    // [SerializeField] private TMP_Text coinText;
    [SerializeField] public int currentCoins = 0;
    [SerializeField] public int coinsTillDiscovery = 5;
    [SerializeField] private GameObject spinningCoinPrefab;
    [SerializeField] private Slider slider;

    public static EconomyManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        slider.maxValue = coinsTillDiscovery;
        slider.value = currentCoins;
    }

    private void Update() {
        // coinText.text = currentCoins.ToString();
    }

    public void CheckDiscovery(int amount) {

        int leftoverAmount = (coinsTillDiscovery - currentCoins) - amount;

        currentCoins += amount;
        slider.value = currentCoins;

        if (currentCoins >= coinsTillDiscovery) {
            NewDiscovery();
            slider.maxValue = Mathf.Floor(coinsTillDiscovery * 1.5f);
            coinsTillDiscovery = (int)slider.maxValue;
            currentCoins = 0;
            slider.value = currentCoins;
        }

        if (leftoverAmount < 0) {
            CheckDiscovery(Mathf.Abs(leftoverAmount));
        }
    }

    public void NewDiscovery() {
        DiscoveryManager.instance.DetermineNewDiscovery();
    }

    public void SellItem(GameObject thisObj, int amount, int stackSize) {
        if (thisObj.GetComponent<Stackable>().isSellable == false) { return; }

        if (thisObj.GetComponent<Worker>()) {
            HousingManager.instance.DetectHowManyWorkers();
        }

        AudioManager.instance.Play("Sell");
        GameObject thisCoin = Instantiate(spinningCoinPrefab, thisObj.transform.position + new Vector3(0, 1f, 0), transform.rotation);
        StartCoroutine(DestroyCoinCo(thisCoin));
        CheckDiscovery(amount * stackSize);


        Destroy(thisObj);
    }

    private IEnumerator DestroyCoinCo(GameObject thisCoin) {
        yield return new WaitForSeconds(.7f);
        Destroy(thisCoin);
    }

    public void BuyPack(int amount) {
        currentCoins -= amount;
        AudioManager.instance.Play("Pop");
    }

    
}
