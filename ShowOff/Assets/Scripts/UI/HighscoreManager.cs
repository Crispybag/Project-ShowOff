using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ServiceLocator;
using System.IO;
using System.Linq;

public class HighscoreManager : MonoBehaviour
{




    // Start is called before the first frame update
    void Start()
    {
        WriteHighscores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WriteHighscores()
    {
        string[] scores = File.ReadAllLines("Assets/Highscores/Highscores.txt");
        int i = 0;
        foreach(string score in scores)
        {

            i++;
            string[] individual = score.Split('{');
            transform.Find("HighscoreList").Find("Score" + i).Find("Name").GetComponent<Text>().text = individual[1] + " & " + individual[2];
            transform.Find("HighscoreList").Find("Score" + i).Find("Score").GetComponent<Text>().text = individual[0];

        }
    }

}
