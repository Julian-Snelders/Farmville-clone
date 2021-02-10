using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{

    public GridL curSelectedGridElement;
    public GridL curHoveredGridElement;

    public GridL[] grid;

    [Header("color")]
    public Color colOnHover = Color.white;
    public Color colOnOccupied = Color.red;

    public Buildings buildings;

    private RaycastHit mouseHit;

    private bool BuildInProgress;

    private GameObject currentCreatedBuildable;

    private Color colOnNormal;

    void Awake()
    {
        colOnNormal = grid[0].GetComponentInChildren<MeshRenderer>().material.color;

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out mouseHit))
        {
            GridL g = mouseHit.transform.GetComponent<GridL>();

            if (!g)
            {
                if (curHoveredGridElement)
                {
                    curHoveredGridElement.GetComponent<MeshRenderer>().material.color = colOnNormal;
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                curSelectedGridElement = g;
            }

            if (g != curHoveredGridElement)
            {
                if (!g.occupied)
                    mouseHit.transform.GetComponent<MeshRenderer>().material.color = colOnHover;
                else
                    mouseHit.transform.GetComponent<MeshRenderer>().material.color = colOnOccupied;

            }

            if (curHoveredGridElement && curHoveredGridElement != g)

                curHoveredGridElement.GetComponent<MeshRenderer>().material.color = colOnNormal;

            curHoveredGridElement = g;

        }
        else
        {
            if (curHoveredGridElement)
                curHoveredGridElement.GetComponent<MeshRenderer>().material.color = colOnNormal;

        }

        MoveBuilding();
        PlaceBuilding();

    }

    public void OnBtnGreateBuilding(int id)
    {
        if (BuildInProgress)
        {
            return;
        }

        GameObject g = null;

        foreach (GameObject g0 in buildings.buildables)
        {
            Building b = g0.GetComponent<Building>();
            if(b.info.id == id)
            {
                g = b.gameObject;
            }
        }

        currentCreatedBuildable = Instantiate(g);

        currentCreatedBuildable.transform.rotation = Quaternion.Euler(0, transform.rotation.y - 225, 0);

        BuildInProgress = true;
    }

    public void MoveBuilding()
    {
        if (!currentCreatedBuildable)
        {
            return;
        }

        currentCreatedBuildable.gameObject.layer = 2;

        if (curHoveredGridElement)
        {
            currentCreatedBuildable.transform.position = curHoveredGridElement.transform.position;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(currentCreatedBuildable);
            currentCreatedBuildable = null;
            BuildInProgress = false;
        }
        if (Input.GetMouseButton(2))
        {
            currentCreatedBuildable.transform.Rotate(transform.up * 2);
        }
    }

    public void PlaceBuilding()
    {
        if(!currentCreatedBuildable || curHoveredGridElement.occupied)
        return;

        if (Input.GetMouseButtonDown(0))
        {
            buildings.builtObjects.Add(currentCreatedBuildable);
            curHoveredGridElement.occupied = true;

            Building b = currentCreatedBuildable.GetComponent<Building>();

            curHoveredGridElement.connectedBuilding = b;
            b.placed = true;

            b.info.connectedGridId = curHoveredGridElement.gridId;
            b.info.yRot = b.transform.localEulerAngles.y;

            b.Upgradebuilding();

            currentCreatedBuildable = null;
            BuildInProgress = false;
        }
    }

    public void RebuildBuilding(int buildingID, int gridID, int buildingLevel, float rotY)      //rebuild building from saved file for the load game
    {
        GameObject g = null;

        foreach(GameObject gO in buildings.buildables)
        {
            Building b = gO.GetComponent<Building>();
            if(b.info.id == buildingID)
            {
                g = b.gameObject;
            }
        }

        GameObject building = Instantiate(g);
        buildings.builtObjects.Add(building);     //reference script buildings list

        Building loadedBuilding = building.GetComponent<Building>();
        loadedBuilding.info.level = buildingLevel;
        loadedBuilding.placed = true;
        loadedBuilding.info.connectedGridId = gridID;

        GridL myElement = grid[gridID].GetComponent<GridL>();    //what building are we working with
        building.transform.position = myElement.transform.position;    // get their position from saved data and place it their
        building.transform.rotation = Quaternion.Euler(0, rotY, 0);    // impliment their last rotation 
        loadedBuilding.info.yRot = rotY;                               
        myElement.occupied = true;

        myElement.connectedBuilding = loadedBuilding;              //actually place the building


    }


}
