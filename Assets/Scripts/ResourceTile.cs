using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTile : MonoBehaviour
{
    [SerializeField] public Sprite[] sprites;
    [SerializeField] public int numOfApples;
    const int MaxApples = 3;
    const int MinApples = 0;

    // Start is called before the first frame update
    private void Start()
    {
        numOfApples = Random.Range(MinApples, MaxApples);
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprites[numOfApples];
    }

    private void FixedUpdate()
    {
        if(BuildingManager.instance.timeTillNextGrowth <= Time.deltaTime)
        {
            GrowTree();
        }
    }

    public void GrowTree()
    {
        if(numOfApples < 5)
        {
            numOfApples++;
            this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sprites[numOfApples];
        }
    }
}
