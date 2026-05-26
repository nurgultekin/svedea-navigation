namespace svedea_navigation.Models;

public record NavItem(
    string Label,
    string? Url = null,
    List<NavItem>? Children = null,
    bool IsExternal = false,
    string? IconUrl = null
);
