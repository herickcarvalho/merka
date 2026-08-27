using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Mercado.ArchitectureTests;

/// <summary>
/// Valida em tempo de build que as camadas respeitam a direção de
/// dependência da Clean Architecture. Qualquer violação futura (ex.: um
/// desenvolvedor referenciando EF Core direto de um Domain) quebra o
/// build de testes, e não apenas uma revisão manual de código.
/// </summary>
public class LayerDependencyTests
{
    private const string DomainNamespacePattern = "Mercado.Modules.*.Domain";
    private const string ApplicationNamespacePattern = "Mercado.Modules.*.Application";

    [Fact]
    public void Domain_NaoDeveDependerDe_Application()
    {
        var result = Types.InAssemblies(LoadAllModuleAssemblies())
            .That().ResideInNamespaceMatching(DomainNamespacePattern)
            .ShouldNot().HaveDependencyOnAny("Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_NaoDeveDependerDe_EntityFrameworkCore()
    {
        var result = Types.InAssemblies(LoadAllModuleAssemblies())
            .That().ResideInNamespaceMatching(DomainNamespacePattern)
            .ShouldNot().HaveDependencyOnAny("Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_NaoDeveDependerDe_Infrastructure()
    {
        var result = Types.InAssemblies(LoadAllModuleAssemblies())
            .That().ResideInNamespaceMatching(ApplicationNamespacePattern)
            .ShouldNot().HaveDependencyOnAny("Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    private static System.Reflection.Assembly[] LoadAllModuleAssemblies() =>
        System.AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName != null && a.FullName.StartsWith("Mercado.Modules"))
            .ToArray();
}
