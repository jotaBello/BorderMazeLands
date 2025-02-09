using System.Collections.Generic;
using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    public List<GameObject> panelList;
    public GameObject lorePanel;
    public GameObject rulePanel;
    public GameObject trapsPanel;
    public GameObject playersPanel;
    public bool lorePanelHided;
    public bool rulePanelHided;
    public bool trapsPanelHided;
    public bool playersPanelHided;

    //Shows/Hides the different panels of the HowToPlay menu

    public void Show_Hide_LorePanel()
    {
        foreach (var panel in panelList)
        {
            if (panel != lorePanel)
                panel.SetActive(false);
        }
        rulePanelHided = true;
        trapsPanelHided = true;
        playersPanelHided = true;




        if (lorePanelHided)
        {
            lorePanel.SetActive(true);
            lorePanelHided = false;
        }
        else
        {
            lorePanel.SetActive(false);
            lorePanelHided = true;
        }
    }
    public void Show_Hide_RulePanel()
    {
        foreach (var panel in panelList)
        {
            if (panel != rulePanel)
                panel.SetActive(false);
        }
        lorePanelHided = true;
        trapsPanelHided = true;
        playersPanelHided = true;



        if (rulePanelHided)
        {
            rulePanel.SetActive(true);
            rulePanelHided = false;
        }
        else
        {
            rulePanel.SetActive(false);
            rulePanelHided = true;
        }
    }
    public void Show_Hide_TrapsPanel()
    {
        foreach (var panel in panelList)
        {
            if (panel != trapsPanel)
                panel.SetActive(false);
        }
        lorePanelHided = true;
        rulePanelHided = true;
        playersPanelHided = true;



        if (trapsPanelHided)
        {
            trapsPanel.SetActive(true);
            trapsPanelHided = false;
        }
        else
        {
            trapsPanel.SetActive(false);
            trapsPanelHided = true;
        }
    }
    public void Show_Hide_PlayersPanel()
    {
        foreach (var panel in panelList)
        {
            if (panel != playersPanel)
                panel.SetActive(false);
        }
        lorePanelHided = true;
        rulePanelHided = true;
        trapsPanelHided = true;



        if (playersPanelHided)
        {
            playersPanel.SetActive(true);
            playersPanelHided = false;
        }
        else
        {
            playersPanel.SetActive(false);
            playersPanelHided = true;
        }
    }

}
