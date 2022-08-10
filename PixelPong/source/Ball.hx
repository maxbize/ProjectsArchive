package;

import box2D.collision.*;
import box2D.collision.shapes.*;
import box2D.common.math.*;
import box2D.common.math.B2Vec2;
import box2D.dynamics.*;
import flixel.FlxG;
import flixel.FlxSprite;
import flixel.util.FlxColor;
import flixel.math.FlxMath;
import flixel.math.FlxRandom;

enum SpawnType {
	CENTER;
	FEED;
	SERVE;
}

class Ball extends FlxSprite
{
	
	private var ball:box2D.dynamics.B2Body;
	private var spawnType:SpawnType;
	private var rand:FlxRandom;

	public function new(world:B2World, X:Float=0, Y:Float=0, radius:Int, spawnType:SpawnType) {
		super(X, Y);

		this.width = radius;
		this.height = radius;

		this.spawnType = spawnType;

		makeGraphic(radius * 2, radius * 2, FlxColor.RED);

		//FlxSpriteUtil.drawCircle(this, X, Y, radius, FlxColor.RED);

		// Make body
		ball = B2Utils.makeCircularBody(world, radius, X, Y, box2D.dynamics.B2BodyType.DYNAMIC_BODY, 1);
		ball.setLinearDamping(0.3);

		this.rand = new FlxRandom();

		respawn();
	}

	override public function update(elapsed:Float):Void {
		checkRespawn();
		applyInput();
		syncBox2D();
		super.update(elapsed);
	}

	private function checkRespawn():Void {
		if (y > FlxG.height * 1.2) {
			respawn();
		}
	}

	private function respawn():Void {
		switch (spawnType) {
			case Ball.SpawnType.CENTER:
				ball.setPosition(new B2Vec2(FlxG.width / 2 / B2Utils.RATIO, FlxG.height / 2 / B2Utils.RATIO));
				ball.setLinearVelocity(new B2Vec2(0,0));
				ball.setAngularVelocity(0);
			case Ball.SpawnType.FEED:
				ball.setPosition(new B2Vec2((FlxG.width - 200) / B2Utils.RATIO, FlxG.height / 2 / B2Utils.RATIO));
				ball.setLinearVelocity(new B2Vec2(rand.float(-15, -13), rand.float(-5, -3)));
				ball.setAngularVelocity(0);
			default: 

		}
	}

	private function applyInput():Void {
		var input = new B2Vec2(0, 9.8/4);
		ball.applyForce(input, ball.getWorldCenter());
	}

	// Sync our position with the one in the Box2D world
	private function syncBox2D():Void {
		x = FlxMath.roundDecimal((ball.getPosition().x * B2Utils.RATIO) - width/2, 0);
		y = FlxMath.roundDecimal((ball.getPosition().y * B2Utils.RATIO) - height/2, 0);
		angle = ball.getAngle() * (180 / Math.PI);
	}
}