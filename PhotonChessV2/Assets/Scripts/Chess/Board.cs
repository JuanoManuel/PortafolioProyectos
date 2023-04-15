using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Board will take care of all the boar logic realated to spawn pieces move pieces and consult the board state
public class Board : MonoBehaviour
{
    public GameObject whitesContainer,blacksContainer;//reference to the empty game object where the chessman will be instantiating
    public GameObject platesContainer;//reference to the empty game object where the plates will be instantiating
    public GameObject boardModel;
    [Space(30)]
    public GameObject[] pieces;//List of all the pieces

    private GameObject[,] board = new GameObject[8, 8];//storing the pieces in their positions
    private Dictionary<string, GameObject> dPieces = new Dictionary<string, GameObject>();//dictionary for easy finding pieces
    private Dictionary<string, GameObject> dPlates = new Dictionary<string, GameObject>();//dictionary for easy finding plates

    private void Awake()
    {
        //populate the dictionaries
        foreach (GameObject piece in pieces)
            dPieces.Add(piece.name, piece);
    }
    //Populates the board with the chessman in order
    public void CreateBoard(string player)
    {
        //////Instantiating players//////
        /*PAWNS*/
        for (int i = 0; i < 8; i++)
        {
            Create(i, 1, "Pawn", player);
        }
        /*Rooks*/
        Create(0, 0, "Rook", player);//Left rook
        Create(7, 0, "Rook", player);//Right rook
        /*Bishops*/
        Create(2, 0, "Bishop", player); //Left bishop
        Create(5, 0, "Bishop", player); //Right bishop
        /*Knights*/
        Create(1, 0, "Knight", player); //Left knight
        Create(6, 0, "Knight", player); //Right knight
        /*King & Queen*/
        Create(3, 0, "Queen", player);
        Create(4, 0, "King", player);
        Debug.Log("Game Created");
    }
    public void CreateEnemyBoard(string enemy)
    {
        //////Instantiating players//////
        /*PAWNS*/
        for (int i = 0; i < 8; i++)
        {
            Create(i, 6, "Pawn", enemy);
        }
        /*Rooks*/
        Create(0, 7, "Rook", enemy);//Left rook
        Create(7, 7, "Rook", enemy);//Right rook
        /*Bishops*/
        Create(2, 7, "Bishop", enemy); //Left bishop
        Create(5, 7, "Bishop", enemy); //Right bishop
        /*Knights*/
        Create(1, 7, "Knight", enemy); //Left knight
        Create(6, 7, "Knight", enemy); //Right knight
        Create(3, 7, "Queen", enemy);
        Create(4, 7, "King", enemy);
    }
    //Instantiates a chessman on the required coordinates//
    public void Create(int x, int y, string pieceName, string player)
    {
        GameObject obj = Instantiate(dPieces[pieceName], new Vector3(0, 0, 0), Quaternion.identity);//Instantiating the chessman on the board
        Chessman cm = obj.GetComponent<Chessman>(); //reference to its chessman script
        cm.Activate(player, pieceName, x, y, player == "white"?whitesContainer:blacksContainer);//visualize the chessman in the correct position
        board[y, x] = obj; //add the chessman in the board matrix to keep tracking the state of the game
    }
    //Indicates wheater the board has a chessman in the indicated position
    public bool BoardHasPiece(int row, int column)
    {
        //first we validate the coordenates given aren't out of bounds
        if (IsOnBoard(row, column))
            return board[row, column] ? true : false;//return if there is a piece in that position
        return false; //return false 'cuz the coordinates are out of bounds
    }
    //Return wheater the coordinates indicates a position on the board
    public bool IsOnBoard(int row, int col)
    {
        return (row >= 0 && row < 8 && col >= 0 && col < 8) ? true : false;
    }
    //returns the object on the current position of the board
    public GameObject GetPieceAtPosition(int row, int col)
    {
        return board[row, col];
    }
    //Empty the current position of the board
    public void SetPositionEmpty(int row, int col)
    {
        board[row, col] = null;
    }
    //Sets an object on the position given
    public void SetPieceAtPosition(int row, int col, GameObject obj)
    {
        board[row, col] = obj;
    }
    //Activates/Deactivates the colliders of a player type
    public void SetPlayerColliders(string player,bool value)
    {
        GameObject container = player == "white" ? whitesContainer : blacksContainer;
        foreach (Transform child in container.transform)
            child.gameObject.GetComponent<Chessman>().chessmanModel.GetComponent<Collider>().enabled = value;
    }
}
