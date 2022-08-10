package;

import box2D.collision.*;
import box2D.collision.shapes.*;
import box2D.collision.shapes.B2CircleShape;
import box2D.collision.shapes.B2PolygonShape;
import box2D.common.math.*;
import box2D.common.math.B2Vec2;
import box2D.dynamics.*;
import box2D.dynamics.B2BodyDef;
import box2D.dynamics.B2BodyType;
import box2D.dynamics.B2FixtureDef;
import box2D.dynamics.B2World;

class B2Utils
{
	public static inline var RATIO:Float = 30;

	public static function makeRectBody(world:B2World, width:Float, height:Float,
			X:Float, Y:Float, type:B2BodyType, restitution:Float):box2D.dynamics.B2Body {
		var boxShape:B2PolygonShape = new B2PolygonShape();
		boxShape.setAsBox(width/2 / RATIO, height/2 / RATIO);

		var fixDef:B2FixtureDef = new B2FixtureDef();
		fixDef.density = 0.7;
		fixDef.restitution = restitution;
		fixDef.friction = 0.8;
		fixDef.shape = boxShape;

		var bodyDef:B2BodyDef = new B2BodyDef();
		bodyDef.position.set((X + width/2) / RATIO, (Y + height/2) / RATIO);
		bodyDef.type = type;

		var obj:B2Body = world.createBody(bodyDef);
		obj.createFixture(fixDef);

		return obj;
	}

	public static function makeCircularBody(world:B2World, radius:Float,
			X:Float, Y:Float, type:B2BodyType, restitution:Float):box2D.dynamics.B2Body {
		var circleShape:B2CircleShape = new B2CircleShape();
		circleShape.setRadius(radius / RATIO);

		var fixDef:B2FixtureDef = new B2FixtureDef();
		fixDef.density = 0.7;
		fixDef.restitution = restitution;
		fixDef.friction = 0.8;
		fixDef.shape = circleShape;

		var bodyDef:B2BodyDef = new B2BodyDef();
		bodyDef.position.set((X + radius) / RATIO, (Y + radius) / RATIO);
		bodyDef.type = type;

		var obj:B2Body = world.createBody(bodyDef);
		obj.createFixture(fixDef);

		return obj;
	}

}