using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField] Sprite[] bottomLeft;
    [SerializeField] Sprite[] bottomRight;
    [SerializeField] Sprite[] topLeft;
    [SerializeField] Sprite[] topRight;

    public bool left, right, top, bottom;

    public void SetWalls()
    {
        int bottomLeftInt = 0;
        int bottomRightInt = 0;
        int topLeftInt = 0;
        int topRightInt = 0;

        if (left)
        {
            bottomLeftInt += 2;
            topLeftInt += 2;
        }
        if (right)
        {
            bottomRightInt += 2;
            topRightInt += 2;
        }
        if (top)
        {
            topLeftInt += 1;
            topRightInt += 1;
        }
        if (bottom)
        {
            bottomLeftInt += 1;
            bottomRightInt += 1;
        }

        SetWallSprites(bottomLeftInt, bottomRightInt, topLeftInt, topRightInt);
    }

    void SetWallSprites(int bl, int br, int tl, int tr)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bottomLeft[bl];
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = bottomRight[br];
        transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = topLeft[tl];
        transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = topRight[tr];
    }
}
