﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureGenerator {
    private Texture2DArray textureArray;
    private int maxTextures;
    private int size;
    private int texturesUsed;
    private string path;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="size"></param>
    /// <param name="maxTextures">Max number of textures that can be added</param>
    /// <param name="path"></param>
    public TextureGenerator(int size, int maxTextures, string path = "textureArray") {
        this.size = size;
        this.maxTextures = maxTextures + 1;
        this.path = path;
        textureArray = new Texture2DArray(size, size, this.maxTextures, TextureFormat.RGBAFloat, false);

        generateEmpty();
    }


    /// <summary>
    /// Generates a texture and stores it.
    /// </summary>
    /// <param name="pixelData">Data to create texture from</param>
    /// <returns>Whether the texture was successfully created and stored.</returns>
    public bool generateTexture(Color[] pixelData) {
        if (texturesUsed == maxTextures)
            return false;

        textureArray.SetPixels(pixelData, texturesUsed);
        texturesUsed++;

        return true;
    }

    /// <summary>
    /// Used to generate one fully transparent texture to be used if modifier == BlockType.NONE
    /// !! Not part of maxTextures!!
    /// </summary>
    private void generateEmpty() {
        Color[] e = new Color[size * size];
        for (int i = 0; i < e.Length; i++)
            e[i] = new Color(0, 0, 0, 0);

        generateTexture(new Color[size * size]);
    }


    /// <returns>How many textures have been created.</returns>
    public int getTexturesUsed() {
        return texturesUsed;
    }

    /// <summary>
    /// Saves the texture array as an asset.
    /// </summary>
    /// <returns>path to the asset, relative to "Resources/Textures/"</returns>
    public string buildAsset() {
        textureArray.Apply();
        AssetDatabase.CreateAsset(textureArray, "Assets/Resources/Textures/" + path);
        return path;
    }


    /// <returns>Returns a texture array with all generated textures.</returns>
    public Texture2DArray getTextureArray() {
        textureArray.Apply();
        return textureArray;
    }

    public bool loadTextureFromFile(string path) {
        Texture2D loadedTexture = Resources.Load<Texture2D>(path);
        if (loadedTexture == null)
            return false;

        return generateTexture(loadedTexture.GetPixels());
    }

}
