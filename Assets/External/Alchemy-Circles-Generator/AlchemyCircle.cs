using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlchemyCirclesGenerator
{
    public class AlchemyCircle : MonoBehaviour
    {
        [SerializeField] private int thickness = 4;  // small 4, big 2
        [SerializeField] private int size = 256;     // small 64, big 128
        private Color color = Color.white;
        private Color backgroundColor = new Color(0, 0, 0, 0);

        private RawImage ri;
        private Texture2D texture;
        private Color[] resetMatrix;

        private void Awake()
        {
            texture = new Texture2D(size, size, TextureFormat.ARGB32, false, false);
            ri = GetComponent<RawImage>();

            resetMatrix = new Color[size * size];
            for (int i = 0; i < size * size; i++)
                resetMatrix[i] = backgroundColor;
        }

        private void OnEnable()
        {
            Transmute();
        }

        public void Transmute()
        {
            texture.SetPixels(resetMatrix);

            Generate(Random.Range(0, 9999998));

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            ri.texture = texture;
        }

        private void Generate(int id)
        {
            CiaccoRandom randomer = new CiaccoRandom();
            randomer.SetSeed(id);

            // draw the hexagon:
            // hexagon's center coordinates and radius
            float hex_x = size / 2;
            float hex_y = size / 2;
            float radius = ((size / 2f) * 3f / 4f);

            TextureDraw.drawCircle(texture, size / 2, size / 2, (int)radius, color, thickness);

            int lati = randomer.GetRand(4, 8);

            TextureDraw.drawPolygon(texture, lati, (int)radius, 0, size, color, thickness);

            int l;
            float ang;

            for (l = 0; l < lati; l++)
            {
                ang = Mathf.Deg2Rad * ((360 / (lati))) * l;
                TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + radius * Mathf.Cos(ang)), (int)((size / 2) + radius * Mathf.Sin(ang)), color, thickness);
            }

            int latis;

            if (lati % 2 == 0)
            {
                latis = randomer.GetRand(2, 6);

                if (latis % 2 != 0) latis = 4 + randomer.GetRand(0, 1) * 2;

                TextureDraw.drawFilledPolygon(texture, latis, (int)radius, 180, size, color, backgroundColor, thickness);

                for (l = 0; l < latis; l++)
                {
                    ang = Mathf.Deg2Rad * ((360 / latis)) * l;
                    TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + radius * Mathf.Cos(ang)), (int)((size / 2) + radius * Mathf.Sin(ang)), color, thickness);
                }
            }
            else
            {
                latis = randomer.GetRand(2, 6);
                if (latis % 2 == 0) latis = 3 + randomer.GetRand(0, 1) * 2;

                TextureDraw.drawFilledPolygon(texture, latis, (int)radius, 180, size, color, backgroundColor, thickness);
            }

            if (randomer.GetRand(0, 1) % 2 == 0)
            {
                int ronad = randomer.GetRand(0, 4);

                if (ronad % 2 == 1)
                {
                    for (l = 0; l < lati + 4; l++)
                    {
                        ang = Mathf.Deg2Rad * ((360 / (lati + 4))) * l;
                        TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Cos(ang)), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Sin(ang)), color, thickness);
                    }

                    TextureDraw.drawFilledPolygon(texture, lati + 4, (int)(radius / 2), 0, size, color, backgroundColor, thickness);
                }
                else if (ronad % 2 == 0)
                {
                    for (l = 0; l < lati - 2; l++)
                    {
                        ang = Mathf.Deg2Rad * ((360 / (lati - 2))) * l;
                        TextureDraw.drawLine(texture, (size / 2), (size / 2), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Cos(ang)), (int)((size / 2) + (((radius / 8) * 5) + 2) * Mathf.Sin(ang)), color, thickness);
                    }

                    TextureDraw.drawFilledPolygon(texture, lati - 2, (int)(radius / 4), 0, size, color, backgroundColor, thickness);
                }
            }

            if (randomer.GetRand(0, 4) % 2 == 0)
            {
                TextureDraw.drawCircle(texture, size / 2, size / 2, (int)((radius / 16f) * 11f), color, thickness);

                if (lati % 2 == 0)
                {
                    latis = randomer.GetRand(2, 8);

                    while (latis % 2 != 0) latis = randomer.GetRand(3, 8);

                    TextureDraw.drawPolygon(texture, latis, (int)((radius / 3) * 2), 180, size, color, thickness);
                }
                else
                {
                    latis = randomer.GetRand(2, 8);

                    while (latis % 2 == 0) latis = randomer.GetRand(3, 8);

                    TextureDraw.drawPolygon(texture, latis, (int)((radius / 3) * 2), 180, size, color, thickness);
                }
            }

            int caso = randomer.GetRand(0, 3);
            float angdiff, posax, posay;
            if (caso == 0)
            {
                for (int i = 0; i < latis; i++)
                {
                    angdiff = (Mathf.Deg2Rad * (360 / (latis)));
                    posax = (((radius / 18) * 11) * Mathf.Cos(i * angdiff));
                    posay = (((radius / 18) * 11) * Mathf.Sin(i * angdiff));
                    TextureDraw.drawFilledCircle(texture, (int)(size / 2 + posax), (int)(size / 2 + posay), (int)((radius / 44) * 6), color, backgroundColor, thickness);
                }
            }
            else if (caso == 1)
            {
                for (int i = 0; i < latis; i++)
                {
                    angdiff = (Mathf.Deg2Rad * (360 / latis));
                    posax = (radius * Mathf.Cos(i * angdiff));
                    posay = (radius * Mathf.Sin(i * angdiff));
                    TextureDraw.drawFilledCircle(texture, (int)(size / 2 + posax), (int)(size / 2 + posay), (int)((radius / 44) * 6), color, backgroundColor, thickness);
                }
            }
            else if (caso == 2)
            {
                TextureDraw.drawCircle(texture, size / 2, size / 2, (int)((radius / 18) * 6), color, thickness);
                TextureDraw.drawFilledCircle(texture, size / 2, size / 2, (int)((radius / 22) * 6), color, backgroundColor, thickness);
            }
            else if (caso == 3)
            {
                for (int i = 0; i < latis; i++)
                {
                    ang = Mathf.Deg2Rad * ((360 / (latis))) * i;
                    TextureDraw.drawLine(texture, (int)((size / 2) + ((radius / 3) * 2) * Mathf.Cos(ang)), (int)((size / 2) + ((radius / 3) * 2) * Mathf.Sin(ang)), (int)((size / 2) + radius * Mathf.Cos(ang)), (int)((size / 2) + radius * Mathf.Sin(ang)), color, thickness);
                }
                if (latis != lati)
                {
                    TextureDraw.drawFilledCircle(texture, size / 2, size / 2, (int)((radius / 3) * 2), color, backgroundColor, thickness);
                    lati = randomer.GetRand(3, 6);
                    TextureDraw.drawPolygon(texture, lati, (int)((radius / 4) * 5), 0, size, color, thickness);
                    TextureDraw.drawPolygon(texture, lati, (int)((radius / 3) * 2), 180, size, color, thickness);
                }
            }
        }
    }
}
