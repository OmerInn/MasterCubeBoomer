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

        //check if contacted with other cube:diðer küp ile temas edilip edilmediðini kontrol et
        if (otherCube != null &&cube.CubeID>otherCube.CubeID)
        {
            //check if both cubes has same number:her iki küpün de ayný sayýya sahip olup olmadýðýný kontrol et
            if (cube.CubeNumber==otherCube.CubeNumber)
            {
                Vector3 contactPoint = collision.contacts[0].point;

                //check if cubes number less than max number in CubeSpawner:
                //Cube Spawner'da küp sayýsýnýn maksimum sayýdan küçük olup olmadýðýný kontrol edin:
                if (otherCube.CubeNumber < CubeSpawner.Instance.maxCubeNumber)
                {
                    //spawn a new cube as a result:// sonuç olarak yeni bir küp oluþtur
                    Cube newCube = CubeSpawner.Instance.Spawn(cube.CubeNumber * 2, contactPoint + Vector3.up * 1.6f);
                    //push the new cube up and forward://yeni küpü yukarý ve ileri itin:
                    float pushForce = 2.5f;
                    newCube.CubeRigidbody.AddForce(new Vector3(0, .3f, 1f) * pushForce, ForceMode.Impulse);

                    //Add same tarque://Ayný tarque ekleyin:
                    float randomValue = Random.Range(-20f, 20f);
                    Vector3 randomDirection = Vector3.one * randomValue;
                    newCube.CubeRigidbody.AddTorque(randomDirection);
                    #region AddTorque
                    /* ///Kuvvet yalnýzca aktif bir katý cisme uygulanabilir. Bir GameObject etkin deðilse, AddTorque'un hiçbir etkisi yoktur.

                     Bu fonksiyonla uygulanan torklarýn etkileri, çaðrý anýnda toplanýr.Fizik sistemi, efektleri bir sonraki simülasyon çalýþtýrmasý sýrasýnda uygular(SabitUpdate'den sonra veya komut dosyasý açýkça Physics.Simulate yöntemini çaðýrdýðýnda).
                     Bu fonksiyonun farklý modlarý olduðu için, fizik sistemi geçen tork deðerlerini deðil, sadece ortaya çýkan açýsal hýz deðiþimini biriktirir. 
                    DeltaTime'ýn(DT) simülasyon adým uzunluðuna(Time.fixedDeltaTime) eþit olduðunu ve kütlenin torkun uygulandýðý Sert Cismin kütlesine eþit olduðunu varsayarsak, tüm modlar için açýsal hýz deðiþimi þu þekilde hesaplanýr:
                     ForceMode.Force: Girdiyi tork(Newton-metre cinsinden ölçülür) olarak yorumlar ve açýsal hýzý tork *DT / kütle deðeriyle deðiþtirir. Etki, simülasyon adým uzunluðuna ve cismin kütlesine baðlýdýr.
                     ForceMode.Acceleration: Parametreyi açýsal hýzlanma(derece / saniye kare cinsinden ölçülür) olarak yorumlar ve açýsal hýzý tork *DT deðeriyle deðiþtirir. Etki, simülasyon adým uzunluðuna baðlýdýr ancak cismin kütlesine baðlý deðildir.
                     ForceMode.Impulse: Parametreyi açýsal momentum(kilogram - metre - kare / saniye cinsinden ölçülür) olarak yorumlar ve açýsal hýzý tork/ kütle deðeriyle deðiþtirir. Etki, vücudun kütlesine baðlýdýr ancak simülasyon adým uzunluðuna baðlý deðildir.
                     ForceMode.VelocityChange: Parametreyi doðrudan açýsal hýz deðiþimi olarak yorumlar(derece / saniye olarak ölçülür) ve açýsal hýzý tork deðeriyle deðiþtirir. Etki, vücudun kütlesine ve simülasyon adým uzunluðuna baðlý deðildir.
                     Varsayýlan olarak Rigidbody'yi uyandýrýr. Tork boyutu sýfýr ise Rigidbody uyanmayacaktýr.
                     ///*/
                    #endregion
                }

                //the explosion should affeect surroundede cubes too:patlama çevrelenmiþ küpleri de etkilemeli:
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
