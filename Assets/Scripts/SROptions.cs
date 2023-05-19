using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public partial class SROptions
{
    [Category("Main Controls")]
    public void ResetSaveData()
    {
        GameManager.Instance.ResetSaveData();
    }

    [Category("Main Controls")]
    public void HardRestart()
    {
        GameManager.Instance.HardRestart();
    }


    [Category("Level")]
    public int JumpToLevel
    {
        get { return GameManager.Instance.LinearLevelIndex + 1; }
        set 
        { 
            if (value > 0)
            {
                GameManager.Instance.LinearLevelIndex = value - 1;
                ReloadCurrentLevel();
            }
        }
    }

    [Category("Level")]
    public void ReloadCurrentLevel()
    {
        GameManager.Instance.RetryGameplay();
    }


    [Category("Cheats")]
    public void AddMoney_5K()
    {
        GameManager.Instance.AddCurrency(5000, null);
    }

    [Category("Cheats")]
    public void AddMoney_500K()
    {
        GameManager.Instance.AddCurrency(500000, null);
    }
}
