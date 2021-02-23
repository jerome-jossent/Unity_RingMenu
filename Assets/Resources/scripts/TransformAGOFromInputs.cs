using UnityEngine;

public class TransformAGOFromInputs : MonoBehaviour
{
    [Header("If none get host GameObject")]
    public GameObject Object;
    private Transform TRF;

    [Header("Parameters")]
    public float mouseSensitivity = 100.0f;
    public float mouseRotYAmpMax = 80.0f;
    public float displacementSpeed = 0.2f;
    public float displacementBoostSpeed = 4.0f;

    private float displacementcoeff;

    [SerializeField] bool Altitude_Fixe;
    float altitude_fixe;

    void Start()
    {
        if (Object == null)
            Object = gameObject;
        TRF = Object.transform;
        displacementcoeff = displacementSpeed;

        if (Altitude_Fixe)
            altitude_fixe = TRF.position.y;
    }

    void Update()
    {
        if (!Input.GetMouseButton(1)) return;

        // Inputs
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float altitude = Input.GetAxis("Mouse ScrollWheel");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        if (Input.GetButtonDown("Fire1")) displacementcoeff = displacementSpeed * displacementBoostSpeed;
        if (Input.GetButtonUp("Fire1")) displacementcoeff = displacementSpeed;

        // translation
        TRF.Translate(horizontal * displacementcoeff, altitude, vertical * displacementcoeff);

        if (Altitude_Fixe)
            TRF.position = new Vector3(TRF.position.x, altitude_fixe, TRF.position.z);

        // rotation
        TRF.rotation = Quaternion.Euler(TRF.localRotation.eulerAngles.x + mouseY * mouseSensitivity * Time.deltaTime,
                                        TRF.localRotation.eulerAngles.y + mouseX * mouseSensitivity * Time.deltaTime,
                                        0.0f);
    }
}