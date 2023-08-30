using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Image thisSprite;
    private int wins;
    private int losses;
    private int ties;

    [SerializeField]private GameObject computerSpriteObject;
    [SerializeField]private Sprite[] actionSprites = new Sprite[3];
    [SerializeField]private TextMeshProUGUI playerWinText;
    [SerializeField]private TextMeshProUGUI computerWinText;
    [SerializeField]private TextMeshProUGUI tieText;
    [SerializeField]private TextMeshProUGUI resultText;

    private void Awake()
    {
        thisSprite = computerSpriteObject.GetComponent<Image>();
    }

    #region subscription and unsubscription from computer actions
    void OnEnable()
    {
        ComputerAI.OnComputerAction += DisplaySprite;
        RPSManager.OnResultsDetermined += UpdateTextResults;
    }

    void OnDisable()
    {
        ComputerAI.OnComputerAction -= DisplaySprite;
        RPSManager.OnResultsDetermined -= UpdateTextResults;
    }
    #endregion

    /// <summary>
    /// Displays the given sprite
    /// </summary>
    /// <param name="actionNumber">The matching sprite from actionChoices</param>
    public void DisplaySprite(int actionNumber)
    {
        thisSprite.enabled = true;
        thisSprite.sprite = GetCorrectSprite(actionNumber);
    }

    /// <summary>
    /// Helper method for getting and returning selected sprite from internal list of sprites.
    /// </summary>
    /// <param name="selectedSprite">Desired sprite in actionSprites</param>
    /// <returns>Selected Sprite from List</returns>
    public Sprite GetCorrectSprite(int selectedSprite)
    {
        return actionSprites[selectedSprite];
    }

    /// <summary>
    /// Updates UI Text Elements
    /// </summary>
    /// <param name="Outcome">Outcome of the game</param>
    private void UpdateTextResults(winStates Outcome)
    {

        resultText.text = Outcome.ToString(); ;
        if (Outcome == winStates.PlayerWin)
        {
            wins += 1;
            playerWinText.text = wins.ToString();
        }
        if (Outcome == winStates.ComputerWin)
        {
            losses += 1;
            computerWinText.text = losses.ToString();
        }
        if (Outcome == winStates.Tie)
        {
            ties += 1;
            tieText.text = ties.ToString();
        }
    }
}
