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
    [SerializeField] Transform target;//uý ekranýnýn en saðýndaki para simgesi için
                                      //paralarý toplayýnca efekt ile oraya aktarýcaz.

    
    [Space]
    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    Queue<GameObject> coinsQueue = new Queue<GameObject>();
    //Þunlardan kaçýnmak için nesne havuzunu kullanacaðýz:
    //Performans açýsýndan kötü bir uygulama olduðu için madeni paralarý sürekli olarak baþlatmak ve yok etmek


    [Space]
    //daha sonra canlandýrmak için paralarý saklamak için sýra
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
            // "Paralar" deðiþkeni her deðiþtirildiðinde kullanýcý arayüzü metnini güncelle
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
    ///ebeveynini "coinmanager"ýn gameobject'ine ayarla
    /// madeni parayý devre dýþý býrak
    ///sýraya ekle
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
    /// kuyrukta bozuk para olup olmadýðýný kontrol et

    ///kuyruktan bir bozuk para çýkar

    /// etkinleþtir(göster)

    ///para pozisyonunu toplamak için pozisyonunu ayarla

    ///onu hedef konuma taþýyýn(canlandýrýn) (sað üst)
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
