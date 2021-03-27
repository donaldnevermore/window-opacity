namespace WindowOpacity {
    public record Config {
        /// <summary>
        ///     Find the name you need in the console output
        /// </summary>
        public string[]? ProcessNames { get; init; }

        /// <summary>
        ///     Must be greater than or equal to 40 and less than or equal to 255
        /// </summary>
        public byte Opacity { get; init; }
    }
}
