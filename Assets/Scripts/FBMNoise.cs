using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
public class FBMNoise : MonoBehaviour
{
    [Tooltip("分型次数")]
    public int octaves = 8;
    [Tooltip("采样频率")]
    public float frequency = 1f;
    [Tooltip("幅度")]
    public float amplitude = 0.5f;
    [Tooltip("频率密度")]
    public float lacunarity = 2f;
    [Tooltip("幅度增益")]
    public float gain = 0.5f;

    public float cellsize = 10;

    public Sprite sprite;
    public Texture2D texture;

    private void Start()
    {

        texture = new Texture2D(512, 512);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                 float grayscale = GenerateFBMNoise((float)x , (float)y , Mathf.PerlinNoise);
               // float grayscale = Mathf.PerlinNoise((float)x/ frequency, (float)y/ frequency);
                texture.SetPixel(x, y, new Color(grayscale, grayscale, grayscale));
            }
        }
        texture.Apply();
        Rect rect = new Rect(default, new Vector2(texture.width, texture.height));
        GetComponent<SpriteRenderer>().sprite=Sprite.Create(texture, rect, default);
    }
    //跟理论上perlin的fbm结果图不一样，很奇怪
    public float GenerateFBMNoise(float x, float y, Func<float, float, float> noise)
    {
        float value = 0;
        float amplitude = this.amplitude;
        float frequency = this.frequency;
        for (int i = 0; i < octaves; i++)
        {
            value += noise(x /frequency, y /frequency) * amplitude;
            frequency *= lacunarity;
            amplitude *= gain;
        }

        return value;
    }

}
