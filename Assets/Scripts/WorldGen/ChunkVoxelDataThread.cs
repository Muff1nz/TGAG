﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChunkVoxelData {
    public MeshData[] meshData;
    public MeshData[] waterMeshData;
    public Vector3 chunkPos;

    public MeshData[] trees;
    public MeshData[] treeTrunks;
    public Vector3[] treePositions;
}

public enum Task { CHUNK = 0, ANIMAL}

/// <summary>
/// A class representing an order to be done by the thread
/// </summary>
public class Order {
    public Order(Vector3 position, Task task) {
        this.position = position;
        this.task = task;
    }

    public Order(AnimalSkeleton animalSkeleton, Task task) {
        this.animalSkeleton = animalSkeleton;
        this.task = task;
    }

    public Vector3 position; // Used for chunks
    public AnimalSkeleton animalSkeleton; // Used for animals
    public Task task;
}

/// <summary>
/// A class representing the result of a job done by the thread
/// </summary>
public class Result {
    public Task task;
    public ChunkVoxelData chunkVoxelData; //May not be set
    public AnimalSkeleton animalSkeleton; //May not be set
}

/// <summary>
/// A thread that generates chunkdata based on positions.
/// </summary>
public class ChunkVoxelDataThread {

    private Thread thread;
//    private BlockingQueue<Order> orders; //When the main thread puts a position in this queue, the thread generates a mesh for that position.
    private BlockingList<Order> orders; //When the main thread puts a position in this queue, the thread generates a mesh for that position.
    private LockingQueue<Result> results; //When this thread makes a mesh for a chunk the result is put in this queue for the main thread to consume.
    private bool run;

    private ChunkVoxelDataGenerator CVDG = new ChunkVoxelDataGenerator();
    /// <summary>
    /// Constructor that takes the two needed queues, also starts thread excecution.
    /// </summary>
    /// <param name="orders"></param>
    /// <param name="results"></param>
//    public ChunkVoxelDataThread(BlockingQueue<Order> orders, LockingQueue<Result> results) {        
    public ChunkVoxelDataThread(BlockingList<Order> orders, LockingQueue<Result> results) {        
        this.orders = orders;
        this.results = results;
        run = true;
        thread = new Thread(new ThreadStart(threadRunner)); //This starts running the update function
        thread.Start();
    }

    /// <summary>
    /// Returns thread run state.
    /// </summary>
    /// <returns>bool isRunning</returns>
    public bool isRunning() {
        return run;
    }

    /// <summary>
    /// Stops thread excecution.
    /// </summary>
    public void stop() {
        run = false;
    }

    /// <summary>
    /// The function running the thread, processes orders and returns results to main thread.
    /// </summary>
    private void threadRunner() {
        Debug.Log("Thread alive!");
        while (run) {
            try {
                //Order order = orders.Dequeue();
                Order order = orders.Take(getClosestChunkIndex);

                if(order != null && order.position != Vector3.down) {
                    results.Enqueue(handleOrder(order));
                }

            } catch(Exception e) {
                Debug.LogException(e);
            }
        }
        Debug.Log("Thread stopped!");
    }

    /// <summary>
    /// Takes an order for a chunk, and produces the data needed to create the chunk.
    /// </summary>
    /// <param name="order">Vector3 order, location of the chunk</param>
    /// <returns>ChunkVoxelData result, data needed by main thread to create chunk</returns>
    private Result handleOrder(Order order) {
        Result result = new Result();
        result.task = order.task;

        switch (order.task) {
            case Task.CHUNK:
                result.chunkVoxelData = handleChunkOrder(order);
                break;
            case Task.ANIMAL:
                order.animalSkeleton.generateInThread();
                result.animalSkeleton = order.animalSkeleton;
                break;
        }

        return result;
    }

    /// <summary>
    /// Generates a chunk with trees
    /// </summary>
    /// <param name="order">Order order</param>
    /// <returns>ChunkVoxelData</returns>
    private ChunkVoxelData handleChunkOrder(Order order) {
        ChunkVoxelData result = new ChunkVoxelData();
        //Generate the chunk terrain
        result.chunkPos = order.position;
        result.meshData = MeshDataGenerator.GenerateMeshData(CVDG.getChunkVoxelData(order.position));
        result.waterMeshData = WaterMeshDataGenerator.GenerateWaterMeshData(CVDG.getChunkVoxelData(order.position));
        //Generate the trees in the chunk
        System.Random rng = new System.Random(NoiseUtils.Vector2Seed(order.position));
        int trees = Mathf.CeilToInt(((float)rng.NextDouble() * ChunkConfig.maxTreesPerChunk) - 0.5f);
        result.trees = new MeshData[trees];
        result.treeTrunks = new MeshData[trees];
        result.treePositions = new Vector3[trees];

        for (int i = 0; i < trees; i++) {
            Vector3 pos = new Vector3((float)rng.NextDouble() * ChunkConfig.chunkSize, 0, (float)rng.NextDouble() * ChunkConfig.chunkSize);
            pos += order.position;
            pos = Utils.floorVector(pos);
            pos = findGroundLevel(pos);
            pos = Utils.floorVector(pos);
            if (pos != Vector3.negativeInfinity) {
                MeshData[] tree = LSystemTreeGenerator.generateMeshData(pos);
                result.trees[i] = tree[0];
                result.treeTrunks[i] = tree[1];
                result.treePositions[i] = pos;
            } else {
                i--; //Try again
            }
        }

        return result;
    }

    /// <summary>
    /// Finds the groundlevel for the x and z coordinate.
    /// </summary>
    /// <param name="pos">Vector3 position to investigate</param>
    /// <returns>Vector3 ground level position</returns>
    private Vector3 findGroundLevel(Vector3 pos) {
        const int maxIter = 100;
        int iter = 0;

        float height = ChunkVoxelDataGenerator.calcHeight(pos);
        pos.y = (int)height;
        bool lastVoxel = ChunkVoxelDataGenerator.posContainsVoxel(pos);
        bool currentVoxel = lastVoxel;
        int dir = (lastVoxel) ? 1 : -1;
        
        while (iter < maxIter) {
            pos.y += dir;
            lastVoxel = currentVoxel;
            currentVoxel = ChunkVoxelDataGenerator.posContainsVoxel(pos);
            if (lastVoxel != currentVoxel) {
                if (!lastVoxel) { //Put the tree in an empty voxel
                    pos.y -= dir;
                }
                return pos;
            }
            iter++;
        }
        Debug.Log("Failed to find ground");
        return Vector3.negativeInfinity;
    }

    /// <summary>
    /// Used to find the index of the order whos chunk position is closest to the player.
    /// </summary>
    /// <param name="list">list of orders</param>
    /// <returns>index of closest</returns>
    private int getClosestChunkIndex(List<Order> list) {
        int closestIndex = -1;
        float closestDistance = Int32.MaxValue;
        for (int i = 0; i < list.Count; i++) {
            float dist = Vector3.Distance(PlayerMovement.playerPos.get(), list[i].position);
            if (dist < closestDistance) {
                closestIndex = i;
                closestDistance = dist;
            }
        }
        return closestIndex;
    }
}
