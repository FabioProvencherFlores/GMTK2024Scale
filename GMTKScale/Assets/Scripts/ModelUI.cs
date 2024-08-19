using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelUI : MonoBehaviour
{
    [SerializeField]
    ModelTable table;
    public void OnNextButtonClicked()
    {
        table.SelectNext();
    }
}
