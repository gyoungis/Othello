using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int[,] boardState = new int[8, 8];
    public GameObject[] rowList;
    public GameObject[] columnList;
    public GameObject piece;

    private int playerSide;
    private int oppositeSide;
    private gameBoard board;
    private GameObject[,] pieceArray;
    private List<GameObject> affectedPieces;
    private pieceScript pScript;

    //void SetGameControllerReferences()
    //{
    //    for (int i = 0; i < rowList.Length; i++)
    //    {
    //        rowList[i].GetComponentInParent<gameBoard>().SetGameControllerReference(this);
    //        columnList[i].GetComponentInParent<gameBoard>().SetGameControllerReference(this);
    //    }
    //}

    //void Awake()
    //{
    //    SetGameControllerReferences();

    //}

    void Start()
    {
        board = GetComponent<gameBoard>();
        pScript = GetComponent<pieceScript>();
        pieceArray = new GameObject[8, 8];
        boardState = new int [8,8]{ { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0}, 
                                    { 0, 0, 0, 0, 0, 0, 0, 0}, 
                                    { 0, 0, 0, 1, 2, 0, 0, 0}, 
                                    { 0, 0, 0, 2, 1, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0},
                                    { 0, 0, 0, 0, 0, 0, 0, 0} };
        Instantiate(piece, new Vector3(3f, 2f, 4f), Quaternion.identity);
        Instantiate(piece, new Vector3(4f, 2f, 3f), Quaternion.identity);
        Instantiate(piece, new Vector3(4f, 2f, 4f), Quaternion.Euler(180, 0, 0));
        Instantiate(piece, new Vector3(3f, 2f, 3f), Quaternion.Euler(180, 0, 0));
        playerSide = 1;
        oppositeSide = 2;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {

            float i;
            float j;
            int x;
            int y;
            Vector3 insertPoint;
            //Vector3 offset = new Vector3(0.5f, 0, 0.5f);
            //Vector3 position = Input.mousePosition;
            //position.y = 10.0f;
            //Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit[] hitList;

            //hitList = Physics.RaycastAll(ray, 9.0f);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 12.0f))
            {
                // World Coord
                i = Mathf.Floor(hit.point.x);
                j = Mathf.Floor(hit.point.z);

                // Matrix index
                x = (int)Mathf.Floor(hit.point.x);
                y = (int)Mathf.Floor(hit.point.z)*-1+7;



                if (i <= 7 && i >= 0 && j <= 7 && j >= 0 && boardState[x, y] == 0)
                {

                    if (playerSide == 1)
                    {
                        if (checkValidMove(x, y, playerSide, oppositeSide))
                        {
                            insertPoint = new Vector3(i, 2, j);
                            boardState[x, y] = playerSide;

                            pieceArray[x, y] = Instantiate(piece, insertPoint, Quaternion.identity);

                            playerSide = 2;
                            oppositeSide = 1;
                        }
                        
                    }
                    else
                    {
                        if (checkValidMove(x, y, playerSide, oppositeSide))
                        {
                            insertPoint = new Vector3(i, 2, j);
                            boardState[x, y] = playerSide;
                            
                            pieceArray[x, y] = Instantiate(piece, insertPoint, Quaternion.Euler(180, 0, 0));

                            playerSide = 1;
                            oppositeSide = 2;
                        }
                    }
                }
                else
                {
                    Debug.Log("Invalid Move");
                }
            }
            
            
        }
    }

    bool checkValidMove(int x, int y, int side, int opSide)
    {

        bool valid = true;
        int march;

        if (boardState[x - 1, y + 1] == opSide && x > 1 && y < 6) // Bottom Left
        {
            march = marchLength(x, y, 0);            
            for (int a = 1; a <= march; a++)
            {
                affectedPieces.Add(pieceArray[x - a, y - a]);
                if (boardState[x - a, y + a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[x - a, y + a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180, Space.World);
                    }
                    affectedPieces.Clear();
                    break;
                }


                if (boardState[x - a, y + a] == 0) // Empty or off board
                {
                    valid = false;
                    affectedPieces.Clear();
                    break;
                }
                //valid = false;
            }
        }




        if (boardState[x, y - 1] == opSide && y > 1) // Down
        {
            

            for (int a = 1; a < y; a++)
            {
                affectedPieces.Add(pieceArray[x, y - a]);
                if (boardState[x, y - a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[x - a, y - a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180, Space.World);
                    }
                    affectedPieces.Clear();
                    break;
                }


                if (boardState[x, y - a] == 0) // Empty or off board
                {
                    valid = false;
                    affectedPieces.Clear();
                    break;
                }
                //valid = false;
            }
        }


        if (boardState[x + 1, y - 1] == opSide && x < 6 && y > 1) // Bottom Right
        {
            march = marchLength(x, y, 1);

            for (int a = 1; a <= march; a++)
            {
                affectedPieces.Add(pieceArray[x - a, y - a]);
                if (boardState[x + a, y - a] == side) //Same Color
                {
                    affectedPieces.Remove(pieceArray[x + a, y - a]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180, Space.World);
                    }
                    affectedPieces.Clear();
                    break;
                }


                if (boardState[x + a, y - a] == 0) // Empty or off board
                {
                    valid = false;
                    affectedPieces.Clear();
                    break;
                }
                //valid = false;
            }
        }

        if (boardState[x - 1, y] == opSide && x > 1) // Left
        {
            for (int a = 1; a <= x; a++)
            {
                affectedPieces.Add(pieceArray[x - 1, y]);
                if (boardState[x-a, y] == side)
                {
                    affectedPieces.Remove(pieceArray[x - 1, y]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180, Space.World);
                    }
                    affectedPieces.Clear();
                    break;
                }
                if (boardState[x + a, y] == 0) // Empty or off board
                {
                    valid = false;
                    affectedPieces.Clear();
                    break;
                }
            }
        }

        if (boardState[x + 1, y] == opSide && x < 6) // Right
        {
            for (int a = 1; a <= 7-x; a++)
            {
                affectedPieces.Add(pieceArray[x + 1, y]);
                if (boardState[x + a, y] == side)
                {
                    affectedPieces.Remove(pieceArray[x + a, y]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180, Space.World);
                    }
                    affectedPieces.Clear();
                    break;
                }
                if (boardState[x + a, y] == 0) // Empty or off board
                {
                    valid = false;
                    affectedPieces.Clear();
                    break;
                }
            }
        }

        if (boardState[x + 1, y] == opSide && x < 6) // Right
        {
            for (int a = 1; a <= 7 - x; a++)
            {
                affectedPieces.Add(pieceArray[x + 1, y]);
                if (boardState[x + a, y] == side)
                {
                    affectedPieces.Remove(pieceArray[x + a, y]);
                    valid = true;
                    for (int c = 0; c < affectedPieces.Count; c++)
                    {
                        affectedPieces[c].transform.Rotate(0, 0, 180, Space.World);
                    }
                    affectedPieces.Clear();
                    break;
                }
                if (boardState[x + a, y] == 0) // Empty or off board
                {
                    valid = false;
                    affectedPieces.Clear();
                    break;
                }
            }
        }

        return valid;
    }

    int marchLength(int x, int y, int dir)
    {
        int len = 0;
        int minX;
        int minY;

        if (dir == 0) // Bottom Left
        {
            minX = x;
            minY = 7 - y;
            if (minX <= minY)
            {
                len = minX;
            }
            else
            {
                len = minY;
            }
        }

        if (dir == 1)
        {
            minX = 7 - x;
            minY = 7 - y;
            if (minX <= minY)
            {
                len = minX;
            }
            else
            {
                len = minY;
            }
        }

        if (dir == 2)
        {
            minX = x;
            minY = y;
            if (minX <= minY)
            {
                len = minX;
            }
            else
            {
                len = minY;
            }
        }

        if (dir == 3)
        {
            minX = 7 - x;
            minY = y;
            if (minX <= minY)
            {
                len = minX;
            }
            else
            {
                len = minY;
            }
        }

        return len;
    }
}
