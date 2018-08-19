using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman {
    public override bool[,] PossibleMove()  //Knight moves in an L-shape style movement (also a rotated L)
    {
        bool[,] r = new bool[8, 8];
        //UpLeft
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);
        //UpRight
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);
        //Right up
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);
        //Rightdown
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);
        //DownLeft
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);
        //DownRight
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);
        //Left UP
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);
        //LeftDown
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);

        return r;
    }
	
    public void KnightMove(int x, int y, ref bool[,] r)
    {
        Chessman c;
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
            {
                r[x, y] = true;
            }
            else if (isWhite != c.isWhite) r[x, y] = true;
        }
    }

}
