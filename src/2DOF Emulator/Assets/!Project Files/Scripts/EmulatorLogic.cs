using System;
using System.Collections;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class EmulatorLogic : MonoBehaviour
{
    private static string MapNameKey => Constants.MAP_NAME_KEY;
    private static int WaitTime => Constants.WAIT_TIME;

    [SerializeField] private Transform targetObject;

    private readonly ObjectTelemetryData _objectTelemetryData = new();

    public void OnEnable()
    {
        StartCoroutine(ReadData());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator ReadData()
    {
        var mapName = PlayerPrefs.GetString(MapNameKey);
        var mapSize = _objectTelemetryData.DataArray.Length * sizeof(double);
        var waitTime = WaitTime / 1000f;

        using var memoryMappedFile = MemoryMappedFile.CreateOrOpen(mapName, mapSize);

        while (true)
        {
            using var accessor = memoryMappedFile.CreateViewAccessor();

            var dataArray = new double[6];
            accessor.ReadArray(0, dataArray, 0, dataArray.Length);

            _objectTelemetryData.Angles = new Vector3((float)dataArray[0], (float)dataArray[1], (float)dataArray[2]);
            _objectTelemetryData.Velocity = new Vector3((float)dataArray[3], (float)dataArray[4], (float)dataArray[5]);

            var (tiltX, tiltY) = CalculateChairTilt(
                _objectTelemetryData.Velocity.x,
                _objectTelemetryData.Velocity.y,
                _objectTelemetryData.Velocity.z,
                _objectTelemetryData.Angles.x,
                _objectTelemetryData.Angles.y,
                _objectTelemetryData.Angles.z);

            targetObject.localEulerAngles = new Vector3(tiltX, 0, tiltY);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private static (float xTilt, float zTilt) CalculateChairTilt(
        float speedX, float speedY, float speedZ,
        float rotationX, float rotationY, float rotationZ
    )
    {
        const float maxSpeed = 100f;
        const float maxRotation = 25f;

        var normalizedSpeedX = Mathf.Clamp(speedX / maxSpeed, -1f, 1f);
        var normalizedSpeedZ = Mathf.Clamp(speedZ / maxSpeed, -1f, 1f);

        var normalizedRotationX = Mathf.Clamp(rotationX / maxRotation, -1f, 1f);
        var normalizedRotationZ = Mathf.Clamp(rotationZ / maxRotation, -1f, 1f);

        var xTilt = Mathf.Clamp(normalizedRotationX * 20f + normalizedSpeedX * 10f, -20f, 20f);
        var zTilt = Mathf.Clamp(normalizedRotationZ * 20f + normalizedSpeedZ * 10f, -20f, 20f);

        return (xTilt, zTilt);
    }

    [Serializable]
    public class ObjectTelemetryData
    {
        public Vector3 Angles { get; set; }
        public Vector3 Velocity { get; set; }

        public double[] DataArray => new[]
        {
            (double)
            Angles.x,
            Angles.z,
            Angles.y,
            Velocity.z,
            Velocity.x,
            Velocity.y
        };

        public override string ToString() => $"Angles: {Angles}, Velocity: {Velocity}";
    }
}