using svedea_navigation.Models;

namespace svedea_navigation.Data;

public static class NavMenuData
{
    public static List<NavItem> GetPrimaryNav() => new()
    {
        new("Försäkringar", "/forsakringar", GetInsuranceCategories()),
        new("Tips & råd", "/tips"),
        new("Om oss", "/om-oss"),
    };

    private static List<NavItem> GetInsuranceCategories() => new()
    {
        new("Bil", "/bilforsakring", new()
        {
            new("Bilförsäkring", "/bilforsakring"),
            new("Bilförsäkring för företag", "/foretagsforsakring/bilforsakring-for-foretag"),
        }, IconUrl: "images/icons/bil.svg"),

        new("Fordon", "/snoskoterforsakring", new()
        {
            new("Snöskoterförsäkring", "/snoskoterforsakring"),
            new("ATV-försäkring", "/atv-forsakring"),
            new("Släpvagnsförsäkring", "/slapvagnsforsakring"),
            new("Husvagnsförsäkring", "/husvagnsforsakring"),
        }, IconUrl: "images/icons/fordon.svg"),

        new("Motorcykel", "/mc-forsakring", new()
        {
            new("Mc-försäkring", "/mc-forsakring"),
            new("Märkesförsäkringar", "/mc-forsakring/markesforsakringar"),
        }, IconUrl: "images/icons/mc.svg"),

        new("Båt", "/batforsakring", new()
        {
            new("Båtförsäkring", "/batforsakring"),
            new("Märkesförsäkringar", "/batforsakring/markesforsakringar"),
            new("Vattenskoterförsäkring", "/vattenskoterforsakring"),
            new("Sportfiskarna", "/batforsakring/sportfiskarna"),
        }, IconUrl: "images/icons/bat.svg"),

        new("Djur", "/hundforsakring", new()
        {
            new("Hundförsäkring", "/hundforsakring"),
            new("Jakthundsförsäkring", "/jakthundsforsakring"),
            new("Kattförsäkring", "/kattforsakring"),
            new("Djurförsäkring", "/djurforsakring"),
        }, IconUrl: "images/icons/djur.svg"),

        new("Hem & hus", "/hemforsakring", new()
        {
            new("Hemförsäkring", "/hemforsakring"),
            new("Villaförsäkring", "/hemforsakring/villaforsakring"),
            new("Bostadsrättsförsäkring", "/hemforsakring/bostadsrattsforsakring"),
            new("Hyresrättsförsäkring", "/hemforsakring/hyresrattsforsakring"),
            new("Fritidshusförsäkring", "/hemforsakring/fritidshusforsakring"),
        }, IconUrl: "images/icons/hem.svg"),

        new("Företag", "/foretagsforsakring", new()
        {
            new("Företagsförsäkring", "/foretagsforsakring"),
            new("Bilförsäkring för företag", "/foretagsforsakring/bilforsakring-for-foretag"),
            new("Släpvagnsförsäkring", "/slapvagnsforsakring"),
            new("Drönarförsäkring", "/dronarforsakring"),
        }, IconUrl: "images/icons/foretag.svg"),

        new("För förmedlare", "/gruppforsakringar", new()
        {
            new("Gruppförsäkringar", "/gruppforsakringar"),
            new("Kommunolycksfall", "/gruppforsakringar/kommunolycksfall"),
            new("Försäkring via förmedlare", "/foretagsforsakring/forsakring-via-formedlare"),
        }, IconUrl: "images/icons/grupp.svg"),
    };
}
