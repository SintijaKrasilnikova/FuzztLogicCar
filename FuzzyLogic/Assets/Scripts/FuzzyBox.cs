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
    [SerializeField] GameObject fallowObject;
    [SerializeField] GameObject rockObject;
    [SerializeField] SpriteRenderer rockSprite;

	IFuzzyEngine engine;
	LinguisticVariable distance;
	LinguisticVariable rockDistance;
	//LinguisticVariable direction;
	LinguisticVariable speed;

    public float closeDistance = 2f;

    public float maxCarSpeed = 70f;

    public float highValue = 100f;
    public float midValue = 50f;

    public float highValueD = 100f;
    public float midValueD = 50f;

  

    public new Rigidbody rigidbody = default;


    void Start()
	{
        // Here we need to setup the Fuzzy Inference System

        //distance variables

        distance = new LinguisticVariable("distance");
        //var farRight = distance.MembershipFunctions.AddTrapezoid("farRight", -highValue, -highValue, -midValue, -midValue);
        var farLeft = distance.MembershipFunctions.AddTrapezoid("farLeft", -highValue, -highValue, -midValue, -midValue);

        //var right = distance.MembershipFunctions.AddTrapezoid("right", -midValue, -midValue, -1, -0.5);
        var left = distance.MembershipFunctions.AddTrapezoid("left", -midValue, -midValue, -1, -0.25);
        var none = distance.MembershipFunctions.AddTrapezoid("none", -1, -0.25, 0.25, 1);
        //var left = distance.MembershipFunctions.AddTrapezoid("left", 0.5, 1, midValue, midValue);
        var right = distance.MembershipFunctions.AddTrapezoid("right", 0.25, 1, midValue, midValue);

       // var farLeft = distance.MembershipFunctions.AddTrapezoid("farLeft", midValue, midValue, highValue, highValue);
        var farRight = distance.MembershipFunctions.AddTrapezoid("farRight", midValue, midValue, highValue, highValue);

        rockDistance = new LinguisticVariable("rockDistance");

        //var rockFarRight = rockDistance.MembershipFunctions.AddTrapezoid("rockFarRight", -20, -20, -5, -2);
        var rockFarLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockFarLeft", -20, -20, -5, -closeDistance);
        //var rockCloseRight = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseRight", -5, -2, -1, -0.5);
        //var rockCloseRight = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseRight", -5, -2, 0, -0);
        var rockCloseLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseLeft", -5, -closeDistance, -0, -0);
        

        //var rockFar = rockDistance.MembershipFunctions.AddTrapezoid("rockFar", -1, -0.25, 0.25, 1);
        //var rockCloseLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseLeft", 0, 0, 2, 5);
        var rockCloseRight = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseRight", 0, 0, closeDistance, 5);
        //var rockFarLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockFarLeft", 2, 5, 20, 20);
        var rockFarRight = rockDistance.MembershipFunctions.AddTrapezoid("rockFarRight", closeDistance, 5, 20, 20);


        ////direction variables

        //direction = new LinguisticVariable("direction");

        //var d_farRight = direction.MembershipFunctions.AddTrapezoid("farRight", -highValueD, -highValueD, -midValueD, -midValueD);

        //var d_right = direction.MembershipFunctions.AddTrapezoid("right", -midValueD, -midValueD, -1, -1);
        ////var d_none = direction.MembershipFunctions.AddTrapezoid("none", -1, -0.25, 0.25, 1);
        //var d_none = direction.MembershipFunctions.AddTrapezoid("none", -1, -1, 1, 1);
        //var d_left = direction.MembershipFunctions.AddTrapezoid("left", 1, 1, midValueD, midValueD);

        //var d_farLeft = direction.MembershipFunctions.AddTrapezoid("farLeft", midValueD, midValueD, highValueD, highValueD);

        //output speed variable
        speed = new LinguisticVariable("speed");



        //var noSpeed = speed.MembershipFunctions.AddTriangle("noSpeed", 0, 0, 0); //no speed outputs a zero
        //var slow = speed.MembershipFunctions.AddTriangle("slow", 0.01, 0.01, 10);
        //var medium = speed.MembershipFunctions.AddTriangle("slow", 5, 15, 25);
        //var fast = speed.MembershipFunctions.AddTriangle("slow", 20, 30, 40);
        //var veryFast = speed.MembershipFunctions.AddTriangle("slow", 35, 45, 55);

        var fastLeft = speed.MembershipFunctions.AddTriangle("fastLeft", -70, -50, -30);
        var mediumLeft = speed.MembershipFunctions.AddTriangle("mediumLeft", -40, -20, -0.01);
        //var mediumLeft = speed.MembershipFunctions.AddTriangle("mediumLeft", -40, -20, -11);
        //var slowLeft = speed.MembershipFunctions.AddTriangle("slowLeft", -15, -11, -0.01);
        var noSpeed = speed.MembershipFunctions.AddTriangle("noSpeed", 0, 0, 0); //no speed outputs a zero
        //var slowRight = speed.MembershipFunctions.AddTriangle("slowRight", 0.1, 11, 15);
        //var mediumRight = speed.MembershipFunctions.AddTriangle("mediumRight", 11, 20, 40);
        var mediumRight = speed.MembershipFunctions.AddTriangle("mediumRight", 0.1, 20, 40);
        var fastRight = speed.MembershipFunctions.AddTriangle("fastRight", 30, 50, 70);





        //engine
        engine = new FuzzyEngineFactory().Default();

        //moving the box back to the centre
        //var rule0 = Rule.If(distance.Is(farRight)).Then(direction.Is(d_farLeft));
        //var rule1 = Rule.If(distance.Is(right)).Then(direction.Is(d_left));//move to the left
        //var rule2 = Rule.If(distance.Is(left)).Then(direction.Is(d_right));//move to the right
        //var rule3 = Rule.If(distance.Is(none)).Then(direction.Is(d_none));//doesnt move
        //var rule4 = Rule.If(distance.Is(farLeft)).Then(direction.Is(d_farRight));

        ////kinda avoids objects
        //var rule5= Rule.If(rockDistance.Is(rockCloseLeft)).Then(direction.Is(d_farLeft));
        //var rule6= Rule.If(rockDistance.Is(rockCloseRight)).Then(direction.Is(d_farRight));

        ////runs into the objects
        //var rule5 = Rule.If(rockDistance.Is(rockCloseLeft)).Then(direction.Is(d_farRight));
        //var rule6 = Rule.If(rockDistance.Is(rockCloseRight)).Then(direction.Is(d_farLeft));

        ////USING DIRECTION AS OUTPUT
        //var rule0 = Rule.If(distance.Is(farRight).And(rockDistance.IsNot(rockCloseLeft))).Then(direction.Is(d_farLeft));
        //var rule1 = Rule.If(distance.Is(right).And(rockDistance.IsNot(rockCloseLeft))).Then(direction.Is(d_left));//move to the left
        //var rule2 = Rule.If(distance.Is(left).And(rockDistance.IsNot(rockCloseRight))).Then(direction.Is(d_right));//move to the right
        //var rule3 = Rule.If(distance.Is(none).And(rockDistance.IsNot(rockCloseRight))).Then(direction.Is(d_none));//doesnt move
        //var rule31 = Rule.If(distance.Is(none).And(rockDistance.IsNot(rockCloseLeft))).Then(direction.Is(d_none));//doesnt move
        //var rule4 = Rule.If(distance.Is(farLeft).And(rockDistance.IsNot(rockCloseRight))).Then(direction.Is(d_farRight));

        ////kinda avoids objects
        ////var rule5 = Rule.If(rockDistance.Is(rockCloseLeft)).Then(direction.Is(d_farLeft));
        ////var rule6 = Rule.If(rockDistance.Is(rockCloseRight)).Then(direction.Is(d_farRight));

        //var rule5 = Rule.If(rockDistance.Is(rockCloseLeft)).Then(direction.Is(d_farRight));
        //var rule6 = Rule.If(rockDistance.Is(rockCloseRight)).Then(direction.Is(d_farLeft));


        ////engine.Rules.Add(rule1, rule2, rule3);//adding rules to the engine
        //engine.Rules.Add(rule0, rule1, rule2, rule3, rule31, rule4, rule5, rule6);//adding rules to the engine

        //USES SPEED AS OUTPUT
        var rule0 = Rule.If(distance.Is(farRight).And(rockDistance.IsNot(rockCloseRight))).Then(speed.Is(fastRight));
        var rule1 = Rule.If(distance.Is(right).And(rockDistance.IsNot(rockCloseRight))).Then(speed.Is(mediumRight));//move to the left
       
       
        var rule2 = Rule.If(distance.Is(none).And(rockDistance.IsNot(rockCloseRight))).Then(speed.Is(noSpeed));//doesnt move
        var rule3 = Rule.If(distance.Is(none).And(rockDistance.IsNot(rockCloseLeft))).Then(speed.Is(noSpeed));//doesnt move

        var rule4 = Rule.If(distance.Is(left).And(rockDistance.IsNot(rockCloseLeft))).Then(speed.Is(mediumLeft));//move to the right
        var rule5 = Rule.If(distance.Is(farLeft).And(rockDistance.IsNot(rockCloseLeft))).Then(speed.Is(fastLeft));

        //kinda avoids objects
        //var rule5 = Rule.If(rockDistance.Is(rockCloseLeft)).Then(direction.Is(d_farLeft));
        //var rule6 = Rule.If(rockDistance.Is(rockCloseRight)).Then(direction.Is(d_farRight));

        var rule6 = Rule.If(rockDistance.Is(rockCloseLeft)).Then(speed.Is(mediumRight));
        var rule7 = Rule.If(rockDistance.Is(rockCloseRight)).Then(speed.Is(mediumLeft));


        //var rule8 = Rule.If(distance.Is(farRight).And(rockDistance.Is(rockFarRight))).Then(speed.Is(mediumRight));
        //var rule9 = Rule.If(distance.Is(farLeft).And(rockDistance.Is(rockFarLeft))).Then(speed.Is(mediumLeft));

        var rule8 = Rule.If(distance.Is(farRight).And(rockDistance.Is(rockFarRight))).Then(speed.Is(mediumLeft));
        var rule9 = Rule.If(distance.Is(farLeft).And(rockDistance.Is(rockFarLeft))).Then(speed.Is(mediumRight));


        //var rule10 = Rule.If(distance.Is(right).And(rockDistance.Is(rockCloseRight))).Then(speed.Is(mediumLeft));
        //var rule11 = Rule.If(distance.Is(left).And(rockDistance.Is(rockCloseLeft))).Then(speed.Is(mediumRight));

        //engine.Rules.Add(rule1, rule2, rule3);//adding rules to the engine
        engine.Rules.Add(rule0, rule1, rule8, rule2, rule3, rule4, rule5, rule9, rule6, rule7);//adding rules to the engine
        //engine.Rules.Add(rule0, rule1, rule2, rule3, rule4, rule5, rule6, rule7);//adding rules to the engine


        rigidbody = GetComponent<Rigidbody>();
        rockSprite = rockObject.GetComponent<SpriteRenderer>();

    }

    void FixedUpdate()
	{
        //FindObjectsOfType<RockMovement>();
        // if (this.transform.position.x != fallowObject.transform.position.x)
        //if (this.transform.position.x + 2 > fallowObject.transform.position.x || this.transform.position.x - 2 < fallowObject.transform.position.x)
        if (this.transform.position.x != fallowObject.transform.position.x)
        {
            // Convert position of box to value between 0 and 100
            //double result = 0.0;//doesnt move the box

            //double diffRock = this.transform.position.x - rockObject.transform.position.x;
            double diffRock = rockObject.transform.position.x - this.transform.position.x;
            //double diffRock = 0;

            //if (rockObject.transform.position.x < this.transform.position.x)//means rock is to the left
            //{
            //    diffRock = rockObject.transform.position.x + (rockSprite.bounds.size.x / 2) - this.transform.position.x;
            //}
            //else//rock in on the right side
            //{
            //    diffRock = rockObject.transform.position.x - (rockSprite.bounds.size.x / 2) - this.transform.position.x;
            //}
            //Debug.Log(diffRock);


            //double diff = this.transform.position.x - fallowObject.transform.position.x;
            double diff = fallowObject.transform.position.x - this.transform.position.x;
            double result = engine.Defuzzify(new { distance = (double)diff, rockDistance = (double)diffRock });

            //double result = engine.Defuzzify(new { distance = (double)diff, speed = (double)Mathf.Abs(rigidbody.velocity.x) });


            Debug.Log(result);

            rigidbody.AddForce(new Vector3((float)(result ), 0f, 0f));
            speedText.text = "Force apllied currently: " + ((int)rigidbody.velocity.x).ToString();

        }

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
