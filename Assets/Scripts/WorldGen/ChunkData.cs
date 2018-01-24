﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A structure containing data on a chunk
/// </summary>
public class ChunkData {

    private Mesh mesh; // Might want to use multiple meshes for each chunk?, as each mesh can only be 53 tall with a chunkSize of 10 (current)
    private Vector3 position;

    /// <summary>
    /// Empty constructor
    /// </summary>
    public ChunkData() {

    }

    /// <summary>
    /// Construct a new chunk
    /// </summary>
    /// <param name="pos">position of the chunk</param>
    public ChunkData(ChunkVoxelData chunkVoxelData) {
        position = chunkVoxelData.chunkPos;

        mesh = new Mesh();
        mesh.vertices = chunkVoxelData.meshData.vertices;
        mesh.triangles = chunkVoxelData.meshData.triangles;
        mesh.colors = chunkVoxelData.meshData.colors;
        mesh.uv = chunkVoxelData.meshData.uvs;
        mesh.uv2 = chunkVoxelData.meshData.uvs2;
        mesh.RecalculateNormals();
    }

    public Vector3 getPos() {
        return position;
    }

    public Mesh getMesh() {
        return mesh;
    }
}