using PetOrg.Infrastructure.Persistence.Entities;

namespace PetOrg.UnitTests.Timeline;

public sealed class TimelineSeparationTests
{
    [Fact]
    public void Donation_and_animal_case_timelines_use_distinct_entity_types()
    {
        var donationTimeline = new DonationTimelineEvent
        {
            DonationId = Guid.NewGuid(),
            Type = "confirmed",
            At = DateTimeOffset.UtcNow,
            ActorId = "staff-01",
        };

        var animalCaseTimeline = new AnimalCaseTimelineEvent
        {
            AnimalCaseId = Guid.NewGuid(),
            Type = "medical_update",
            At = DateTimeOffset.UtcNow,
            ActorId = "vet-01",
        };

        Assert.IsType<DonationTimelineEvent>(donationTimeline);
        Assert.IsType<AnimalCaseTimelineEvent>(animalCaseTimeline);
        Assert.NotEqual(donationTimeline.GetType(), animalCaseTimeline.GetType());
    }
}
