public class Path
{
    private int[] _path;
    private Grid _grid;
    private int _currentStep;

    public Path(int[] path, Grid grid)
    {
        _path = path;
        _grid = grid;
        _currentStep = 0;
    }
    public bool HasNextStep()
    {
        return _currentStep < _path.Length;
    }

    public int GetNextStep()
    {
        if (!HasNextStep())
        {
            throw new System.InvalidOperationException("No more steps in the path.");
        }

        return _path[_currentStep++];
    }

    public int PreviousStep()
    {
        if (_currentStep <= 0)
        {
            throw new System.InvalidOperationException("No previous step available.");
        }

        return _path[--_currentStep];
    }
}
