using Assets.Scripts;
using Assets.Scripts.Bases;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    [HideInInspector] public Transform winCanvas, failCanvas, inGameCanvas, startCanvas, objectWinCanvas;
    [HideInInspector] public Image progressBarFill;
    [HideInInspector] public int levelCubeCount, mincubeCount;
    [HideInInspector] public ParticleSystem confetti;
    [HideInInspector] public TMP_Text remainingTime;
    public GameObject fingerAnim, tapToPlay;

    public bool timerActive;
    public float timer;
    [SerializeField] private TMP_Text activeLevel, nextLevel, winScreenText;

    private void Start()
    {
        if (PlayerPrefs.GetInt("HasOpened") == 0)
        {
            fingerAnim.SetActive(true);
            tapToPlay.SetActive(false);
            PlayerPrefs.SetInt("HasOpened", 1);

        }
        activeLevel.text = (PlayerData.Instance.curLevel + 1).ToString();
        nextLevel.text = (PlayerData.Instance.curLevel + 2).ToString();
        winScreenText.text = activeLevel.text;



        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        remainingTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void FixedUpdate()
    {
        if (timer > 0 && timerActive)
        {
            timer -= Time.deltaTime;

            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            remainingTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            var levelObject = CubeMovement.Instance.currentLevel.GetComponent<LevelObject>();
            progressBarFill.fillAmount = Mathf.Lerp(1, 0, (float)levelObject.cubes.Count / (float)levelCubeCount);


        }
        else if (timer > 0 && !timerActive)
        {



            Debug.Log("YouWin");



        }
        else
        {
            failCanvas.gameObject.SetActive(true);
            Debug.Log("Failed");
            timer = 0;
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HasOpened", 0);
    }

    public void StartGame()
    {

        timerActive = true;
        startCanvas.gameObject.SetActive(false);

    }
    public void NextLevel()
    {

        PlayerData.Instance.curLevel += 1;
        SceneManager.LoadScene(0);


    }
    public void ReloadScene()
    {

        SceneManager.LoadScene(0);
    }



}
