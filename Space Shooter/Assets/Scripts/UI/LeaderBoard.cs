using UnityEngine;
using TMPro;
using System.IO;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private TextMeshProUGUI[] namesLeaders;
    [SerializeField] private TextMeshProUGUI[] scoresLeaders;
    
    private string filePath = "Assets/UI/Leaderboard.txt";

    private void Start()
    {
        playerUI.SubmitScoreEvent += SetLeaderboardEntry;
        GetLeaderboard();
    }

    private void OnDisable()
    {
        playerUI.SubmitScoreEvent -= SetLeaderboardEntry;
    }

    public void GetLeaderboard()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Leaderboard is not exist!");
        }
        string[] lines = File.ReadAllLines(filePath);
        Debug.Log(lines);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] data = lines[i].Split(' ');
            Debug.Log(data);
            namesLeaders[i].text = data[0];
            scoresLeaders[i].text = data[1];
        }
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        for(int i = 0; i < scoresLeaders.Length; i++)
        {
            if (score > int.Parse(scoresLeaders[i].text))
            {
                namesLeaders[i].text = username;
                scoresLeaders[i].text = score.ToString();
                string[] lines = File.ReadAllLines(filePath);
                string[] data = lines[i].Split(' ');
                data[0] = namesLeaders[i].text;
                data[1] = scoresLeaders[i].text;
                lines[i] = data[0] + ' ' + data[1];
                File.WriteAllLines(filePath, lines);
                break;
            }
        }
        GetLeaderboard();
    }

}
