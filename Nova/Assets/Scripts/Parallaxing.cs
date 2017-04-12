using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds; //Array list of all the back and foregrounds to be parallaxed
    private float[] parallaxScales; //The proportion of the cameras movement to move the backgrounds by
    public float smoothing = 1f; //How smooth the parallax is going to be. Make sure to set this above 0.
    private Camera cam; //Reference to the main camera's transform;
    private Vector3 previousCamPos; //Store the position of the camera in the previous frame
    
    private bool first = false;
    //Called before Start. Great for references
    void Awake()
    {
        //Set up the reference to the camera
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //Application.LoadLevelAdditive("VerticalSlice");
        
    }


	// Use this for initialization
	void Start () {
        //The previous frame had the current frame's camera position
        previousCamPos = cam.transform.position;
        //Set parallaxScales array to the size of the backgrounds array length
        parallaxScales = new float[backgrounds.Length];
        //For each background set it's corresponding parallaxScale to that background's z value, times -1 for correct motion
        for(int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //for each background
        for(int i = 0; i < backgrounds.Length; i++)
        {
            //the parallax is the opposite of the camera movement because the previous frame is multiplied by the scale
            float parallax = (previousCamPos.x - cam.transform.position.x) * parallaxScales[i];
            //Set a target X position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;
            //Create a target position which is the background's current position with it's target X position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            //fade between current position and the target position using a lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
            //Time.deltaTime is accounting for the variable frame rate
        }
        //Set previous cam pos to the current camera position
        previousCamPos = cam.transform.position;
        
        
        	
	}

}
