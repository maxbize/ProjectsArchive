class Tracer extends MovieClip
{
	function onEnterFrame()
	{
		if(_root.pauseMenu._visible == false)
		{
			_alpha -= 1;
			if(_alpha < 10)
			{
				this.removeMovieClip();
			}
		}
	}
}