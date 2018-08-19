﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman {
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;
        int i, j;
        //Top side
        i = CurrentX - 1;
        j = CurrentY + 1;
        if(CurrentY != 7)   //can't go up anymore
        {
            for(int k = 0; k < 3; k++)
            {
                if(i >= 0 || i < 8)
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null) r[i, j] = true;
                    else if (isWhite != c.isWhite) r[i, j] = true;
                }
                i++;
            }
        }

        //Down side
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)   //can't go down anymore
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 || i < 8)
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null) r[i, j] = true;
                    else if (isWhite != c.isWhite) r[i, j] = true;
                }
                i++;
            }
        }

        //Middle Left
        if(CurrentX != 0)
        {
            c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
            if (c == null)
                r[CurrentX - 1, CurrentY] = true;//!!!!!! may have to change this
            else if (isWhite != c.isWhite)
                r[CurrentX - 1, CurrentY] = true;
            

        }
        //Middle Right
        if (CurrentX != 7)
        {
            c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else if (isWhite != c.isWhite)
                r[CurrentX + 1, CurrentY] = true;


        }

        return r;
    }
	
}
