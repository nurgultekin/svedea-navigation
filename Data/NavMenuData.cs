using svedea_navigation.Models;

namespace svedea_navigation.Data;

public static class NavMenuData
{
    public static List<NavCategory> GetCategories() => new()
    {
        new("Bil", "images/icons/bil.svg", new()
        {
            new("Bilförsäkring", "/bilforsakring"),
            new("Bilförsäkring för företag", "/foretagsforsakring/bilforsakring-for-foretag")
        }),
        new("Fordon", "images/icons/fordon.svg", new()
        {
            new("Snöskoterförsäkring", "/snoskoterforsakring"),
            new("ATV-försäkring", "/atv-forsakring"),
            new("Släpvagnsförsäkring", "/slapvagnsforsakring"),
            new("Husvagnsförsäkring", "/husvagnsforsakring")
        }),
        new("Motorcykel", "images/icons/mc.svg", new()
        {
            new("Mc-försäkring", "/mc-forsakring"),
            new("Märkesförsäkringar", "/mc-forsakring/markesforsakringar")
        }),
        new("Båt", "images/icons/bat.svg", new()
        {
            new("Båtförsäkring", "/batforsakring"),
            new("Märkesförsäkringar", "/batforsakring/markesforsakringar"),
            new("Vattenskoterförsäkring", "/vattenskoterforsakring"),
            new("Sportfiskarna", "/batforsakring/sportfiskarna")
        }),
        new("Djur", "images/icons/djur.svg", new()
        {
            new("Hundförsäkring", "/hundforsakring"),
            new("Jakthundsförsäkring", "/jakthundsforsakring"),
            new("Kattförsäkring", "/kattforsakring"),
            new("Djurförsäkring", "/djurforsakring")
        }),
        new("Hem & hus", "images/icons/hem.svg", new()
        {
            new("Hemförsäkring", "/hemforsakring"),
            new("Villaförsäkring", "/hemforsakring/villaforsakring"),
            new("Bostadsrättsförsäkring", "/hemforsakring/bostadsrattsforsakring"),
            new("Hyresrättsförsäkring", "/hemforsakring/hyresrattsforsakring"),
            new("Fritidshusförsäkring", "/hemforsakring/fritidshusforsakring")
        }),
        new("Företag", "images/icons/foretag.svg", new()
        {
            new("Företagsförsäkring", "/foretagsforsakring"),
            new("Bilförsäkring för företag", "/foretagsforsakring/bilforsakring-for-foretag"),
            new("Släpvagnsförsäkring", "/slapvagnsforsakring"),
            new("Drönarförsäkring", "/dronarforsakring")
        }),
        new("För förmedlare", "images/icons/grupp.svg", new()
        {
            new("Gruppförsäkringar", "/gruppforsakringar"),
            new("Kommunolycksfall", "/gruppforsakringar/kommunolycksfall"),
            new("Försäkring via förmedlare", "/foretagsforsakring/forsakring-via-formedlare")
        })
    };
}
