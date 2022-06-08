using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pushForce;
    [SerializeField] private float cubeMaxPosX;
    [Space] //bölümlere ayýrmak için kullanýlýr
    [SerializeField] private TouchSlider touchSlider;
    private bool canMove;
    private Cube mainCube;

    private bool isPointerDown;
    private Vector3 cubePos;

    private void Start()
    {
        //TODO:Spawn new cube
        SpawnCube();
        canMove = true;
        //listen to slider events
        // Listen to slider events:
        touchSlider.OnPointerDownEvent += OnPointerDown;
        touchSlider.OnPointerDragEvent += OnPointerDrag;
        touchSlider.OnPointerUpEvent += OnPointerUp;

    }
    private void Update()
    {
        if (isPointerDown)
        {
            mainCube.transform.position = Vector3.Lerp(mainCube.transform.position,cubePos,moveSpeed*Time.deltaTime);
        }
    }
    private void OnPointerDown()
    {
        isPointerDown = true;
    }

    private void OnPointerDrag(float xMovement)
    {        
        if (isPointerDown)
        {
            cubePos = mainCube.transform.position;
            cubePos.x = xMovement * cubeMaxPosX;
        }
    }

  
    private void OnPointerUp()
    {
        if (isPointerDown)
        {
            isPointerDown = false;
            canMove = false;
            //push the cube;
            mainCube.CubeRigidbody.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);

            //TODO:Spawn a new Cube after 0.3 seconds:  0,3 saniye sonra yeni bir Küp oluþtur:
            Invoke("SpawnNewCube", 0.3f);
        }
    }

    private void SpawnNewCube()
    {
        mainCube.IsMainCube = false;
        canMove = true;
        SpawnCube();    
    }
    
    private void SpawnCube()
    {
        mainCube = CubeSpawner.Instance.SpawnRandom();
        mainCube.IsMainCube = true;

        //reset cubepos variable--cubepos deðiþkenini sýfýrla

        cubePos = mainCube.transform.position;
    }

    private void OnDestroy()
    {
        //remove listeners: 
        touchSlider.OnPointerDownEvent -= OnPointerDown;
        touchSlider.OnPointerDragEvent -= OnPointerDrag;
        touchSlider.OnPointerUpEvent -= OnPointerUp;

    }
}
