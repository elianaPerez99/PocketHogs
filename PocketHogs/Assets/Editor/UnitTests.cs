using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework; //unit testing framework
using UnityEngine.SceneManagement;

public class UnitTests{

    [Test]
    public void TestTextField()
    {
        //ARRANGE
        GameObject obj = new GameObject();
        obj.AddComponent<Client>();
        string correctOutput = "Please enter an IP";
        //ACT
        string testOutput = obj.GetComponent<Client>().TextFieldTest();
        //ASSERT
        Assert.That(correctOutput.Equals(testOutput));
        
    }

    [Test]
    public void TestClientConnectionWithCorrectIP()
    {
        //ARRANGE
        GameObject obj = new GameObject();
        obj.AddComponent<Client>();
        string correctOutput = "Ok";
        //ACT
        string testOutput = obj.GetComponent<Client>().ConnectTest("127.0.0.1");
        //ASSERT
        Assert.That(correctOutput.Equals(testOutput));
    }

    [Test]
    public void TestChangedScene()
    {
        //ARRANGE
        GameObject obj = new GameObject();
        obj.AddComponent<Client>();
        string correctOutput = "Map1";
        //ACT
        obj.GetComponent<Client>().ConnectTest("127.0.0.1");
        //i plan on adding a part that changes the scene if we connect correctly
        string testOutput = SceneManager.GetActiveScene().name;
        //ASSERT
        Assert.That(correctOutput.Equals(testOutput));
    }
}
