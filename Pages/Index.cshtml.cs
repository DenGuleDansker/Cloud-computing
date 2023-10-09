using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IBAS_menu.Pages;



public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public static List<menu> menu = new List<menu>();

    public void OnGet()
    {
        // Initialiser listen til opbevaring af menuen
        menu = new List<menu>();

        // Opret en filterstreng for at hente de ønskede rækker (undgå 'id-001')
        //string filter = $"RowKey ne 'id-001'";

        // Opret en TableClient til at arbejde med Azure Table Storage
        var tableClient = new TableClient(
            new Uri("https://ibasmenu.table.core.windows.net/"),
            "KantineMenu",
            new TableSharedKeyCredential("ibasmenu", "vnqIfjqOxyk3/xa7CuwwTkRXfmnaWOpyXQ+GT3SBuYbQrxo0TiFtc6qUzfMXkEqgvYCHWJVEmALL+AStFTWJOg=="));

        // Udfør query på Azure Table Storage med filter
        //Pageable<TableEntity> entities = tableClient.Query<TableEntity>(filter: filter);
        Pageable<TableEntity> entities = tableClient.Query<TableEntity>();

        // Iterer igennem resultater og konverter til menu-objekter
        foreach (TableEntity entity in entities)
        {
            menu data = new menu
            {
                UgeDag = (string)entity["UgeDag"],     // Antagelse: "Weekday" er kolonnenavnet for ugedage
                VarmeRetter = (string)entity["VarmeRetter"],   // Antagelse: "VarmDish" er kolonnenavnet for varme retter
                KoldeRetter = (string)entity["KoldeRetter"]    // Antagelse: "ColdDish" er kolonnenavnet for kolde retter
            };

            // Tilføj menuobjektet til listen
            menu.Add(data);
        }

        // Nu har du alle menuobjekterne i _productionRepo-listen
        // Du kan bruge _productionRepo til at vise data i din Razor-visning eller udføre andre handlinger efter behov.
    }
}

