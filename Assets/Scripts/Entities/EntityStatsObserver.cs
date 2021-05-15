using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EntityStatsObserver
{
    public abstract void ChangedHp(int value);
}
