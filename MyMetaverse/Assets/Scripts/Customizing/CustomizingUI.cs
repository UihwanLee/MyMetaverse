using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizingUI : MonoBehaviour
{
    [Header("CustomizingManager")]
    [SerializeField] private CustomizeManager customizeManager;

    [Header("Customizing Button UI")]
    [SerializeField] private Button capBtn;
    [SerializeField] private Button clothesBtn;
    [SerializeField] private Button eyeBtn;
    [SerializeField] private List<Sprite> typeSpriteList;
    [SerializeField] private GameObject customizingUI;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
