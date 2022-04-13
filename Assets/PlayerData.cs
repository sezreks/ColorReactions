using Assets.Scripts.Bases;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public List<Transform> levels;
    public List<Material> skyboxMats;

    public int curLevel
    {
        get
        {
            return PlayerPrefs.GetInt("curlevel");
        }
        set
        {

            PlayerPrefs.SetInt("curlevel", value);
        }
    }
    void Awake()
    {
        OpenLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OpenLevel()
    {

        foreach (Transform child in levels)
        {
            child.gameObject.SetActive(false);
        }

        if (curLevel > levels.Count - 1)
        {
            levels[Random.Range(0, levels.Count - 1)].gameObject.SetActive(true);
        }
        else
        {
            levels[curLevel].gameObject.SetActive(true);
        }

        RenderSettings.skybox = skyboxMats[Random.Range(0, skyboxMats.Count - 1)];


    }


}
