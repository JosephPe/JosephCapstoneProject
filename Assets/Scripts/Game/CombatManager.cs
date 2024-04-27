using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
[SerializeField] GameManager gameManager;


public enum State
	{
		INITIATE_COMBAT,
		PLAYER_TURN,
		ENEMY_TURN,
		WORLD_TURN,
		GAME_OVER
	}

	public State gameState;

	

	
}