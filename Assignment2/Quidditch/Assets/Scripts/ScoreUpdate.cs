using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdate : MonoBehaviour
{
    private const string GRYFFINDOR_TEXT = "Gryffindor: ";
    private const string SLYTHERIN_TEXT = "Slytherin: ";
    private int gryffindorScore;
    private int slytherinScore;

    // Start is called before the first frame update
    void Start()
    {
        gryffindorScore = 0;
        slytherinScore = 0;
        UpdateScore();
    }

    // Update is called once per frame
    void UpdateScore()
    {
        GameObject.Find("GryffindorScore").GetComponent<UnityEngine.UI.Text>().text = GRYFFINDOR_TEXT + gryffindorScore;
        GameObject.Find("SlytherinScore").GetComponent<UnityEngine.UI.Text>().text = SLYTHERIN_TEXT + slytherinScore;
    }

    public void GryffindorPoint()
    {
        gryffindorScore += 1;
        UpdateScore();
    }

    public void SlytherinPoint()
    {
        slytherinScore += 1;
        UpdateScore();
    }
}
