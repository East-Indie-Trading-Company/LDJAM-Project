using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class MapUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Button brightspireButton;
    [SerializeField] Button trestelButton;
    [SerializeField] Button dursimButton;
    [SerializeField] Button styxButton;
    [SerializeField] Button lochButton;
    [SerializeField] Button scorchedButton;
    [SerializeField] Button taravalButton;

    [SerializeField] NPCData brightspireData;
    [SerializeField] NPCData trestelData;
    [SerializeField] NPCData dursimData;
    [SerializeField] NPCData styxData;
    [SerializeField] NPCData lochData;
    [SerializeField] NPCData scorchedData;
    [SerializeField] NPCData taravalData;

    [SerializeField] GameObject mapButtonUI;
    [SerializeField] GameObject mapBackButtonUI;
    [SerializeField] TownUI townUI;

    Button backButton;
    void Start()
    {
        backButton = mapBackButtonUI.GetComponent<Button>();
        mapButtonUI.SetActive(true);
        mapBackButtonUI.SetActive(false);

        backButton.onClick.AddListener(onBackClick);

        brightspireButton.onClick.AddListener(brightspireOnClick);
        trestelButton.onClick.AddListener(trestelOnClick);
        dursimButton.onClick.AddListener(dursimOnClick);
        styxButton.onClick.AddListener(styxOnClick);
        lochButton.onClick.AddListener(lochOnClick);
        scorchedButton.onClick.AddListener(scorchedOnClick);
        taravalButton.onClick.AddListener(taravalOnClick);
    }



    void brightspireOnClick()
    {
        updateUI(brightspireData);
    }
    void trestelOnClick()
    {
        updateUI(trestelData);
    }

    void dursimOnClick()
    {
        updateUI(dursimData);
    }

    void styxOnClick()
    {
        updateUI(styxData);
    }

    void lochOnClick()
    {
        updateUI(lochData);
    }
    void scorchedOnClick()
    {
        updateUI(scorchedData);
    }

    void taravalOnClick()
    {
        updateUI(taravalData);
    }


    void updateUI(NPCData npcData)
    {
        // Close the map ui
        mapButtonUI.SetActive(false);
        mapBackButtonUI.SetActive(true);
        townUI.SetTownUI(npcData);

    }

    void onBackClick()
    {
        mapButtonUI.SetActive(true);
        mapBackButtonUI.SetActive(false);
        // Close Market UI
        townUI.RemoveTownUI();
    }
    
}
