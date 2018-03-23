﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A premissive BlockDataMap that lets you sample outside the bounds of the actual map
/// you need to provide a nomral BlockDataMap
/// </summary>
public class PremissiveBlockDataMap {
    private class xzData {
        public float height;
        public List<Pair<BiomeBase, float>> biomes;
    }

    BlockDataMap map;
    BiomeManager biomeManager;
    Dictionary<Vector2Int, xzData> xzDataDict = new Dictionary<Vector2Int, xzData>();

    /// <summary>
    /// Constructor taking a BlockDataMap
    /// </summary>
    /// <param name="map">Map that this map is based on</param>
    public PremissiveBlockDataMap(BlockDataMap map, BiomeManager biomeManager) {
        this.map = map;
        this.biomeManager = biomeManager;
    }

    /// <summary>
    /// Checks if theres is a block at the given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns>bool is empty</returns>
    public bool indexEmpty(Vector3Int index) {
        if (map.checkBounds(index)) {
            return map.mapdata[map.index1D(index)].blockType == BlockData.BlockType.NONE;
        } else {
            Vector3 pos = new Vector3(index.x, index.y, index.z);
            Vector2Int xzPos = new Vector2Int(index.x, index.z);
            xzData xzdata;
            if (!xzDataDict.TryGetValue(xzPos, out xzdata)) {
                xzdata = new xzData();
                xzdata.biomes = biomeManager.getInRangeBiomes(xzPos);
                xzdata.height = ChunkVoxelDataGenerator.calcHeight(pos, xzdata.biomes);
                xzDataDict.Add(xzPos, xzdata);
            }
            return !ChunkVoxelDataGenerator.posContainsVoxel(pos, (int)xzdata.height, xzdata.biomes);
        }
    } 
}
