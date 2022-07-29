using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private GameObject sliderCanvas;
    [SerializeField] private Slider tileSlider;
    [SerializeField] private float startingCraftTime;
    [SerializeField] private float currentCraftTime;
    [SerializeField] public RecipeInfo recipeInfo;
    
    private int amountOfWorkers;
    public int amountLeftToCraft;

    public bool hasCompleteRecipe = false;
    public bool hasWorkers = false;
    private bool isCrafting = false;


    private void Start() {
        currentCraftTime = 10f;
    }

    private void Update() {
        if (currentCraftTime > 0) {
            tileSlider.value = currentCraftTime;
            currentCraftTime -= Time.deltaTime * amountOfWorkers;
        }

        if (currentCraftTime < .1f && hasCompleteRecipe && hasWorkers && isCrafting) {
            PopOutNewItemFromRecipe();
        }
    }

    public void UpdateAmountLeftToCraft(ItemInfo itemInfo) {
        amountLeftToCraft = itemInfo.amountRecipeCanCreate;
    }

    public void CheckCanStartCrafting() {
        if (hasCompleteRecipe && hasWorkers && !isCrafting && amountLeftToCraft > 0) {
            StartCrafting();
        } 
    }

    public void IncreaseWorkerCount() {
        amountOfWorkers += 1;
    }

    public void WorkerCountToZero() {
        amountOfWorkers = 0;
    }

    private void StartCrafting() {
        sliderCanvas.SetActive(true);
        isCrafting = true;
        startingCraftTime = recipeInfo.recipeCraftTime;
        currentCraftTime = startingCraftTime;
        tileSlider.maxValue = startingCraftTime;
    }

    public void DoneCrafting() {
        sliderCanvas.SetActive(false);
        hasCompleteRecipe = false;
        // hasWorkers = false;
        isCrafting = false;
    }

    public void PopOutNewItemFromRecipe() {
        Vector3 spawnItemsVector3 = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1);
        Instantiate(recipeInfo.itemInfo.draggableItemPrefab, spawnItemsVector3, transform.rotation);
        isCrafting = false;
        amountLeftToCraft -= 1;

        if (amountLeftToCraft == 0) {
            DoneCrafting();
            GetComponent<Tile>().DoneCraftingDestroyItem();
        }

        CheckCanStartCrafting();
    }
}
