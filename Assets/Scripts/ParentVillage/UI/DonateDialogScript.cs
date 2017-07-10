using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonateDialogScript : MonoBehaviour {

    private GameObject donateDialogUI;
    private GameObject payButton;
    private int amountToPay;

	// Use this for initialization
	void Start ()
    {
        donateDialogUI = transform.FindChild("DonateDialogUI").gameObject;
        donateDialogUI.SetActive(false);
        payButton = donateDialogUI.transform.FindChild("PayButton").gameObject;
        payButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAmountToPay()
    {
        string textAmountToPay = donateDialogUI.transform.FindChild("InputField").FindChild("InputFieldText").GetComponent<Text>().text;
        if (int.TryParse(textAmountToPay, out amountToPay))
        {
            payButton.SetActive(amountToPay > 0);
        }
        else
        {
            payButton.SetActive(false);
        }
    }

    public void CloseAndPay()
    {
        // 770 CFA to 1 pound
        IncomeManager.AddMoney(amountToPay);
        ChildManager.ApplyEventToAllChildren(new DataPacket(0, 100, 0, 100));
        GameObject.Find("MoneyText").GetComponent<Text>().color = new Color(34.0f / 255, 139.0f / 255, 34.0f / 255, 1.0f);

        Close();
    }

    public void Close()
    {
        transform.Find("DonateDialogUI").gameObject.SetActive(false);
        TimeManager.Paused = false;
    }
}
