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
    public bool hasObject = false;

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

        if (hasObject)
        {

            StartCoroutine(RotateObject());
            //var rot = new Vector3(Mathf.Lerp(transform.eulerAngles.x, 0, Time.deltaTime * 2f), transform.eulerAngles.y, Mathf.Lerp(transform.eulerAngles.z, 0, Time.deltaTime * 2f));
            //transform.eulerAngles = rot;//Vector3.Slerp(transform.eulerAngles, Vector3.zero, Time.deltaTime * 2f);


            Extentions.TasksExtentions.DoActionAfterSecondsAsync(() => { hasObject = false; }, 3f);



        }
        else
        {
            transform.Rotate(Vector3.up, Space.World);
        }


        UIManager.Instance.progressBarFill.fillAmount = 1f;
        var _cubes = cubes;
        for (int i = 0; i <= _cubes.Count - 1; i++)
        {

            _cubes[i].GetComponent<ColorCube>().delay = Random.Range(.2f, 1.8f);
            StartCoroutine(_cubes[i].GetComponent<ColorCube>().DestroyCube());
            CubeMovement.Instance.cube.GetComponent<BoxCollider>().enabled = false;


            yield return new WaitForSeconds(.1f);


            //TODO: WinCanvas
            if (i >= _cubes.Count - 1)
            {
                if (!hasObject)
                {
                    yield return new WaitForSeconds(2f);
                    UIManager.Instance.confetti.Play();
                    UIManager.Instance.winCanvas.gameObject.SetActive(true);
                }
                else
                {
                    UIManager.Instance.confetti.Play();
                    UIManager.Instance.objectWinCanvas.gameObject.SetActive(true);


                }


            }

        }

        UIManager.Instance.timerActive = false;




    }

    public IEnumerator RotateObject()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2);
        yield return 0;
    }
}
