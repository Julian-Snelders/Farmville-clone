using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pricetag
{
        public float price_wood;
        public float price_stone;
        public float price_food;
}

[System.Serializable]
public class BuildingInfo
{
    public int id;
    public float level = 0;
    public float yRot = 0;
    public int connectedGridId;
}
public class Building : MonoBehaviour
{

    public BuildingInfo info;
    public Pricetag price;

    public string objName;
    public bool placed;
    public int baseResourceGain = 1;

    private Resources resources;

     void Awake()
    {
        resources = FindObjectOfType<Resources>(); 
    }

    void Update()
    {
        if (!placed)
        {
            return;
        }

        switch (info.id)
        {
            //Lumberjack
            case 1:
                resources.wood += (baseResourceGain * info.level) * Time.deltaTime;
                return;
                //Stone mason
            case 2:
                resources.stones += (baseResourceGain * info.level) * Time.deltaTime;
                return;
                //Wind mill
            case 3:
                resources.food += (baseResourceGain * info.level) * Time.deltaTime;
                return;
        }
    }

    public void Upgradebuilding()
    {
        info.level += 1;

        resources.wood -= price.price_wood;
        resources.stones -= price.price_stone;
        resources.food -= price.price_food;
    }
}
