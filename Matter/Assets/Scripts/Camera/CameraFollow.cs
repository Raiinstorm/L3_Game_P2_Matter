using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject cameraFollowObj; 
    public GameObject cameraObj;
	CameraSnapping _snap;

    Vector3 FollowPos; // position
    
    public float cameraMoveSpeed = 120.0f; // speed move
    public float clampAngle = 80.0f; // angle displacement
    public float inputSensibility = 150.0f; // sensibility move camera
    public float camDistanceXToPlayer; // Distance with the player on Axis X
    public float camDistanceYToPlayer; // "" Axis Y
    public float camDistanceZToPlayer; // "" Axis Z
    public float mouseX;
    public float mouseY;
    public float finalInputX; //Add mouse + sticks
    public float finalInputZ;
    public float smoothX;
    public float smoothY;

    public float rotX = 0.0f; // rotation cam
    public float rotY = 0.0f;
	[HideInInspector] public bool BlockRotation;
	[HideInInspector] public bool Lerping;

    float _oldRotY;
	float _oldRotX;
	bool _antispam;
        
    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles; //obtenir la rotation de l'objet camera dans l'espace sur lui même
        rotX = rotation.x;
        rotY = rotation.y;
		_snap = GetComponent<CameraSnapping>();
        Cursor.lockState = CursorLockMode.Locked; //bloquer le curseur de la souris 
        Cursor.visible = false; // Rend le curseur de la souris invisible
    }

    void Update()
    {
        // mise en place de la rotation
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical"); 
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        // fusionner les entrées de la souris avec celle des sticks manettes
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;

		if(!BlockRotation)
		{
			//action de rotation de la caméra
			rotY += finalInputX * inputSensibility * Time.deltaTime;
			rotX += finalInputZ * inputSensibility * Time.deltaTime;
		}

		if((finalInputX != 0 || finalInputZ !=0) && !BlockRotation)//final Input
		{
			_snap.ActivateLerp = false;
			_snap.StopCoroutine();
		}

		//clamp Rotation Y
		if (rotY < 0)
		{
			rotY = 360;
		}
		else if (rotY > 360)
		{
			rotY = 0 ;
		}

		if(GameMaster.i.ResetRotation)
		{
			rotX = 0;
			rotY = 0;
		}

        //bloquer l'angle de rotation
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

		//Debug.Log("rotX : " + rotX + " | rotY : " + rotY);
    }

    void LateUpdate()
    {
        CameraUpdater();    
    }
    
    void CameraUpdater()
    {
        // Definition du target (la cible) à suivre
        Transform target = cameraFollowObj.transform;

        //Deplacer le gameObject vers le target
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
} 
