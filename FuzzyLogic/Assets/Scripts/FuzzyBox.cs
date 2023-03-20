using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using FLS;
using FLS.Rules;
using FLS.MembershipFunctions;
using static UnityEngine.Networking.UnityWebRequest;

public class FuzzyBox : MonoBehaviour {

	//bool selected = false;

    public GameObject fallowObject;
    public GameObject rockObject;

	IFuzzyEngine engine;
	LinguisticVariable distance;
	LinguisticVariable rockDistance;
	LinguisticVariable direction;
	//LinguisticVariable speed;

    public float highValue = 100f;
    public float midValue = 50f;

    public float highValueD = 100f;
    public float midValueD = 50f;

    public float highValueSpeed = 200f;
    public float lowValueSpeed = 100f;

    public new Rigidbody rigidbody = default;


    void Start()
	{
        // Here we need to setup the Fuzzy Inference System

        //distance variables

        distance = new LinguisticVariable("distance");
        var farRight = distance.MembershipFunctions.AddTrapezoid("farRight", -highValue, -highValue, -midValue, -midValue);

        var right = distance.MembershipFunctions.AddTrapezoid("right", -midValue, -midValue, -1, -0.5);
        var none = distance.MembershipFunctions.AddTrapezoid("none", -1, -0.25, 0.25, 1);
        var left = distance.MembershipFunctions.AddTrapezoid("left", 0.5, 1, midValue, midValue);

        var farLeft = distance.MembershipFunctions.AddTrapezoid("farLeft", midValue, midValue, highValue, highValue);

        rockDistance = new LinguisticVariable("rockDistance");

        var rockFarRight = rockDistance.MembershipFunctions.AddTrapezoid("rockFarRight", -20, -20, -5, -2);
        var rockCloseRight = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseRight", -5, -2, -1, -0.5);
        

        //var rockFar = rockDistance.MembershipFunctions.AddTrapezoid("rockFar", -1, -0.25, 0.25, 1);
        var rockCloseLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockCloseLeft", 0.5, 1, 2, 5);
        var rockFarLeft = rockDistance.MembershipFunctions.AddTrapezoid("rockFarLeft", 2, 5, 20, 20);


        //direction variables

        direction = new LinguisticVariable("direction");

        var d_farRight = direction.MembershipFunctions.AddTrapezoid("farRight", -highValueD, -highValueD, -midValueD, -midValueD);

        var d_right = direction.MembershipFunctions.AddTrapezoid("right", -midValueD, -midValueD, -1, -1);
        var d_none = direction.MembershipFunctions.AddTrapezoid("none", -1, -0.25, 0.25, 1);
        var d_left = direction.MembershipFunctions.AddTrapezoid("left", 1, 1, midValueD, midValueD);

        var d_farLeft = direction.MembershipFunctions.AddTrapezoid("farLeft", midValueD, midValueD, highValueD, highValueD);


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

        var rule0 = Rule.If(distance.Is(farRight).And(rockDistance.IsNot(rockCloseLeft))).Then(direction.Is(d_farLeft));
        var rule1 = Rule.If(distance.Is(right).And(rockDistance.IsNot(rockCloseLeft))).Then(direction.Is(d_left));//move to the left
        var rule2 = Rule.If(distance.Is(left).And(rockDistance.IsNot(rockCloseRight))).Then(direction.Is(d_right));//move to the right
        var rule3 = Rule.If(distance.Is(none)).Then(direction.Is(d_none));//doesnt move
        var rule4 = Rule.If(distance.Is(farLeft).And(rockDistance.IsNot(rockCloseRight))).Then(direction.Is(d_farRight));

        //kinda avoids objects
        var rule5 = Rule.If(rockDistance.Is(rockCloseLeft)).Then(direction.Is(d_farLeft));
        var rule6 = Rule.If(rockDistance.Is(rockCloseRight)).Then(direction.Is(d_farRight));



        //engine.Rules.Add(rule1, rule2, rule3);//adding rules to the engine
        engine.Rules.Add(rule0, rule1, rule2, rule3, rule4, rule5, rule6);//adding rules to the engine


        rigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
	{
        //FindObjectsOfType<RockMovement>();
        if (this.transform.position.x != fallowObject.transform.position.x)
        {
            // Convert position of box to value between 0 and 100
            //double result = 0.0;//doesnt move the box

            double diffRock = this.transform.position.x - rockObject.transform.position.x;
            Debug.Log(diffRock);


            double diff = this.transform.position.x - fallowObject.transform.position.x;
            double result = engine.Defuzzify(new { distance = (double)diff, rockDistance = (double)diffRock });
            //double result = engine.Defuzzify(new { distance = (double)diff, speed = (double)Mathf.Abs(rigidbody.velocity.x) });


            //Debug.Log(result);

            rigidbody.AddForce(new Vector3((float)(result), 0f, 0f));

        }

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
