﻿using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    public enum IncomeLevel
    {
        kLow,
        kMedium,
        kHigh,
        kExcellent
    }

    public const int StartingMoney = 15;
    public static int Money { get; private set; }
    public static IncomeLevel CurrentIncomeLevel { get; private set; }
    public static int CurrentIncome
    {
        get
        {
            switch (CurrentIncomeLevel)
            {
                case IncomeLevel.kExcellent:
                    return 88;

                case IncomeLevel.kHigh:
                    return 66;

                case IncomeLevel.kMedium:
                    return 44;

                case IncomeLevel.kLow:
                    return 22;

                default:
                    return 0;
            }

        }
    }
    public static int IncomeFromChildren { get { return 88 * ChildManager.ChildrenGraduated; } }

    public IncomeManager()
    {
        CurrentIncomeLevel = IncomeLevel.kLow;
        Money = StartingMoney;
    }

    public static void AddMoney(int money)
    {
        Money += money;
    }

    public static void IncreaseIncomeLevel()
    {
        switch (CurrentIncomeLevel)
        {
            case IncomeLevel.kExcellent:
                break;

            case IncomeLevel.kHigh:
                CurrentIncomeLevel = IncomeLevel.kExcellent;
                break;

            case IncomeLevel.kMedium:
                CurrentIncomeLevel = IncomeLevel.kHigh;
                break;

            case IncomeLevel.kLow:
                CurrentIncomeLevel = IncomeLevel.kMedium;
                break;

            default:
                break;
        }
    }

    public static void DecreaseIncomeLevel()
    {
        switch (CurrentIncomeLevel)
        {
            case IncomeLevel.kExcellent:
                CurrentIncomeLevel = IncomeLevel.kHigh;
                break;

            case IncomeLevel.kHigh:
                CurrentIncomeLevel = IncomeLevel.kMedium;
                break;

            case IncomeLevel.kMedium:
                CurrentIncomeLevel = IncomeLevel.kLow;
                break;

            case IncomeLevel.kLow:
                break;

            default:
                break;
        }
    }
}