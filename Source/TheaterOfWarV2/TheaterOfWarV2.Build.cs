// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class TheaterOfWarV2 : ModuleRules
{
	public TheaterOfWarV2(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore", "EnhancedInput" });
	}
}
