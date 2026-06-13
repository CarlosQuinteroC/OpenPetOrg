namespace PetOrg.Services.Auth;

public static class KeycloakOptionsValidator
{
    public static IReadOnlyList<string> Validate(KeycloakOptions options)
    {
        List<string> errors = [];

        AddRequiredErrorIfBlank(errors, options.Authority, "Authentication:Keycloak:Authority");
        AddRequiredErrorIfBlank(errors, options.Audience, "Authentication:Keycloak:Audience");
        AddRequiredErrorIfBlank(errors, options.Realm, "Authentication:Keycloak:Realm");
        AddRequiredErrorIfBlank(errors, options.ResourceAccessClientId, "Authentication:Keycloak:ResourceAccessClientId");

        return errors;
    }

    private static void AddRequiredErrorIfBlank(List<string> errors, string value, string key)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add($"{key} is required.");
        }
    }
}
