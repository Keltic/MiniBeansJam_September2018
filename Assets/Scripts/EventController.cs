using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventController
{
    public delegate void NpcSpawnedDelegate(GameObject spawned, bool isMilitary);
    public static event NpcSpawnedDelegate EventNpcSpawned;

    public delegate void NpcInfectedDelegate(GameObject infected, bool isMilitary);
    public static event NpcInfectedDelegate EventNpcInfected;

    public delegate void ZombieKilledDelegate(GameObject zombie);
    public static event ZombieKilledDelegate EventZombieKilled;

    public static void ReportNpcSpawned(GameObject spawned, bool isMilitary)
    {
        if (EventNpcSpawned != null)
        {
            EventNpcSpawned(spawned, isMilitary);
        }
    }

    public static void ReportNpcInfected(GameObject infected, bool isMilitary)
    {
        if(EventNpcInfected != null)
        {
            EventNpcInfected(infected, isMilitary);
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
