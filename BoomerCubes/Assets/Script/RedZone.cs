using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedZone : MonoBehaviour
{
    //OnTriggerStay: Ayný þekilde bir obje trigger alanýnýn içerisinin durduðu sürece çaðrýlýr.
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
