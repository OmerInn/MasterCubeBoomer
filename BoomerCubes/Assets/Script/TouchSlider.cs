using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityAction OnPointerDownEvent;
    public UnityAction <float> OnPointerDragEvent;
    public UnityAction OnPointerUpEvent;

    #region Aciklama
    /// <summary>
    /// Touch Slider : kaydýrýcý deðiþtirilirse diðer komut dosyalarýný bilgilendirmek için 3 olay eklememiz gerekiyor
    /// Unity'nin UI sistemi, bu iki interface'ten de otomatik olarak haberdardýr;
    /// Image objesine mouse/parmak (pointer) ile dokununca otomatik olarak ""OnPointerDown"" fonksiyonunu
    /// , pointer ekrandan çekilince de otomatik olarak ""OnPointerUp"" fonksiyonunu çaðýrýr.
    /// </summary>
    #endregion

    private Slider uiSlider;

    private void Awake()
    {
        uiSlider = GetComponent<Slider>();
        uiSlider.onValueChanged.AddListener(OnSliderValueChanged); 
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnPointerDownEvent != null) 
        {
            OnPointerDownEvent.Invoke();
        }
        if (OnPointerDragEvent != null)
        {
            OnPointerDragEvent.Invoke(uiSlider.value);
        }
    }
    private void OnSliderValueChanged(float value)
    {
        if (OnPointerDragEvent != null)
            OnPointerDragEvent.Invoke(value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnPointerUpEvent != null)
        {
            OnPointerUpEvent.Invoke();
        }
        //reset slider value :
        uiSlider.value = 0f;
    }
    private void OnDestroy()
    {
        //OnDestroy: Obje yok olmadan önceki son frame'inde, tüm Update() fonksiyonlarý çalýþtýrýldýktan sonra gerçekleþir.
        //Objenin yok olmasýnýn sebebi Destroy metodunun kullanýmý olabileceði gibi baþka bir sahneye (scene) geçiþ de olabilir.
        //remove listeners: dinleyicileri kaldýr: bellek sýzýntýlarýný önlemek için
        uiSlider.onValueChanged.RemoveListener(OnSliderValueChanged);

    }
}
