using UnityEngine;
using System.Collections.Generic;

public static class PhysicsCollisionMatrixLayerMasks
{
    private static Dictionary<int, int> _masksByLayer = null;

    private static void Init()
    {
        _masksByLayer = new Dictionary<int, int>();
        for (int i = 0; i < 32; i++)
        {
            int mask = 0;
            for (int j = 0; j < 32; j++)
            {
                if (!Physics.GetIgnoreLayerCollision(i, j))
                {
                    mask |= 1 << j;
                }
            }
            _masksByLayer.Add(i, mask);
        }
    }

    public static int MaskForLayer(int layer)
    {
        if (_masksByLayer == null)
            Init();
        return _masksByLayer[layer];
    }
}
