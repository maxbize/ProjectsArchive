class ShopMenu extends MovieClip
{
	var shop;
	var currentPage:Number;

	function onLoad()
	{
		currentPage = 1;
		
		_root.shopMenu.equippedGlove.swapDepths(20);
		_root.shopMenu.equippedCape.swapDepths(21);
		_root.shopMenu.equippedBoots.swapDepths(22);
		_root.shopMenu.equippedWig.swapDepths(23);

		_root.shopMenu._visible = false;

		_root.shopMenu.exitWeb1.onPress = function()
		{
			_root.mainMenu._visible = true;
			_root.shopMenu._visible = false;
		};

		_root.shopMenu.cape0.onPress = function()
		{
			_root.shopMenu.equippedCape._x = _root.shopMenu.cape0._x;
			_root.shopMenu.equippedCape._y = _root.shopMenu.cape0._y + 2;
			_root.flyGuy.cape = 0;
		};
		_root.shopMenu.cape1.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.cape1,1,200, _root.shopMenu.equippedCape)
		};
		_root.shopMenu.cape2.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.cape2,2,5000, _root.shopMenu.equippedCape)
		};
		_root.shopMenu.cape3.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.cape3,3,20000, _root.shopMenu.equippedCape)
		};
		_root.shopMenu.cape4.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.cape4,4,50000, _root.shopMenu.equippedCape)
		};
		_root.shopMenu.cape5.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.cape5,5,100000, _root.shopMenu.equippedCape)
		};
		_root.shopMenu.cape6.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.cape6,6,999999, _root.shopMenu.equippedCape)
		};
		_root.shopMenu.gloveRegen.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.gloveRegen,10,100,_root.shopMenu.equippedGlove);
		};
		_root.shopMenu.gloveSplitter.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.gloveSplitter,11,50000,_root.shopMenu.equippedGlove);
		};
		_root.shopMenu.gloveHoming.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.gloveHoming,12,20000,_root.shopMenu.equippedGlove);
		};
		_root.shopMenu.bootsLucky.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.bootsLucky,9,20000,_root.shopMenu.equippedBoots);
		};
		_root.shopMenu.bootsRegen.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.bootsRegen,7,20000,_root.shopMenu.equippedBoots);
		};
		_root.shopMenu.bootsMagnet.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.bootsMagnet,8,20000,_root.shopMenu.equippedBoots);
		};
		_root.shopMenu.randomObject.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.randomObject,0,35000,0);
		};
		_root.shopMenu.wig.onPress = function()
		{
			_root.shopMenu.purchaseItem(_root.shopMenu.wig,13,1000,_root.shopMenu.equippedWig);
		};
		_root.shopMenu.searchBar.button1.onPress = function()
		{
			_root.shopMenu.searchEvent();
		};
		_root.shopMenu.nextPage1.onPress = function()
		{
			if (_root.shopMenu.currentPage <= 2)
			{
				_root.shopMenu.currentPage++;
				switch (_root.shopMenu.currentPage)
				{
					case 2 :
						_root.shopMenu._x = -250;
						_root.shopMenu.prevPage._x = 476.4;
						_root.shopMenu.exitWeb1._x = 728.15;
						_root.shopMenu.nextPage1._x = 567.85;
						_root.shopMenu.searchBar._x = 301.4;
						_root.shopMenu.goldText._x = 142.95 + 500;
						break;
					case 3 :
						_root.shopMenu._x = -750;
						_root.shopMenu.prevPage._x = 976.4;
						_root.shopMenu.exitWeb1._x = 1228.15;
						_root.shopMenu.nextPage1._x = 1067.85;
						_root.shopMenu.searchBar._x = 801.4;
						_root.shopMenu.goldText._x = 142.95 + 1000;
						break;
				}
			}
		};

		_root.shopMenu.prevPage.onPress = function()
		{
			if (_root.shopMenu.currentPage >= 2)
			{
				_root.shopMenu.currentPage--;
				switch (_root.shopMenu.currentPage)
				{
					case 1 :
						_root.shopMenu._x = 250;
						_root.shopMenu.prevPage._x = -23.6;
						_root.shopMenu.exitWeb1._x = 228.12;
						_root.shopMenu.nextPage1._x = 67.85;
						_root.shopMenu.searchBar._x = -198.6;
						_root.shopMenu.goldText._x = 142.95;
						break;
					case 2 :
						_root.shopMenu._x = -250;
						_root.shopMenu.prevPage._x = 476.4;
						_root.shopMenu.exitWeb1._x = 728.15;
						_root.shopMenu.nextPage1._x = 567.85;
						_root.shopMenu.searchBar._x = 301.4;
						_root.shopMenu.goldText._x = 142.95 + 500;
						break;
				}
			}
		};
	}

	function searchEvent()
	{
		switch (_root.shopMenu.searchBar.text1.text.toUpperCase())
		{
			case "GLOVES" :
			case "GLOVE" :
				_root.shopMenu._x = -250;
				_root.shopMenu.prevPage._x = 476.4;
				_root.shopMenu.exitWeb1._x = 728.15;
				_root.shopMenu.nextPage1._x = 567.85;
				_root.shopMenu.searchBar._x = 301.4;
				_root.shopMenu.goldText._x = 142.95 + 500;
				break;

			case "5 - STAR KONGREGATE GAME: FLY GUY" :
			case "CAPES" :
			case "CAPE" :
				_root.shopMenu._x = 250;
				_root.shopMenu.prevPage._x = -23.6;
				_root.shopMenu.exitWeb1._x = 228.12;
				_root.shopMenu.nextPage1._x = 67.85;
				_root.shopMenu.searchBar._x = -198.6;
				_root.shopMenu.goldText._x = 142.95;
				break;

			case "WIG" :
			case "WIGS" :
			case "HAIR" :
			case "BOOT" :
			case "BOOTS" :
				_root.shopMenu._x = -750;
				_root.shopMenu.prevPage._x = 976.4;
				_root.shopMenu.exitWeb1._x = 1228.15;
				_root.shopMenu.nextPage1._x = 1067.85;
				_root.shopMenu.searchBar._x = 801.4;
				_root.shopMenu.goldText._x = 142.95 + 1000;
				break;

			default :
				_root.shopMenu._x = 750;
				_root.shopMenu.exitWeb1._x = 228.12 - 500;
				_root.shopMenu.searchBar._x = -198.6 - 500;
				_root.shopMenu.goldText._x = 142.95 - 500;
				break;
		}
	}
	function addPurchased(itemPurchased,itemDepth)
	{
		var clip = _root.shopMenu.attachMovie("Purchased","Purchased"+_root.shopMenu.getNextHighestDepth(),itemDepth)
		clip._x = itemPurchased._x;
		clip._y = itemPurchased._y;
	}
	function purchaseItem(cape,instance,price,highliter)
	{
		var alreadyPurchased = false;

			if (_root.flyGuy.capesPurchased[instance][0] == 1)
			{
				alreadyPurchased = true;
			}

			if (!alreadyPurchased)
			{
				if (_root.flyGuy.gold >= price)
				{
					highliter._x = cape._x;
					highliter._y = cape._y + 2;
					_root.shopMenu.equip(instance)
					_root.flyGuy.cape = instance;
					_root.flyGuy.gold -= price;
					_root.flyGuy.capesPurchased[instance][0] = 1;
					_root.shopMenu.goldText.text = _root.flyGuy.gold;
					_root.shopMenu.addPurchased(cape,instance);
				}
			}
			else
			{
				highliter._x = cape._x;
				highliter._y = cape._y + 2;
				_root.shopMenu.equip(instance);
			}
	}
	function equip(instance)
	{
		switch(instance)
		{
			case 1:
				_root.flyGuy.cape = 1;
				break;
			case 2:
				_root.flyGuy.cape = 2;
				break;
			case 3:
				_root.flyGuy.cape = 3;
				break;
			case 4:
				_root.flyGuy.cape = 4;
				break;
			case 5:
				_root.flyGuy.cape = 5;
				break;
			case 6:
				_root.flyGuy.cape = 6;
				break;
			case 7:
				if(_root.flyGuy.capesPurchased[instance][2] == 1)
				{
					_root.flyGuy.boots = 'regen1';
				}
				else
				{
					_root.flyGuy.boots = 'regen2';
				}
				break;
			case 8:
				if(_root.flyGuy.capesPurchased[instance][2] == 1)
				{
					_root.flyGuy.boots = 'magnet1';
				}
				else
				{
					_root.flyGuy.boots = 'magnet2';
				}
				break;
			case 9:
				if(_root.flyGuy.capesPurchased[instance][2] == 1)
				{
					_root.flyGuy.boots = 'lucky1';
				}
				else
				{
					_root.flyGuy.boots = 'lucky2';
				}
				break;
			case 10:
				_root.flyGuy.currentWeapon = 'regen';
				break;
			case 11:
				_root.flyGuy.currentWeapon = 'splitter';
				break;
			case 12:
				_root.flyGuy.currentWeapon = 'homing';
				break;
		}
	}
}