using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DataManager : MonoBehaviour
{
    public VariablesState variablesState = null;
    public static DataManager instance;

    private void Awake()
    {
        if(instance == null || instance == this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }

    }

}
