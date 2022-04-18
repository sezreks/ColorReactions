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
        private bool moveCube = false, rayHitCube = false;
        public Transform currentLevel;
        public Transform nextCube;
        public Image nextCubeColor;

        //Mouse controls

        public Image finger;
        [SerializeField] private float rotationRate = 3.0f;
        [SerializeField] private bool xRotation;
        [SerializeField] private bool yRotation;
        [SerializeField] private bool invertX;
        [SerializeField] private bool invertY;
        [SerializeField] private bool touchAnywhere;
        private float m_previousX;
        private float m_previousY;
        private Camera m_camera;
        private bool m_rotating = false;

        private void Start()
        {
            Input.simulateMouseWithTouches = false;
            m_camera = Camera.main;
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


            if (UIManager.Instance.timerActive)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    finger.gameObject.SetActive(true);
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Physics.Raycast(ray, out hitInformation);

                    if (hitInformation.transform == cube)
                    {
                        Debug.Log("Hayde");
                        moveCube = true;

                    }
                    else
                    {
                        m_rotating = true;
                        m_previousX = Input.mousePosition.x;
                        m_previousY = Input.mousePosition.y;
                    }
                }

                #region MouseControls
                if (Input.GetMouseButton(0))
                {
                    finger.transform.position = Input.mousePosition;
                    var mouse = Input.mousePosition;
                    if (moveCube)
                    {
                        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));
                        crosshair.position = new Vector3(touchedPos.x, touchedPos.y + 1.5f, crosshair.position.z);
                        cube.transform.position = crosshair.position;
                        cube.GetComponent<BoxCollider>().enabled = false;


                        if (Physics.Raycast(Camera.main.transform.position, (crosshair.transform.position - Camera.main.transform.position), out hit))
                        {

                            if (hit.transform.TryGetComponent<ColorCube>(out ColorCube hitCube))
                            {

                                cube.transform.position = hit.transform.position + hit.normal;
                                cube.transform.rotation = hit.transform.rotation;
                                rayHitCube = true;
                            }


                        }
                        else
                        {

                            cube.transform.position = crosshair.transform.position;
                            cube.transform.rotation = Quaternion.Euler(0, 0, 0);
                            rayHitCube = false;

                        }

                    }
                    else
                    {

                        var touch = Input.mousePosition;
                        var deltaX = -(Input.mousePosition.y - m_previousY) * rotationRate;
                        var deltaY = -(Input.mousePosition.x - m_previousX) * rotationRate;
                        if (!yRotation) deltaX = 0;
                        if (!xRotation) deltaY = 0;
                        if (invertX) deltaY *= -1;
                        if (invertY) deltaX *= -1;
                        levelObject.transform.Rotate(deltaX, deltaY, 0, Space.World);

                        m_previousX = Input.mousePosition.x;
                        m_previousY = Input.mousePosition.y;
                    }


                }

                if (Input.GetMouseButtonUp(0))
                {
                    finger.gameObject.SetActive(false);
                    m_rotating = false;

                    if (moveCube && rayHitCube)
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

                    rayHitCube = false;
                    moveCube = false;
                }
                #endregion



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


                            crosshair.position = new Vector3(touchedPos.x, touchedPos.y + 1.5f, crosshair.position.z);

                        }
                        cube.transform.position = crosshair.position;


                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {


                        #region RotateLevelObject and MoveCube
                        if (hitInformation.transform != cube)
                        {

                            levelObject.transform.Rotate(Input.GetTouch(0).deltaPosition.y / 2, Input.GetTouch(0).deltaPosition.x / -2, 0f, Space.World);
                            //var clampRot = ClampAngle(transform.eulerAngles.x, -50, 50);
                            //levelObject.transform.eulerAngles = new Vector3(clampRot, levelObject.eulerAngles.y, levelObject.eulerAngles.z);

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
                                    rayHitCube = true;
                                }


                            }
                            else
                            {

                                cube.transform.position = crosshair.transform.position;
                                cube.transform.rotation = Quaternion.Euler(0, 0, 0);
                                rayHitCube = false;

                            }

                            #endregion


                        }
                        #endregion

                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {




                        if (moveCube && rayHitCube)
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

                        rayHitCube = false;
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



}
