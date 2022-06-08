using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SettingsMenu : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing; //boþluk

    [Space]
    [Header("Main button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase; //SetEase( Ease yumusakHareketTuru )
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")] //solma
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;



    Button mainButton;
    SettingsMenuItem[] menuItems;

    ////menü açýldý mý açýlmadý mý
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemsCount;

    void Start()
    {
        //-1 ana düðmeyi sayma
        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }
        mainButton = transform.GetChild(0).GetComponent<Button>();
        mainButton.onClick.AddListener(ToggleMenu);
        mainButton.transform.SetAsLastSibling();
        //setAsLastSibling=ana düðmenin her zaman en üst katmanda olmasýný saðlamak için.

       // mainButtonPosition = mainButton.transform.position;
        mainButtonPosition = mainButton.GetComponent<RectTransform>().anchoredPosition;

        //reset all menu items position to mainButtonPosition:// tüm menü öðelerinin konumunu ana Düðme Konumuna sýfýrla
        ResetPosition();
    }

     void ResetPosition()
        {
            for (int i = 0; i < itemsCount; i++)
            {
            // menuItems[i].trans.position = mainButtonPosition;
            menuItems[i].rectTrans.anchoredPosition = mainButtonPosition;

        }
    }
    void ToggleMenu()
    {
        isExpanded = !isExpanded; //Geniþletilmiþ deðeri aç/kapat

        if (isExpanded)
        {
            //menu opened
            for (int i = 0; i < itemsCount; i++)
            {
                //menuItems[i].trans.position = mainButtonPosition + spacing * (i + 1); //i+1=sýfýrla çarpmamak için
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                //Fade to alpha=1 starting from alpha=0 immediately
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
        }
        else
        {
            //menuclosed
            for (int i = 0; i < itemsCount; i++)
            {
                //  menuItems[i].trans.position = mainButtonPosition;             
                    menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                    //Fade to alpha=0
                    menuItems[i].img.DOFade(0f, collapseFadeDuration);                
            }
            mainButton.transform
                .DORotate(Vector3.forward * 180f, rotationDuration)
                .From(Vector3.zero)
                .SetEase(rotationEase);
        }
    }
    public void OnItemClick(int index)
    {
        //here you can add you logic 
        switch (index)
        {
            case 0:
                //first button              
                Debug.Log("Music");
                break;
            case 1:
                //second button
                Debug.Log("Sounds");
                break;
            case 2:
                //third button
                Debug.Log("Vibration");
                break;
        }
    }
    void OnDestroy()
    {
        //bellek sýzýntýlarýný önlemek için olay dinleyicilerini kaldýrýn
        mainButton.onClick.RemoveListener(ToggleMenu);

    }
}
