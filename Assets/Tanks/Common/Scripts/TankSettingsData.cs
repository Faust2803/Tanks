using UnityEngine;

public class TankSettingsData : MonoBehaviour
{
    [SerializeField] private Renderer[] _renderersFirst;
    [SerializeField] private Renderer[] _renderersSecond;
    [SerializeField] private Renderer[] _renderersThird;
    
    public Renderer[] RenderersFirst  => _renderersFirst;
    public Renderer[] RenderersSecond => _renderersSecond;
    public Renderer[] RenderersThird => _renderersThird;
}
