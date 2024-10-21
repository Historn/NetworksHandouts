using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameObject computerTCP;
    public GameObject computerUDP;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(computerTCP);
        DontDestroyOnLoad(computerUDP);
    }
}
