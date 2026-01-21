using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    [SerializeField] public Object building;
    bool isOverThis;

    private void OnMouseDown()
    {
        if (isOverThis && BuildingManager.instance.deconstructionMode)
        {
            BuildingManager.instance.ReplaceDecider(null);
        }
    }

    public void DestroyBuilding(Vector2Int loc)
    {
        BuildingManager.instance.spaces[loc.x, loc.y] = null;
        Destroy(this.gameObject);
    }

    private void OnMouseEnter()
    {
        isOverThis = true;
    }

    private void OnMouseExit()
    {
        isOverThis = false;
    }
}
