using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    Cube cube;
    private void Awake()
    {
        cube = GetComponent<Cube>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Cube otherCube = collision.gameObject.GetComponent<Cube>();

        //check if contacted with other cube:di�er k�p ile temas edilip edilmedi�ini kontrol et
        if (otherCube != null &&cube.CubeID>otherCube.CubeID)
        {
            //check if both cubes has same number:her iki k�p�n de ayn� say�ya sahip olup olmad���n� kontrol et
            if (cube.CubeNumber==otherCube.CubeNumber)
            {
                Vector3 contactPoint = collision.contacts[0].point;

                //check if cubes number less than max number in CubeSpawner:
                //Cube Spawner'da k�p say�s�n�n maksimum say�dan k���k olup olmad���n� kontrol edin:
                if (otherCube.CubeNumber < CubeSpawner.Instance.maxCubeNumber)
                {
                    //spawn a new cube as a result:// sonu� olarak yeni bir k�p olu�tur
                    Cube newCube = CubeSpawner.Instance.Spawn(cube.CubeNumber * 2, contactPoint + Vector3.up * 1.6f);
                    //push the new cube up and forward://yeni k�p� yukar� ve ileri itin:
                    float pushForce = 2.5f;
                    newCube.CubeRigidbody.AddForce(new Vector3(0, .3f, 1f) * pushForce, ForceMode.Impulse);

                    //Add same tarque://Ayn� tarque ekleyin:
                    float randomValue = Random.Range(-20f, 20f);
                    Vector3 randomDirection = Vector3.one * randomValue;
                    newCube.CubeRigidbody.AddTorque(randomDirection);
                    #region AddTorque
                    /* ///Kuvvet yaln�zca aktif bir kat� cisme uygulanabilir. Bir GameObject etkin de�ilse, AddTorque'un hi�bir etkisi yoktur.

                     Bu fonksiyonla uygulanan torklar�n etkileri, �a�r� an�nda toplan�r.Fizik sistemi, efektleri bir sonraki sim�lasyon �al��t�rmas� s�ras�nda uygular(SabitUpdate'den sonra veya komut dosyas� a��k�a Physics.Simulate y�ntemini �a��rd���nda).
                     Bu fonksiyonun farkl� modlar� oldu�u i�in, fizik sistemi ge�en tork de�erlerini de�il, sadece ortaya ��kan a��sal h�z de�i�imini biriktirir. 
                    DeltaTime'�n(DT) sim�lasyon ad�m uzunlu�una(Time.fixedDeltaTime) e�it oldu�unu ve k�tlenin torkun uyguland��� Sert Cismin k�tlesine e�it oldu�unu varsayarsak, t�m modlar i�in a��sal h�z de�i�imi �u �ekilde hesaplan�r:
                     ForceMode.Force: Girdiyi tork(Newton-metre cinsinden �l��l�r) olarak yorumlar ve a��sal h�z� tork *DT / k�tle de�eriyle de�i�tirir. Etki, sim�lasyon ad�m uzunlu�una ve cismin k�tlesine ba�l�d�r.
                     ForceMode.Acceleration: Parametreyi a��sal h�zlanma(derece / saniye kare cinsinden �l��l�r) olarak yorumlar ve a��sal h�z� tork *DT de�eriyle de�i�tirir. Etki, sim�lasyon ad�m uzunlu�una ba�l�d�r ancak cismin k�tlesine ba�l� de�ildir.
                     ForceMode.Impulse: Parametreyi a��sal momentum(kilogram - metre - kare / saniye cinsinden �l��l�r) olarak yorumlar ve a��sal h�z� tork/ k�tle de�eriyle de�i�tirir. Etki, v�cudun k�tlesine ba�l�d�r ancak sim�lasyon ad�m uzunlu�una ba�l� de�ildir.
                     ForceMode.VelocityChange: Parametreyi do�rudan a��sal h�z de�i�imi olarak yorumlar(derece / saniye olarak �l��l�r) ve a��sal h�z� tork de�eriyle de�i�tirir. Etki, v�cudun k�tlesine ve sim�lasyon ad�m uzunlu�una ba�l� de�ildir.
                     Varsay�lan olarak Rigidbody'yi uyand�r�r. Tork boyutu s�f�r ise Rigidbody uyanmayacakt�r.
                     ///*/
                    #endregion
                }

                //the explosion should affeect surroundede cubes too:patlama �evrelenmi� k�pleri de etkilemeli:
                Collider[] surroundedCubes = Physics.OverlapSphere(contactPoint, 2f);
                float explosionForce = 400f;
                float explosionRadius = 1.5f;

                foreach (Collider coll in surroundedCubes)
                {
                    if (coll.attachedRigidbody != null)
                        coll.attachedRigidbody.AddExplosionForce(explosionForce, contactPoint, explosionRadius);
                }
                //explosion FX
                fx.Instance.PlayCubeExplosionFX(contactPoint, cube.CubeColor);

                // Destroy the two cubes:
                CubeSpawner.Instance.DestroyCube(cube);
                CubeSpawner.Instance.DestroyCube(otherCube);
            }
        }
    }
}
