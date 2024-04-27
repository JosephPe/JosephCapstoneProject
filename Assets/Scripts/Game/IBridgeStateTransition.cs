using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public interface IBridgeStateTransition
	{
		//void TransitionBetweenStates(Enum @enum, List<IBridgeStateTransition> states);
		
		State TransitionBetweenStates(Enum @enum);
	}