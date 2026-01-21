using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Outline : MonoBehaviour
{
    [SerializeField] public Building building;
    [SerializeField] public Resource[] resourceList;
    [SerializeField] public int[] resourceNeeded;
    [SerializeField] RectTransform checkOut;

    bool isOverThis;

    private void Start()
    {
        checkOut = GameObject.Find("CheckOut").GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        if(isOverThis && BuildingManager.instance.deconstructionMode)
        {
            checkOut.anchoredPosition = checkOut.anchorMin + new Vector2(-200, Screen.height);
            BuildingManager.instance.ReplaceDecider(null);
        }
    }

    public void DestroyOutline(Vector2Int loc)
    {
        BuildingManager.instance.spaces[loc.x, loc.y] = null;
        foreach (RectTransform rect in checkOut.GetComponentInChildren<RectTransform>())
        {
            Destroy(rect.gameObject);
            isOverThis = false;
        }
        Destroy(this.gameObject);
    }

    private void OnMouseEnter()
    {
        foreach (RectTransform rect in checkOut.GetComponentInChildren<RectTransform>())
        {
            Destroy(rect.gameObject);
        }

        for (int i = 0; i < resourceList.Length; i++)
        {
            GameObject gameObject = new GameObject(i.ToString());
            RectTransform rect = gameObject.AddComponent<RectTransform>();

            rect.AddComponent<Image>();
            rect.GetComponent<Image>().sprite = resourceList[i].sprite;
            rect.GetComponent<Image>().color = Color.white;

            gameObject.transform.SetParent(checkOut);
            rect.localPosition = new Vector3(-75, 100 * i - ((resourceList.Length - 1) * 50), 0);

            string name = resourceList[i].name + "Num";
            GameObject resourceNum = new GameObject(name);
            TextMeshProUGUI textR = resourceNum.AddComponent<TextMeshProUGUI>();
            textR.text = resourceNeeded[i].ToString();
            textR.fontSize = 75;
            textR.color = Color.gray;
            textR.font = ResourceManager.Instance.font;

            resourceNum.transform.SetParent(checkOut);
            resourceNum.transform.localPosition = new Vector3(100, 100 * i - ((resourceList.Length - 1) * 50) + 10, 0);
            
        }
        
        GameObject top = new GameObject("CheckOutTop");
        RectTransform topRect = top.AddComponent<RectTransform>();
        topRect.AddComponent<Image>();
        topRect.GetComponent<Image>().sprite = ResourceManager.Instance.checkOutBackground[0];
        topRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
        topRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);

        GameObject bottom = new GameObject("CheckOutBottom");
        RectTransform bottomRect = bottom.AddComponent<RectTransform>();
        bottomRect.AddComponent<Image>();
        bottomRect.GetComponent<Image>().sprite = ResourceManager.Instance.checkOutBackground[2];
        bottomRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
        bottomRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);

        topRect.SetParent(checkOut);
        bottomRect.SetParent(checkOut);

        topRect.localPosition = new Vector3(0, resourceList.Length * 85 - ((resourceList.Length - 1) * 35), 0);
        bottomRect.localPosition = new Vector3(0, resourceList.Length * -50 - 35, 0);

        checkOut.GetComponent<Image>().sprite = ResourceManager.Instance.checkOutBackground[1];
        checkOut.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, resourceList.Length * 100);
        checkOut.gameObject.SetActive(true);
        checkOut.anchoredPosition = checkOut.anchorMin + (Vector2)Camera.main.WorldToScreenPoint(this.transform.position) + new Vector2(150, resourceList.Length * 50);

        isOverThis = true;
    }

    private void OnMouseExit()
    {
        checkOut.anchoredPosition = checkOut.anchorMin + new Vector2(-200, Screen.height);
        isOverThis = false;
    }
}
