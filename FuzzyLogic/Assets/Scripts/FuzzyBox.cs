using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using FLS;
using FLS.Rules;
using FLS.MembershipFunctions;
using static UnityEngine.Networking.UnityWebRequest;
using UnityEngine.UI;

public class FuzzyBox : MonoBehaviour {

    //bool selected = false;
    [SerializeField] Text speedText;
    [SerializeField] Text distText;
    [SerializeField] Text rockDistText;
    [SerializeField] GameObject fallowObject;
    [SerializeField] GameObject rockObject;
    [SerializeField] LevelSO level;

	IFuzzyEngine engine;
	LinguisticVariable distance;
	LinguisticVariable rockDistance;
	LinguisticVariable speed;

    public float closeDistance=0;

    public new Rigidbody rigidbody = default;


    void Start()
	{

        //Based on UI
        if (level.closeDist == 0)
        {
            closeDistance = 3.5f;
        }
        else
        {
            closeDistance = (float)level.closeDist;
        }

        // Fuzzy Inference System Setup

        //distance variables
        distance = new LinguisticVariable("distance");

        //for easier debug
        float highValue = 50;
        float midValue = 5;

        //cars distance from the road
        var farLeft = distance.MembershipFunctions.AddTrapezoid("farLeft", -highValue, -highValue, -midValue, -midValue);
        var left = distance.MembershipFunctions.AddTrapezoid("left", -midValue, -midValue, -1, -0.25);
        var none = distance.MembershipFunctions.AddTrapezoid("none", -1, -0.25, 0.25, 1);
        var right = distance.MembershipFunctions.AddTrapezoid("right", 0.25, 1, midValue, midValue);
        var farRight = distance.MembershipFunctions.AddTrapezoid("farRight", midValue, midValue, highValue, highValue);

        rockDistance = new LinguisticVariable("rockDistance");

        //cars distance from the rock
        var rockFarLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockFarLeft", -20, -20, -6, -closeDistance);
        var rockCloseLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseLeft", -6, -closeDistance, -0, -0);

        var rockCloseRight = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseRight", 0, 0, closeDistance, 6);
        var rockFarRight = rockDistance.MembershipFunctions.AddTrapezoid("rockFarRight", closeDistance, 6, 20, 20);


        //output speed variable
        speed = new LinguisticVariable("speed");

        //determines what force is added to the car
        var fastLeft = speed.MembershipFunctions.AddTriangle("fastLeft", -70, -50, -30); //moves to the left quickly
        var mediumLeft = speed.MembershipFunctions.AddTriangle("mediumLeft", -40, -20, -0.01); //moves to the left
        var noSpeed = speed.MembershipFunctions.AddTriangle("noSpeed", 0, 0, 0); //no speed outputs a zero
        var mediumRight = speed.MembershipFunctions.AddTriangle("mediumRight", 0.1, 20, 40); //moves to the right 
        var fastRight = speed.MembershipFunctions.AddTriangle("fastRight", 30, 50, 70);//moves to the right quickly


        //engine
        engine = new FuzzyEngineFactory().Default();


        //USES SPEED AS OUTPUT
        var rule0 = Rule.If(distance.Is(farRight).And(rockDistance.IsNot(rockCloseRight))).Then(speed.Is(fastRight));//move to the right quickly
        var rule1 = Rule.If(distance.Is(right).And(rockDistance.IsNot(rockCloseRight))).Then(speed.Is(mediumRight));//move to the right
       
       
        var rule2 = Rule.If(distance.Is(none).And(rockDistance.IsNot(rockCloseRight))).Then(speed.Is(noSpeed));//doesnt move
        var rule3 = Rule.If(distance.Is(none).And(rockDistance.IsNot(rockCloseLeft))).Then(speed.Is(noSpeed));//doesnt move

        var rule4 = Rule.If(distance.Is(left).And(rockDistance.IsNot(rockCloseLeft))).Then(speed.Is(mediumLeft));//move to the left
        var rule5 = Rule.If(distance.Is(farLeft).And(rockDistance.IsNot(rockCloseLeft))).Then(speed.Is(fastLeft));//move to the left quickly

        //avoids rocks when they are close no matter where the road is
        var rule6 = Rule.If(rockDistance.Is(rockCloseLeft)).Then(speed.Is(mediumRight));//move to the right
        var rule7 = Rule.If(rockDistance.Is(rockCloseRight)).Then(speed.Is(mediumLeft));//move to the left

        //priority avoiding rocks
        var rule8 = Rule.If(distance.Is(farRight).And(rockDistance.Is(rockFarRight))).Then(speed.Is(mediumLeft));//move to the left
        var rule9 = Rule.If(distance.Is(farLeft).And(rockDistance.Is(rockFarLeft))).Then(speed.Is(mediumRight));//move to the right

        ////ignores rocks far away
        //var rule8 = Rule.If(distance.Is(farRight).And(rockDistance.Is(rockFarRight))).Then(speed.Is(fastRight));//move to the right quickly
        //var rule9 = Rule.If(distance.Is(farLeft).And(rockDistance.Is(rockFarLeft))).Then(speed.Is(fastLeft));//move to the left quickly

        ////priority road
        //var rule8 = Rule.If(distance.Is(farRight).And(rockDistance.Is(rockFarRight))).Then(speed.Is(mediumRight));//move to the right
        //var rule9 = Rule.If(distance.Is(farLeft).And(rockDistance.Is(rockFarLeft))).Then(speed.Is(mediumLeft));//move to the left


        ////the left over velocity is too big so the car cannot stop in time with these values
        //var rule8 = Rule.If(distance.Is(farRight).And(rockDistance.Is(rockFarRight))).Then(speed.Is(noSpeed));
        //var rule9 = Rule.If(distance.Is(farLeft).And(rockDistance.Is(rockFarLeft))).Then(speed.Is(noSpeed));


        engine.Rules.Add(rule0, rule1, rule8, rule2, rule3, rule4, rule5, rule9, rule6, rule7);//adding rules to the engine


        rigidbody = GetComponent<Rigidbody>();
        //rockSprite = rockObject.GetComponent<SpriteRenderer>();

    }

    void FixedUpdate()
	{
         if (this.transform.position.x != fallowObject.transform.position.x)
        {
           
            double diffRock = rockObject.transform.position.x - this.transform.position.x;
            double diff = fallowObject.transform.position.x - this.transform.position.x;

            //defuzzifies distance to the road and distance to the rock
            double result = engine.Defuzzify(new { distance = (double)diff, rockDistance = (double)diffRock });

            //adds force according to output (the speed variabe)
            rigidbody.AddForce(new Vector3((float)(result ), 0f, 0f));

            //shows force on screen
            speedText.text = "Force apllied currently: " + ((int)rigidbody.velocity.x).ToString();
            distText.text = "Distance from road:" + ((int)diff).ToString();
            rockDistText.text = "Distance from rock:" + ((int)diffRock).ToString();

            Debug.Log(result);
        }

    }
	
}
