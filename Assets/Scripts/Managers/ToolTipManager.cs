using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField] private TMP_Text itemText; 
    [SerializeField] private TMP_Text toolTipText; 
    [SerializeField] private GameObject appleContainer;
    [SerializeField] private GameObject pickaxeContainer;
    [SerializeField] private GameObject coinContainer;
    [SerializeField] private GameObject heartContainer;
    [SerializeField] private GameObject hatchetContainer;
    [SerializeField] private TMP_Text foodValueText;
    [SerializeField] private TMP_Text coinValueText;
    [SerializeField] private TMP_Text heartValueText;
    [SerializeField] private TMP_Text pickaxeValueText;
    [SerializeField] private TMP_Text hatchetValueText;
    [SerializeField] private GameObject toolTipImage;
    [SerializeField] private GameObject maximizeButton;
    [SerializeField] private GameObject minimizeButton;
    [SerializeField] private LayerMask toolTipLayerMask = new LayerMask();
    [SerializeField] private Transform shownItemsContainer;

    public bool isMaximized = true;
    public bool isOverUI = false;

    public static ToolTipManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        ToggleToolTipOff();
    }

    private void Update() {
        RaycastMouseOver();

        if (shownItemsContainer.childCount > 0) {
            toolTipText.text = "Required Components:";
        }
    }

    public void DetectFeedPopTooltip(GameObject go) {
        if (go.GetComponent<Worker>()) {
            string newStr = "You are about to feed worker ";
            UpdateValues("Feed worker?", newStr, 0, 0, 0, 0, 0);
        }

        if (go.GetComponent<Knight>()) {
            string newStr = "You are about to feed Knight ";
            UpdateValues("Feed Knight?", newStr, 0, 0, 0, 0, 0);
        }

        if (go.GetComponent<Archer>()) {
            string newStr = "You are about to feed Archer ";
            UpdateValues("Feed Archer?", newStr, 0, 0, 0, 0, 0);
        }

        // if (go.GetComponent<Population>().isMaxLevel) {
        //     UpdateValues(go.GetComponent<DraggableItem>().itemInfo.itemName, "You cannot level up this" + go.GetComponent<DraggableItem>().itemInfo.itemName + "any more", 0, 0, 0, 0, 0);
        // }

    }

    private void RaycastMouseOver() {
        if (!isMaximized || isOverUI) { return; }

        RaycastHit2D[] hit = Physics2D.RaycastAll(UtilsClass.GetMouseWorldPosition(), Vector2.zero, 100f, toolTipLayerMask);

        if (hit.Length > 0) {
        
            ToggleToolTipOn();

            if (hit[0].transform.GetComponent<DraggableItem>()) {
                ItemInfo thisItem = hit[0].transform.GetComponent<DraggableItem>().itemInfo;
                if (hit[0].transform.GetComponent<Worker>()) {
                    int healthValue = hit[0].transform.GetComponent<Worker>().myHealth;
                    int workStrengthValue = hit[0].transform.GetComponent<Worker>().myWorkingStrength;
                    int hatchetCombatValue = hit[0].transform.GetComponent<Worker>().myCombatValue;
                    string neededFoodToUpgradeStrength;
                    if (hit[0].transform.GetComponent<Population>().isMaxLevel) {
                        neededFoodToUpgradeStrength = "Max Level";
                    } else {
                        neededFoodToUpgradeStrength = "Food until power up: " + hit[0].transform.GetComponent<Worker>().foodNeededToUpPickaxeStrengthCurrent.ToString() + "\n" + "\n" + "basic working unit";
                    }

                    UpdateValues(thisItem.itemName, neededFoodToUpgradeStrength, thisItem.foodValue, thisItem.coinValue, healthValue, workStrengthValue, hatchetCombatValue);
                    return;
                } else if (hit[0].transform.GetComponent<Archer>()) {
                    int healthValue = hit[0].transform.GetComponent<Archer>().myHealth;
                    int workStrengthValue = hit[0].transform.GetComponent<Archer>().myWorkingStrength;
                    int hatchetCombatValue = hit[0].transform.GetComponent<Archer>().myCombatValue;
                    string neededFoodToUpgradeStrength;
                    if (hit[0].transform.GetComponent<Population>().isMaxLevel) {
                        neededFoodToUpgradeStrength = "Max Level";
                    } else {
                        neededFoodToUpgradeStrength = "Food until power up: " + hit[0].transform.GetComponent<Archer>().foodNeededToUpCombatValue.ToString() + "\n" + "\n" + "ranged unit";
                    }
                    UpdateValues(thisItem.itemName, neededFoodToUpgradeStrength, thisItem.foodValue, thisItem.coinValue, healthValue, 0, hatchetCombatValue);
                    return;
                } else if (hit[0].transform.GetComponent<Knight>()) {
                    int healthValue = hit[0].transform.GetComponent<Knight>().myHealth;
                    int workStrengthValue = hit[0].transform.GetComponent<Knight>().myWorkingStrength;
                    int hatchetCombatValue = hit[0].transform.GetComponent<Knight>().myCombatValue;
                    string neededFoodToUpgradeStrength;
                    if (hit[0].transform.GetComponent<Population>().isMaxLevel) {
                        neededFoodToUpgradeStrength = "Max Level";
                    } else {
                        neededFoodToUpgradeStrength = "Food until power up: " + hit[0].transform.GetComponent<Knight>().foodNeededToUpCombatValue.ToString() + "\n" + "\n" + "melee unit";
                    }
                    UpdateValues(thisItem.itemName, neededFoodToUpgradeStrength, thisItem.foodValue, thisItem.coinValue, healthValue, 0, hatchetCombatValue);
                    return;
                } else {
                    UpdateValues(thisItem.itemName, thisItem.toolTipText, thisItem.foodValue, thisItem.coinValue, 0, 0, 0);
                    return;
                }
            } 
            
            else if (hit[0].transform.GetComponent<Tile>()) {
                TileInfo thisTile = hit[0].transform.GetComponent<Tile>().tileInfo;
                if (hit[0].transform.GetComponent<Tile>().currentPlacedItem != null) {
                    string newStr = hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<PlacedItem>().itemInfo.itemName + ": " + hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<PlacedItem>().itemInfo.toolTipText;
                    UpdateValues(thisTile.name, newStr, 0, 0, 0, 0, 0);

                    if (hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<OrcRelic>())
                    {
                        string orcHealthOne = null;
                        string orcHealthTwo= null;

                        if (hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<OrcRelic>().orcSpawnPoints[0].childCount == 1) {
                            orcHealthOne = hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<OrcRelic>().orcSpawnPoints[0].GetChild(0).gameObject.GetComponent<Enemy>().myHealth.ToString();
                        }

                        if (hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<OrcRelic>().orcSpawnPoints.Length > 1) {
                            if (hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<OrcRelic>().orcSpawnPoints[1].childCount == 1) {
                                orcHealthTwo = hit[0].transform.GetComponent<Tile>().currentPlacedItem.GetComponent<OrcRelic>().orcSpawnPoints[1].GetChild(0).gameObject.GetComponent<Enemy>().myHealth.ToString();
                            }
                        }

                        if (orcHealthOne != null && orcHealthTwo != null) {
                            UpdateValues(thisTile.name, "Orc 1 Health: " + orcHealthOne + "\n" + "Orc 2 Health: " + orcHealthTwo, 0, 0, 0, 0, 0);
                        } else if (orcHealthOne != null && orcHealthTwo == null) {
                            UpdateValues(thisTile.name, "Orc 1 Health: " + orcHealthOne, 0, 0, 0, 0, 0);
                        } else if (orcHealthOne == null && orcHealthTwo != null) {
                            UpdateValues(thisTile.name,"Orc 2 Health: " + orcHealthTwo, 0, 0, 0, 0, 0);
                        }
                    }

                    return;
                } else {
                    UpdateValues(thisTile.name, null, 0, 0, 0, 0, 0);
                    return;
                }
            } 
        } else {
            ToggleToolTipOff();
        }
    }

    public void ToggleToolTipOn() {
        itemText.gameObject.SetActive(true);
        toolTipText.gameObject.SetActive(true);
        appleContainer.gameObject.SetActive(true);
        coinContainer.gameObject.SetActive(true);
        heartContainer.gameObject.SetActive(true);
        pickaxeContainer.gameObject.SetActive(true);
        hatchetContainer.gameObject.SetActive(true);
    }

    public void ToggleToolTipOff() {
        itemText.gameObject.SetActive(false);
        toolTipText.gameObject.SetActive(false);
        appleContainer.gameObject.SetActive(false);
        coinContainer.gameObject.SetActive(false);
        heartContainer.gameObject.SetActive(false);
        pickaxeContainer.gameObject.SetActive(false);
        hatchetContainer.gameObject.SetActive(false);
    }

    public void UpdateValues(string itemText, string toolTipText, int foodValue, int coinValue, int heartValue, int pickaxeValue, int hatchetValue) {


        this.itemText.text = itemText;
        this.toolTipText.text = toolTipText;
        if (foodValue > 0) {
            this.foodValueText.text = foodValue.ToString();
        } else {
            appleContainer.SetActive(false);
        }
        // if (coinValue > 0) {
        //     this.coinValueText.text = coinValue.ToString();
        // } else {
        //     coinContainer.SetActive(false);
        // }
        // currently disabled xp/coin container on tooltip
        coinContainer.SetActive(false);
        if (heartValue > 0) {
            this.heartValueText.text = heartValue.ToString();
        } else {
            heartContainer.SetActive(false);
        }
        if (pickaxeValue > 0) {
            this.pickaxeValueText.text = pickaxeValue.ToString();
        } else {
            pickaxeContainer.SetActive(false);
        }
        if (hatchetValue > 0) {
            this.hatchetValueText.text = hatchetValue.ToString();
        } else {
            hatchetContainer.SetActive(false);
        }
    }

    public void MinimizeWindow() {
        ToggleToolTipOff();
        toolTipImage.SetActive(false);
        maximizeButton.SetActive(true);
        minimizeButton.SetActive(false);
        isMaximized = false;
        isOverUI = false;
        AudioManager.instance.Play("UI Click");
    }

    public void MaximizeWindow() {
        // ToggleToolTipOn();
        toolTipImage.SetActive(true);
        maximizeButton.SetActive(false);
        minimizeButton.SetActive(true);
        isMaximized = true;
        AudioManager.instance.Play("UI Click");
    }

    // event listener in inspector
    public void HoverOverUI(Transform sender) {
        isOverUI = true;
        
        if (!isMaximized) { return; }

        ToggleToolTipOn();
        UpdateValues(sender.GetComponent<UITooltip>().toolTipName, sender.GetComponent<UITooltip>().toolTipText, 0, 0, 0, 0, 0);
        sender.GetComponent<UITooltip>().UpdateEncyclopediaToolTip();
    }

    // event listener in inspector
    public void ExitUI() {
        isOverUI = false;
        ToggleToolTipOff();
    }

}
