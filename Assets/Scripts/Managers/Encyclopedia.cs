using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] private Sprite inactiveButtonSprite;
    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Image newDiscoveredItemPrefab;
    [SerializeField] private GameObject encylopediaContainer;
    [SerializeField] public GameObject encylopediaGridLayout;
    [SerializeField] private GameObject newDiscoveredItemsGridLayout;
    [SerializeField] private GameObject itemsThatCanBeCraftedGridLayout;
    [SerializeField] private Image currentInspectedItem;
    [SerializeField] private GameObject backButton;
    [SerializeField] private TMP_Text bottomContainerText;
    [SerializeField] public List<ItemInfo> startingItems = new List<ItemInfo>();
    [SerializeField] public Dictionary<ItemInfo, bool> discoveredItems = new Dictionary<ItemInfo, bool>();
    [SerializeField] private List<ItemInfo> newlyDiscoveredItems = new List<ItemInfo>();
    [SerializeField] private List<ItemInfo> itemsThatCanBeCrafted = new List<ItemInfo>();


    private ToolTipManager toolTipManager;
    private GameObject shownItemsContainer;

    public static Encyclopedia instance;


    private void Awake() {
        toolTipManager = FindObjectOfType<ToolTipManager>();

        shownItemsContainer = GameObject.Find("ShownItemsContainer");

        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        foreach (var item in startingItems)
        {
            AddItemToDiscoveredList(item);
        }

    }

    public void CraftedDiscoveredItem(ItemInfo itemInfo) {
        discoveredItems[itemInfo] = true;
    }

    public void OpenEncylopedia() {
        if (backButton.GetComponent<Image>().enabled == true) {
            BackButton();
            return;
        }

        if (encylopediaContainer.activeInHierarchy) {
            encylopediaContainer.SetActive(false);
            OnPointerExitDelegate();
        } else {
            encylopediaContainer.SetActive(true);
            BackButton();
        }
    }

    // minimize icon button
    public void CloseEncylopedia() {
        encylopediaContainer.SetActive(false);
        OnPointerExitDelegate();
    }

    public void BackButton() {
        backButton.GetComponent<Image>().enabled = false;
        backButton.GetComponent<Button>().enabled = false;
        currentInspectedItem.transform.parent.gameObject.SetActive(false);

        encylopediaGridLayout.SetActive(true);
        itemsThatCanBeCraftedGridLayout.SetActive(false);

        bottomContainerText.text = "Known Recipes:";
        OnPointerExitDelegate();
    }

    public void DisplayWhatRecipesCanBeMade(Transform sender) {
        OnPointerExitDelegate();
        bottomContainerText.text = "Can Craft:";
        currentInspectedItem.transform.parent.gameObject.SetActive(true);

        currentInspectedItem.sprite = sender.transform.GetComponent<UITooltip>().itemInfo.itemSprite;

        encylopediaGridLayout.SetActive(false);
        itemsThatCanBeCraftedGridLayout.SetActive(true);

        itemsThatCanBeCrafted.Clear();

        foreach (Transform child in itemsThatCanBeCraftedGridLayout.transform)
        {
            Destroy(child.gameObject);
        }

        addItemsToPotentialCraftedList(sender.transform.GetComponent<UITooltip>().itemInfo);

        foreach (var item in itemsThatCanBeCrafted)
        {
            NewItemToEncyclopedia(item, itemsThatCanBeCraftedGridLayout.transform, false);
        }

        backButton.GetComponent<Image>().enabled = true;
        backButton.GetComponent<Button>().enabled = true;
        
    }

    private void addItemsToPotentialCraftedList(ItemInfo itemInfo) {
        foreach (var item in itemInfo.potentialOffSpring)
        {
            if (DiscoveryManager.instance.allKnownItems.Contains(item)) {
                itemsThatCanBeCrafted.Add(item);
            }
        }
    }

    public void AddItemToDiscoveredList(ItemInfo newItem) {
        if (!discoveredItems.ContainsKey(newItem)) {
            discoveredItems.Add(newItem, false);
            NewItemToEncyclopedia(newItem, encylopediaGridLayout.transform, false);
            newlyDiscoveredItems.Add(newItem);
            NewItemToEncyclopedia(newItem, newDiscoveredItemsGridLayout.transform, true);
            DiscoveryManager.instance.NewDiscoveredItem(newItem);

            if (newDiscoveredItemsGridLayout.transform.childCount > 7) {
                Destroy(newDiscoveredItemsGridLayout.transform.GetChild(newDiscoveredItemsGridLayout.transform.childCount - 1).gameObject);
            }
        } 
    }


    private void NewItemToEncyclopedia(ItemInfo newItem, Transform parentTransform, bool isNew) {
        GameObject discoveredItem = Instantiate(newDiscoveredItemPrefab.gameObject, transform.position, transform.rotation);
        discoveredItem.transform.SetParent(parentTransform);
        discoveredItem.transform.localScale = Vector3.one;
        if (isNew) { discoveredItem.transform.SetAsFirstSibling(); }
        discoveredItem.GetComponent<Image>().sprite = newItem.itemSprite;
        discoveredItem.GetComponent<UITooltip>().itemInfo = newItem;

        EventTrigger thisHoverTrigger = discoveredItem.GetComponent<EventTrigger>();
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
        hoverEntry.eventID = EventTriggerType.PointerEnter;
        hoverEntry.callback.AddListener((data) => { OnPointerHoverDelegate(discoveredItem.transform); });
        thisHoverTrigger.triggers.Add(hoverEntry);
        
        EventTrigger thisExitTrigger = discoveredItem.GetComponent<EventTrigger>();
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) => { OnPointerExitDelegate(); });
        thisHoverTrigger.triggers.Add(exitEntry);

        if (!isNew && newItem.recipeInfo.neededRecipeItems.Length > 0 && discoveredItems[newItem] == false) {
            discoveredItem.GetComponent<UITooltip>().AddAutoToDoList();
        }
    }


    public void OnPointerHoverDelegate(Transform discoveredItem)
    {
        toolTipManager.HoverOverUI(discoveredItem);
    }

    public void OnPointerExitDelegate()
    {
        foreach (Transform child in shownItemsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }


    public void UnCraftedFilterButton(Transform sender) {
        if (sender.GetComponent<Image>().sprite == activeButtonSprite) {
            UncraftedButtonFilterOff();
            sender.GetComponent<Image>().sprite = inactiveButtonSprite;
            sender.GetChild(0).GetComponent<RectTransform>().offsetMin = new Vector2(sender.GetChild(0).GetComponent<RectTransform>().offsetMin.x, 5);
        } else if (sender.GetComponent<Image>().sprite == inactiveButtonSprite) {
            UncraftedButtonFilterOn();
            sender.GetComponent<Image>().sprite = activeButtonSprite;
            sender.GetChild(0).GetComponent<RectTransform>().offsetMin = new Vector2(sender.GetChild(0).GetComponent<RectTransform>().offsetMin.x, 0);
        }
    }


    private void UncraftedButtonFilterOn() {
        foreach (Transform item in encylopediaGridLayout.transform)
        {
            if (discoveredItems[item.GetComponent<UITooltip>().itemInfo] == true || item.GetComponent<UITooltip>().itemInfo.recipeInfo.neededRecipeItems.Length == 0) {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void UncraftedButtonFilterOff() {
        foreach (Transform item in encylopediaGridLayout.transform)
        {
            item.gameObject.SetActive(true);
        }
    }
}
