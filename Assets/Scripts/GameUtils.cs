using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils
{
    public static class Math
    {
        public static float easeInExpo(float x)
        {
            return x == 0 ? 0 : MathF.Pow(2, 10 * x - 10);
        }

        public static float easeOutExpo(float x)
        {
            return x == 1 ? 1 : 1 - MathF.Pow(2, -10 * x);
        }
    }
}
