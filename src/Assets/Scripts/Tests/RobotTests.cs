using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class RobotTests
{
    [Test]
    public void AddingSensor()
    {
        GameObject sensor = new GameObject();
        sensor.AddComponent<ContactSensorManager>();
        sensor.GetComponent<ContactSensorManager>().sensorName = "SensorTest";
        GameObject robotManager = new GameObject();
        robotManager.AddComponent<RobotManager>();

        robotManager.GetComponent<RobotManager>().AddSensor(sensor, "front_snap");
        Assert.AreEqual(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest"), true);
    }

    [Test]
    public void AddingMultipleSensors()
    {
        GameObject sensor = new GameObject();
        sensor.AddComponent<ContactSensorManager>();
        sensor.GetComponent<ContactSensorManager>().sensorName = "SensorTest";
        GameObject sensor02 = new GameObject();
        sensor02.AddComponent<ContactSensorManager>();
        sensor02.GetComponent<ContactSensorManager>().sensorName = "SensorTest02";

        GameObject robotManager = new GameObject();
        robotManager.AddComponent<RobotManager>();

        robotManager.GetComponent<RobotManager>().AddSensor(sensor, "front_snap");
        robotManager.GetComponent<RobotManager>().AddSensor(sensor02, "back_snap");
        Assert.AreEqual(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest"), true);
        Assert.AreEqual(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest02"), true);
    }

    [Test]
    public void RemoveSensor()
    {
        GameObject sensor = new GameObject();
        sensor.AddComponent<ContactSensorManager>();
        sensor.GetComponent<ContactSensorManager>().sensorName = "SensorTest";

        GameObject robotManager = new GameObject();
        robotManager.AddComponent<RobotManager>();

        robotManager.GetComponent<RobotManager>().AddSensor(sensor, "front_snap");
        Assert.AreEqual(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest"), true);

        robotManager.GetComponent<RobotManager>().RemoveSensor("front_snap");
        Assert.AreEqual(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest"), false);
    }

    [Test]
    public void ClearSensors()
    {
        GameObject sensor = new GameObject();
        sensor.AddComponent<ContactSensorManager>();
        sensor.GetComponent<ContactSensorManager>().sensorName = "SensorTest";
        GameObject sensor02 = new GameObject();
        sensor02.AddComponent<ContactSensorManager>();
        sensor02.GetComponent<ContactSensorManager>().sensorName = "SensorTest02";

        GameObject robotManager = new GameObject();
        robotManager.AddComponent<RobotManager>();

        robotManager.GetComponent<RobotManager>().AddSensor(sensor, "front_snap");
        robotManager.GetComponent<RobotManager>().AddSensor(sensor02, "back_snap");
        Assert.True(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest"));
        Assert.True(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest02"));

        robotManager.GetComponent<RobotManager>().ClearAllSensors();
        Assert.False(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest"));
        Assert.False(robotManager.GetComponent<RobotManager>().HasSensor("SensorTest02"));
    }
}
