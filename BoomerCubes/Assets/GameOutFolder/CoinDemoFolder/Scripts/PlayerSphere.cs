using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphere : MonoBehaviour
{
    [SerializeField] GameObject coinNumPrefab;

    CoinManager coinManager;
    // Start is called before the first frame update
    void Start()
    {
        coinManager = FindObjectOfType<CoinManager>();
    }
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinManager.AddCoins(other.transform.position, 7);

            Destroy(other.gameObject);

            Destroy(Instantiate(coinNumPrefab, other.gameObject.transform.position, Quaternion.identity), 1f);
        }
    }
}
