using UnityEngine;
using System.Collections.Generic;

public class AIManager : MonoBehaviour
{
    private static readonly List<IAgentUpdatable> agents = new();
    private static float aiUpdateInterval = 0.1f; // обновляем 10 раз в секунду
    private float nextUpdateTime;

    public static void Register(IAgentUpdatable agent)
    {
        if (!agents.Contains(agent))
            agents.Add(agent);
    }

    public static void Unregister(IAgentUpdatable agent)
    {
        agents.Remove(agent);
    }

    private void Update()
    {
        if (Time.time < nextUpdateTime) return;
        nextUpdateTime = Time.time + aiUpdateInterval;

        foreach (var agent in agents)
        {
            agent.TickAI();
        }
    }
}
