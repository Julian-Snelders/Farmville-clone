using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridL : MonoBehaviour
{

    public int gridId = 5;
    public bool occupied;
    public Building connectedBuilding;

    void Awake()
    {

        Build b = FindObjectOfType<Build>();
        for(int i = 0; i< b.grid.Length; i++)
        {
            if (b.grid[i].transform == transform)
            {
                gridId = i;
                break;
            }
        }

    }

}
