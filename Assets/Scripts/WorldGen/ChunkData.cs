﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A structure containing data on a chunk
/// </summary>
public class ChunkData {
    public Vector3 pos;
    public GameObject chunkParent;
    public List<GameObject> terrainChunk = new List<GameObject>();
    public List<GameObject> waterChunk = new List<GameObject>();
    public GameObject[] trees;

    private bool collidersEnabled = false;

    public ChunkData(Vector3 pos) {
        this.pos = pos;
    }

    public ChunkData() {
        
    }

    /// <summary>
    /// Enables colliders for objects in chunk
    /// </summary>
    public void tryEnableColliders() {
        if (!collidersEnabled) {
            collidersEnabled = true;
            //DO SHIT
        }
    } 
}