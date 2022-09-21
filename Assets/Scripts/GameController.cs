using UnityEngine;
using System;
public class GameController : MonoBehaviour
{
    public static Action GameEnding;
    public static Action NewBlock;
    public static Action PerfectBLock;

    public int perfectComboAmount = 0;

    public int score = 0;
    public int continuePrice = 200;

    public bool isContinued = false;

    [HideInInspector] public BlockManager bm;

    public static GameController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        bm = BlockManager.instance;
        bm.enabled = false;
        score = 0;
    }

    public bool IsHighScore()
    {
        return score > PlayerPrefs.GetInt("highScore");
    }

    private void OnEnable()
    {
        NewBlock += () => score++;
        NewBlock += () => UIManager.instance.scoreTexts[2].text = score.ToString();
        PerfectBLock += () => perfectComboAmount++;
    }

    private void OnDisable()
    {
        NewBlock -= () => score++;
        NewBlock -= () => UIManager.instance.scoreTexts[2].text = score.ToString();
        PerfectBLock -= () => perfectComboAmount++;
    }

    private void OnDestroy()
    {
        GameEnding = null;
        NewBlock = null;
        PerfectBLock = null;
    }

    
}
