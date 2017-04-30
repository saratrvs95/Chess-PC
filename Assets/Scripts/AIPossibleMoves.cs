using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

struct unit
{
    public Chessman c;
    public bool[,] pm;
    public int[,] sc;
    public int max, mx, my;
}

public class AIPossibleMoves : MonoBehaviour
{
    public static AIPossibleMoves Instance { set; get;}
    public int X, m;
    unit[] piece = new unit[16];
    Chessman c2;
    bool[,] rw = BoardManager.Instance.allowedMoves;

    private void Start()
    {
        Instance = this;
    }

    public void moves()
    {
        X = 0;
        piece[X].max = m = -999;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                c2 = BoardManager.Instance.Chessmans[i, j];
                if (c2 != null && c2.isWhite)
                    rw = c2.PossibleMove();
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                c2 = BoardManager.Instance.Chessmans[i, j];
                if(c2!=null && !c2.isWhite)
                {
                        piece[X].c = BoardManager.Instance.Chessmans[i, j];
                        piece[X].pm = piece[X].c.PossibleMove();
                        if (piece[X].pm != null)
                            eval();

                        X++;
                }
            }
        }

        for(int i=0;i<16;i++)
        {
            if(piece[i].max >= m)
            {
                X = i;
                m = piece[i].max;
                BoardManager.Instance.selectedChessman = piece[i].c;
            }
        }
        BoardManager.Instance.MoveChessman(piece[X].mx, piece[X].my);


    }

    public void eval()
    {
        piece[X].sc = new int[8, 8];
        Chessman c2;
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                c2 = BoardManager.Instance.Chessmans[i, j];
                if (piece[X].pm[i, j] == true && rw[i, j] == true)
                    piece[X].sc[i, j] = -15;

                if(c2 != null && !c2.isWhite && rw[i,j] == true)
                {
                    if(c2.GetType() == typeof(King))
                    {
                        if (-1 > piece[X].sc[i, j])
                        {
                            piece[X].sc[i, j] = -1;
                            break;
                        }

                    }
                    else if (c2.GetType() == typeof(Queen))
                    {
                        if (-2 > piece[X].sc[i, j])
                            piece[X].sc[i, j] = -2;
                    }
                    else if (c2.GetType() == typeof(Rook))
                    {
                        if (-3 > piece[X].sc[i, j])
                            piece[X].sc[i, j] = -3;
                    }
                    else if (c2.GetType() == typeof(Knight))
                    {
                        if (-4 > piece[X].sc[i, j])
                            piece[X].sc[i, j] = -4;
                    }
                    else if (c2.GetType() == typeof(Bishop))
                    {
                        if (-5 > piece[X].sc[i, j])
                            piece[X].sc[i, j] = -5;
                    }
                    else if (c2.GetType() == typeof(Pawn))
                    {
                        if (-14 > piece[X].sc[i, j])
                            piece[X].sc[i, j] = -14;
                    }
                }

                if (c2 != null && c2.isWhite && piece[X].pm[i, j] == true)
                {
                    if (c2.GetType() == typeof(King))
                    {
                        if (-1 >= piece[X].sc[i, j])
                        {
                            piece[X].sc[i, j] = -1;
                            break;
                        }
                    }
                    else if (c2.GetType() == typeof(Queen))
                    {
                        if (-3 >= piece[X].sc[i, j])
                            piece[X].sc[i, j] = -3;
                    }
                    else if (c2.GetType() == typeof(Rook))
                    {
                        if (-4 >= piece[X].sc[i, j])
                            piece[X].sc[i, j] = -4;
                    }
                    else if (c2.GetType() == typeof(Knight))
                    {
                        if (-5 >= piece[X].sc[i, j])
                            piece[X].sc[i, j] = -5;
                    }
                    else if (c2.GetType() == typeof(Bishop))
                    {
                        if (-6 >= piece[X].sc[i, j])
                            piece[X].sc[i, j] = -6;
                    }
                    else if (c2.GetType() == typeof(Pawn))
                    {
                        if (-8 >= piece[X].sc[i, j])
                            piece[X].sc[i, j] = -8;
                    }
                }
                if(c2 == null && rw[i, j] == false && piece[X].pm[i, j] == true)
                {
                    System.Random rand = new System.Random();
                    piece[X].sc[i, j] = -(rand.Next(5, 14));
                }
            }
        }

        best(piece[X].sc);
    }

    public void best(int[,] s)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (s[i, j] > piece[X].max)
                {
                    piece[X].mx = i;
                    piece[X].my = j;
                    piece[X].max = s[i, j];
                }
            }
        }
    }

}
