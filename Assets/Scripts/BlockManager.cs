using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockManager : MonoBehaviour
{
    public GameObject MainBlock;
    public GameObject block;
    public Transform blockParent;

    public bool fail = false;
    private bool onDestination = false;

    private float horzExtent;

    private SpriteRenderer sr;
    private Color fallingColor;
    private float colorCounter = 1;

    private float gapX;
    public float lastPosX;
    private Vector3 lastSize;
    public float LastX
    {
        get
        {
            return lastSize.x;
        }
    }

    [HideInInspector] public float posY;

    [Range(0.1f, 20)] public float BlockTime = 1;
    public float disregardDistance = 0.04f;
    private float currentSpeed = 4;
    public float startSpeed = 4;
    public float maxSpeed = 8;

    private GameObject currentBlock;
    private GameObject recentBlock;
    private GameObject fallingBlock;

    private readonly float unvaluedSize = .03f;


    public static BlockManager instance;
    private void Awake()
    {
        instance = this;

        posY = block.transform.localScale.y - 4;
        

        horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        currentSpeed = startSpeed;
    }

    private void Update()
    {
       

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) //PC CONTROLS
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                return;

            if (currentBlock == null || recentBlock == null || fail)
                return;

            SetBlock();

            if (IsFailed() && !fail)
            {
                fail = true;
                GameController.GameEnding?.Invoke();
                return;
            }
            else
            {
                GameController.NewBlock?.Invoke();
                AdjustBlock();
                FallingPart();
                AfterAdjust();
            }
        }
        else if (Input.touchCount > 0)     //MOBILE CONTROLS
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.currentSelectedGameObject != null)
                    return;

                if (currentBlock == null || recentBlock == null)
                    return;


                SetBlock();

                if (IsFailed() && !fail)
                {
                    fail = true;
                    GameController.GameEnding?.Invoke();
                    return;
                }
                else
                {
                    GameController.NewBlock?.Invoke();
                    AdjustBlock();
                    FallingPart();
                    AfterAdjust();
                }
            }
        }

        MoveBlock();

        if (fail)
            return;


        FallBlock(fallingBlock);
    }

    private bool IsFailed()
    {
        gapX = currentBlock.transform.position.x - recentBlock.transform.position.x;

        if (Mathf.Abs(gapX) > lastSize.x - unvaluedSize)
        {
            return true;
        }
        return false;
    }


    public bool perfectBlock;
    public void SetBlock()
    {
        
        onDestination = false;
        colorCounter = 1;

        if (Mathf.Abs(recentBlock.transform.position.x - currentBlock.transform.position.x) < disregardDistance)
        {
            //PERFECT BLOCK
            perfectBlock = true;
            currentBlock.transform.position = new Vector3(recentBlock.transform.position.x, currentBlock.transform.position.y, 0);
            GameController.PerfectBLock?.Invoke();
        }
        else
        {
            perfectBlock = false;
            GameController.instance.perfectComboAmount = 0;

        }
    }

    public void AdjustBlock()
    {
        if (currentSpeed < maxSpeed)
            currentSpeed += 0.05f;

        lastSize.x -= Mathf.Abs(gapX);
        lastPosX = recentBlock.transform.position.x + (gapX * 0.5f);
        currentBlock.transform.position = new Vector3(lastPosX, currentBlock.transform.position.y, 0);
        currentBlock.transform.localScale = lastSize;

    }

    public void AfterAdjust()
    {
        posY += block.transform.localScale.y;
        recentBlock = currentBlock;
        currentBlock = Instantiate(block, new Vector2(-BlockTime - 4.2f, posY), Quaternion.identity, blockParent);
        currentBlock.transform.localScale = lastSize;
    }

    public void FallingPart()
    {
        if (fallingBlock != null)
            Destroy(fallingBlock);

        Vector3 fallingBlockPos = currentBlock.transform.position + (Mathf.Sign(gapX) * lastSize.x + gapX) * 0.5f * Vector3.right;
        fallingBlock = Instantiate(block, fallingBlockPos, Quaternion.identity, blockParent);

        fallingBlock.transform.localScale = new Vector3(Mathf.Abs(gapX), fallingBlock.transform.localScale.y, 1);
        recentBlock = currentBlock;

    }

    public void FallBlock(GameObject fallingObject)
    {
        if (fallingObject == null)
            return;

        sr = fallingObject.GetComponent<SpriteRenderer>();
        if (colorCounter > 0)
        {
            colorCounter -= Time.deltaTime;
        }
        else
        {
            Destroy(fallingObject);
        }

        fallingColor = sr.color;
        fallingColor.a = colorCounter;
        sr.color = fallingColor;
        fallingObject.transform.position += 6f * colorCounter * Time.deltaTime * Vector3.down;
    }

    public void MoveBlock()
    {
        if (fail)
            return;

        if (!onDestination)
        {
            if (currentBlock.transform.position.x < horzExtent + (currentBlock.transform.localScale.x * -0.5f))
            {
                currentBlock.transform.position += currentSpeed * Time.deltaTime * Vector3.right;
            }
            else
            {
                onDestination = true;
            }
        }
        else
        {
            if (-currentBlock.transform.position.x < horzExtent - (currentBlock.transform.localScale.x * 0.5f))
            {
                currentBlock.transform.position -= currentSpeed * Time.deltaTime * Vector3.right;
            }
            else
            {
                onDestination = false;
            }
        }

    }



    public void ResetGame()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        GameController.instance.score = 0;

        for (int i = 0; i < blockParent.transform.childCount; i++)
        {
            Destroy(blockParent.GetChild(i).gameObject);
        }

        fail = false;
        enabled = false;
        recentBlock = MainBlock;
        currentSpeed = startSpeed;
        posY = block.transform.localScale.y - 4;
        lastSize = MainBlock.transform.localScale;
        GameController.instance.isContinued = false;

        if (GoogleAdsManager.instance!=null)
            GoogleAdsManager.instance.RequestAndLoadRewardedAd();
    }

    public void StartGame()
    {
        ResetGame();
        PrepareUI();

        posY = MainBlock.transform.position.y + block.transform.localScale.y;
        currentBlock = Instantiate(block, new Vector2(-BlockTime - 4.2f, posY), Quaternion.identity, blockParent);

       
        if (GoogleAdsManager.instance != null)
            GoogleAdsManager.instance.RequestAndLoadInterstitialAd();
    }

    private void PrepareUI()
    {
        UIManager.instance.OpenPanel(2);
        enabled = true;
    }

    public void ContinueGame()
    {
        PrepareUI();
        Destroy(fallingBlock);

        fail = false;
        currentBlock.transform.position = new Vector2(-BlockTime - 4.2f, posY);

        if (MarketManager.token>= GameController.instance.continuePrice)
        {
            MarketManager.token -= GameController.instance.continuePrice;
        }
        PlayerPrefs.SetInt("token", MarketManager.token);
        UIManager.instance.inGameTokenText.text = MarketManager.token.ToString();
        GameController.instance.isContinued = true;

    }
}
