using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelInfo {

    public string mapTilesName;
    public string mapColorsName;

    public int doorHP;

    public int bossID;

    public Texture2D GetTilesTexture(string basePath)
    {
        string fullPath = basePath + mapTilesName;
        Texture2D map = (Texture2D)Resources.Load(fullPath, typeof(Texture2D));

        return map;
    }

    public Texture2D GetColorsTexture(string basePath)
    {
        string fullPath = basePath + mapColorsName;
        Texture2D map = (Texture2D)Resources.Load(fullPath, typeof(Texture2D));

        return map;
    }
}
