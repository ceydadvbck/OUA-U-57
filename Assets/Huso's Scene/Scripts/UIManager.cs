using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private BarbarianAction barbarian;
    [SerializeField] private RangerAction ranger;




    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
