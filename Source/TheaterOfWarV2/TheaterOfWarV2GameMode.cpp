// Copyright Epic Games, Inc. All Rights Reserved.

#include "TheaterOfWarV2GameMode.h"
#include "TheaterOfWarV2Character.h"
#include "UObject/ConstructorHelpers.h"

ATheaterOfWarV2GameMode::ATheaterOfWarV2GameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/BP_FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

}
