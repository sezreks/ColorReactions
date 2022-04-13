using Assets.Scripts;
using Assets.Scripts.Bases;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Transform winCanvas, failCanvas, inGameCanvas;
    public Image progressBarFill;
    [HideInInspector] public int levelCubeCount, mincubeCount;
    public TMP_Text remainingTime;

    public bool timerActive;
    public float timer, countdownTime;
    [SerializeField] private TMP_Text activeLevel, nextLevel, winScreenText;

    private void Start()
    {
        countdownTime = timer;
        activeLevel.text = (PlayerData.Instance.curLevel + 1).ToString();
        nextLevel.text = (PlayerData.Instance.curLevel + 2).ToString();
        winScreenText.text = activeLevel.text;
    }
    public void FillProgressBar()
    {

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
