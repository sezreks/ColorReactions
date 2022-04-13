using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public List<Transform> cubes;
    public int minCubeCount;
    public int colorCount;
    public float time;
    [SerializeField] private double fillAmount;

    private void Awake()
    {
        CubeMovement.Instance.currentLevel = transform;
        UIManager.Instance.levelCubeCount = transform.childCount;
        UIManager.Instance.mincubeCount = minCubeCount;
        UIManager.Instance.timer = time;


        foreach (Transform child in transform)
        {

            cubes.Add(child);
        }
    }
    private void Update()
    {
        if (cubes.Count <= minCubeCount && cubes.Count >= 0)
        {
            StartCoroutine(EndGame());
        }

    }
    public IEnumerator EndGame()
    {
        UIManager.Instance.progressBarFill.fillAmount = 1f;
        var _cubes = cubes;
        for (int i = 0; i <= _cubes.Count - 1; i++)
        {

            _cubes[i].GetComponent<ColorCube>().delay = Random.Range(.2f, 1.8f);
            StartCoroutine(_cubes[i].GetComponent<ColorCube>().DestroyCube());
            //Destroy(_cubes[i].gameObject, _cubes[i].GetComponent<Animation>().clip.length);

            yield return new WaitForSeconds(.1f);


            //TODO: WinCanvas
            if (i >= _cubes.Count - 1)
            {
                CubeMovement.Instance.cube.GetComponent<BoxCollider>().enabled = false;
                yield return new WaitForSeconds(2f);
                UIManager.Instance.winCanvas.gameObject.SetActive(true);

            }

        }

        UIManager.Instance.timerActive = false;



    }

}
