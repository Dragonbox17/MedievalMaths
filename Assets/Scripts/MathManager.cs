using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class MathManager : MonoBehaviour
{
    [SerializeField] Sprite[] NumberSprites;
    [SerializeField] Sprite[] FunctionSprites;
    [SerializeField] Sprite[] OperatorSprites;
    [SerializeField] Sprite[] ParenthesesSprites;
    [SerializeField] TMP_InputField textInput;
    [SerializeField] BoxCollider2D background;
    int[] BuildingCount;
    float answer;
    GameObject display;
    int resourceId;

    public static MathManager Instance;
    void Start()
    {
        Instance = this;
    }

    public void SetBuildingCount(int[] numbers)
    {
        BuildingCount = numbers;
        background.enabled = false;
        ConvertNumbers(0);
    }

    void ConvertNumbers(int rl)
    {
        resourceId = rl;
        Resource[] resources = ResourceManager.Instance.resources;

        if (rl < resources.Length)
        {
            int[] resourceCount = new int[BuildingCount.Length];

            for (int bid = 0; bid < BuildingManager.instance.outlines.Length; bid++)
            {
                for (int rc = 0; rc < BuildingManager.instance.outlines[bid].resourceList.Length; rc++)
                {
                    if (BuildingManager.instance.outlines[bid].resourceList[rc] == resources[rl])
                    {
                        resourceCount[bid] += BuildingManager.instance.outlines[bid].resourceNeeded[rc];
                        break;
                    }
                }
            }

            string equation = "";
            float total = 0;

            for (int j = 0; j < BuildingCount.Length; j++)
            {
                if (BuildingCount[j] > 0 && resourceCount[j] > 0)
                {
                    if (equation != "")
                    {
                        equation += string.Format("+({0}*{1})", BuildingCount[j], resourceCount[j]);
                    }
                    else
                    {
                        equation += string.Format("({0}*{1})", BuildingCount[j], resourceCount[j]);
                    }
                    total += BuildingCount[j] * resourceCount[j];
                }
            }

            answer = total;
            equation += "=";


            if (equation != "=")
            {
                display = new GameObject("DisplayEquation");
                display.transform.parent = transform;

                int currentPosition = 50;
                Sprite currentSprite = NumberSprites[0];

                foreach (char c in equation)
                {
                    switch (c)
                    {
                        case '0':
                            currentSprite = NumberSprites[0];
                            break;
                        case '1':
                            currentSprite = NumberSprites[1];
                            break;
                        case '2':
                            currentSprite = NumberSprites[2];
                            break;
                        case '3':
                            currentSprite = NumberSprites[3];
                            break;
                        case '4':
                            currentSprite = NumberSprites[4];
                            break;
                        case '5':
                            currentSprite = NumberSprites[5];
                            break;
                        case '6':
                            currentSprite = NumberSprites[6];
                            break;
                        case '7':
                            currentSprite = NumberSprites[7];
                            break;
                        case '8':
                            currentSprite = NumberSprites[8];
                            break;
                        case '9':
                            currentSprite = NumberSprites[9];
                            break;
                        case '+':
                            currentSprite = FunctionSprites[0];
                            break;
                        case '-':
                            currentSprite = FunctionSprites[1];
                            break;
                        case '*':
                            currentSprite = FunctionSprites[2];
                            break;
                        case '/':
                            currentSprite = FunctionSprites[3];
                            break;
                        case '(':
                            currentSprite = ParenthesesSprites[0];
                            break;
                        case ')':
                            currentSprite = ParenthesesSprites[1];
                            break;
                        case '=':
                            currentSprite = OperatorSprites[0];
                            break;
                    }

                    GameObject gameObject = new GameObject(c.ToString());
                    RectTransform rect = gameObject.AddComponent<RectTransform>();
                    rect.anchoredPosition = rect.anchorMin + new Vector2(currentPosition, 70);

                    rect.AddComponent<Image>();
                    rect.GetComponent<Image>().sprite = currentSprite;
                    rect.GetComponent<Image>().color = Color.white;

                    currentPosition += 300 / 4;
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3 * rect.sizeDelta.x / 4);
                    rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3 * rect.sizeDelta.y / 4);

                    gameObject.transform.SetParent(display.transform);
                }

                TMP_InputField input = Instantiate(textInput, display.transform, false);
                input.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentPosition + 75, 70);
                input.Select();
                var submit = new TMP_InputField.SubmitEvent();
                foreach(Outline outline in BuildingManager.instance.outlines)
                {
                    for(int outlineResource = 0; outlineResource < outline.resourceList.Length; outlineResource++)
                    {
                        if (outline.resourceList[outlineResource] == resources[rl])
                        {
                            if(outlineResource != outline.resourceList.Length - 1)
                            {
                                submit.AddListener(DestroyEquation);
                            }
                            else
                            {
                                submit.AddListener(BuildBuildings);
                            }
                            
                        }
                    }
                }

                input.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3 * input.GetComponent<RectTransform>().sizeDelta.x / 4);
                input.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3 * input.GetComponent<RectTransform>().sizeDelta.y / 4);
                input.pointSize = 60;
                input.onEndEdit = submit;
            }
            else
            {
                ConvertNumbers(resourceId + 1);
            }
        }
    }

    private void DestroyEquation(string Arg0)
    {
        if(Arg0 == answer.ToString())
        {
            Destroy(display.gameObject);
            ConvertNumbers(resourceId + 1);
        }
    }

    private void BuildBuildings(string Arg0)
    {
        if (Arg0 == answer.ToString())
        {
            Destroy(display.gameObject);

            for (int x = 0; x < 25; x++)
            {
                for (int y = 48; y >= 0; y--)
                {
                    if (BuildingManager.instance.spaces[x, y] && BuildingManager.instance.spaces[x, y].GameObject().GetComponent<Outline>())
                    {
                        Resource[] resources = BuildingManager.instance.spaces[x, y].GetComponent<Outline>().resourceList;
                        int[] resourcesNeeded = BuildingManager.instance.spaces[x, y].GetComponent<Outline>().resourceNeeded;
                        Building building = BuildingManager.instance.spaces[x, y].GameObject().GetComponent<Outline>().building;
                        GameObject outline = BuildingManager.instance.spaces[x, y].GameObject();
                        BuildBuilding(resources, resourcesNeeded, (x - 12) * 2, y - 24, building, outline);
                    }
                }
            }
        }
    }

    public void BuildBuilding(Resource[] resourceList, int[] resourceNeeded, int x, int y, Building building, Object outline)
    {
        bool purchased = true;
        for (int r = 0; r < ResourceManager.Instance.resources.Length; r++)
        {
            for (int c = 0; c < resourceList.Length; c++)
            {
                if (ResourceManager.Instance.resources[r] == resourceList[c])
                {
                    if (resourceList[c].amount >= resourceNeeded[c])
                    {
                        resourceList[c].amount -= resourceNeeded[c];
                    }
                    else
                    {
                        purchased = false;
                    }
                }
            }
        }
        if (purchased)
        {
            outline.GetComponent<Outline>().DestroyOutline(new Vector2Int(x / 2 + 13, y + 26));
            BuildingManager.instance.ReplaceSpace(building, x, y);
        }

        background.enabled = true;
    }
}
