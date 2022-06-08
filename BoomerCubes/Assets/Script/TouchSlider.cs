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
    /// Touch Slider : kayd�r�c� de�i�tirilirse di�er komut dosyalar�n� bilgilendirmek i�in 3 olay eklememiz gerekiyor
    /// Unity'nin UI sistemi, bu iki interface'ten de otomatik olarak haberdard�r;
    /// Image objesine mouse/parmak (pointer) ile dokununca otomatik olarak ""OnPointerDown"" fonksiyonunu
    /// , pointer ekrandan �ekilince de otomatik olarak ""OnPointerUp"" fonksiyonunu �a��r�r.
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
        //OnDestroy: Obje yok olmadan �nceki son frame'inde, t�m Update() fonksiyonlar� �al��t�r�ld�ktan sonra ger�ekle�ir.
        //Objenin yok olmas�n�n sebebi Destroy metodunun kullan�m� olabilece�i gibi ba�ka bir sahneye (scene) ge�i� de olabilir.
        //remove listeners: dinleyicileri kald�r: bellek s�z�nt�lar�n� �nlemek i�in
        uiSlider.onValueChanged.RemoveListener(OnSliderValueChanged);

    }
}
