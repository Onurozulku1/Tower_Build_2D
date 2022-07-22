using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static int token;
    private int comboToken = 0;

    private void Awake()
    {
        token = PlayerPrefs.GetInt("token");
    }

    public void GainToken()
    {
        if (BlockManager.instance.perfectBlock)
        {
            comboToken += 5;
            token += comboToken;
            UIManager.instance.comboTokenText.text = comboToken.ToString();
        }
        else
        {
            comboToken = 0;
            token++;
        }
        PlayerPrefs.SetInt("token", token);
        UIManager.instance.inGameTokenText.text = token.ToString();
    }

    private void OnEnable()
    {
        GameController.NewBlock += GainToken;
    }

    private void OnDisable()
    {
        GameController.NewBlock -= GainToken;
    }


}
