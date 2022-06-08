class MyButton extends MovieClip
{
	var mod;
	
	function setModifier(modifier)
	{
		mod = modifier;
		if(mod == 3)
		{
			gotoAndStop(2);
		}
	}
	
	function onRelease()
	{
		if(mod == 0)
		{
			_root.game.gravity *= -1;
			play();
		}
		else if(mod == 1)
		{
			_root.game.collision *= -1;
			play();
		}
		else if(mod == 2)
		{
			_root.game.collision2 *= -1;
			play();
		}
		else if(mod == 3)
		{
			_root.game.attract *= -1;
			play();
		}
		else if(mod == 4)
		{
			_root.game.spawnPlanet();
		}
		else if(mod == 5)
		{
			_root.game.clearPlanets();
		}
		else if(mod == 6)
		{
			_root.game.tracers *= -1;
			play();
		}
		else
		{
			trace("ERROR! SEE MYBUTTON SCRIPT");
		}
	}
}