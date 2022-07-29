using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item")]
public class ItemInfo : ScriptableObject
{
    public new string name;
    public int coinValue;
    public GameObject draggableItemPrefab;
    public GameObject onTilePrefab;
    public TileInfo[] tileInfoValidLocations;

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
