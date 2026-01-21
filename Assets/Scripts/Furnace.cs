using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Furnace : MonoBehaviour
{
    [SerializeField] public Resource[] inputList;
    [SerializeField] public Resource[] outputList;
    [SerializeField] Button convertButton;
    [SerializeField] RectTransform furnaceUI;

    // Start is called before the first frame update
    void Start()
    {
        furnaceUI = GameObject.Find("CheckOut").GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        foreach (RectTransform rect in furnaceUI.GetComponentInChildren<RectTransform>())
        {
            Destroy(rect.gameObject);
        }

        for (int i = 0; i < inputList.Length; i++)
        {
            GameObject gameObject = new GameObject(i.ToString() + "input");
            RectTransform inputRect = gameObject.AddComponent<RectTransform>();

            inputRect.AddComponent<Image>();
            inputRect.GetComponent<Image>().sprite = inputList[i].sprite;
            inputRect.GetComponent<Image>().color = Color.white;

            gameObject.transform.SetParent(furnaceUI);
            inputRect.localPosition = new Vector3(-75, 100 * i - ((inputList.Length - 1) * 50), 0);
        }

        for (int i = 0; i < outputList.Length; i++)
        {
            GameObject gameObject = new GameObject(i.ToString() + "output");
            RectTransform outputRect = gameObject.AddComponent<RectTransform>();

            outputRect.AddComponent<Image>();
            outputRect.GetComponent<Image>().sprite = outputList[i].sprite;
            outputRect.GetComponent<Image>().color = Color.white;

            gameObject.transform.SetParent(furnaceUI);
            outputRect.localPosition = new Vector3(75, 100 * i - ((outputList.Length - 1) * 50), 0);
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

        topRect.SetParent(furnaceUI);
        bottomRect.SetParent(furnaceUI);

        topRect.localPosition = new Vector3(0, inputList.Length * 85 - ((inputList.Length - 1) * 35), 0);
        bottomRect.localPosition = new Vector3(0, inputList.Length * -50 - 35, 0);

        furnaceUI.GetComponent<Image>().sprite = ResourceManager.Instance.checkOutBackground[1];
        furnaceUI.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inputList.Length * 100);
        furnaceUI.gameObject.SetActive(true);
        furnaceUI.anchoredPosition = furnaceUI.anchorMin + (Vector2)Camera.main.WorldToScreenPoint(this.transform.position) + new Vector2(150, inputList.Length * 50);
    }
}
