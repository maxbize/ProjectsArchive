class Coin extends MovieClip
{
	var speed;
	var goldGained:Number;
	
	function onLoad()
	{
		speed = 5;
		goldGained = Math.round((Math.random() * 20 * _root.flyGuy.currentLevel)) + 5 * _root.flyGuy.currentLevel;
		_xscale = 50 + goldGained / 5;
		_yscale = 50 + goldGained / 5;
	}
	
	function onEnterFrame()
	{
		_y += speed;
		
		if(this.hitTest(_root.flyGuy.base))
		{
			_root.flyGuy.gold += goldGained;
			_root.userInterface.goldText.text = _root.flyGuy.gold;
			var reward = _root.attachMovie("CoinRewardText","CoinRewardText" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
			reward._x = _x;
			reward._y = _y;
			reward.field.text = "+" + goldGained;
			_root.flyGuy.bulletsList.push(reward);
			this.removeMovieClip();
		}
		
		if(_y > 650)
		{
			this.removeMovieClip();
		}
	}
}