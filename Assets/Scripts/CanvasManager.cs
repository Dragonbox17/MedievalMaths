using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] RectTransform ResourcePanel;
    [SerializeField] RectTransform BuildingPanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            EnableDisableResourcePanel();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnableDisableBuildingPanel();
        }
    }

    public void EnableDisableResourcePanel()
    {
        if(ResourcePanel.gameObject.activeInHierarchy)
        {
            ResourcePanel.gameObject.SetActive(false);
        }
        else
        {
            ResourcePanel.gameObject.SetActive(true);
        }
    }

    public void EnableDisableBuildingPanel()
    {
        if (BuildingPanel.gameObject.activeInHierarchy)
        {
            BuildingPanel.gameObject.SetActive(false);
        }
        else
        {
            BuildingPanel.gameObject.SetActive(true);
        }
    }
}
