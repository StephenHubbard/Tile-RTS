using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Economy Manager
    public int currentXP;
    public int xpTillDiscovery;

    // Time
    public float timeLeftInDay;
    public float totalTimeElapsed;

    // Discovery Manager
    public int discoveryIndex;

    // Camera Controller
    public float cameraOrthoSize;

    // Tiles 
    public SerializableDictionary<string, Vector3> tilePositions;
    public SerializableDictionary<string, TileInfo> tileInfo;
    public SerializableDictionary<string, ItemInfo> itemInfo;

    // Items
    public SerializableDictionary<string, Vector3> draggableItemPositions;
    public SerializableDictionary<string, ItemInfo> draggableItemsItemInfo;

    // Clouds
    public SerializableDictionary<string, Vector3> cloudPositions;

    // Placed Items
    public SerializableDictionary<string, ItemInfo> placedItems;
    public SerializableDictionary<string, Vector3> placedItemsPos;

    // Workers
    public SerializableDictionary<string, ItemInfo> draggableItemPopulation;
    public SerializableDictionary<string, Vector3> draggablePopulationPos;

    public SerializableDictionary<string, ItemInfo> placedItemPopulation;
    public SerializableDictionary<string, Vector3> placedItemsPopulationPos;

    // Storage
    public SerializableDictionary<string, ItemInfo> storageItemsItemInfo;
    public SerializableDictionary<string, int> storageItemsAmount;

    // Encyclopedia
    public SerializableDictionary<ItemInfo, bool> discoveredItemsCraftedDict;

    // Population
    public SerializableDictionary<string, int> populationLevels;

    // Enemies
    public SerializableDictionary<string, bool> isSkeletonWarrior;
    public SerializableDictionary<string, Vector3> enemyPos;

    // To Do List
    public SerializableDictionary<string, ItemInfo> toDoList;



    public GameData() {
        // Economy Manager
        this.currentXP = 0;
        this.xpTillDiscovery = 0;

        // Time
        this.timeLeftInDay = 0;
        this.totalTimeElapsed = 0;

        // Discovery Manager
        this.discoveryIndex = 0;

        /// Camera Controller 
        this.cameraOrthoSize = 0;

        // Tiles
        tilePositions = new SerializableDictionary<string, Vector3>();
        tileInfo = new SerializableDictionary<string, TileInfo>();
        itemInfo = new SerializableDictionary<string, ItemInfo>();

        // Items
        draggableItemPositions = new SerializableDictionary<string, Vector3>();
        draggableItemsItemInfo = new SerializableDictionary<string, ItemInfo>();

        // Clouds
        cloudPositions = new SerializableDictionary<string, Vector3>();

        // PlacedItems
        placedItems = new SerializableDictionary<string, ItemInfo>();
        placedItemsPos = new SerializableDictionary<string, Vector3>();

        // Workers 
        draggableItemPopulation = new SerializableDictionary<string, ItemInfo>();
        draggablePopulationPos = new SerializableDictionary<string, Vector3>();

        placedItemPopulation = new SerializableDictionary<string, ItemInfo>();
        placedItemsPopulationPos = new SerializableDictionary<string, Vector3>();

        // Storage
        storageItemsItemInfo = new SerializableDictionary<string, ItemInfo>();
        storageItemsAmount = new SerializableDictionary<string, int>();

        // Encyclopedia
        discoveredItemsCraftedDict = new SerializableDictionary<ItemInfo, bool>();

        // Population
        populationLevels = new SerializableDictionary<string, int>();

        // Enemies
        isSkeletonWarrior = new SerializableDictionary<string, bool>();
        enemyPos = new SerializableDictionary<string, Vector3>();

        // To Do List
        toDoList = new SerializableDictionary<string, ItemInfo>();
    }
}
