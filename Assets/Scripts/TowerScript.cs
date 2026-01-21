using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerScript : MonoBehaviour
{
    [SerializeField] Sprite[] leftWall;
    [SerializeField] Sprite[] rightWall;
    [SerializeField] Sprite[] topWall;
    [SerializeField] Sprite[] bottomWall;

    public Object above;

    public bool left, right, top, bottom;

    public void SetTowers()
    {
        int leftInt = 0;
        int rightInt = 0;
        int topInt = 0;
        int bottomInt = 0;

        if (left)
        {
            leftInt++;
        }
        if (right)
        {
            rightInt++;
        }
        if (top)
        {
            topInt++;
            if (above && ((above.GetComponent<Outline>() && above.GetComponent<Outline>().building.GetComponent<TowerScript>()) || (above.GetComponent<TowerScript>()))) 
            {
                leftInt += 2;
                rightInt += 2;
            }
        }
        if (bottom)
        {
            bottomInt++;
        }

        SetTowerSprites(leftInt, rightInt, topInt, bottomInt);
    }

    void SetTowerSprites(int l, int r, int t, int b)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = leftWall[l];
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = rightWall[r];
        transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = topWall[t];
        transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = bottomWall[b];
    }
}
