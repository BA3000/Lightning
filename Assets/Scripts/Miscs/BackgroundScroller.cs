using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]
    private Vector2 scrollVelocity;
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    void Start()
    {
        
    }

    void Update()
    {
        _material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    }
}
