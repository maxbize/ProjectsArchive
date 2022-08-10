package;

import box2D.common.math.B2Vec2;
import box2D.dynamics.B2Body;
import box2D.dynamics.B2FixtureDef;
import flixel.FlxG;
import flixel.FlxSprite;
import flixel.util.FlxColor;
import flixel.math.FlxMath;
import flixel.math.FlxVector;

import box2D.dynamics.*;
import box2D.collision.*;
import box2D.collision.shapes.*;
import box2D.common.math.*;

class StaticB2Rect extends FlxSprite 
{
	private static inline var MOVE_FORCE:Float = 15.0;

	private var table:B2Body;

	public function new(world:B2World, X:Float=0, Y:Float=0, width:Int, height:Int) {
		super(X, Y);

		this.width = width;
		this.height = height;
		makeGraphic(width, height, FlxColor.WHITE);

		// Make body
		table = B2Utils.makeRectBody(world, width, height, X, Y, box2D.dynamics.B2BodyType.STATIC_BODY, 0);
	}
}