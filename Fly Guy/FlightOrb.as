class FlightOrb extends MovieClip
{
	var speed;
	var flightTimeAdded;
	var effect;
	
	function onLoad()
	{
		speed = 5;
		effect = 5;
		flightTimeAdded = (_root.flyGuy.currentLevel / 10) - Math.random() / 10;
		_xscale = 180 - 100 * flightTimeAdded;
		_yscale = 180 - 100 * flightTimeAdded;
	}
	
	function onEnterFrame()
	{
		_y += speed;
		
		if(this.hitTest(_root.flyGuy.base))
		{
			_root.flyGuy.flightTimerIncrement *= flightTimeAdded;
			var reward = _root.attachMovie("FlightOrbRewardText","FlightOrbRewardText"+_root.getNextHighestDepth(),_root.getNextHighestDepth());
			reward._x = _x;
			reward._y = _y;
			reward.field.text = "+" + (100 - (Math.round(flightTimeAdded * 100))) + "%";
			this.removeMovieClip();
		}
		
		// Add a fading effect to the power up. Remove if slowing down too much
		_alpha -= effect;
		if(_alpha <= 50 || _alpha >= 100)
		{
			effect *= -1;
		}
		
		if(_y > 650)
		{
			this.removeMovieClip();
		}
	}
}