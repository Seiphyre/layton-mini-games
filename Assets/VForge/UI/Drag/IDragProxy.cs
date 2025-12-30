using UnityEngine;

public interface IDragProxy
{
    void SetScreenPosition(Vector2 screenPos);
    void Show();
    void Hide();
    void Destroy();
}