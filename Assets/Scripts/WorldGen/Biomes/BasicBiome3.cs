﻿using System;
using System.Collections.Generic;
using UnityEngine;

class BasicBiome3 : Biome {
    public BasicBiome3() :
        base(
            //General
            snowHeight: 10,
            //2D noise settings
            frequency2D: 0.001f,
            noiseExponent2D: 3,
            octaves2D: 6,
            //3D noise settings
            Structure3DRate: 0.8f,
            Unstructure3DRate: 0.7f,
            frequency3D: 0.009f,
            //Foliage
            maxTreesPerChunk: 2,
            treeLineLength: 2.0f,
            treeVoxelSize: 1.0f,
            treeThickness: 0.5f,
            treeLeafThickness: 3f,
            grammarRecursionDepth: 4
        ) { }


}