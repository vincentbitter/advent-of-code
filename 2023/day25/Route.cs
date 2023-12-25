namespace day25;

public record Route(Component Component, Wire? Wire = null, Route? Previous = null, int Length = 1);