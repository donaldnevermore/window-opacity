namespace WindowOpacity;

public record Config {
    /// <summary>
    /// Find the name you need in the console output.
    /// </summary>
    public string[] ProcessNames { get; init; } = new string[] { };

    /// <summary>
    /// Opacity should be 40-255.
    /// </summary>
    public byte Opacity { get; init; }
}
