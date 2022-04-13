using Assets.Scripts;
using Assets.Scripts.Components;
using System.Collections;
using UnityEngine;

public class ColorCube : MonoBehaviour
{
    public string cubeColor;
    public RaycastHit hit;
    private Vector3[] rayDirections = { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
    public bool showRays, checkCubes = false;
    public float delay;
    [SerializeField] private Material[] colors;
    private void Start()
    {
        var renderer = transform.GetComponent<Renderer>();
        switch (Random.Range(0, CubeMovement.Instance.currentLevel.GetComponent<LevelObject>().colorCount))
        {
            case 0:
                renderer.material = colors[0];
                cubeColor = "Red";
                break;
            case 1:
                renderer.material = colors[1];
                cubeColor = "Green";
                break;
            case 2:
                renderer.material = colors[2];
                cubeColor = "Blue";
                break;
            case 3:
                renderer.material = colors[3];
                cubeColor = "Yellow";
                break;
            case 4:
                renderer.material = colors[4];
                cubeColor = "null";
                break;
            default:
                Destroy(gameObject);
                break;
        }

    }
    private void FixedUpdate()
    {
        rayDirections = new Vector3[] { transform.up, -transform.up, transform.forward, -transform.forward, transform.right, -transform.right };
        if (showRays)
        {
            for (int i = 0; i < 6; i++)
            {
                Debug.DrawRay(transform.position, rayDirections[i], Color.green);
            }
        }


        if (checkCubes)
        {
            rayDirections = new Vector3[] { transform.up, -transform.up, transform.forward, -transform.forward, transform.right, -transform.right };
            for (int i = 0; i < 6; i++)
            {
                if (Physics.Raycast(transform.position, rayDirections[i], out hit, 1))
                {

                    if (hit.transform.TryGetComponent<ColorCube>(out ColorCube cube) && cube.cubeColor == cubeColor)
                    {

                        //transform.GetComponent<BoxCollider>().enabled = false;

                        cube.checkCubes = true;
                        cube.delay += delay + .1f;
                        StartCoroutine(DestroyCube());



                    }


                }

            }
            checkCubes = false;
        }




    }

    public IEnumerator DestroyCube()
    {
        if (delay > 3f)
        {
            delay = 3f;
        }

        CubeMovement.Instance.currentLevel.GetComponent<LevelObject>().cubes.Remove(transform);

        yield return new WaitForSeconds(delay);

        var smoke = ObjectPoolManager.Instance.GetObject("Smoke");
        ParticleSystem.MainModule main = smoke.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        var smokeColor = transform.GetComponent<Renderer>().material.color;
        main.startColor = new Color(smokeColor.r, smokeColor.g, smokeColor.b, .5f);


        //main.startColor.color.a = transform.GetComponent<Renderer>().material.color.a / 2f;

        smoke.transform.position = transform.position;
        smoke.SetActive(true);
        smoke.GetComponent<ParticleSystem>().Play();


        transform.GetComponent<Animation>().Play();
        Destroy(gameObject, this.GetComponent<Animation>().clip.length);

        UIManager.Instance.timer += .15f;







    }
}
