using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour {

    public static BoardManager Instance { set; get; } //with this object, we'll be able to access this from anywhere
    private bool[,] allowedMoves { set; get; }  //!!!!!!! could possibly be a problem

    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;

    private const float TILE_SIZE = 1.0f;   //each tile will be 1x1 meter
    private const float TILE_OFFSET = 0.5f; //tile size divided by 2

    private int selectionX = -1;        //whenever user browses over board, selected tile will be glowing
    private int selectionY = -1;

    
    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    private Material previousMat;
    public Material selectedMat;

    public int[] EnPassantMove { set; get; }
    private Quaternion orientation = Quaternion.Euler(0, 180, 0);
    
    public bool isWhiteTurn = true;
    private void Start()
    {
        //Debug.Log("Hello");   console test
        Instance = this;
        SpawnAllChessmans();
    }
    private void Update(){  //Didn't know that C# function names are supposed to be UPPERCASE
        SelectionUpdate();
        CreateBoard();

        if(Input.GetMouseButtonDown(0))
        {
            if(selectionX >= 0 && selectionY >= 0)
            {
                if(selectedChessman == null)
                {
                    //select desired piece
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    //move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }
    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
            return; //return nothing if user is not selecting a piece
        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return; //white player is trying to select a black piece

        bool hasAtLeastOneMove = false;

        allowedMoves = Chessmans [x, y].PossibleMove();
        for(int i = 0; i < 8; i++)
            for(int j = 0; j < 8; j++)
                if (allowedMoves[i, j])
                    hasAtLeastOneMove = true;

        if (!hasAtLeastOneMove)
            return;

        selectedChessman = Chessmans[x,y];
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;

        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        //BoardFeedback.Instance.HighlightAllowedMoves(allowedMoves); //bug when uncommented, when this line is used, more than the appropriate # of 
                                                                      //allowed moves are highlighted, not sure why this is.

    }
    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x,y])
        {

            Chessman c = Chessmans[x, y];
            if(c != null && c.isWhite != isWhiteTurn)
            {
                //Capture/remove a piece after it has been killed

                //If the capture piece is a king , game ends!!
                if(c.GetType() == typeof(King))
                {
                    // Game over
                    GameOver();
                    return;
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            //EnPassant and Promotion rules are established here
            if(x == EnPassantMove[0] && y== EnPassantMove[1])
            {
                if(isWhiteTurn)
                {
                    c = Chessmans[x, y-1];
                 }
                else
                {
                    c = Chessmans[x, y+1];
                    
                }
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
            if (selectedChessman.GetType() == typeof(Pawn))
            {
                if(y == 7)  //white player pawn promotion conditions
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnPieces(1, x, y);
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0)  //black player pawn promotion conditions
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnPieces(7, x, y);
                    selectedChessman = Chessmans[x, y];
                }

                if (selectedChessman.CurrentY == 1 && y == 3)   //for white player
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y-1;
                }
                else if (selectedChessman.CurrentY == 6 && y == 4)  //for black player
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y+1;
                }
            }
            Chessmans [selectedChessman.CurrentX, selectedChessman.CurrentY] = null; 
            selectedChessman.transform.position = GetTileCenter(x, y);

            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }

        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;

        BoardFeedback.Instance.HideHighlights();       //not sure why this is an error, works in Unity
        selectedChessman = null;
    }
    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        EnPassantMove = new int[2] { -1, -1 };
        // Spawn white player pieces

        // King
        SpawnPieces(0, 3, 0);
        // Queen
        SpawnPieces(1, 4, 0);
        // Rooks
        SpawnPieces(2,0, 0);
        SpawnPieces(2,7, 0);
        //Bishops
        SpawnPieces(3,2, 0);
        SpawnPieces(3,5, 0);
        //Knights
        SpawnPieces(4,1, 0);
        SpawnPieces(4, 6, 0);
        //Pawns
        for(int i = 0; i < 8; i++){
            SpawnPieces(5,i, 1);
        }
        // Spawn black player pieces

        // King
        SpawnPieces(6,4, 7);
        // Queen
        SpawnPieces(7,3, 7);
        // Rooks
        SpawnPieces(8, 0, 7);
        SpawnPieces(8,7, 7);
        //Bishops
        SpawnPieces(9,2, 7);
        SpawnPieces(9, 5, 7);
        //Knights
        SpawnPieces(10, 1, 7);
        SpawnPieces(10,6, 7);
        //Pawns
        for (int i = 0; i < 8; i++)
            SpawnPieces(11,i, 6);
        


    }

    private void SelectionUpdate()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
            
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }

    }

    private void SpawnPieces(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y),  Quaternion.identity) as GameObject;   //Game object (pieces) instantiation
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();      //Multidimensional array
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
       
    private void CreateBoard()
    {
        Vector3 lineWidth = Vector3.right * 8;  //chessboard will be 8x8
        Vector3 lineHeight = Vector3.forward * 8;

        for(int k = 0; k <= 8; k++){

            Vector3 start = Vector3.forward * k;
            Debug.DrawLine(start, start + lineWidth);
            for(int i = 0; i <= 8; i++)
            {
                start = Vector3.right * i;
                Debug.DrawLine(start, start + lineHeight);

            }
        }

        //Draw the selection
        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));


            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
    private void GameOver()
    {
        if (isWhiteTurn) Debug.Log("White Player Wins!!!");
        else
            Debug.Log("Black Player Wins!!!");

        foreach (GameObject go in activeChessman)   //After victory message has popped up, board will reset itself
            Destroy(go);

        isWhiteTurn = true;
        BoardFeedback.Instance.HideHighlights();
        SpawnAllChessmans();
    }
}
