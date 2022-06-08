class CoinRewardText extends MovieClip
{
	function onEnterFrame()
	{
		_alpha -= 5;
		if(_alpha < 1)
		{
			this.removeMovieClip();
		}
	}
} 