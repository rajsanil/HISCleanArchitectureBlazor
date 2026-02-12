namespace HIS.Core.Abstractions;

/// <summary>
/// Exception thrown when module loading, validation, or initialization fails.
/// </summary>
public class ModuleException : Exception
{
    public string? ModuleId { get; }

    public ModuleException(string message) : base(message)
    {
    }

    public ModuleException(string message, string moduleId) : base(message)
    {
        ModuleId = moduleId;
    }

    public ModuleException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ModuleException(string message, string moduleId, Exception innerException) : base(message, innerException)
    {
        ModuleId = moduleId;
    }
}

/// <summary>
/// Exception thrown when a required module dependency is missing or not licensed.
/// </summary>
public class ModuleDependencyException : ModuleException
{
    public string[] MissingDependencies { get; }

    public ModuleDependencyException(string moduleId, string[] missingDependencies)
        : base($"Module '{moduleId}' has missing dependencies: {string.Join(", ", missingDependencies)}", moduleId)
    {
        MissingDependencies = missingDependencies;
    }
}

/// <summary>
/// Exception thrown when circular module dependencies are detected.
/// </summary>
public class CircularDependencyException : ModuleException
{
    public string[] DependencyCycle { get; }

    public CircularDependencyException(string[] dependencyCycle)
        : base($"Circular dependency detected: {string.Join(" → ", dependencyCycle)}")
    {
        DependencyCycle = dependencyCycle;
    }
}
