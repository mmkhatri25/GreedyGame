using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeScript : MonoBehaviour
{
    public InputField AmountText;
    public Text _amountText_payment, _amountText_screenshot, _amountText_Popup;
    public List<Image> _checkImage;

    public void copyText(string _text)
    {
        GUIUtility.systemCopyBuffer = _text;
    }
    public void CheckList(int _val)
    {
        for(int i = 0 ; i < _checkImage.Count; i++)
        {
            if(i == _val)
            {
                _checkImage[i].color = new Color(0,0,0,1);
            }
            else
            {
                _checkImage[i].color = new Color(1,1,1,1);
            }
        }
    }

    public void AmountBtn(string _amount)
    {
        AmountText.text = _amount;
        _amountText_payment.text = AmountText.text;
        _amountText_screenshot.text = AmountText.text;
        _amountText_Popup.text = AmountText.text;
    }

}
