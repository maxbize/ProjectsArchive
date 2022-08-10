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
class PracticeRallyPlayState extends BasePlayState
{
	/**
	 * Function that is called up when to state is created to set it up. 
	 */
	override public function create():Void
	{
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
	override public function update():Void
	{
		super.update();
	}	
}