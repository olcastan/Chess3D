using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const float TILE_SIZE = 1.0f;   //each tile will be 1x1 meter
    private const float TILE_OFFSET = 0.5f; //tile size divided by 2

    private int selectionX = -1;        //whenever user browses over board, selected tile will be glowing
    private int selectionY = -1;

    private void update() { createBoard(); }

    private void createBoard()
    {
        Vector3 lineWidth = Vector3.right * 8;  //chessboard will be 8x8
        Vector3 lineHeight = Vector3.forward * 8;

        for (int k = 0; k <= 8; k++)
        {

            Vector3 start = Vector3.forward * k;
            Debug.DrawLine(start, start + lineWidth);
            for (int i = 0; i <= 8; i++)
            {

            }
        }
    }
}