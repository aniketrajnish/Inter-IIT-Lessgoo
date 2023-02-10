using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Pieces
    {
        Null,
        WhitePawn,
        BlackPawn,
        Bishop,
        Knight,
        Rook,
        Queen,
        King
    }

    public enum States
    {
        Tutorial,
        Play,
        Fade
    }

    private int count = 0;
    private States chessState;
    private Pieces[,] Board;
    private Vector3[,] Positions;
    [SerializeField] int size;
    [SerializeField] GameObject whiteSprite;
    [SerializeField] GameObject blackSprite;
    [SerializeField] GameObject whitePawn;
    [SerializeField] GameObject whiteBishop;
    [SerializeField] GameObject whiteKnight;
    [SerializeField] GameObject whiteRook;
    [SerializeField] GameObject whiteQueen;
    [SerializeField] GameObject whiteKing;
    [SerializeField] GameObject blackPawn;
    [SerializeField] GameObject blackBishop;
    [SerializeField] GameObject blackKnight;
    [SerializeField] GameObject blackRook;
    [SerializeField] GameObject blackQueen;
    [SerializeField] GameObject blackKing;
    [SerializeField] GameObject chamundaQueen;

    [SerializeField] GameObject playerSprite;
    [SerializeField] GameObject chamundaSprite;

    [SerializeField] Camera playerCam;
    [SerializeField] Camera chamundaCam;

    [SerializeField] GameObject board;

    [SerializeField] Vector3 Origin;
    private Vector3 pcamInitialpos;
    private Vector3 ccamInitialpos;
    Vector3 spriteTransform;

    private GameObject[,] Pawn;
    private GameObject[,] Bishop;
    private GameObject[,] Knight;
    private GameObject[,] Rook;
    private GameObject[,] Queen;
    private GameObject[,] King;

    private UnityEvent StartPlayerControl;

    [SerializeField] GameObject light, go;

    bool final;
    GameObject chamunda;

    void Start()
    {
        chessState = States.Tutorial;
        StartPlayerControl = new UnityEvent();
        StartPlayerControl.AddListener(StartPlayer);
        Board = new Pieces[8, 8];
        Positions = new Vector3[8, 8];
        Pawn = new GameObject[2, 8];
        Bishop = new GameObject[2, 2];
        Knight = new GameObject[2, 2];
        Rook = new GameObject[2, 2];
        Queen = new GameObject[2, 1];
        King = new GameObject[2, 1];
        StartBoard();
        StartCoroutine(TestPlay());
        pcamInitialpos = playerCam.transform.position;
        ccamInitialpos = chamundaCam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (chessState == States.Play)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                chessState = States.Tutorial;
                StartCoroutine(PlayState());
            }
        }
        if (final)
        {
            Debug.Log("print");
            //chamunda.transform.position += new Vector3(0f, -0.5f, 0f) * Time.deltaTime;
        }
    }

    void StartBoard()
    {
        spriteTransform = new Vector3();
        int flag = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                spriteTransform = Origin + new Vector3((j * size) + (size / 2f), (i * size) + (size / 2f), 0);
                Positions[i, j] = spriteTransform;
                if ((flag + i) % 2 == 0)
                {
                    Debug.Log(spriteTransform);
                    Instantiate(whiteSprite, spriteTransform, Quaternion.identity);
                }
                else
                {
                    Instantiate(blackSprite, spriteTransform, Quaternion.identity);
                }
                flag++;
            }
        }
        InitializePawn();
        InitializeBishop();
        InitializeKnight();
        InitializeRook();
        InitializeQueen();
        InitializeKing();
        MakeChild();
    }

    void InitializePawn()
    {
        for (int i = 0; i < 8; i++)
        {
            Board[1, i] = Pieces.WhitePawn;
            Pawn[0, i] = Instantiate(whitePawn, Positions[1, i], Quaternion.identity);
            Board[6, i] = Pieces.BlackPawn;
            Pawn[1, i] = Instantiate(blackPawn, Positions[6, i], Quaternion.identity);
            Pawn[1, i].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
        Pawn[0, 4].layer = LayerMask.NameToLayer("Player");
    }

    void InitializeBishop()
    {
        Board[0, 2] = Pieces.Bishop;
        Bishop[0, 0] = Instantiate(whiteBishop, Positions[0, 2], Quaternion.identity);
        Board[0, 5] = Pieces.Bishop;
        Bishop[0, 1] = Instantiate(whiteBishop, Positions[0, 5], Quaternion.identity);
        Board[7, 2] = Pieces.Bishop;
        Bishop[1, 0] = Instantiate(blackBishop, Positions[7, 2], Quaternion.identity);
        Board[7, 5] = Pieces.Bishop;
        Bishop[1, 1] = Instantiate(blackBishop, Positions[7, 5], Quaternion.identity);
        Bishop[1, 0].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        Bishop[1, 1].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
    }

    void InitializeKnight()
    {
        Board[0, 1] = Pieces.Knight;
        Knight[0, 0] = Instantiate(whiteKnight, Positions[0, 1], Quaternion.identity);
        Board[0, 6] = Pieces.Knight;
        Knight[0, 1] = Instantiate(whiteKnight, Positions[0, 6], Quaternion.identity);
        Board[7, 1] = Pieces.Knight;
        Knight[1, 0] = Instantiate(blackKnight, Positions[7, 1], Quaternion.identity);
        Board[7, 6] = Pieces.Knight;
        Knight[1, 1] = Instantiate(blackKnight, Positions[7, 6], Quaternion.identity);
        Knight[1, 0].transform.rotation = Quaternion.Euler(180f, 180f, 0f);
        Knight[1, 1].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
    }

    void InitializeRook()
    {
        Board[0, 0] = Pieces.Rook;
        Rook[0, 0] = Instantiate(whiteRook, Positions[0, 0], Quaternion.identity);
        Board[0, 7] = Pieces.Rook;
        Rook[0, 1] = Instantiate(whiteRook, Positions[0, 7], Quaternion.identity);
        Board[7, 0] = Pieces.Rook;
        Rook[1, 0] = Instantiate(blackRook, Positions[7, 0], Quaternion.identity);
        Board[7, 7] = Pieces.Rook;
        Rook[1, 1] = Instantiate(blackRook, Positions[7, 7], Quaternion.identity);
        Rook[1, 0].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        Rook[1, 1].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
    }

    void InitializeQueen()
    {
        Board[0, 3] = Pieces.Queen;
        Queen[0, 0] = Instantiate(whiteQueen, Positions[0, 3], Quaternion.identity);
        Board[7, 3] = Pieces.Rook;
        Queen[1, 0] = Instantiate(blackQueen, Positions[7, 3], Quaternion.identity);
        Queen[1, 0].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        Queen[1, 0].layer = LayerMask.NameToLayer("PP");
    }

    void InitializeKing()
    {
        Board[0, 4] = Pieces.King;
        King[0, 0] = Instantiate(whiteKing, Positions[0, 4], Quaternion.identity);
        Board[7, 4] = Pieces.King;
        King[1, 0] = Instantiate(blackKing, Positions[7, 4], Quaternion.identity);
        King[1, 0].transform.rotation = Quaternion.Euler(180f, 0f, 0f);
    }

    IEnumerator TestPlay()
    {
        float i;
        for (i = 0.0f; i <= 1.0f; i += 0.2f)
        {
            Pawn[0, 3].transform.position = (Positions[3, 3] - Positions[1, 3]) * i + Positions[1, 3];
            yield return new WaitForSecondsRealtime(0.025f);
        }
        yield return new WaitForSecondsRealtime(1f);
        for (i = 0.0f; i <= 1; i += 0.2f)
        {
            Knight[1, 0].transform.position = Positions[7, 1] + (Positions[5, 2] - Positions[7, 1]) * i;
            yield return new WaitForSecondsRealtime(0.025f);

        }
        yield return new WaitForSecondsRealtime(0.5f);
        for (i = 0.0f; i <= 1; i += 0.2f)
        {
            Pawn[0, 0].transform.position = Positions[1, 0] + (Positions[2, 0] - Positions[1, 0]) * i;
            yield return new WaitForSecondsRealtime(0.025f);

        }
        yield return new WaitForSecondsRealtime(0.5f);
        for (i = 0.0f; i <= 1; i += 0.2f)
        {
            Pawn[1, 3].transform.position = Positions[6, 3] + (Positions[5, 3] - Positions[6, 3]) * i;
            yield return new WaitForSecondsRealtime(0.025f);

        }
        yield return new WaitForSecondsRealtime(0.5f);
        StartPlayer();
    }

    IEnumerator MoveSpecific(GameObject piece, Vector3 initialPos, Vector3 finalPos)
    {
        for (float i = 0f; i <= 1.0f; i += 0.2f)
        {
            piece.transform.position = initialPos + (finalPos - initialPos) * i;
            yield return new WaitForSecondsRealtime(0.025f);
        }
    }
    
    IEnumerator MoveCamera(Camera cam, Vector3 initialPos, Vector3 finalPos)
    {
        for (float i = 0f; i <= 1.0f; i += 0.2f)
        {
            cam.transform.position = initialPos + (finalPos - initialPos) * i;
            yield return new WaitForSecondsRealtime(0.0125f);
        }
    }

    IEnumerator MoveCombined(GameObject piece, Vector3 finalPiecePosition)
    {
        yield return StartCoroutine(ZoomOutCamera());
        yield return new WaitForSecondsRealtime(0.5f);
        yield return StartCoroutine(MoveSpecific(piece, piece.transform.position, finalPiecePosition));
        yield return new WaitForSecondsRealtime(0.5f);
    }

    IEnumerator ZoomInCamera()
    {
        for (float i = 0f; i <= 1.0f; i += 0.2f)
        {
            playerCam.transform.position = pcamInitialpos + (Pawn[0,4].transform.position - pcamInitialpos) * i;
            chamundaCam.transform.position = ccamInitialpos + (Queen[1, 0].transform.position - ccamInitialpos) * i;
            playerCam.orthographicSize = 4 - 2 * i;
            chamundaCam.orthographicSize = 4 - 2 * i;
            yield return new WaitForSecondsRealtime(0.0125f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        chessState = States.Play;
    }

    IEnumerator ZoomOutCamera()
    {
        for (float i = 0f; i <= 1.0f; i += 0.2f)
        {
            playerCam.transform.position = Pawn[0,4].transform.position + (pcamInitialpos - Pawn[0, 4].transform.position) * i;
            chamundaCam.transform.position = Queen[1,0].transform.position + (ccamInitialpos - Queen[1, 0].transform.position) * i;
            playerCam.orthographicSize = 2 + 2 * i;
            chamundaCam.orthographicSize = 2 + 2 * i;
            yield return new WaitForSecondsRealtime(0.0125f);
        }
/*        for (float i = 0f; i <= 1.0f; i += 0.2f)
        {
            playerCam.orthographicSize = 2 + 2 * i;
            chamundaCam.orthographicSize = 2 + 2 * i;
            yield return new WaitForSecondsRealtime(0.0125f);
        }*/
        yield return new WaitForEndOfFrame();
    }

    IEnumerator FadePieces()
    {

            foreach (GameObject gameObject in Pawn)
            {
                if (gameObject != Pawn[0,4])
                {
                Destroy(gameObject); 
                }
            }
            foreach (GameObject gameObject in Bishop)
            {
            Destroy(gameObject); 
        }
            foreach (GameObject gameObject in Knight)
            {
            Destroy(gameObject); Destroy(gameObject);

        }
        foreach (GameObject gameObject in Rook)
            {
            Destroy(gameObject); Destroy(gameObject);


        }
        foreach (GameObject gameObject in Queen)
            {
                if (gameObject != Queen[1, 0])
                {
                Destroy(gameObject); Destroy(gameObject);

            }
        }
            foreach (GameObject gameObject in King)
            {
            Destroy(gameObject); Destroy(gameObject);


        }
        yield return new WaitForSecondsRealtime(0.0125f);

    }

    void MakeChild()
    {

        foreach (GameObject gameObject in Pawn)
        {
            gameObject.transform.parent = board.transform;
        }
        foreach (GameObject gameObject in Bishop)
        {
            gameObject.transform.parent = board.transform;
        }
        foreach (GameObject gameObject in Knight)
        {
            gameObject.transform.parent = board.transform;
        }
        foreach (GameObject gameObject in Rook)
        {
            gameObject.transform.parent = board.transform;
        }
        foreach (GameObject gameObject in Queen)
        {
            gameObject.transform.parent = board.transform;
        }
        foreach (GameObject gameObject in King)
        {
            gameObject.transform.parent = board.transform;
        }
    }

    public void StartPlayer()
    {
        StartCoroutine(Switch());
    }

    IEnumerator Switch()
    {
        yield return StartCoroutine(ZoomInCamera());

        Destroy(light);

        Destroy(Pawn[0, 4]);
        Destroy(Queen[1, 0]);

        Pawn[0, 4] = Instantiate(playerSprite, Positions[1, 4], Quaternion.identity);
        Queen[1, 0] = Instantiate(chamundaSprite, Positions[7, 3], Quaternion.Euler(180f, 0f, 0f));

        //Debug.Log(Pawn[0, 4].transform.localScale);
        Pawn[0, 4].transform.localScale = new Vector3(0.15f * transform.localScale.x, 0.15f * transform.localScale.y, 0.15f * transform.localScale.z);
        //Debug.Log(Pawn[0, 4].transform.localScale);
        Queen[1, 0].transform.localScale = new Vector3(0.15f * transform.localScale.x, 0.15f * transform.localScale.y, 0.15f * transform.localScale.z);
        Queen[1, 0].transform.localScale = new Vector3(0.15f * transform.localScale.x, 0.15f * transform.localScale.y, 0.15f * transform.localScale.z);


        yield return new WaitForSecondsRealtime(0.5f);


    }
    


    IEnumerator PlayState()
    {
        if (count == 0)
        {

        
        
            yield return StartCoroutine(MoveSpecific(Pawn[0, 4], Positions[1, 4], Positions[2, 4]));

            yield return new WaitForSecondsRealtime(0.5f);
            yield return StartCoroutine(MoveSpecific(Queen[1, 0], Positions[7, 3], Positions[6, 3]));
            yield return new WaitForSecondsRealtime(0.5f);
            yield return StartCoroutine(MoveCombined(Rook[0, 0], Positions[1, 0]));
            yield return StartCoroutine(MoveSpecific(Pawn[1, 1], Pawn[1, 1].transform.position, Positions[5, 1]));
            yield return new WaitForSecondsRealtime(0.5f);
            yield return StartCoroutine(MoveSpecific(Knight[0, 1], Knight[0, 1].transform.position, Positions[2, 5]));
            yield return new WaitForSecondsRealtime(0.5f);
            yield return StartCoroutine(MoveSpecific(Knight[1, 0], Knight[1, 0].transform.position, Positions[4, 0]));
            yield return new WaitForSecondsRealtime(0.5f);
            count++;
            yield return StartCoroutine(ZoomInCamera());
            yield break;
        }
        if (count == 1)
        {
            yield return StartCoroutine(MoveSpecific(Pawn[0, 4], Positions[2, 4], Positions[3, 4]));

            yield return new WaitForSecondsRealtime(0.5f);
            yield return StartCoroutine(MoveSpecific(Queen[1, 0], Positions[6, 3], Positions[5, 4]));
            yield return new WaitForSecondsRealtime(0.5f);
            yield return StartCoroutine(MoveCombined(Bishop[0, 1], Positions[1, 4]));
            yield return StartCoroutine(MoveSpecific(Bishop[1, 0], Positions[7, 2], Positions[5, 0]));
            yield return new WaitForSecondsRealtime(0.5f);
            count++;
            yield return StartCoroutine(ZoomInCamera());
            yield break;
        }
        if (count == 2)
        {
            yield return StartCoroutine(MoveSpecific(Pawn[0, 4], Positions[3, 4], Positions[4, 4]));
            yield return new WaitForSecondsRealtime(0.5f);
            yield return StartCoroutine(ZoomOutCamera());
            yield return StartCoroutine(FadePieces());
            SpriteRenderer sr = Queen[1, 0].GetComponent<SpriteRenderer>();
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            

            
            
            chamunda = Instantiate(chamundaQueen, Positions[5, 4], Quaternion.Euler(180f, 0f, 0f));
            Animator anim = chamunda.GetComponent<Animator>();
            anim.Play("bandianimation");
            yield return new WaitForSecondsRealtime(1.1f);
            SpriteRenderer cr = chamunda.GetComponent<SpriteRenderer>();
            cr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            yield return new WaitForSecondsRealtime(1f);
            chamunda.layer = LayerMask.NameToLayer("Player");
            chamunda.transform.position = Positions[5, 4] - new Vector3(0f, 0.6f, 0f);
            cr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
            anim.Play("bandianimation 1");
            final = true;
            yield return new WaitForSecondsRealtime(0.2f);

            GameObject gamoo = Instantiate(go, Pawn[0, 4].transform.position, Quaternion.identity);
            gamoo.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            Pawn[0, 4].GetComponent<SpriteRenderer>().enabled = false;
            yield break;
        }   
    }
    
}
