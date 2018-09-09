using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventController
{
    public delegate void NpcSpawnedDelegate(GameObject spawned);
    public static event NpcSpawnedDelegate EventNpcSpawned;

    public delegate void NpcInfectedDelegate(GameObject infected);
    public static event NpcInfectedDelegate EventNpcInfected;

    public delegate void ZombieKilledDelegate(GameObject zombie);
    public static event ZombieKilledDelegate EventZombieKilled;

    public static void ReportNpcSpawned(GameObject spawned)
    {
        if (EventNpcSpawned != null)
        {
            EventNpcSpawned(spawned);
        }
    }

    public static void ReportNpcInfected(GameObject infected)
    {
        if(EventNpcInfected != null)
        {
            EventNpcInfected(infected);
        }
    }

    public static void ReportZombieKilled(GameObject zombie)
    {
        GameObject.DestroyImmediate(zombie);

        if (EventZombieKilled != null)
        {
            EventZombieKilled(zombie);
        }
    }

}
