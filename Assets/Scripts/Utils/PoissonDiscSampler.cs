﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/**
 * Implementation of Poisson Dsic Sampling
 * Inspired by https://bl.ocks.org/mbostock/19168c663618b7f07158
 *             http://gregschlom.com/devlog/2014/06/29/Poisson-disc-sampling-Unity.html
 *             http://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf
 */
class PoissonDiscSampler {
    private const int k = 30; // Max number of samples to test for any active list items

    private int radius;

    private int width;
    private int height;
    private bool wrap;

    private List<Vector2Int> activeList;
    private bool[,] grid;

    private System.Random rng;

    /// <summary>
    /// Constructor for a blank sampler
    /// </summary>
    /// <param name="radius">Sampling radius. All points will be minimum 'radius' meters away from eachother, and there will be no position further away than 2 * 'radius' from a point</param>
    /// <param name="width">width of sampling area</param>
    /// <param name="height">height of sampling area</param>
    /// <param name="wrap"></param>
    public PoissonDiscSampler(int radius, int width, int height, bool wrap = false, int seed = 42) {
        this.radius = radius;
        this.width = width;
        this.height = height;
        this.wrap = wrap;

        rng = new System.Random(seed);

        activeList = new List<Vector2Int>();
        grid = new bool[width, height];
    }

    /// <summary>
    /// Constructor for a sampler with a collection of pre-defined points. Good to use if you want to grow an existing sample set.
    /// </summary>
    /// <param name="radius">Sampling radius. All points will be minimum 'radius' meters away from eachother, and there will be no position further away than 2 * 'radius' from a point</param>
    /// <param name="width">width of sampling area</param>
    /// <param name="height">height of sampling area</param>
    /// <param name="preExistingPoints">An array of pre-existing points</param>
    /// <param name="wrap"></param>
    public PoissonDiscSampler(int radius, int width, int height, Vector2Int[] preExistingPoints, int seed = 42) {
        this.radius = radius;
        this.width = width;
        this.height = height;
        this.wrap = false;

        rng = new System.Random(seed);

        activeList = new List<Vector2Int>(preExistingPoints);
        grid = new bool[width, height];
    }


    /// <summary>
    /// Adds a sample to the active and accepted lists.
    /// </summary>
    /// <param name="position">position of sample</param>
    /// <returns></returns>
    private Vector2Int addSample(Vector2Int position) {
        activeList.Add(position);
        grid[position.x, position.y] = true;
        return position;
    }


    /*
     * Using PoissonDiscSampler.sample():
     * ------------------
     * 
     * foreach(Vector2 sample in sampler.sampler())
     *      // Do stuff with sample here
     * 
     * 
     * ------- or -------
     * using System.Linq; // Needed for ToList() function
     * ...
     * Vector2[] samplesAsList = sampler.sample().ToList();
     * // Do stuff with samplesAsList
     * 
     * ------------------
     */

    /// <summary>
    /// Returns a lazy sequence for use in foreach loops.
    /// </summary>
    /// <returns>Lazy sequence for </returns>
    public IEnumerable<Vector2Int> sample() {
        if(activeList.Count == 0) {
            yield return addSample(new Vector2Int(rng.Next(1, width), rng.Next(1, height)));
        }

        while(activeList.Count > 0) {
            int activeSampleIndex = rng.Next(0, activeList.Count);
            Vector2Int activeSample = activeList[activeSampleIndex];
            bool addedNew = false;
            for(int i = 0; i < k; i++) {
                float angle = 2 * Mathf.PI * (float)rng.NextDouble();
                float distance = radius + radius * (float)rng.NextDouble();
                Vector2Int samplePos = activeSample + new Vector2Int((int)(Mathf.Cos(angle) * distance), (int)(Mathf.Sin(angle) * distance));
                if (samplePos.x >= 0 && samplePos.y >= 0 && samplePos.x < width && samplePos.y < height && validateSample(samplePos)) {
                    addedNew = true;
                    yield return addSample(samplePos);
                    break;
                }
            }
            if (!addedNew) {
                activeList.RemoveAt(activeSampleIndex);
            }
        }
    }


    /// <summary>
    /// Checks if the sample clears its neighbourhood
    /// </summary>
    /// <param name="samplePos"></param>
    /// <returns></returns>
    private bool validateSample(Vector2Int samplePos) {
        int gridX = 0;
        int gridY = 0;
        for(int x = samplePos.x - radius; x <= samplePos.x + radius; x++) {
            for(int y = samplePos.y - radius; y <= samplePos.y + radius; y++) {
                gridX = x;
                gridY = y;

                if (wrap) {
                    gridX = Utils.mod(gridX, width);
                    gridY = Utils.mod(gridY, height);
                } else if(x < 0 || y < 0 || x >= width || y >= height) {
                    continue;
                }

                if (grid[gridX, gridY] && Vector2.Distance(new Vector2Int(x, y), samplePos) < radius)
                    return false;
            }
        }
        return true;
    }


}
 
 
 
 
 