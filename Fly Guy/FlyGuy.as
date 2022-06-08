class FlyGuy extends MovieClip
{
	//Vars that affect the player's attributes
	var speed:Number;
	var currentWeapon;
	var MAXHEALTH;
	var health;
	var cape;
	var gold:Number;
	var shootSpeed;
	var Damage;
	var bulletSpeed;
	var experience;
	var capesPurchased;
	var boots;
	var luck;
	
	//Enemy vars
	var enemiesList;
	var enemySpawnTimer;
	var splitterBlacklist;
	
	//Level vars
	var shootTimer;
	var levelTimerTotal;
	var levelTimerIncrement;
	var flightTimerTotal;
	var flightTimerIncrement;
	var flightTimerIncrementIncrement;
	var flightTimerIncrementBase;
	var flightTimerMAX;
	var currentLevel;
	var playingSurvival;
	var siftTimer;
	var nextDepth;
	var levelKills;
	var levelXP;
	var levelMoney;
	var spawning;
	var odd;

	var highestLevelCompleted;
	var pickupsList;
	var bulletsList;

	// Vars for Achievements and stats API
	var kills;
	var achievements;
	
	var savefile;
	
	function onLoad()
	{
		_root.kongregateServices.connect();
		_root.backgroundMusic = new Sound();
		_root.oceanBackground._visible = false;
		_root.flyGuy.base._alpha = 0;
		newGameStats();
		_root.introMenu._visible = true;
		hideAllMenus();
		_root.introMenu.newGame.onPress = function()
		{
			_root.introMenu._visible = false;
			_root.mainMenu._visible = true;
		}
		var savefile = SharedObject.getLocal("Fly Guy");
	}
	
	function onEnterFrame()
	{		
		if(_root.hideGame._visible == false)
		{
			//siftArrays();
			_root.userInterface.swapDepths(_root.getNextHighestDepth());
			
			_root.testPickups.text = pickupsList.length;
			_root.testEnemies.text = enemiesList.length;
			_root.testBullets.text = bulletsList.length;
			
			shootTimer++;
			enemySpawnTimer++;
			levelTimerIncrement++;
			flightTimerIncrement += flightTimerIncrementIncrement;
			
			//4-directional movement
			if(Key.isDown(Key.UP)){_y -= speed; flightTimerIncrement += flightTimerIncrementIncrement / 2; if(_y < 100) {_y = 100}}
			if(Key.isDown(Key.RIGHT)){_x += speed; flightTimerIncrement += flightTimerIncrementIncrement / 2; if(_x > 470) {_x = 470}}
			if(Key.isDown(Key.DOWN)){_y += speed; flightTimerIncrement += flightTimerIncrementIncrement / 2; if(_y > 470) {_y = 470}}
			if(Key.isDown(Key.LEFT)){_x -= speed; flightTimerIncrement += flightTimerIncrementIncrement / 2; if(_x < 30) {_x = 30}}
			
			if( shootTimer > shootSpeed && Key.isDown(Key.SPACE))
			{
				shootTimer = 0;
				shoot(currentWeapon);
			}
			
			if(enemySpawnTimer > 30)
			{
				spawnEnemy();
			}
			
			if(levelTimerIncrement > levelTimerTotal)
			{
				if(playingSurvival)
				{
					currentLevel++;
					levelTimerIncrement = currentLevel * 100;
				}
				else
				{
					levelComplete(true);
				}
			}
			
			_root.userInterface.flightBar._xscale = (flightTimerTotal - flightTimerIncrement) / flightTimerTotal * 100;
			if (flightTimerIncrement >= flightTimerTotal)
			{
				levelComplete(false);
				_root.userInterface.flightBar._xscale = 0;
			}
			
			_root.userInterface.healthBar._xscale = (health) / MAXHEALTH * 100;
			if (health <= 0)
			{
				levelComplete(false);
				_root.userInterface.healthBar._xscale = 0;
			}
			
			if(0 > flightTimerIncrement)
			{
				flightTimerIncrement = 0;
			}
			
			bootsEffect();
		}
	}
	
	function shoot(Weapon)
	{
		var bullet;
		//Check to see which bullet we're firing
		switch(Weapon)
		{
			//Regular red Bullet
			case 'normal':
				bullet = _root.attachMovie("BulletNormal","BulletNormal" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				bullet._x = _x + 25;
				bullet._y = _y - 100;
				bulletsList.push(bullet);
				break;
			case 'splitter':
				bullet = _root.attachMovie("BulletSplitter","BulletSplitter" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				bullet._x = _x + 25;
				bullet._y = _y - 100;
				bulletsList.push(bullet);
				if(capesPurchased[11][2] == 1)
				{
					bullet.splitAmount = 0;
				}
				else
				{
					bullet.splitAmount = 1;
				}
				break;
			case 'homing':
				bullet = _root.attachMovie("BulletHoming","BulletHoming" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				bullet._x = _x + 25;
				bullet._y = _y - 100;
				bulletsList.push(bullet);
				break;
			case 'regen':
				bullet = _root.attachMovie("BulletRegen","BulletRegen" + _root.getNextHighestDepth(), _root.getNextHighestDepth());
				bullet._x = _x + 25;
				bullet._y = _y - 100;
				bulletsList.push(bullet);
				break;
		}
		/*
		if(odd == -1)
		{
			bullet._x -= 50;
		}
		odd *= -1;
		*/
	}
	
	function newGameStats()
	{
		speed = 5;
		currentWeapon = 'normal'; // 'normal'
		shootTimer = 0;
		enemySpawnTimer = 0;
		MAXHEALTH = 3;
		cape = 0; // 0
		gold = 5000000;
		shootSpeed = 10; // 20
		kills = 0;
		flightTimerMAX = 3000;
		bulletSpeed = 6;
		Damage = 100;
		flightTimerIncrementBase = 10; // 10
		experience = 200000000; // 0
		luck = 5;
		achievements = [];
		highestLevelCompleted = 0;
		capesPurchased = new Array(0,1,2,3,4,5,6,7,8,9,10,11,12,13);
			for(var i in capesPurchased)
			{
				capesPurchased[i] = [0,0,0];
			}
			// First array is for capes 0-6 Second array is for stats [0] purchased?, [1] EXP?, [2] lvl?
			// 7,8,9 are boots. 7 == regen, 8 == magnet, 9 == lucky
			// 10,11,12 are gloves. 10 == regen, 11 == splitter, 12 == homing
			// 0 == random object. 13 == wig from shop. Need these to track purchase
	}
	
	function spawnEnemy()
	{
		enemySpawnTimer = currentLevel * 1.5 + 10 //0;
		if(spawning){
		var enemy;
		//var enemyType = currentLevel;
		var enemyType = Math.round(Math.random()*currentLevel);
		switch(enemyType)
		{
			case 0:
				break;
			case 1:
				enemy = _root.attachMovie("Enemy2", "Enemy2" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
			case 2:
				enemy = _root.attachMovie("Enemy1", "Enemy1" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
			case 3:
				enemy = _root.attachMovie("Enemy3", "Enemy3" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
			case 4:
				enemy = _root.attachMovie("Enemy4", "Enemy4" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
			case 5:
				enemy = _root.attachMovie("Enemy5", "Enemy5" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
			case 6:
				enemy = _root.attachMovie("Enemy6", "Enemy6" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
			case 7:
				enemy = _root.attachMovie("Enemy7", "Enemy7" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
			case 8:
				enemy = _root.attachMovie("Enemy8", "Enemy8" + _root.getNextHighestDepth(), _root.getNextHighestDepth())
				break;
		}
		
		if(enemyType != 0)
		{
			enemiesList.push(enemy);
			enemy.blacklistInstance = splitterBlacklist;
			splitterBlacklist++;
			nextDepth++;
		}}
	}
	
	function nextLevel(level)
	{
		_root.levelsMenu._visible = false;
		_root.hideGame._visible = false;
		levelTimerTotal = level * 300 + 2400;
		levelTimerIncrement = 2400;
		flightTimerIncrement = 0;
		flightTimerTotal = getFlightTimerMAX();
		updateCharacterDisplay();
		_root.introMenu._visible = false;
		_root.shopMenu._visible = false;
		currentLevel = level;
		pickupsList = [];
		bulletsList = [];
		enemiesList = [];
		siftTimer = 0;
		_root.userInterface.goldText.text = gold;
		_root.userInterface.xpText.text = experience;
		health = MAXHEALTH;
		nextDepth = 100;
		_root.userInterface.swapDepths(999999);
		_root.userInterface._visible = true;
		splitterBlacklist = 0;
		getLuck();
		levelXP = experience;
		levelKills = kills;
		levelMoney = gold;
		chooseMusic(level);
		_root.levelComplete._visible = false;
		spawning = true;
		_root.levelComplete.underlineAnim.gotoAndStop(1);
		chooseBackground(level);
		odd = 1;
		flightTimerIncrementIncrement = getFlightTimerIncrement();
		if(level == 11)
		{
			currentLevel = 1;
			levelTimerTotal = currentLevel * 1800;
			playingSurvival = true;
		}
	}
	
	function coolNumber(number,textBox, multiplier)
	{
		var displayNumber = 0;
		var increment = number / 60 / multiplier;
		textBox.field.text = 0;
		textBox.onEnterFrame = function()
		{
			if(displayNumber < number)
			{
			displayNumber += increment;
			textBox.field.text = Math.round(displayNumber);
			}
			else
			{
				textBox.field.text = number;
				delete this.onEnterFrame;
			}
		}
	}
	
	function levelComplete(win)
	{
		var menu = _root.levelComplete;
		var MoneyBonus;
		var EXPBonus;
		
		spawning = false;
		clearMapScreen();
		
		_root.hideGame._alpha = 0;
		_root.hideGame._visible = true;
		menu._visible = true;
		if(win)
		{
			menu.gotoAndStop(1);
		}
		else
		{
			menu.gotoAndStop(2);
		}
		menu.underlineAnim.play();
		
		levelXP = experience - levelXP;
		levelKills = kills - levelKills;
		levelMoney = gold - levelMoney;
		if(win)
		{
			MoneyBonus = achievements.length / 10;
			EXPBonus = Math.round(levelKills / enemiesList.length * 100) / 100;
		}
		else
		{
			MoneyBonus = 0;
			EXPBonus = 0;
		}
		gold += Math.round(levelMoney * MoneyBonus);
		experience += Math.round(levelXP * EXPBonus);
		coolNumber(levelMoney, menu.money, 1);
		menu.moneyBonus.text = "+" + (MoneyBonus * 100) + "%";
		coolNumber((levelMoney + Math.round(levelMoney * MoneyBonus)), menu.moneyTotal, 1.5);
		coolNumber(levelXP, menu.eXP, 1);
		menu.eXPBonus.text = "+" + (EXPBonus * 100) + "%";
		coolNumber((levelXP + Math.round(levelXP * EXPBonus)), menu.eXPTotal, 1.5);
		_root.userInterface.goldText.text = gold;
		_root.userInterface.xpText.text = experience;
		
		if(win && currentLevel > highestLevelCompleted && currentLevel < 11)
		{
			highestLevelCompleted = currentLevel;
		}
		
		submitStats();
		
		upgradeEquipment(EXPBonus, MoneyBonus);
	}
	
	function updateCharacterDisplay()
	{
		clearCharacterDisplay();
		switch (cape)
		{
			case 0:
				speed = 3;
				break;
			case 1:
				_root.flyGuy.cape1._visible = true;
				speed = 4;
				break;
			case 2:
				_root.flyGuy.cape2._visible = true;
				speed = 6;
				break;
			case 3:
				_root.flyGuy.cape3._visible = true;
				speed = 9;
				break;
			case 4:
				_root.flyGuy.cape4._visible = true;
				speed = 12;
				break;
			case 5:
				_root.flyGuy.cape5._visible = true;
				speed = 15;
				break;
			case 6:
				_root.flyGuy.cape6._visible = true;
				speed = 20;
				break;
		}
	}
	
	function clearCharacterDisplay()
	{
		// Clear all the capes
		_root.flyGuy.cape1._visible = false;
		_root.flyGuy.cape2._visible = false;
		_root.flyGuy.cape3._visible = false;
		_root.flyGuy.cape4._visible = false;
		_root.flyGuy.cape5._visible = false;
		_root.flyGuy.cape6._visible = false;

		// Clear all the gloves
		
		// Clear all the boots
		
		// Clear all the hair
	}
	
	function clearMapScreen()
	{
		var enemy;
		for (var i in enemiesList)
		{
			enemy = enemiesList[i];
			enemy.removeMovieClip();
		}
		var bullet;
		for (var i in bulletsList)
		{
			bullet = bulletsList[i];
			bullet.removeMovieClip();
		}
		var pickup;
		for (var i in pickupsList)
		{
			pickup = pickupsList[i];
			pickup.removeMovieClip();
		}
	}
	
	function hideAllMenus()
	{
		_root.mainMenu._visible = false;
		_root.shopMenu._visible = false;
		_root.levelsMenu._visible = false;
		_root.achievementsMenu._visible = false;
		_root.trainingMenu._visible = false;
		_root.newEnemy._visible = false;
	}
	
	function siftArrays()
	{
		siftTimer++;
		if(siftTimer > 0)
		{
			siftTimer = 0;
			for(var i = enemiesList.length; i >= 0; i--)
			{
				if (!enemiesList[i]._y)
				{
					enemiesList.splice(i,1);
				}
			}
			
			for(var i = bulletsList.length; i >= 0; i--)
			{
				if (!bulletsList[i]._y)
				{
					bulletsList.splice(i,1);
				}
			}
		}
	}
	
	function saveGame()
	{
		savefile.data.gold = gold;
		savefile.data.experience = experience;
		savefile.data.capesPurchased = capesPurchased;
		
		savefile.data.MAXHEALTH = MAXHEALTH;
		savefile.data.shootSpeed = shootSpeed;
		savefile.data.flightTimerMAX = flightTimerMAX;
		savefile.data.bulletSpeed = bulletSpeed;
		savefile.data.Damage = Damage;
		savefile.data.flightTimerIncrementIncrement = flightTimerIncrementIncrement;
		
		savefile.data.kills = kills;
		
		savefile.flush();
	}
	
	function bootsEffect()
	{
		switch (boots)
		{
			case "regen1":
				if(!Key.isDown(Key.UP) && !Key.isDown(Key.RIGHT) && !Key.isDown(Key.DOWN) && !Key.isDown(Key.LEFT))
				{
					flightTimerIncrement -= flightTimerIncrementIncrement;
				}
				break;
			case "regen2":
				flightTimerIncrement -= flightTimerIncrementIncrement;
				if(Key.isDown(Key.UP))
				{
					flightTimerIncrement -= flightTimerIncrementIncrement / 2;
				}
				if(Key.isDown(Key.RIGHT))
				{
					flightTimerIncrement -= flightTimerIncrementIncrement / 2;
				}
				if(Key.isDown(Key.DOWN))
				{
					flightTimerIncrement -= flightTimerIncrementIncrement / 2;
				}
				if(Key.isDown(Key.LEFT))
				{
					flightTimerIncrement -= flightTimerIncrementIncrement / 2;
				}
				break;
			case "magnet1":
				for(var i = pickupsList.length - 10; i < pickupsList.length; i++)
				{
					if (pickupsList[i]._x > _x)
					{
						pickupsList[i]._x -= 1;
					}
					else
					{
						pickupsList[i]._x += 1;
					}
				}
				break;
			case "magnet2":
				for(var i = pickupsList.length - 10; i < pickupsList.length; i++)
				{
					if (pickupsList[i]._x > _x)
					{
						pickupsList[i]._x -= 3;
					}
					else
					{
						pickupsList[i]._x += 3;
					}
				}
				break;
		}
	}
	
	function getLuck()
	{
		switch (boots)
		{
			case "lucky1":
				luck = 4;
				break;
			case "lucky2":
				luck = 3;
				break;
			default:
				luck = 5;
				break;
		}
	}
	
	function checkAchievements(type)
	{
		var unlocked = false;
		var achievement;
		switch(type)
		{
			case "kills":
				if(kills == 1)
				{
					achievement = "First blood","Awarded for getting your first kill";
					unlocked = true;
				}
				if(kills == 50)
				{
					achievement = "Leaving your mark";
					unlocked = true;
				}
				if(kills == 500)
				{
					achievement = "Clear skies";
					unlocked = true;
				}
				if(kills == 1000)
				{
					achievement = "Emotional janitor";
					unlocked = true;
				}
				if(kills == 5000)
				{
					achievement = "Cold Blooded";
					unlocked = true;
				}
				if(kills == 10000)
				{
					achievement = "Really...";
					unlocked = true;
				}
				break;
		}
		if(unlocked)
		{
			achievements.push(achievement);
			playAchievement(achievement);
		}
	}
	function playAchievement(messageField)
	{
		_root.achievementUnlocked.field.text = messageField;
		_root.achievementUnlocked.play()
	}
	
	function chooseMusic(level)
	{
		if(level == 10)
		{
			_root.backgroundMusic.attachSound("dungeon.mp3");
			_root.backgroundMusic.start(0,1000);
		}
	}
	
	function chooseBackground(level)
	{
		_root.dirtBackground._visible = false;
		_root.oceanBackground._visible = false;
		
		switch(level)
		{
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				_root.oceanBackground._visible = true;
				break;
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				_root.dirtBackground._visible = true;
				break;
			default:
				chooseBackground(Math.round(Math.random()*10));
		}
	}
	
	function submitStats()
	{
		//_root.kongregateStats.submit("Name of stat", value);
		_root.kongregateStats.submit("Kills", kills);
		_root.kongregateStats.submit("Achievements", achievements.length);
		_root.kongregateStats.submit("Highest Level Completed", highestLevelCompleted);
	}
	
	function upgradeEquipment(EXPBonus, MoneyBonus)
	{
		var item;
		capesPurchased[cape][1] += levelXP + Math.round(levelXP * EXPBonus);
		checkLevelUp(cape);
		switch(currentWeapon)
		{
			case 'regen':
				item = 10;
				break;
			case 'splitter':
				item = 11;
				break;
			case 'homing':
				item = 12;
				break;
		}
		capesPurchased[item][1] += levelXP + Math.round(levelXP * EXPBonus);
		checkLevelUp(item);
		switch(boots)
		{
			case 'regen':
				item = 7;
				break;
			case 'magnet':
				item = 8;
				break;
			case 'lucky':
				item = 9;
				break;
		}
		capesPurchased[item][1] += levelXP + Math.round(levelXP * EXPBonus);
		checkLevelUp(item);
	}
	function checkLevelUp(instance)
	{
		var xpRequired;
		if(instance < 7 && instance > 0)
		{
			xpRequired = 500 * instance * instance * instance;
		}
		else
		{
			switch(instance)
			{
				case 7: // R Boots
					xpRequired = 40000
					break;
				case 8: // M Boots
					xpRequired = 10000
					break;
				case 9: // L Boots
					xpRequired = 20000
					break;
				case 10: // R Gloves
					xpRequired = 50000
					break;
				case 11: // S Gloves
					xpRequired = 50000
					break;
				case 12: // H Gloves
					xpRequired = 10000
					break;
			}
		}
		if(capesPurchased[instance][1] >= xpRequired && capesPurchased[instance][2] == 0)
		{
			capesPurchased[instance][2] = 1;
			_root.itemLevelUp.gotoAndStop(instance);
			_root.itemLevelUp._visible = true;
		}
	}
	function getFlightTimerMAX():Number
	{
		if ( capesPurchased[cape][2] == 1 )
		{
			return (flightTimerMAX * ( 1 + cape / 10));
		}
		else
		{
			return flightTimerMAX;
		}
	}
	function getFlightTimerIncrement():Number
	{
		return flightTimerIncrementBase - 5 + currentLevel * 2;
	}
}
