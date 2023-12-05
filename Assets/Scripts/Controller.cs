using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{
    Button[,] buttons;
    Image[] images;
    GameObject[] imageFutures;
    
    Lines lines;
    public TextMeshProUGUI textNum;
    public GameObject panel;
    public GameObject panelGame;
    public GameObject butMenu;
    public GameObject butAgain;
    public TextMeshProUGUI bestScoreD;
    public TextMeshProUGUI firstScoreD;
    public TextMeshProUGUI secondScoreD;
    public TextMeshProUGUI thirdScoreD;
    public TextMeshProUGUI bestScore;
    public TextMeshProUGUI firstScore;
    public TextMeshProUGUI secondScore;
    public TextMeshProUGUI thirdScore;
    public GameObject panelBalls;



    void Start()
    {
        lines = new Lines(ShowBox, PlayCut);
        InitButtons();
        InitImages();
        InitImagesFutures();
        Lines.isFinish = false;
        lines.Start();
        panelGame.SetActive(false);
        butMenu.SetActive(false);
        butAgain.SetActive(false);
        Lines.num = 0;
        
        textNum.text = Lines.num.ToString();

        bestScoreD.text = Result.LoadInfo("bestScoreD");
        firstScoreD.text = Result.LoadInfo("firstScoreD");
        secondScoreD.text = Result.LoadInfo("secondScoreD");
        thirdScoreD.text = Result.LoadInfo("thirdScoreD");

        bestScore.text = Result.LoadInfo("bestScore");
        firstScore.text = Result.LoadInfo("firstScore");
        secondScore.text = Result.LoadInfo("secondScore");
        thirdScore.text = Result.LoadInfo("thirdScore");
        panelBalls.SetActive(false);

    }

    public void ShowBox(int x, int y, int ball)
    {
        buttons[x, y].GetComponent<Image>().sprite = images[ball].sprite;
    }
    public void PlayCut(int i, int ball)
    {
            imageFutures[i].GetComponent<Image>().sprite = images[ball].sprite;
    }

    public void Click()
    {
        string nameButton = EventSystem.current.currentSelectedGameObject.name;
        int nr = GetNumber(nameButton);
        int x = nr % Lines.size;
        int y = nr / Lines.size;
        
        lines.Click(x, y);
        textNum.text = Lines.num.ToString();
        if (Lines.isFinish)
        {
            ChangeResult(Lines.num);
            Menu();
        }
    }

    private void InitButtons()
    {
        buttons = new Button[Lines.size, Lines.size];
        for (int i = 0; i < Lines.size * Lines.size; i++)
        {
            buttons[i % Lines.size, i / Lines.size] = GameObject.Find($"Button ({i})").GetComponent<Button>();
        }
    }
    
    private void InitImages()
    {
        images = new Image[Lines.balls];
        
        for (int j = 0; j< Lines.balls; j++)
        {
            images[j] = GameObject.Find($"Image ({j})").GetComponent<Image>();
        }
    }

    private void InitImagesFutures()
    {
        imageFutures = new GameObject[Lines.balls];

        for (int j = 0; j < Lines.balls; j++)
        {
            imageFutures[j] = GameObject.Find($"Ball_{j}");
        }
    }

    private int GetNumber(string name)
    {
        Regex reg = new Regex("\\((\\d+)\\)");
        Match match = reg.Match(name);
        if (!match.Success)
        {
            throw new Exception("Unrecognized Object Name");
        }
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Menu()
    {
        panel.SetActive(true);
        panelGame.SetActive(false);
        butMenu.SetActive(false);
        butAgain.SetActive(false);
        panelBalls.SetActive(false);
    }

    public void Return()
    {
        
        panel.SetActive(false);
        panelGame.SetActive(true);
        butMenu.SetActive(true);
        butAgain.SetActive(true);
        panelBalls.SetActive(true);

        if (Lines.isFinish)
            Again();
    }


    public void Again()
    {
        panelGame.SetActive(true);
        InitButtons();
        InitImages();
        Lines.num = 0;
        textNum.text = Lines.num.ToString();
        Lines.isFinish = false;
        lines.Start();
        panel.SetActive(false);
        butMenu.SetActive(true);
        butAgain.SetActive(true);
        panelBalls.SetActive(true);
    }

    public void ChangeResult(int res)
    {
        string date = System.DateTime.Now.ToString("MM.dd.yyyy");
        int lastRes = Convert.ToInt32(Result.LoadInfo("bestScore"));

        Result.SaveText("thirdScoreD", Result.LoadInfo("secondScoreD"));
        Result.SaveText("secondScoreD", Result.LoadInfo("firstScoreD"));
        Result.SaveText("firstScoreD", date);
                
        Result.SaveText("thirdScore", Result.LoadInfo("secondScore"));
        Result.SaveText("secondScore", Result.LoadInfo("firstScore"));
        Result.SaveText("firstScore", res.ToString());

        if (res > lastRes)
        {
            Result.SaveText("bestScoreD", date);
            Result.SaveText("bestScore", res.ToString());
        }

        bestScoreD.text = Result.LoadInfo("bestScoreD");
        firstScoreD.text = Result.LoadInfo("firstScoreD");
        secondScoreD.text = Result.LoadInfo("secondScoreD");
        thirdScoreD.text = Result.LoadInfo("thirdScoreD");

        bestScore.text = Result.LoadInfo("bestScore");
        firstScore.text = Result.LoadInfo("firstScore");
        secondScore.text = Result.LoadInfo("secondScore");
        thirdScore.text = Result.LoadInfo("thirdScore");
    }
}
