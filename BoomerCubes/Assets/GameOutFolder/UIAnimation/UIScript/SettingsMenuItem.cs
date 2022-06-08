using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenuItem : MonoBehaviour
{
    [HideInInspector] public Image img;
     public Sprite openSprite;
     public Sprite closeSprite;
    // [HideInInspector] public Transform trans; //��enin d�n���m�n� trans de�i�keninde �nbelle�e al
    [HideInInspector] public RectTransform rectTrans;

    SettingsMenu settingsMenu;

    //item button
    Button button;

    //index of the item in the hierarchy
    int index;

    public bool temp;
    private void Awake()
    {
        img = GetComponent<Image>();


        //trans = transform;
        rectTrans = GetComponent<RectTransform>();

        settingsMenu = rectTrans.parent.GetComponent<SettingsMenu>();

        //-1 to ignore the main button:-1 ana d��meyi yok saymak i�in
        //GetSiblingIndex : E�er obje bir ebeveyne sahip ise, bu ebeveyn alt�ndaki index de�erini elde etmemizi sa�lar.
        index = rectTrans.GetSiblingIndex() - 1;

        //add click listener
        button = GetComponent<Button>();
        button.onClick.AddListener(OnItemClick);
    }


    void OnItemClick()
    {
        if (!temp) //FALSE DURUMUNU KONTROL EDER.
        {
            img.sprite = openSprite;
            temp = true;
        }
        else
        {
            img.sprite = closeSprite;
            temp = false;
        }
        settingsMenu.OnItemClick(index);

    }
    void OnDestroy()
    {
        //remove click listener to avoid memory leaks
        button.onClick.RemoveListener(OnItemClick);
    }
}
