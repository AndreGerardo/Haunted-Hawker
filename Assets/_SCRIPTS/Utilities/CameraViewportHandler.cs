using UnityEngine;

[ExecuteInEditMode]
public class CameraViewportHandler : MonoBehaviour
{
    public enum Constraint { Landscape, Portrait }

    public float UnitsSize = 1;
    public Constraint constraint = Constraint.Portrait;
    private new Camera camera;

    public bool executeInUpdate;

    private float _width;
    private float _height;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        CalculateScreenResolution();
    }

    private void CalculateScreenResolution()
    {
        if (constraint == Constraint.Landscape)
        {
            camera.orthographicSize = 1f / camera.aspect * UnitsSize / 2f;
        }
        else
        {
            camera.orthographicSize = UnitsSize / 2f;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR

        if (executeInUpdate)
            CalculateScreenResolution();

#endif
    }

}