namespace PetOrg.Api.Security;

public static class AuthorizationPolicies
{
    public const string DonorRole = "Donor";
    public const string StaffRole = "Staff";

    public const string DonorPolicy = "DonorOnly";
    public const string StaffPolicy = "StaffOnly";
}
