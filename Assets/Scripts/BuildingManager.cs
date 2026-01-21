using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] public Outline[] outlines;
    [SerializeField] public Object[] connectable;
    [SerializeField] public ResourceTile[] resourceTiles;
    [SerializeField] public UnityEngine.Object[,] spaces;
    [SerializeField] GameObject objectHolder;

    [SerializeField] public float timeTillNextGrowth;
    float timeTillNextGrowthMin = 50;
    float timeTillNextGrowthMax = 150;

    public int buildingType;

    public bool deconstructionMode;

    const int buildingXOffset = 12;
    const int buildingYOffset = 24;
    const int sortingYOffset = 16;

    public static BuildingManager instance;

    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        spaces = new Object[25, 49];
        PopulateTrees();
        timeTillNextGrowth = 0;
    }

    private void FixedUpdate()
    {
        timeTillNextGrowth -= Time.deltaTime;
        if(timeTillNextGrowth < 0)
        {
            timeTillNextGrowth = Random.Range(timeTillNextGrowthMin, timeTillNextGrowthMax);
        }
    }

    public void SetDeconstructionMode(bool deconstructionMode)
    {
        this.deconstructionMode = deconstructionMode;
    }

    private void OnMouseDown()
    {
        if(!GameObject.FindAnyObjectByType<EventSystem>().IsPointerOverGameObject() && buildingType >= 0)
        {
            ReplaceDecider(outlines[buildingType]);
        }
    }

    public void BuildBuildings()
    {
        foreach(Object obj in spaces)
        {
            if (obj != null && obj.GetComponent<Outline>())
            {
                ReplaceDecider(obj.GetComponent<Outline>().building);
                break;
            }
        }
    }

    public void ReplaceDecider(Object objectToReplace)
    {
        if (objectToReplace == null ||objectToReplace.GetComponent<Outline>())
        {
            Vector3 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ReplaceSpace(objectToReplace, loc.x, loc.y);
        }
        else
        {
            Building[] buildings = new Building[outlines.Length];
            for (int i = 0; i < buildings.Length; i++)
            {
                buildings[i] = outlines[i].building;
            }

            int[] buildingCount = new int[buildings.Length];

            for (int x = 0; x < 25; x++)
            {
                for (int y = 48; y >= 0; y--)
                {
                    if (spaces[x, y] && spaces[x, y].GameObject().GetComponent<Outline>())
                    {
                        for (int i = 0; i < buildings.Length; i++)
                        {
                            if (spaces[x, y].GameObject().GetComponent<Outline>().building == buildings[i])
                            {
                                buildingCount[i]++;
                            }
                        }
                    }
                }
            }
            MathManager.Instance.SetBuildingCount(buildingCount);
        }
    }

    public void ReplaceSpace(Object replacement, float x, float y)
    {
        Vector2 loc = new Vector2(x, y);
        
        Vector3Int wholePoint = new Vector3Int(Mathf.CeilToInt(loc.x), Mathf.CeilToInt(loc.y), 0);
        if (loc.x < 0)
        {
            wholePoint = new Vector3Int(Mathf.FloorToInt(loc.x), Mathf.CeilToInt(loc.y), 0);
        }

        // Add 13 so it's above 0
        int bx = wholePoint.x / 2 + buildingXOffset;
        int by = wholePoint.y + buildingYOffset;

        if(replacement == null)
        {
            if (spaces[bx, by] && spaces[bx, by].GetComponent<Outline>())
            {
                spaces[bx, by].GameObject().GetComponent<Outline>().DestroyOutline(new Vector2Int(bx, by));
            }
            else
            {
                spaces[bx, by].GameObject().GetComponent<Building>().DestroyBuilding(new Vector2Int(bx, by));
            }
        }
        else
        {
            spaces[bx, by] = null;

            if (!spaces[bx, by])
            {
                Vector3Int doubleWide = new Vector3Int(RoundToEven(wholePoint.x) - 1, wholePoint.y, 0);
                spaces[bx, by] = Instantiate(replacement, doubleWide, Quaternion.identity);
                spaces[bx, by].GameObject().transform.SetParent(objectHolder.transform);
                SpriteRenderer[] spriteRenderers = spaces[bx, by].GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.sortingLayerName = "Objects";
                    spriteRenderer.sortingOrder -= wholePoint.y + sortingYOffset;
                }
            }

            if (spaces[bx, by] && spaces[bx, by].GameObject().GetComponent<TowerScript>())
            {
                TowerScript tower = spaces[bx, by].GameObject().GetComponent<TowerScript>();

                if (by < 25)
                {
                    tower.above = spaces[bx, by + 1];
                }
                for (int i = 0; i < connectable.Length; i++)
                {
                    if (spaces[bx - 1, by] && spaces[bx - 1, by].name == connectable[i].name + "(Clone)")
                    {
                        tower.left = true;
                        SetAdjacent(bx - 1, by, 1);
                    }

                    if (spaces[bx + 1, by] && spaces[bx + 1, by].name == connectable[i].name + "(Clone)")
                    {
                        tower.right = true;
                        SetAdjacent(bx + 1, by, 0);
                    }

                    if (spaces[bx, by + 1] && spaces[bx, by + 1].name == connectable[i].name + "(Clone)")
                    {
                        tower.top = true;
                        SetAdjacent(bx, by + 1, 3);
                    }

                    if (spaces[bx, by - 1] && spaces[bx, by - 1].name == connectable[i].name + "(Clone)")
                    {
                        tower.bottom = true;
                        SetAdjacent(bx, by - 1, 2);
                    }
                }

                tower.SetTowers();
            }

            if (spaces[bx, by] && spaces[bx, by].GameObject().GetComponent<WallScript>())
            {
                WallScript wall = spaces[bx, by].GameObject().GetComponent<WallScript>();

                for (int i = 0; i < connectable.Length; i++)
                {
                    if (spaces[bx - 1, by] && spaces[bx - 1, by].name == connectable[i].name + "(Clone)")
                    {
                        wall.left = true;
                        SetAdjacent(bx - 1, by, 1);
                    }

                    if (spaces[bx + 1, by] && spaces[bx + 1, by].name == connectable[i].name + "(Clone)")
                    {
                        wall.right = true;
                        SetAdjacent(bx + 1, by, 0);
                    }

                    if (spaces[bx, by + 1] && spaces[bx, by + 1].name == connectable[i].name + "(Clone)")
                    {
                        wall.top = true;
                        SetAdjacent(bx, by + 1, 3);
                    }

                    if (spaces[bx, by - 1] && spaces[bx, by - 1].name == connectable[i].name + "(Clone)")
                    {
                        wall.bottom = true;
                        SetAdjacent(bx, by - 1, 2);
                    }
                }

                wall.SetWalls();
            }
        }
    }

    public void SetBuildingType(int buildingID)
    {
        buildingType = buildingID;
    }

    void SetAdjacent(int x, int y, int d)
    {
        if (spaces[x, y] && (spaces[x, y].GameObject().GetComponent<TowerScript>())) 
        {
            switch(d)
            {
                case 0:
                    spaces[x, y].GameObject().GetComponent<TowerScript>().left = true;
                    break;
                case 1:
                    spaces[x, y].GameObject().GetComponent<TowerScript>().right = true;
                    break;
                case 2:
                    spaces[x, y].GameObject().GetComponent<TowerScript>().top = true;
                    break;
                case 3:
                    spaces[x, y].GameObject().GetComponent<TowerScript>().bottom = true;
                    break;
            }

            spaces[x, y].GameObject().GetComponent<TowerScript>().SetTowers();
        }
        else if (spaces[x, y] && spaces[x, y].GameObject().GetComponent<WallScript>())
        {
            switch (d)
            {
                case 0:
                    spaces[x, y].GameObject().GetComponent<WallScript>().left = true;
                    break;
                case 1:
                    spaces[x, y].GameObject().GetComponent<WallScript>().right = true;
                    break;
                case 2:
                    spaces[x, y].GameObject().GetComponent<WallScript>().top = true;
                    break;
                case 3:
                    spaces[x, y].GameObject().GetComponent<WallScript>().bottom = true;
                    break;
            }

            spaces[x, y].GameObject().GetComponent<WallScript>().SetWalls();
        }
    }

    void PopulateTrees()
    {
        for (int x = 0; x < 8; x++) 
        {
            for (int y = 0; y < 16; y++) 
            {
                if(Random.value < 0.1)
                {
                    ReplaceSpace(resourceTiles[0], 2 * (x -buildingXOffset), y - buildingYOffset);
                }
            }
        }
    }

    int RoundToEven(int number)
    {
        if(number >= 0)
        {
            return ((number / 2) * 2) + 1;
        }  
        else
        {
            return ((number / 2) * 2) + 1;
        }
    }
}
