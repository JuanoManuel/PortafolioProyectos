using UnityEngine;

//Clase referente a una pieza de ajedrez en general
public abstract class Chessman : MonoBehaviour
{
    //Materials of the chessman
    [Header("Chessman Options")]
    public Material whites; //whites material
    public Material blacks; //blacks material
    public GameObject chessmanModel;//3D model of the chessman

    [Header("Debuging Fields")]
    public string player;//Player who owns this chessman
    public string pieceName;//kind of piece
    Vector3 nextPosition;//Next position of the chessman
    [HideInInspector]
    public bool isFirstMove = true;
    
    public int col, row; //row & column positions on the board;

    protected GameMaster gameMaster;
    protected Board board;
    public static float offset = 0.1254f;//offset between squares on the board
    private Camera mainCamera;
    private float speed = 0.025f;
    private void Start()
    {
        //obtain a reference to the main camera
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    //Update method
    void Update()
    {
        if (gameMaster != null)
        {
            //Touch input. verify its chessman turn
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && gameMaster.currentTurn == player)
                {
                    Ray ray = mainCamera.ScreenPointToRay(touch.position);
                    RaycastHit hitObject;
                    if (Physics.Raycast(ray, out hitObject))
                    {
                        if (hitObject.collider == gameObject.transform.GetChild(0).GetComponent<Collider>())
                        {
                            ShowMovements();//Create Plates
                        }
                    }
                }
            }
            //Mouse imput
            if (Input.GetMouseButtonDown(0) && gameMaster.currentTurn == player)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if (hitObject.collider == gameObject.transform.GetChild(0).GetComponent<Collider>())
                    {
                        ShowMovements();//Create Plates
                    }
                }
            }
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPosition, speed);
    }

    //Create the chessman in game
    public void Activate(string player, string name, int xBoard, int yBoard, GameObject container)
    {
        //Completing information//
        this.player = player;
        pieceName = name;
        this.col = xBoard;
        this.row = yBoard;
        //Changing the material of the mesh renderer according the kind of player
        if (player.Equals("white"))
            chessmanModel.GetComponent<MeshRenderer>().material = whites;
        else
            chessmanModel.GetComponent<MeshRenderer>().material = blacks;
        //reference to the gamecontroller
        gameMaster = GameMaster.master;
        if(gameMaster!=null)
            board = gameMaster.GetBoard();
        float x = yBoard;
        float y = xBoard;
        transform.parent = container.transform; //Set the piece container as its parent in the hierarchy
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(y * offset, 0, x * offset);
        nextPosition = transform.localPosition;//set nextPosition to its initial position
    }

    //Abstract method that shows the posible movements of the chessman
    public abstract void ShowMovements();

    //printing chessman information in the console
    public void PrintInformation()
    {
        Debug.Log("Player: " + player+"   Name: " + pieceName+"   Coords: ("+row+","+col+")");
    }

    protected void SpawnPlateAtPoint(int row,int col)
    {
        //checking if the position is out of bounds
        if (board.IsOnBoard(row, col))
        {
            //check if it's already a piece in that position
            if (board.BoardHasPiece(row, col))
            {
                //determine if its enemy or not
                GameObject obj = board.GetPieceAtPosition(row, col);
                if (obj.GetComponent<Chessman>().player != player) //if it's an enemy
                {
                    //spawn an attack plate
                    SpawnPlate(row, col, "Attack");
                }
            }
            else //if not it means the piece can move that place
            {
                SpawnPlate(row, col, "Moves");
            }
        }
    }

    protected void SpawnPlate(int row,int col,string type,GameObject rook = null)
    {
        GameObject newPlate = Instantiate(gameMaster.GetPlate(type),Vector3.zero,Quaternion.identity);
        newPlate.transform.parent = gameMaster.platesContainer.transform;//set the container as its parent
        newPlate.transform.localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);
        newPlate.GetComponent<Plate>().Activate(gameObject, row, col,rook);//Initializing Plate script
    }


    //Spawn a line of plates, rowIncrement and colIncrement indicates the direction
    protected void LinealSpawnPlate(int rowIncrement,int colIncrement)
    {
        /*Initiazing increments*/
        int r = row + rowIncrement;
        int c = col + colIncrement;
        //////////////////////////////////////////////////////
        //While the board doesn't have a piece on this space//
        //or is an out of bounds position/////////////////////
        //////////////////////////////////////////////////////
        while (!board.BoardHasPiece(r, c) && board.IsOnBoard(r,c)) 
        {
            //Spawn a move plate
            SpawnPlate(r, c, "Moves");
            //Increment row and col according de increment parameters
            r += rowIncrement;
            c += colIncrement;
        }
        //At this point the r & c variables have an out of bounds position or the position of another piece
        //First check this position has a piece
        if (board.IsOnBoard(r, c))
        {
            //check if it's an enemy
            if(board.GetPieceAtPosition(r,c).GetComponent<Chessman>().player != player)
            {
                //Spawn an enemy plate
                SpawnPlate(r, c, "Attack");
            }
        }
    }

    //Starts animating the chessman to the next position
    public void SetNewPosition(int row,int col)
    {
        this.row = row;
        this.col = col;
        nextPosition = new Vector3(col * offset, 0, row * offset);
    }
}
