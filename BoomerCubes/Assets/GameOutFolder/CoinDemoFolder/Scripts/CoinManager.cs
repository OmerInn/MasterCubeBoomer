using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

 [System.Serializable]
public class CoinManager : MonoBehaviour
{

    //References
    [Header("UI references")]
    [SerializeField] TMP_Text coinUIText;
    [SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] Transform target;//u� ekran�n�n en sa��ndaki para simgesi i�in
                                      //paralar� toplay�nca efekt ile oraya aktar�caz.

    
    [Space]
    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    Queue<GameObject> coinsQueue = new Queue<GameObject>();
    //�unlardan ka��nmak i�in nesne havuzunu kullanaca��z:
    //Performans a��s�ndan k�t� bir uygulama oldu�u i�in madeni paralar� s�rekli olarak ba�latmak ve yok etmek


    [Space]
    //daha sonra canland�rmak i�in paralar� saklamak i�in s�ra
    [Header("Animation settings")]
    [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;


    [SerializeField] Ease easeType;
    [SerializeField] float spread;

    public Vector3 targetPosition;

    private int _c = 0;
    public int Coins
    {
        get { return _c; }
        set
        {
            _c = value;
            // "Paralar" de�i�keni her de�i�tirildi�inde kullan�c� aray�z� metnini g�ncelle
            coinUIText.text = Coins.ToString();
        }
    }
    private void Awake()
    {
        targetPosition = target.position;

        PrepareCoins();
    }
    /// <summary>
    /// madeni para
    ///ebeveynini "coinmanager"�n gameobject'ine ayarla
    /// madeni paray� devre d��� b�rak
    ///s�raya ekle
    /// </summary>
    private void PrepareCoins()
    {
        GameObject coin;
        for (int i = 0; i < maxCoins; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.parent = transform;
            coin.SetActive(false);
            coinsQueue.Enqueue(coin);
        }
    }


    /// <summary>
    /// kuyrukta bozuk para olup olmad���n� kontrol et

    ///kuyruktan bir bozuk para ��kar

    /// etkinle�tir(g�ster)

    ///para pozisyonunu toplamak i�in pozisyonunu ayarla

    ///onu hedef konuma ta��y�n(canland�r�n) (sa� �st)
    /// </summary>
    /// <param name="amount"></param>
    void Animate(Vector3 collectedCoinPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //check if there's coins in the pool
            if (coinsQueue.Count > 0)
            {
                //extract a coin from the pool
                GameObject coin = coinsQueue.Dequeue();
                coin.SetActive(true);

                //move coin to the collected coin pos
                coin.transform.position = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);

                //animate coin to target position
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                coin.transform.DOMove(targetPosition, duration)
                .SetEase(easeType)
                .OnComplete(() => {
                    //executes whenever coin reach target position
                    coin.SetActive(false);
                    coinsQueue.Enqueue(coin);

                    Coins++;
                });
            }
        }
    }


public void AddCoins (Vector3 collectedCoinPosition, int amount)
	{
		Animate (collectedCoinPosition, amount);
	}
}
