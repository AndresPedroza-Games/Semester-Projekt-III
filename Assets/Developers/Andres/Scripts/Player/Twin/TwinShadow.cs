using UnityEngine;

public class TwinShadow : MonoBehaviour
{
    private int _Steps = 0;
    private Material _Mat;

    private void Start()
    {
        _Mat = GetComponent<Renderer>().material;

        EventSystemTestRoom.instance.onCrossAnswer += Vanish;
        EventSystemTestRoom.instance.onRestartPuzzle += Restart;

    }

    private void ChangeMat(int step)
    {
        Color currentColor = _Mat.color;
        currentColor.a = step * 0.15f;

        _Mat.color = currentColor;
    }

    private void Vanish(bool answer)
    {
        if (answer && _Steps < 6)
            _Steps++;
        else if (!answer && _Steps > 0)
            _Steps--;

        ChangeMat(_Steps);
    }

    private void Restart()
    {
        _Steps = 0;
        ChangeMat(_Steps);
    }

}
