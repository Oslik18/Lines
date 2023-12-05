using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public void SaveText(string key, string text)
    {
            PlayerPrefs.SetString(key, text);
    }

    static public string LoadInfo(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key);

        return "0";
    }

}
