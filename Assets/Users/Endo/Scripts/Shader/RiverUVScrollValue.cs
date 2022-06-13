using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RiverUVScrollValue : MonoBehaviour
{
    [SerializeField]
    private float xScrollSpeed;

    [SerializeField]
    private float yScrollSpeed;

    private Renderer              _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    private static readonly int ScrollSpeedX = Shader.PropertyToID("_ScrollSpeedX");
    private static readonly int ScrollSpeedY = Shader.PropertyToID("_ScrollSpeedY");

    private void Start()
    {
        _renderer              = GetComponent<Renderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();

        _renderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetFloat(ScrollSpeedX, xScrollSpeed);
        _materialPropertyBlock.SetFloat(ScrollSpeedY, yScrollSpeed);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
