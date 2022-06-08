class bullet extends MovieClip
{
	var dx:Number;
	var dy:Number;
	var mspeed:Number;
	var xTarget:Number;
	var yTarget:Number;
	var maxSize:Number;
	var growth:Number;
	var growing:Boolean;
	var aimY:Number;
	var aimX:Number;

	function onLoad()
	{
		_rotation = _root.player._rotation;
		maxSize = 300; // % of normal size
		//dx = 0;
		//dy = 0;
		xTarget = _root._xmouse;
		yTarget = _root._ymouse;
		growth = mspeed / Math.sqrt((xTarget - _x) * (xTarget - _x) + (yTarget - _y) * (yTarget - _y)) * (maxSize - 100);
		growing = true;
		aimX = Math.random() * _root.player.accuracy;
		aimY = Math.random() * _root.player.accuracy;
		if(Math.random() < 0.5)
		{
			aimX *= -1;
		}
		if(Math.random() < 0.5)
		{
			aimY *= -1;
		}
	}
	
	function onEnterFrame()
	{
		changeSize();
		_x += dx + aimX;
		_y += dy + aimY;
	}
	
	function changeSize()
	{
		if(growing)
		{
			_xscale += growth;
			_yscale += growth;
			
			if(_xscale > maxSize)
			{
				_xscale = maxSize;
				_yscale = maxSize;
				growing = false;
				growth *= 6;
			}
		}
		else
		{
			_xscale -= growth;
			_yscale -= growth;
		}
	}
	
	function normalizeSpeed()
	{
		var hyp:Number = Math.sqrt(dx * dx + dy * dy);
		dx = dx / hyp * mspeed;
		dy = dy / hyp * mspeed;
	}
}