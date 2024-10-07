using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CocktailSort : MonoBehaviour
{
    float[] array;
    List<GameObject> mainObjects;
    public GameObject prefab;

    void Start()
    {
        mainObjects = new List<GameObject>();
        array = new float[30000];
        for (int i = 0; i < 30000; i++)
        {
            array[i] = (float)Random.Range(0, 1000) / 100;
        }

        logArray();
        spawnObjs();

        Thread thread2 = new Thread(() => cocktailSort());
        thread2.Start();

    }

    void Update()
    {
        updateHeights();
    }

    // Cocktail Sort algorithm
    void cocktailSort()
    {
        bool swapped = true;
        int start = 0;
        int end = array.Length;

        while (swapped)
        {
            swapped = false;

            // Forward pass: move the larger elements to the right
            for (int i = start; i < end - 1; i++)
            {
                if (array[i] > array[i + 1])
                {
                    (array[i], array[i + 1]) = (array[i + 1], array[i]);
                    swapped = true;
                }
            }

            // If no elements were swapped in the forward pass, the array is sorted
            if (!swapped)
                break;

            // Otherwise, reset swapped for the backward pass
            swapped = false;

            // Shrink the end point for the backward pass
            end--;

            // Backward pass: move the smaller elements to the left
            for (int i = end - 1; i > start; i--)
            {
                if (array[i] < array[i - 1])
                {
                    (array[i], array[i - 1]) = (array[i - 1], array[i]);
                    swapped = true;
                }
            }

            // Increment the starting point for the next forward pass
            start++;
        }
    }


    void logArray()
    {
        string text = "";

        for (int i = 0; i < array.Length; i++)
        {
            text += array[i].ToString();
        }

        Debug.Log(text);
    }

    void spawnObjs()
    {

        for (int i = 0; i < array.Length; i++)
        {
            mainObjects.Add(Instantiate(prefab, new Vector3((float)i / 1000,
                this.gameObject.GetComponent<Transform>().position.y, 0), Quaternion.identity));
        }

    }

    bool updateHeights()
    {

        bool changed = false;
        for (int i = 0; i < array.Length; i++)
        {
            Vector3 newScale = mainObjects[i].transform.localScale;
            newScale.y = array[i];
            mainObjects[i].transform.localScale = newScale;
            changed = true;
        }
        return changed;
    }
}
