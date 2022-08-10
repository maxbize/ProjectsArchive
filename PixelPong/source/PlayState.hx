package;

import box2D.common.math.B2Vec2;
import flixel.FlxG;
import flixel.FlxSprite;
import flixel.FlxState;
import flixel.text.FlxText;
import flixel.ui.FlxButton;
import flixel.math.FlxMath;

import box2D.dynamics.*;
import box2D.dynamics.*;
import box2D.collision.*;
import box2D.collision.shapes.*;
import box2D.common.math.*;

/**
 * A FlxState which can be used for the actual gameplay.
 */
class PlayState extends FlxState
{
	public var world:B2World;

	private var timeSincePhysicsStep:Float = 0;

	private static inline var PHYSICS_STEP:Float = 1.0/50.0;
	private static inline var VELOCITY_ITERATIONS:Int = 10;
	private static inline var POSITION_ITERATIONS:Int = 10;

	/**
	 * Function that is called up when to state is created to set it up. 
	 */
	public override function create():Void
	{
		world = new B2World(new B2Vec2(0, 0), true);

		// Set up the table
		var tableWidth = 600;
		var tableHeight = 30;
		var tableX = (FlxG.width - tableWidth) / 2;
		var tableY = FlxG.height - 100;
		add(new StaticB2Rect(world, tableX, tableY, tableWidth, tableHeight));
		
		// Set up the table
		var netWidth = 15;
		var netHeight = 50;
		var netX = tableX + (tableWidth - netWidth) / 2;
		var netY = tableY - netHeight;
		add(new StaticB2Rect(world, netX, netY, netWidth, netHeight));

		add(new Ball(world, FlxG.width / 2 - 10, FlxG.height / 2, 10, Ball.SpawnType.FEED));
		add(new Player(world, 20, 20, 16, 64, new B2Vec2(netX + netWidth / 2, netY - 50)));

		super.create();
	}
	
	/**
	 * Function that is called when this state is destroyed - you might want to 
	 * consider setting all objects this state uses to null to help garbage collection.
	 */
	override public function destroy():Void
	{
		super.destroy();
	}

	/**
	 * Function that is called once every frame.
	 */
	override public function update(elapsed:Float):Void
	{
		timeSincePhysicsStep += FlxG.elapsed;
		while (timeSincePhysicsStep > PHYSICS_STEP) {
			timeSincePhysicsStep -= FlxG.elapsed;
			world.step(PHYSICS_STEP, VELOCITY_ITERATIONS, POSITION_ITERATIONS);
		}
		world.clearForces();
		super.update(elapsed);
	}	
}