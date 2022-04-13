using Assets.Scripts.Bases;
using Assets.Scripts.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{

    public class CubeMovement : Singleton<CubeMovement>
    {
        public Transform cube, crosshair, levelObject;
        public RaycastHit hit;
        Vector3 touchPosWorld;
        private RaycastHit hitInformation;
        [SerializeField] private bool moveCube = false;
        public Transform currentLevel;
        public Transform nextCube;
        public Image nextCubeColor;

        private void Start()
        {
            levelObject = currentLevel;
            nextCube = ObjectPoolManager.Instance.GetObject("Cube").transform;
            nextCube.transform.position = Vector3.down * 30;
            nextCube.transform.rotation = Quaternion.identity;
            nextCube.gameObject.SetActive(true);
            nextCubeColor.GetComponent<Image>().color = nextCube.GetComponent<Renderer>().material.color;

        }
        void Update()
        {

            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    Physics.Raycast(ray, out hitInformation);


                    if (hitInformation.transform == cube)
                    {
                        Debug.Log("Hayde");
                        moveCube = true;
                        cube.GetComponent<BoxCollider>().enabled = false;
                        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));


                        crosshair.position = new Vector3(touchedPos.x, touchedPos.y + 1.5f, crosshair.position.z);//Vector3.Lerp(crosshair.position,   desiredPos  , Time.deltaTime / 25);

                    }
                    cube.transform.position = crosshair.position;


                }
                else if (touch.phase == TouchPhase.Moved)
                {


                    #region RotateLevelObject and MoveCube
                    if (hitInformation.transform != cube)
                    {

                        levelObject.transform.Rotate(Input.GetTouch(0).deltaPosition.y / 2, Input.GetTouch(0).deltaPosition.x / -2, 0f, Space.World);

                    }
                    else
                    {
                        moveCube = true;
                        cube.GetComponent<BoxCollider>().enabled = false;
                        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
                        crosshair.position = Vector3.Lerp(crosshair.position, new Vector3(touchedPos.x, touchedPos.y + 1.5f, crosshair.position.z), 1);

                        #region PlacingCube
                        if (Physics.Raycast(Camera.main.transform.position, (crosshair.transform.position - Camera.main.transform.position), out hit))
                        {

                            if (hit.transform.TryGetComponent<ColorCube>(out ColorCube hitCube))
                            {

                                cube.transform.position = hit.transform.position + hit.normal;
                                cube.transform.rotation = hit.transform.rotation;

                            }


                        }
                        else
                        {

                            cube.transform.position = crosshair.transform.position;
                            cube.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }

                        #endregion


                    }
                    #endregion

                }
                else if (touch.phase == TouchPhase.Ended)
                {




                    if (moveCube)
                    {
                        if (hit.collider != null && hit.transform.TryGetComponent<ColorCube>(out ColorCube _cube))
                        {




                            currentLevel.GetComponent<LevelObject>().cubes.Add(cube);
                            cube.transform.parent = levelObject;
                            cube.GetComponent<ColorCube>().checkCubes = true;
                            cube.GetComponent<BoxCollider>().enabled = true;

                            cube = nextCube;
                            cube.transform.position = Vector3.down;
                            cube.GetComponent<Animation>().Play("CubeSpawn");
                            cube.transform.rotation = Quaternion.identity;


                            var newCube = ObjectPoolManager.Instance.GetObject("Cube").transform;
                            newCube.transform.position = Vector3.down * 30;
                            newCube.transform.rotation = Quaternion.identity;
                            newCube.gameObject.SetActive(true);
                            nextCube = newCube;




                        }
                    }




                    cube.GetComponent<BoxCollider>().enabled = true;
                    nextCube.GetComponent<BoxCollider>().enabled = true;
                    crosshair.position = Vector3.down * 3;
                    cube.position = crosshair.position;

                    moveCube = false;






                }


            }
            else
            {

                nextCubeColor.GetComponent<Image>().color = nextCube.GetComponent<Renderer>().material.color;
            }

        }

    }



}
