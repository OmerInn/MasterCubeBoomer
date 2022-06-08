using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fx : MonoBehaviour
{
    [SerializeField] public ParticleSystem cubeExplosionFX;
    ParticleSystem.MainModule cubeExplosionFXMainModule;

    //singleton class
    public static fx Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cubeExplosionFXMainModule = cubeExplosionFX.main;
    }

    public void PlayCubeExplosionFX(Vector3 position, Color color)
    {
        Debug.Log("fx");
        cubeExplosionFXMainModule.startColor = new ParticleSystem.MinMaxGradient(color);
        cubeExplosionFX.transform.position = position;
        cubeExplosionFX.Play();
    }

}
