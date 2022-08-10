package;

import box2D.collision.*;
import box2D.collision.shapes.*;
import box2D.common.math.*;
import box2D.common.math.B2Vec2;
import box2D.dynamics.*;
import box2D.dynamics.B2Body;
import box2D.dynamics.B2BodyType;
import box2D.dynamics.B2World;
import flixel.FlxG;
import flixel.FlxSprite;
import flixel.util.FlxColor;
import flixel.math.FlxMath;

class Player extends FlxSprite 
{
	private static inline var MOVE_FORCE:Float = 230.0;

	private var bat:B2Body;
	private var focus:B2Vec2; // The point the bat aims towards

	public function new(world:B2World, X:Float=0, Y:Float=0, width:Int, height:Int, focus:B2Vec2) {
		super(X, Y);

		this.focus = focus;

		this.width = width;
		this.height = height;
		makeGraphic(width, height, FlxColor.BLUE);

		// Make body
		bat = B2Utils.makeRectBody(world, width, height, X, Y, B2BodyType.DYNAMIC_BODY, 0);
		bat.setLinearDamping(15);
	}

	override public function update(elapsed:Float):Void {
		applyInput(getInput());
		faceFocus();
		syncBox2D();
		super.update(elapsed);
	}

	private function getInput():B2Vec2 {
		var dir:box2D.common.math.B2Vec2 = new B2Vec2();
		
		if (FlxG.keys.anyPressed(["UP", "W"])) {
			dir.add(new B2Vec2(0, -1));
		}
		if (FlxG.keys.anyPressed(["DOWN", "S"])) {
			dir.add(new B2Vec2(0, 1));
		}
		if (FlxG.keys.anyPressed(["RIGHT", "D"])) {
			dir.add(new B2Vec2(1, 0));
		}
		if (FlxG.keys.anyPressed(["LEFT", "A"])) {
			dir.add(new B2Vec2(-1, 0));
		}

		return dir;
	}

	private function applyInput(input:B2Vec2):Void {
		var friction:B2Vec2 = new B2Vec2();
		
		if (FlxMath.signOf(input.x) != FlxMath.signOf(bat.getLinearVelocity().x)) {
			friction.x = bat.getLinearVelocity().x;
		}
		if (FlxMath.signOf(input.y) != FlxMath.signOf(bat.getLinearVelocity().y)) {
			friction.y = bat.getLinearVelocity().y;
		}
		if (input.length() == 0) {
			bat.setLinearVelocity(new B2Vec2(0,0));
		}

		friction.multiply(-0.1);

		input.normalize();
		input.multiply(MOVE_FORCE);

		input.add(friction);

		bat.applyForce(input, bat.getWorldCenter());
	}

	private function faceFocus():Void {
		FlxG.log.notice(focus);
		var focusCopy:B2Vec2 = focus.copy();
		var batCopy:B2Vec2 = bat.getPosition().copy();
		batCopy.multiply(B2Utils.RATIO);
		focusCopy.subtract(batCopy);
		bat.setAngle(Math.atan2(focusCopy.y, focusCopy.x));
	}

	// Sync our position with the one in the Box2D world
	private function syncBox2D():Void {
		x = FlxMath.roundDecimal((bat.getPosition().x * B2Utils.RATIO) - width/2, 0);
		y = FlxMath.roundDecimal((bat.getPosition().y * B2Utils.RATIO) - height/2, 0);
		angle = bat.getAngle() * (180 / Math.PI);
	}
}