using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventController
{
    public delegate void NpcSpawnedDelegate();
    public static event NpcSpawnedDelegate EventNpcSpawned;

    public delegate void NpcInfectedDelegate();
    public static event NpcInfectedDelegate EventNpcInfected;

    public delegate void ZombieKilledDelegate(GameObject zombie);
    public static event ZombieKilledDelegate EventZombieKilled;

    public static void ReportNpcSpawned()
    {
        if (EventNpcSpawned != null)
        {
            EventNpcSpawned();
        }
    }

    public static void ReportNpcInfected()
    {
        if(EventNpcInfected != null)
        {
            EventNpcInfected();
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
