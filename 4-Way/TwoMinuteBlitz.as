class TwoMinuteBlitz extends MovieClip
{
	var time;

	function onLoad()
	{
		time = 34 * 120;
		for (var i = 1; i <= 100; i++)
		{
			forceSpawn();
		}
	}

	function onEnterFrame()
	{
		time--;
		//_root.blitzTimer.text = 
	}

	function forceSpawn()
	{
		var ball = _root.attachMovie("Ball", "Ball" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
		ball._x = Math.random() * 200 + 175;
		ball._y = Math.random() * 200 + 175;
		_root.game.ballsList.push(ball);
		_root.game.ballsOnScreen += 1;
		_root.soundFX.attachSound("ballSpawn.wav");
		_root.soundFX.start();
	}
}