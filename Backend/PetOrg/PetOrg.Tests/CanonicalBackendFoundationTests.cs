using System.Runtime.CompilerServices;
using Xunit;

namespace PetOrg.Tests;

public sealed class CanonicalBackendFoundationTests
{
    [Fact]
    public void CanonicalSolutionTargetsNewBackendAndTestProjectsOnly()
    {
        var solution = ReadRepositoryFile("Backend", "PetOrg.sln");

        Assert.Contains(@"PetOrg\PetOrg\PetOrg.csproj", solution);
        Assert.Contains(@"PetOrg\PetOrg.Tests\PetOrg.Tests.csproj", solution);
        Assert.DoesNotContain(@"src\", solution, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(@"tests\PetOrg.UnitTests", solution, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(@"tests\PetOrg.IntegrationTests", solution, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SolutionFiltersStayAlignedWithCanonicalBackendProjects()
    {
        var rootSolutionFilter = ReadRepositoryFile("Backend", "PetOrg.slnx");
        var nestedSolutionFilter = ReadRepositoryFile("Backend", "PetOrg", "PetOrg.slnx");

        Assert.Contains("PetOrg/PetOrg/PetOrg.csproj", rootSolutionFilter);
        Assert.Contains("PetOrg/PetOrg.Tests/PetOrg.Tests.csproj", rootSolutionFilter);
        Assert.DoesNotContain("src/", rootSolutionFilter, StringComparison.OrdinalIgnoreCase);

        Assert.Contains("PetOrg/PetOrg.csproj", nestedSolutionFilter);
        Assert.Contains("PetOrg.Tests/PetOrg.Tests.csproj", nestedSolutionFilter);
        Assert.DoesNotContain("../src/", nestedSolutionFilter, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DevelopmentComposeStartsKeycloakAndPostgresWithLocalDefaults()
    {
        var compose = ReadRepositoryFile("deploy", "keycloak", "docker-compose.yml");

        Assert.Contains("keycloak:", compose);
        Assert.Contains("postgres:", compose);
        Assert.Contains("postgres:16", compose);
        Assert.Contains("petorg-postgres-data:", compose);
        Assert.Contains("${POSTGRES_DB:-petorg}", compose);
        Assert.Contains("${POSTGRES_USER:-petorg}", compose);
        Assert.Contains("${POSTGRES_PASSWORD:-petorg_dev_password}", compose);
        Assert.Contains("${KEYCLOAK_ADMIN:-admin}", compose);
        Assert.Contains("${KEYCLOAK_ADMIN_PASSWORD:-admin}", compose);
        Assert.Contains("pg_isready", compose);
        Assert.Contains("condition: service_healthy", compose);
    }

    private static string ReadRepositoryFile(params string[] pathSegments)
    {
        return File.ReadAllText(Path.Combine(GetRepositoryRoot(), Path.Combine(pathSegments)));
    }

    private static string GetRepositoryRoot([CallerFilePath] string sourceFile = "")
    {
        var directory = new DirectoryInfo(Path.GetDirectoryName(sourceFile)!);

        while (directory is not null && !Directory.Exists(Path.Combine(directory.FullName, ".git")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new DirectoryNotFoundException("Repository root containing .git was not found.");
    }
}
