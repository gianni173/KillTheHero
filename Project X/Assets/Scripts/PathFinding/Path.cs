using UnityEngine;
using System;

[Serializable]
public class Path
{
    [SerializeField]
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
        return _currentStep + 1 < _path.Length;
    }

    public int GetNextStep()
    {
        if (!HasNextStep())
        {
            Debug.LogWarning("No more steps in the path.");
            return _path[_currentStep];
        }
        return _path[++_currentStep];
    }

    public int PreviousStep()
    {
        if (_currentStep <= 0)
        {
            throw new InvalidOperationException("No previous step available.");
        }

        return _path[--_currentStep];
    }

    public Vector2 GetCurrentStepGridCoord()
    {
        if (_currentStep == 0 || _currentStep > _path.Length)
        {
            throw new InvalidOperationException("Current step is out of bounds.");
        }

        return _grid.IndexToGridCoord(_path[_currentStep]);
    }
    
    public Vector3 GetCurrentStepWorldCoord()
    {
        if (_currentStep > _path.Length)
        {
            throw new InvalidOperationException("Current step is out of bounds.");
        }

        var gridCoord = _grid.IndexToGridCoord(_path[_currentStep]);
        return _grid.GridCoordToWorldCoord(gridCoord);
    }
}
