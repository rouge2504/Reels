using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationCounter : MonoBehaviour
{
    public int totalCoins;

    public float amountCoins;

    public TextMeshProUGUI counterCoin;
    public float prizeWonCoins;
    public AnimationCurve curve;
    public float lerpTimeCoin = 1;
    public float _timerCoin = 0;
    public bool activeAnimation;


    public void Init(int amount)
    {
        counterCoin.text = amount.ToString("#,##0").Replace(".", ",");
    }

    public void SetAnimation(float amount, float restAmount)
    {
        amountCoins = amount;
        prizeWonCoins = restAmount;
        if (amount == 0)
        {
            counterCoin.text = prizeWonCoins.ToString("#,##0").Replace(".", ",");
            return;
        }
        activeAnimation = true;
    }
    void Update()
    {
        if (activeAnimation)
        {
            float amount = amountCoins  ;
            _timerCoin += Time.deltaTime;
            if (_timerCoin > lerpTimeCoin)
            {
                _timerCoin = lerpTimeCoin;
                activeAnimation = false;
            }

            float lerpRatio = _timerCoin / lerpTimeCoin;

            float rest = curve.Evaluate(lerpRatio);

            float x = rest * prizeWonCoins ;

            amount += x;


            counterCoin.text = amount.ToString("#,##0").Replace(".", ",");


            if (amountCoins <= 0)
            {
                counterCoin.text = "0";

            }


        }
        else
        {
            _timerCoin = 0;
        }


    }
}

