using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventController
{
    public delegate void NpcSpawnedDelegate();
    public static event NpcSpawnedDelegate EventNpcSpawned;

    public delegate void NpcInfectedDelegate();
    public static event NpcInfectedDelegate EventNpcInfected;

    public static void ReportNpcSpawned()
    {

    }

    public static void ReportNpcInfected()
    {

    }

}
