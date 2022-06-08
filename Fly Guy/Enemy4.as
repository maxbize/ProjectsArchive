//Pink Guy. Healer, stays in back
class Enemy4 extends Enemy
{
	var healTimer;
	var enemyHealed;
	
	function onLoad()
	{
		this.spawn();
		healTimer = 0;
		health = 50 + _root.flyGuy.currentLevel * 30;
	}
	
	function onEnterFrame()
	{
		healTimer++;
		this.logic();
		
		
		if(healTimer > 45)
		{
			var anim;
			healTimer = 0;
			for(var i = _root.flyGuy.enemiesList.length - 20; i < _root.flyGuy.enemiesList.length; i++) // Hit test the last twenty enemies
			{
				_root.flyGuy.enemiesList[i].health += _root.flyGuy.currentLevel * 5;
				anim = _root.attachMovie("HealAnimation","HealAnimation" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				anim._x = _root.flyGuy.enemiesList[i]._x;
				anim._y = _root.flyGuy.enemiesList[i]._y;
				_root.flyGuy.bulletsList.push(anim);
			}
		}
	}
}