# State Machine Diagrams
---

## SimpleAIController
> *"simple"*

```mermaid
stateDiagram-v2
	direction LR

	[*] --> Idle
	Idle --> Attack: CanSeePlayer
	Attack --> Idle: !CanSeePlayer

	state Idle {
		[*] --> Wait
	}

	state Attack {
		[*] --> Sequence
		Sequence --> MoveToTransform
		Sequence --> FocusObject
		Sequence --> Shoot
		Sequence --> ClearFocus
	}
```

## ShyAIController

```mermaid
stateDiagram-v2
	direction LR

	[*] --> Idle
	Idle --> Flee: IsPlayerNear
	Flee --> Idle: !IsPlayerNear

	state Idle {
		[*] --> Wait
	}

	state Flee {
		[*] --> AvoidTransform
	}
```

## CuriousAIController

```mermaid
stateDiagram-v2
	direction LR
	
	[*] --> Idle
	Idle --> Investigate: CanHear
	Investigate --> Idle: !IsPlayerNear
	InvestigateSound
	Wait2: Wait

	state Idle {
		[*] --> Wait
	}

	state Investigate {
		[*] --> Sequence
		Sequence --> InvestigateSound
		Sequence --> Wait2
	}
```

## NoAIController

```mermaid
stateDiagram-v2
	direction LR

	[*] --> DoNothing
```

> [!NOTE]
> There's also `TestController`, but it inherits from NoAIController anyway