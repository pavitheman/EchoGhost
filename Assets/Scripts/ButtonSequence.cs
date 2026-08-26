using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonSequence : MonoBehaviour
{
    public GameObject memoryPanel;
    public Button[] buttons;
    private List<int> sequence = new List<int>();
    private int playerIndex;
    private bool playerTurn;
    private int round = 0;
    private int maxRounds = 5;

    public System.Action OnGameWon;

    void Start()
    {
        memoryPanel.SetActive(false);
    }

    public void StartGame()
    {
        memoryPanel.SetActive(true);
        sequence.Clear();
        round = 0;
        StartCoroutine(NewRound());
    }

    IEnumerator NewRound()
    {
        playerTurn = false;
        playerIndex = 0;
        round++;
        sequence.Add(Random.Range(0, buttons.Length));
        yield return new WaitForSeconds(0.5f);
        foreach (int index in sequence)
        {
            yield return StartCoroutine(FlashButton(index));
            yield return new WaitForSeconds(0.2f);
        }
        playerTurn = true;
    }

    IEnumerator FlashButton(int index)
    {
        Color originalColor = buttons[index].image.color;
        buttons[index].image.color = Color.white;
        yield return new WaitForSeconds(0.4f);
        buttons[index].image.color = originalColor;
    }

    public void PressButton(int index)
    {
        if (!playerTurn)
            return;

        StartCoroutine(FlashButton(index));

        if (index == sequence[playerIndex])
        {
            playerIndex++;
            if (playerIndex >= sequence.Count)
            {
                playerTurn = false;
                if (round >= maxRounds)
                {
                    StartCoroutine(GameWon());
                }
                else
                {
                    StartCoroutine(NewRound());
                }
            }
        }
        else
        {
            StartCoroutine(GameLost());
        }
    }

    IEnumerator GameWon()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Memory Game Complete!");
        memoryPanel.SetActive(false);
        OnGameWon?.Invoke();
    }

    IEnumerator GameLost()
    {
        playerTurn = false;
        yield return new WaitForSeconds(0.5f);
        sequence.Clear();
        round = 0;
        Debug.Log("Wrong sequence!");
        StartCoroutine(NewRound());
    }
}