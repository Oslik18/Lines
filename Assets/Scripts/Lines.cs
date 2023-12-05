using System;
using UnityEngine;

public delegate void ShowBox(int x, int y, int ball);
public delegate void PlayCut(int i, int ball);

public class Lines
{
    public const int size = 9;
    public const int balls = 7;
    const int addBalls = 3;
    public static int num = 0;
    public static bool isFinish = false;
    System.Random rnd = new System.Random();

    ShowBox showBox;
    PlayCut playCut;
    
    int[,] map;
    int fromX, fromY;
    bool ballTaking;
    int[] futureBalls;

    public Lines(ShowBox showBox, PlayCut playCut)
    {
        this.showBox = showBox;
        this.playCut = playCut;
        map = new int[size, size];
    }

    public void Start()
    {
        ClearMap();
        RandomStartBalls();
        ballTaking = false;
    }

    private void RandomStartBalls()
    {
        for (int i = 0; i < 5; i++)
        {
            int x, y;
            int loop = size * size;
            do
            {
                x = rnd.Next(size);
                y = rnd.Next(size);
                if (--loop <= 0) return;
            } while (map[x, y] > 0);
            int ball = 1 + rnd.Next(balls - 1);
            SetMap(x, y, ball);
        }
        RandomBalls();
    }


    public void Click(int x, int y)
    {
        if (IsGameOver())
            isFinish = true;
        else
           if (map[x, y] > 0)
             TakeBall(x, y);
           else
             MoveBall(x, y);
    }

    private void TakeBall(int x, int y)
    {
        fromX = x;
        fromY = y;
        ballTaking = true;
    }
    
    private void MoveBall(int x, int y)
    {
        if (!ballTaking) return;
        if (!CanMove(x, y)) return;
        SetMap(x, y, map[fromX, fromY]);
        SetMap(fromX, fromY, 0);
        ballTaking = false;
        if (!CutLines())
        {
            AddRandomBalls();
            RandomBalls();
            CutLines();
        }     
    }

    private bool IsGameOver()
    {
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                if (map[x, y] == 0)
                    return false;
        return true;
    }
    
    private void ClearMap()
    {
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                SetMap(x, y, 0);
    }

    private void SetMap(int x, int y, int ball)
    {
        map[x, y] = ball;
        showBox(x, y, ball);
    }

    private bool OnMap(int x, int y)
    {
        return x >= 0 && x < size && y >= 0 && y < size;
    }
    
    private int GetMap(int x, int y)
    {
        if (!OnMap(x, y)) return 0;
        return map[x, y];
    }
    
    private void SetBalls(int x, int ball)
    {
        playCut(x, ball);
    }
    
    private void RandomBalls()
    {
        futureBalls = new int[balls];
        for (int x = 0; x < addBalls; x++)
        {
            RandomBall(x);
        }  
    }

    private void RandomBall(int i)
    {
        int ball = 1 + rnd.Next(balls - 1);
        SetBalls(i, ball);
        futureBalls[i] = ball;
    }

    private void AddRandomBalls()
    {
        for (int x = 0; x < addBalls; x++)
            AddRandomBall(x);
    }
    private void AddRandomBall(int i)
    {
        int x, y;
        int loop = size * size;
        do
        {
            x = rnd.Next(size);
            y = rnd.Next(size);
            if (--loop <= 0) return;
        } while (map[x, y] > 0);
        SetMap(x, y, futureBalls[i]);
    }

    private bool[,] mark;
    private bool CutLines()
    {
        int balls = 0;
        mark = new bool[size, size];
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                balls += CalculateLine(x, y, 1, 0);
                balls += CalculateLine(x, y, 0, 1);
                balls += CalculateLine(x, y, 1, 1);
                balls += CalculateLine(x, y, -1, 1);
            }
        if (balls > 0)
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (mark[x, y])
                    {
                        SetMap(x, y, 0);
                        num += 10;
                    }
            return true;
        }
        return false;
    }

    private int CalculateLine(int x0, int y0, int sx, int sy)
    {
        int ball = map[x0, y0];
        int count = 0;

        if (ball == 0) return 0;

        for (int x = x0, y = y0; GetMap(x, y) == ball; x += sx, y += sy)
            count++;

        if (count < 5)
            return 0;

        for (int x = x0, y = y0; GetMap(x, y) == ball; x += sx, y += sy)
            mark[x, y] = true;

        return count;
    }

    private bool[,] used;
    private bool CanMove(int x, int y)
    {
        used = new bool[size, size];
        Walk(fromX, fromY, true);
        return used[x,y];
    }

    private void Walk(int x, int y, bool start = false)
    {
        if (!start)
        {
            if (!OnMap(x, y)) return;
            if (map[x, y] > 0) return;
            if (used[x, y]) return;
        }
        used[x, y] = true;
        Walk(x + 1, y);
        Walk(x - 1, y);
        Walk(x, y +1);
        Walk(x, y - 1);
    }
}
