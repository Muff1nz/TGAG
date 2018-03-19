﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour {
    // Values are needed in the CVDTs for calculating order priority
    public static ThreadSafeVector3 playerPos = new ThreadSafeVector3();
    public static ThreadSafeVector3 playerRot = new ThreadSafeVector3();
    public static ThreadSafeVector3 playerSpeed = new ThreadSafeVector3();

    public GameObject magicTrailPrefab;

    private GameObject magicTrail;
    private GameObjectPool[] animalPool;

    private Rigidbody rb;

    // Use this for initialization
    void Start() {
        ChunkManager cm = GameObject.FindObjectOfType<ChunkManager>();
        cm.player = transform;

        if (magicTrailPrefab != null) {
            magicTrail = Instantiate(magicTrailPrefab);
            magicTrail.SetActive(false);
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            shootMagicTrail();
        }

        playerPos.set(transform.position);
        playerRot.set(transform.rotation * Vector3.forward);
        playerSpeed.set(rb.velocity);
    }

    /// <summary>
    /// Sets the animals list for the player
    /// </summary>
    /// <param name="animals">Animals</param>
    public void initPlayer(GameObjectPool[] animals) {
        this.animalPool = animals;
    }

    /// <summary>
    /// Transfers the important data for this script to another instance
    /// </summary>
    /// <param name="magicTrail">The magic trail</param>
    /// <param name="animals">The list of all animals</param>
    public void transferPlayer(GameObject magicTrail, GameObjectPool[] animals) {
        this.magicTrail = magicTrail;
        this.animalPool = animals;
    }

    /// <summary>
    /// Shoots a magic trail, the trail allows the player to become other animals
    /// </summary>
    private void shootMagicTrail() {
        const float maxAngle = 40;
        const float maxDist = 50;

        float bestAngle = 9999;
        int bestIndex = -1;
        int bestPool = -1;

        for (int pool = 0; pool < animalPool.Length; pool++) {
            List<GameObject> animals = animalPool[pool].activeList;
            for (int i = 0; i < animals.Count; i++) {
                if (animals[i].activeSelf) {
                    float angle = Vector3.Angle(Camera.main.transform.forward, animals[i].transform.position - Camera.main.transform.position);
                    if (angle < bestAngle && Vector3.Distance(transform.position, animals[i].transform.position) < maxDist) {
                        bestAngle = angle;
                        bestIndex = i;
                        bestPool = pool;
                    }
                }
            }
        }

        if (bestIndex != -1 && bestAngle < maxAngle) {
            if (!magicTrail.activeSelf) {
                StartCoroutine(moveMagicTrail(animalPool[bestPool].activeList[bestIndex]));
                animalPool[bestPool].activeList[bestIndex] = gameObject;
            }
        }
    }

    /// <summary>
    /// Moves the magic trail, and triggers player transfer when done
    /// </summary>
    /// <param name="target">Target to move towards</param>
    /// <returns></returns>
    private IEnumerator moveMagicTrail(GameObject target) {
        magicTrail.SetActive(true);
        Camera.main.GetComponent<CameraController>().target = magicTrail.transform;
        for (float t = 0; t <= 1f; t += Time.deltaTime) {
            magicTrail.transform.position = Vector3.Lerp(transform.position, target.transform.position, t);
            yield return 0;
        }
        yield return new WaitForSeconds(1f);
        magicTrail.SetActive(false);
        becomeOtherAnimal(target);
    }

    /// <summary>
    /// Function that transfers the player to another animal
    /// </summary>
    /// <param name="other">Animal to become</param>
    private void becomeOtherAnimal(GameObject other) {
        Animal myAnimal = GetComponent<Animal>();
        Animal otherAnimal = other.GetComponent<Animal>();

        AnimalUtils.addAnimalBrainPlayer(otherAnimal);
        AnimalBrainNPC otherBrain = (AnimalBrainNPC)AnimalUtils.addAnimalBrainNPC(myAnimal);
        otherBrain.takeOverPlayer();

        other.AddComponent<Player>().transferPlayer(magicTrail, animalPool);

        Camera.main.GetComponent<CameraController>().target = other.transform;

        Destroy(GetComponent<Player>());
    }    
}