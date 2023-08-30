using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public enum actionChoices
{
    Rock,//0
    Paper,//1
    Scissors//2
};
public enum winStates
{
    PlayerWin,//0
    ComputerWin,//1
    Tie//2
};

[RequireComponent(typeof(ComputerAI))]
public class RPSManager : MonoBehaviour
{
    private ComputerAI computerAI;
    private int playerChoice;
    private List<actionChoices> playerPastChoices = new List<actionChoices>();

    private void Awake()
    {
        ComputerAI = GetComponent<ComputerAI>();
    }


    /// <summary>
    /// A 3x3 matrix for outcomes with player choice along the Y axis, and computer choice along the Y axis. 
    /// </summary>
    private winStates[,] winMatrix = new winStates[3, 3] 
    {  
        //Computer: Rock,     Paper             Scissors                \Player choice
        { winStates.Tie, winStates.ComputerWin, winStates.PlayerWin},   //Rock
        { winStates.PlayerWin, winStates.Tie, winStates.ComputerWin },  //Paper
        { winStates.ComputerWin, winStates.PlayerWin, winStates.Tie }   //Scissors
    };
    private winStates winStateResult;
    public ComputerAI ComputerAI { get => computerAI; set => computerAI = value; }
    public static event Action<winStates> OnResultsDetermined;

    /// <summary>
    /// Takes player action and computer action, then determines a winner.
    /// </summary>
    public void DetermineWinner()
    {
        
        int AISelection = ComputerAI.TakeAction();
        winStateResult = winMatrix[playerChoice, AISelection];
        PostGameUpdate();
    }

    /// <summary>
    /// Method for player to make a game-action selection;
    /// </summary>
    /// <param name="playerSelection">int representation of actionChoice</param>
    public void PlayerOptionSelect(int playerSelection)
    {
        playerChoice = playerSelection; 
    }

    /// <summary>
    /// Stores relevant results from previous game
    /// </summary>
    private void PostGameUpdate()
    {
        OnResultsDetermined?.Invoke(winStateResult);
        //Save a history of player choices for the AI to learn from
        playerPastChoices.Add((actionChoices)playerChoice);
        ComputerAI.UpdateHistory(playerPastChoices);

    }
 
}
