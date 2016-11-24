using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

    public int teleporterNumber;

    static int currentStage = 3;
    static string[] stages = { "leftFinal", "left2", "left1", "middle", "right1", "right2", "rightFinal"};

    PlayerController[] players;

    private void ProgressToNextStage(PlayerController player)
    {
        if (player.playerNumber == 1 && teleporterNumber > 0 || player.playerNumber == 2 && teleporterNumber < 0)
        {
            DontDestroyOnLoad(player);
            currentStage += teleporterNumber;
            LoadNextStage(stages[currentStage]);
        }
    }

    private void LoadNextStage(string stageName)
    {
        Application.LoadLevel(stageName);
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.GetComponent<PlayerCollisionDetector>())
        {
            players = FindObjectsOfType<PlayerController>();
            if (players.Length == 1) ProgressToNextStage(players[0]);
        }
    }
}
