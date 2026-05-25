namespace svedea_navigation.Models;

public record NavLink(string Label, string Url);

public record NavCategory(
    string Name,
    string IconUrl,
    List<NavLink> Links
);
