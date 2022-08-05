using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item")]
public class ItemInfo : ScriptableObject
{
    public string itemName;
    public int coinValue;
    public GameObject draggableItemPrefab;
    public GameObject onTilePrefab;
    public TileInfo[] tileInfoValidLocations;
    public RecipeInfo recipeInfo;
    public int amountRecipeCanCreate = 1;
    public ItemInfo[] potentialOffSpring;
    public bool isResourceOnly = true;
    public bool isStationary = false;
    public int LeftInStack = 1;
    public bool isSmeltable = false;
    public int foodValue = 0;

    [TextArea]
    public string toolTipText;

    public bool checkValidTiles(TileInfo tileInfo) { 
        foreach (var tile in tileInfoValidLocations)
        {
            if (tile.name == tileInfo.name) {
                return true;
            }
        }

        return false;
    }

}
