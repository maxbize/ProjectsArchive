class Background extends MovieClip
{
	var speed;
	
	function onLoad()
	{
		speed = 2;
	}
	
	function onEnterFrame()
	{
		_y += speed;
		if(_y > 2400)
		{
			_y = 0;
		}
	}
}