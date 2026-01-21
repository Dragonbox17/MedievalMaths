using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] RectTransform panel;
    [SerializeField] public Resource[] resources;
    [SerializeField] public TMP_FontAsset font;
    [SerializeField] public Sprite[] checkOutBackground;
    TextMeshProUGUI [] number;

    public static ResourceManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        number = new TextMeshProUGUI[10];
        for(int r = 0; r < resources.Length; r++) 
        {
            resources[r].amount = 1000;

            string name = resources[r].name + "Icon";
            GameObject resourceIcon = new GameObject(name);
            RectTransform rect = resourceIcon.AddComponent<RectTransform>();
            if(r < 9)
            {
                rect.anchoredPosition = rect.anchorMin + new Vector2(r + 70, Screen.height - (r * 100) - 210);
            }
            else
            {
                rect.anchoredPosition = rect.anchorMin + new Vector2(r + 350, Screen.height - ((r - 9) * 100) - 210);
            }
            

            rect.AddComponent<Image>();
            rect.GetComponent<Image>().sprite = resources[r].sprite;
            rect.GetComponent <Image>().color = Color.white;

            name = resources[r].name + "Num";
            GameObject resourceNum = new GameObject(name);
            number[r] = resourceNum.AddComponent<TextMeshProUGUI>();
            number[r].text = resources[r].amount.ToString();
            number[r].fontSize = 75;
            number[r].color = Color.gray;
            number[r].font = font;
            resourceNum.transform.position = rect.anchoredPosition + new Vector2(resourceNum.GetComponent<RectTransform>().rect.width / 2 + 70, 10);

            resourceNum.transform.SetParent(panel);
            resourceIcon.transform.SetParent(panel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (number != null) 
        {
            for (int r = 0; r < resources.Length; r++)
            {
                number[r].text = resources[r].amount.ToString();
            }
        }
    }
}
