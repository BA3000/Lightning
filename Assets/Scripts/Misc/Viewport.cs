using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
    private float _midX;

    private void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector2 topRight = mainCamera.ViewportToWorldPoint(Vector3.one);

        _minX = bottomLeft.x;
        _minY = bottomLeft.y;
        _maxX = topRight.x;
        _maxY = topRight.y;

        _midX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
    }

    public Vector3 PlayerMovablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, _minX + paddingX, _maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, _minY + paddingY, _maxY - paddingY);

        return position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = _maxX + paddingX;
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);

        return position;
    }

    /// <summary>
    /// limit enmey moving only in the right half screen
    /// </summary>
    /// <param name="paddingX"></param>
    /// <param name="paddingY"></param>
    /// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(_midX, _maxX - paddingX);
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);

        return position;
    }

    /// <summary>
    /// move in whole screen randomly
    /// </summary>
    /// <param name="paddingX"></param>
    /// <param name="paddingY"></param>
    /// <returns></returns>
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(_minX + paddingX, _maxX - paddingX);
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);

        return position;
    }
}
