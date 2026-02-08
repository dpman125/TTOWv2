// Copyright Epic Games, Inc. All Rights Reserved.

#include "TheaterOfWarV2PickUpComponent.h"

UTheaterOfWarV2PickUpComponent::UTheaterOfWarV2PickUpComponent()
{
	// Setup the Sphere Collision
	SphereRadius = 32.f;
}

void UTheaterOfWarV2PickUpComponent::BeginPlay()
{
	Super::BeginPlay();

	// Register our Overlap Event
	OnComponentBeginOverlap.AddDynamic(this, &UTheaterOfWarV2PickUpComponent::OnSphereBeginOverlap);
}

void UTheaterOfWarV2PickUpComponent::OnSphereBeginOverlap(UPrimitiveComponent* OverlappedComponent, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex, bool bFromSweep, const FHitResult& SweepResult)
{
	// Checking if it is a First Person Character overlapping
	ATheaterOfWarV2Character* Character = Cast<ATheaterOfWarV2Character>(OtherActor);
	if(Character != nullptr)
	{
		// Notify that the actor is being picked up
		OnPickUp.Broadcast(Character);

		// Unregister from the Overlap Event so it is no longer triggered
		OnComponentBeginOverlap.RemoveAll(this);
	}
}
