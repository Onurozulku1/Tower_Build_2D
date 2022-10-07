using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] MenuPanels;
    public GameObject ContinuePanel;
    public GameObject TokenContinueButton;
    public Text perfectComboText;

    public GameObject removeAdsObject;

    private Vector2 PerfectComboTextRandomPos
    {
        get
        {
            float x = Random.Range(-150, 150);
            float y = Random.Range(-50, 300);
            return new Vector3(x, y, 0);
        }
    }

    [Header("Score & Token Elements")]
    public Text[] scoreTexts;
    public Text inGameTokenText;
    public Text comboTokenText;

    [Header("Audio Elements")]
    public Image audioImage;
    public Sprite[] audioSprites;

    

    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inGameTokenText.text = PlayerPrefs.GetInt("token").ToString();
        scoreTexts[3].text = "High Score : " + PlayerPrefs.GetInt("highScore").ToString();

        if (PlayerPrefs.GetInt("removeAds") == 0)
            removeAdsObject.SetActive(true);
        else
            Destroy(removeAdsObject); 

    }

    public void UpdateEndingScores()
    {
        scoreTexts[0].text = GameController.instance.score.ToString();

        if (GameController.instance.IsHighScore())
        {
            scoreTexts[1].text = "New High Score";
            PlayerPrefs.SetInt("highScore", GameController.instance.score);
        }
        else
            scoreTexts[1].text = "High Score : " + PlayerPrefs.GetInt("highScore").ToString();

        OpenPanel(1);
    }
    
    public void MainMenu()
    {
        BlockManager.instance.ResetGame();
        scoreTexts[3].text = "High Score : " + PlayerPrefs.GetInt("highScore").ToString();
        OpenPanel(0);
    }

    public void OpenPanel(int index)
    {
        if (removeAdsObject != null && PlayerPrefs.GetInt("removeAds") == 1)
                Destroy(removeAdsObject);

        foreach (GameObject item in MenuPanels)
        {
            item.SetActive(false);
        }
        MenuPanels[index].SetActive(true);

        scoreTexts[2].text = "0";

    }

    private bool TogglingAudio = false;
    public void ToggleAudio()
    {
        TogglingAudio = !TogglingAudio;

        if (!TogglingAudio)
        {
            AudioManager.instance.ToggleAudio(1);
            audioImage.sprite = audioSprites[0];
        }
        else
        {
            AudioManager.instance.ToggleAudio(0);
            audioImage.sprite = audioSprites[1];

        }

    }

    public void SetPerfectBlockText()
    {
        Vector2 pos = PerfectComboTextRandomPos;
        perfectComboText.transform.rotation = Quaternion.Euler(Mathf.Sign(pos.x) * Random.Range(9, 10) * Vector3.forward);
        perfectComboText.transform.localPosition = pos;
    }

    private void DisplayContinuePanel()
    {

        if (GameController.instance.isContinued)
        {
            ContinuePanel.SetActive(false);
            return;
        }

        ContinuePanel.SetActive(true);

        if (PlayerPrefs.GetInt("token") < GameController.instance.continuePrice)
        {
            TokenContinueButton.SetActive(false);
        }
        else
        {
            TokenContinueButton.SetActive(true);
        }

    }

    private void OnEnable()
    {
        GameController.GameEnding += UpdateEndingScores;
        GameController.GameEnding += DisplayContinuePanel;

        GameController.PerfectBLock += SetPerfectBlockText;
    }

    private void OnDisable()
    {
        GameController.GameEnding -= UpdateEndingScores;
        GameController.GameEnding -= DisplayContinuePanel;

        GameController.PerfectBLock -= SetPerfectBlockText;
    }
}
