class VelArrow extends MovieClip
{
	var dragging:Boolean;
	
	function onLoad()
	{
		dragging = false;
	}
	
	function onEnterFrame()
	{
		if(dragging)
		{
			var dx = _root._xmouse - _x;
			var dy = _root._ymouse - _y;
			var distance = Math.sqrt(dx * dx + dy * dy)
			_rotation = Math.atan2(dy,dx) * 180 / 3.14;
			_xscale = distance * 2;
		}
		else if(_xscale > 300)
		{
			_xscale = 300;
		}
	}
	
	function onPress()
	{
		dragging = true;
	}
	
	function onRelease()
	{
		dragging = false;
	}
}