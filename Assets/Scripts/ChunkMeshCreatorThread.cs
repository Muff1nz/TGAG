﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// A thread that generates chunkdata based on positions.
/// </summary>
public class ChunkMeshCreatorThread {

    private Thread thread;
    private BlockingQueue<Vector3> orders; //When the main thread puts a position in this queue, the thread generates a mesh for that position.
    private LockingQueue<ChunkData> results; //When this thread makes a mesh for a chunk the result is put in this queue for the main thread to consume.
    private bool run;

    /// <summary>
    /// Constructor that takes the two needed queues, also starts thread excecution.
    /// </summary>
    /// <param name="orders"></param>
    /// <param name="results"></param>
    public ChunkMeshCreatorThread(BlockingQueue<Vector3> orders, LockingQueue<ChunkData> results) {
        this.orders = orders;
        this.results = results;
        run = true;
        thread = new Thread(new ThreadStart(threadRunner)); //This starts running the update function
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
    void threadRunner() {
        while (run) {
            var order = orders.Dequeue();
            var result = new ChunkData(order);
            results.Enqueue(result);
        }
    }

}
