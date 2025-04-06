using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
}
