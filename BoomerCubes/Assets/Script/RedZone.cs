using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    //OnTriggerStay: Ayn� �ekilde bir obje trigger alan�n�n i�erisinin durdu�u s�rece �a�r�l�r.
    private void OnTriggerStay(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube != null)
        {
            if (!cube.IsMainCube && cube.CubeRigidbody.velocity.magnitude < .1f)
            {
                Debug.Log("Gameover");
            }
        }
    }
}
