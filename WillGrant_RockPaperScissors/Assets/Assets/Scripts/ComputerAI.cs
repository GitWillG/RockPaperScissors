using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;
using UnityEditor;
using TMPro;

/// <summary>
/// Simplistic AI that determines actions based on a raw number of times player has picked an option.
/// </summary>
public class ComputerAI : MonoBehaviour
{
    private List<actionChoices> pastPlayerActions = new List<actionChoices>();
    private int selection;
    private int[] availableActions;
    private int weightTotal;

    public static event Action<int> OnComputerAction;

    /// <summary>
    /// Updates the AI with a new list of previous player choices
    /// </summary>
    /// <param name="previousActions">A list of all previous player choices</param>
    public void UpdateHistory(List<actionChoices> previousActions)
    {
        pastPlayerActions = previousActions;
    }

    /// <summary>
    /// Method for computer to take a game action
    /// </summary>
    /// <returns>Int representing action choice</returns>
    public int TakeAction()
    {
        DetermineAction();
        OnComputerAction?.Invoke(selection);
        return selection;
    }

    /// <summary>
    /// Method for determining action. Random on first turn, utilizing weightings afterwards.
    /// </summary>
    private void DetermineAction()
    {
        if (pastPlayerActions.Count == 0)
        {
            selection = UnityEngine.Random.Range(0, 3);
        }
        else
        {
            GetNewWeighting();
            selection = RandomWeighted();
        }

    }

    /// <summary>
    /// Updates weightings based on player actions
    /// </summary>
    private void GetNewWeighting()
    {
        availableActions = new int[3]; //number of things

        //weighting of each thing, high number means more occurrance
        //more player scissors, means the AI wants to take more Rock actions
        availableActions[(int)actionChoices.Rock] = pastPlayerActions.Count(n => n == actionChoices.Scissors);
        //more player rocks, means the AI wants to take more paper actions
        availableActions[(int)actionChoices.Paper] = pastPlayerActions.Count(n => n == actionChoices.Rock);
        //more player paper, means the AI wants to take more scissors actions
        availableActions[(int)actionChoices.Scissors] = pastPlayerActions.Count(n => n == actionChoices.Paper); ;
        weightTotal = 0;
        foreach (int w in availableActions)
        {
            weightTotal += w;
        }
    }

    /// <summary>
    /// Randomly selects action based on weightings.
    /// </summary>
    /// <returns></returns>
    int RandomWeighted()
    {
        int result = 0, total = 0;
        int randVal = UnityEngine.Random.Range(0, weightTotal + 1);
        for (result = 0; result < availableActions.Length; result++)
        {
            total += availableActions[result];
            if (total >= randVal) break;
        }
        return result;
    }

}
