using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject[] MenuPanels;
    public GameObject ContinueButton;

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
        ContinueButton.SetActive(false);
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

    private void OnEnable()
    {
        GameController.GameEnding += UpdateEndingScores;
    }

    private void OnDisable()
    {
        GameController.GameEnding -= UpdateEndingScores;
    }

    public void MainMenu()
    {
        BlockManager.instance.ResetGame();
        scoreTexts[3].text = "High Score : " + PlayerPrefs.GetInt("highScore").ToString();
        OpenPanel(0);
    }

    public void OpenPanel(int index)
    {
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
}
