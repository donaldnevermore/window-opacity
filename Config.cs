namespace WindowOpacity {
    public record Config {
        /// <summary>
        /// Find the name you need in the console output
        /// </summary>
        public string[] ProcessNames { get; init; }

        /// <summary>
        /// Opacity should be 40 through 255
        /// </summary>
        public byte Opacity { get; init; }
    }
}
