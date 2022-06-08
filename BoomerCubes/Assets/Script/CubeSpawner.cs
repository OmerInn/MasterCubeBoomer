using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    //Singleton Class
    public static CubeSpawner Instance;
    Queue<Cube> cubesQueue = new Queue<Cube>();
    [SerializeField] private int cubesQueueCapacity = 20;
    [SerializeField] private bool autoQueueGrow = true; //otomatik Kuyruk B�y�mesi

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Color[] cubeColors;

    [HideInInspector] public int maxCubeNumber;
    //in out case it's 4896(2^12)
    private int maxPower = 12;

    private Vector3 defaultSpawnPosition;

    private void Awake()
    {
        Instance = this;

        defaultSpawnPosition = transform.position;
        maxCubeNumber = (int)Mathf.Pow(2, maxPower);

        InitializeCubesQueue();
    }

    private void InitializeCubesQueue()
    {
        for (int i = 0; i < cubesQueueCapacity; i++)
        {
            AddCubeToQueue();
        }
        
    }
    private void AddCubeToQueue()
    {
        Cube cube = Instantiate(cubePrefab, defaultSpawnPosition, Quaternion.identity, transform).GetComponent<Cube>();
        cube.gameObject.SetActive(false);
        cube.IsMainCube = false;
        cubesQueue.Enqueue(cube);
        ///Queue (Kuyruk), ilk giren ilk ��kar i�leyi�ine sahip bir koleksiyondur(FIFO). 
        ///Koleksiyondan bir eleman ��kar�lmak istenildi�inden, kuyru�un en �n�nde yer eleman ��kart�lacakt�r.
        ///Yeni eklenmek istenen bir eleman ise kuyru�un en sonuna eklenecektir.
        ///Enqueue() Metodu; Parametre olarak girilen ��eyi kuyru�un sonuna eklemektedir.
        ///Dequeue() Metodu; Kuyru�un ba��ndaki ��eyi d�nd�r�r ve sonra ��e kuyruktan ��kar�l�r/silinir.
        ///Kuyruk bo�ken Dequeue() metodu �a�r�l�rsa InvalidOperationException f�rlat�r.
    }

    public Cube Spawn(int number , Vector3 position)
    {
        if (cubesQueue.Count==0)
        {
            if (autoQueueGrow)
            {
                cubesQueueCapacity++;
                AddCubeToQueue();
            }
            else
            {
                Debug.LogError("[Cubes Queue] : no more cubes available in the pool");
                return null ;
            }
        }
        Cube cube = cubesQueue.Dequeue();
        cube.transform.position = position;
        cube.SetNumber(number);
        cube.SetColor(GetColor(number));
        cube.gameObject.SetActive(true);

        return cube;
    }
    public Cube SpawnRandom()
    {
        return Spawn(GenerateRandomNumber(), defaultSpawnPosition);
    }

    public void DestroyCube(Cube cube)
    {
        cube.CubeRigidbody.velocity = Vector3.zero;
        cube.CubeRigidbody.angularVelocity = Vector3.zero;
        cube.transform.rotation = Quaternion.identity;
        cube.IsMainCube = false;
        cube.gameObject.SetActive(false);
        cubesQueue.Enqueue(cube);
    }

    public int GenerateRandomNumber()
    {
        return (int)Mathf.Pow(2, Random.Range(1, 6)); 
    }
    private Color GetColor(int number)
    {
        return cubeColors[(int)(Mathf.Log(number) / Mathf.Log(2)) - 1]; //-1(index start with 0) :dizin 0 ile ba�lar

    }

}
