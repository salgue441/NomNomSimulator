using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class SimManager : MonoBehaviour
{
    public Warehouse warehouse;
    public List<Collector> collector;
    public List<Explorer> explorer;
    public Food food;

    private readonly Simulation sim = APIHelper.GetSimulation();
    private Queue<StepModel> simulationSteps;
    private bool isSimulationRunning = false;

    /// <summary>
    /// Initializes the simulation environment.
    /// </summary>
    private void Start()
    {
        warehouse.Appearance(sim.storage_location.x, sim.storage_location.y);
        InitializeAgents();

        simulationSteps = new Queue<StepModel>(sim.agent_positions);
        isSimulationRunning = true;
    }

    /// <summary>
    /// Updates the simulation environment.
    /// </summary>
    private void Update()
    {
        if (isSimulationRunning && simulationSteps.Count > 0)
        {
            ProcessStep(simulationSteps.Dequeue());
        }
    }

    /// <summary>
    /// Initializes the agents in the simulation environment.
    /// </summary>
    private void InitializeAgents()
    {
        if (sim.agent_positions.Count > 0)
        {
            StepModel firstStep = sim.agent_positions[0];

            int collector_count = 0;
            int explorer_count = 0;

            foreach (AgentModel agent in firstStep.positions)
            {
                if (agent.type == "collector_")
                    collector[collector_count++].Appearance(agent.x, agent.y);

                else
                    explorer[explorer_count++].Appearance(agent.x, agent.y);
            }
        }
    }

    /// <summary>
    /// Processes a step in the simulation.
    /// </summary>
    private void ProcessStep(StepModel step)
    {
        int explorer_count = 0;
        int collector_count = 0;

        foreach (AgentModel agent in step.positions)
        {
            if (agent.type == "collector_")
                collector[collector_count++].Move(agent.x, agent.y);

            else if (agent.type == "explorer_")
                explorer[explorer_count++].Move(agent.x, agent.y);
            
        }
    }
}

