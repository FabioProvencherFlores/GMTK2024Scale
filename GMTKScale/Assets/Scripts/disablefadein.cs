using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disablefadein : MonoBehaviour
{
    [SerializeField]
    GameObject go;
    [SerializeField]
    GameObject ggo;

    // Start is called before the first frame update
    void Start()
    {
		ggo.SetActive(false);
		go.SetActive(false);
    }

}
